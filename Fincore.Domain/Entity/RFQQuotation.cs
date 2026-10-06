using Fincore.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fincore.Domain.Entity
{
   public  class RFQQuotation
    {
        [Key]
        public int QuotationId { get; set; }

        public string? BidNo { get; set; }

        [Precision(18, 4)]
        public decimal? QuotedAmount { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string? PaymentTerms { get; set; }

        public string? Remarks { get; set; }

        public string? Status { get; set; } = "Pending";

        public DateTime? SubmittedDate { get; set; } = DateTime.Now;

        public int RFQId { get; set; }

        [ForeignKey("RFQId")]
        public RFQ? RFQ { get; set; }

        public int VendorId { get; set; }

        [ForeignKey("VendorId")]
        public User? Vendor { get; set; }

        public FinalizedQuotation? FinalizedQuotation { get; set; }
    }
}
