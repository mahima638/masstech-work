
using Fincore.Domain.Entity;
using Fincore.Infrastructure.Data;
using Fincore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fincore.Infrastructure.Services
{
    public class RFQService : IRFQService
    {
        private readonly AppDbContext db;

        public RFQService(AppDbContext db)
        {
            this.db = db;
        }

        public List<RFQ> GetAll()
        {
            return db.RFQs.ToList();
        }

        public RFQ? GetById(int id)
        {
            return db.RFQs.Include(x => x.FinalizedQuotation).FirstOrDefault(x => x.RFQId == id);
        }

        public void Add(RFQ rfq)
        {
            db.RFQs.Add(rfq);
            db.SaveChanges();

            string year = DateTime.Now.ToString("yy");

           
            rfq.RFQNo = $"RFQ/{year}/{rfq.RFQId:D6}";

         
            db.SaveChanges();
        }

        public void Update(RFQ rfq)
        {
            db.RFQs.Update(rfq);
            db.SaveChanges();
        }

        public List<RFQItem> GetItems(int rfqId)
        {
            return db.RFQItems.Where(x => x.RFQId == rfqId).ToList();
        }

        public void AddItem(RFQItem item)
        {
            db.RFQItems.Add(item);
            db.SaveChanges();
        }
        public List<RFQ> GetVendorRFQs(int vendorId)
        {
            return db.VendorRFQMappings
                .Where(x => x.VendorId == vendorId && x.RFQ.Status == "Published")
                .Select(x => x.RFQ)
                .ToList();
        }
        public void AddQuotation(RFQQuotation quotation)
        {
            db.RFQQuotations.Add(quotation);
            db.SaveChanges();
        }
        public List<RFQQuotation> GetQuotations(int rfqId)
        {
            return db.RFQQuotations
                .Where(x => x.RFQId == rfqId)
                .ToList();
        }
        public void FinalizeQuotation(int rfqId, int quotationId)
        {
            var existing = db.FinalizedQuotations.FirstOrDefault(x => x.RFQId == rfqId);

            if (existing != null)
            {
                return;
            }

            FinalizedQuotation final = new FinalizedQuotation();

            final.RFQId = rfqId;
            final.QuotationId = quotationId;
            final.FinalizedDate = DateTime.Now;

            db.FinalizedQuotations.Add(final);

            db.SaveChanges();
        }
        public List<RFQQuotation> GetVendorQuotations(int rfqId, int vendorId)
        {
            return db.RFQQuotations.Where(x => x.RFQId == rfqId && x.VendorId == vendorId)
                .ToList();
        }
    }
}