
using Fincore.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fincore.Infrastructure.Repositories
{
    public interface IRFQService
    {
        List<RFQ> GetAll();
        RFQ GetById(int id);
        void Add(RFQ rfq);
        void Update(RFQ rfq);
        List<RFQItem> GetItems(int rfqId);

        void AddItem(RFQItem item);
        List<RFQ> GetVendorRFQs(int vendorId);
        void AddQuotation(RFQQuotation quotation);
        List<RFQQuotation> GetQuotations(int rfqId);
        void FinalizeQuotation(int rfqId, int quotationId);
        List<RFQQuotation> GetVendorQuotations(int rfqId, int vendorId);

    }
}
