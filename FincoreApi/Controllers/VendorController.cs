using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FincoreApi.Controllers
{
    public class VendorController : Controller
    {

        [Authorize(Roles = "Vendor")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
