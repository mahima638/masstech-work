
using Fincore.Domain.Entity;
using Fincore.Infrastructure.Data;
using Fincore.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace FincoreApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RFQController : Controller
    {
        private readonly IRFQService rf;
        private readonly AppDbContext db;

        public RFQController(IRFQService rf, AppDbContext db)
        {
            this.rf = rf;
            this.db = db;
        }

        public IActionResult Index()
        {
            var rfqs = rf.GetAll();

            return View(rfqs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Create(RFQ rfq, string action)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized();
            }

            rfq.UserId = userId.Value;
            rfq.Status = "Draft";

            rf.Add(rfq);

            if (action == "draft")
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Items", new { id = rfq.RFQId });
        }
        public IActionResult Items(int id)
        {
            var rfq = rf.GetById(id);

            if (rfq == null)
            {
                return NotFound();
            }
            ViewBag.Items = rf.GetItems(id);

            return View(rfq);
        }
        [HttpPost]
        public IActionResult AddItem(RFQItem item)
        {
            rf.AddItem(item);

            return RedirectToAction("Items", new { id = item.RFQId });
        }
        public IActionResult Duration(int id)
        {
            var rfq = rf.GetById(id);

            if (rfq == null)
            {
                return NotFound();
            }

            var vendors = db.user.Where(x => x.role.rname == "Vendor").ToList();

            ViewBag.Vendors = vendors;

            return View(rfq);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Submit(int id, DateTime bidStartDate, DateTime bidEndDate, int[] vendorIds, string action)
        {
            var rfq = rf.GetById(id);

            if (rfq == null)
            {
                return NotFound();
            }

            rfq.BidDate = bidStartDate;
            rfq.ExpiryDateofBid = bidEndDate;

            if (action == "draft")
            {
                rfq.Status = "Draft";
                rf.Update(rfq);

                return RedirectToAction("Index");
            }

            rfq.Status = "Published";
            rf.Update(rfq);

            foreach (var vendorId in vendorIds)
            {
                db.VendorRFQMappings.Add(new VendorRFQMapping
                {
                    RFQId = id,
                    VendorId = vendorId
                });
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var rfq = rf.GetById(id);

            if (rfq == null)
            {
                return NotFound();
            }

            return View(rfq);
        }
        [HttpPost]
        public IActionResult Edit(RFQ rfq)
        {
            var existingRfq = rf.GetById(rfq.RFQId);

            if (existingRfq == null)
            {
                return NotFound();
            }

            

            rf.Update(existingRfq);

            return RedirectToAction("Items", new { id = rfq.RFQId });
        }
        public IActionResult Quotations(int id)
        {
            var rfq = rf.GetById(id);

            if (rfq == null)
            {
                return NotFound();
            }

            ViewBag.Quotations = rf.GetQuotations(id);

            return View(rfq);
        }
        [HttpPost]
        public IActionResult FinalizeQuotation(int rfqId, int quotationId)
        {
            rf.FinalizeQuotation(rfqId, quotationId);
            TempData["SuccessMessage"] = "Quotation selected successfully.";

            return RedirectToAction("Quotations", new { id = rfqId });
        }
    }
}