using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Fincore.Domain.Entity;
using Fincore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace FincoreApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration con;
        private readonly AppDbContext db;

        public AuthController(IConfiguration con, AppDbContext db)
        {
            this.con = con;
            this.db = db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User u)
        {
            var user = db.user
                .Include(x => x.role)
                .FirstOrDefault(x => x.email == u.email && x.pass == u.pass);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View(u);
            }

            if (!user.TwoFactorEnabled)
            {
                TempData["Setup2FAUserId"] = user.eid;

                return RedirectToAction("Setup2FA");
            }

            TempData["TwoFactorUserId"] = user.eid;

            return RedirectToAction("Verify2FA");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User u)
        {
            var existingUser = db.user
                .FirstOrDefault(x => x.email == u.email);

            if (existingUser != null)
            {
                ViewBag.Error = "Email already exists";
                return View(u);
            }

            db.user.Add(u);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult CreateJwtAndLogin(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.email),
                new Claim(ClaimTypes.Role, user.role.rname)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(con["JwtConfig:Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: con["JwtConfig:Issuer"],
                audience: con["JwtConfig:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            Response.Cookies.Append("JwtToken", tokenString);

            if (user.role.rname == "Vendor")
            {
                return RedirectToAction("Dashboard", "Vendor");
            }

            return RedirectToAction("Dashboard", "Admin");
        }

        public IActionResult Setup2FA()
        {
            var userId = TempData["Setup2FAUserId"];

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = db.user.Include(x => x.role).FirstOrDefault(x => x.eid == (int)userId);

            if (user == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(user.TwoFactorSecret))
            {
                var secret = KeyGeneration.GenerateRandomKey(20);

                user.TwoFactorSecret = Base32Encoding.ToString(secret);

                db.SaveChanges();
            }

            TempData["Setup2FAUserId"] = user.eid;

            string otpUri =$"otpauth://totp/Fincore:{Uri.EscapeDataString(user.email)}" +
                $"?secret={user.TwoFactorSecret}&issuer=Fincore";

            var qrGenerator = new QRCodeGenerator();

            var qrCodeData = qrGenerator.CreateQrCode(otpUri, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeBytes = qrCode.GetGraphic(20);
            ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(qrCodeBytes);
            return View();
        }

        [HttpPost]
        public IActionResult Enable2FA(string code)
        {
            var userId = TempData["Setup2FAUserId"];

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = db.user.Include(x => x.role).FirstOrDefault(x => x.eid == (int)userId);

            if (user == null)
            {
                return NotFound();
            }

            var secretBytes = Base32Encoding.ToBytes(user.TwoFactorSecret);

            var totp = new Totp(secretBytes);

            bool isValid = totp.VerifyTotp(
                code,
                out _,
                new VerificationWindow(1, 1)
            );

            if (isValid)
            {
                user.TwoFactorEnabled = true;

                db.SaveChanges();

                return CreateJwtAndLogin(user);
            }

            TempData["Setup2FAUserId"] = user.eid;

            return Content("Invalid OTP!");
        }

        [HttpGet]
        public IActionResult Verify2FA()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Verify2FA(string code)
        {
            var userId = TempData["TwoFactorUserId"];

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = db.user
                .Include(x => x.role)
                .FirstOrDefault(x => x.eid == (int)userId);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var secretBytes = Base32Encoding.ToBytes(user.TwoFactorSecret);

            var totp = new Totp(secretBytes);

            bool isValid = totp.VerifyTotp(
                code,
                out long timeStepMatched,
                new VerificationWindow(1, 1)
            );

            if (isValid)
            {
                return CreateJwtAndLogin(user);
            }

            TempData.Keep("TwoFactorUserId");

            ViewBag.Error = "Invalid OTP. Please try again.";

            return View();
        }
    }
}
