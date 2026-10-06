using Fincore.Domain.Entity;
using Fincore.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FincoreApi.Controllers
{
    public class VendorController : Controller
    {
        private readonly IRFQService rf;
        public VendorController(IRFQService rf)
        {
            this.rf = rf;
        }

        [Authorize(Roles = "Vendor")]
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult MyRFQs()
        {
            var vendorId = HttpContext.Session.GetInt32("UserId");

            if (vendorId == null)
            {
                return Unauthorized();
            }

            var rfqs = rf.GetVendorRFQs(vendorId.Value);

            return View(rfqs);
        }
        public IActionResult ViewRFQ(int id)
        {
            var rfq = rf.GetById(id);

            if (rfq == null)
            {
                return NotFound();
            }

            ViewBag.Items = rf.GetItems(id);

            return View(rfq);
        }
        public IActionResult Quote(int id)
        {
            var vendorId = HttpContext.Session.GetInt32("UserId");

            if (vendorId == null)
            {
                return Unauthorized();
            }

            var rfq = rf.GetById(id);

            if (rfq == null)
            {
                return NotFound();
            }

            ViewBag.MyQuotations = rf.GetVendorQuotations(id, vendorId.Value);

            return View(rfq);
        }
        [HttpPost]
        public IActionResult Quote(RFQQuotation quotation)
        {
            var vendorId = HttpContext.Session.GetInt32("UserId");

            if (vendorId == null)
            {
                return Unauthorized();
            }

            quotation.VendorId = vendorId.Value;
            quotation.Status = "Pending";
            quotation.SubmittedDate = DateTime.Now;

            rf.AddQuotation(quotation);

            return RedirectToAction("MyRFQs");
        }
    }
}
