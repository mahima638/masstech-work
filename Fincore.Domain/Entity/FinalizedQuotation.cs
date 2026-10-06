using Fincore.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fincore.Domain.Entity
{
    public class FinalizedQuotation
    {
        [Key]
        public int FinalId { get; set; }

        public DateTime? FinalizedDate { get; set; } = DateTime.Now;

        public int RFQId { get; set; }

        [ForeignKey("RFQId")]
        public RFQ? RFQ { get; set; }

        public int QuotationId { get; set; }

        [ForeignKey("QuotationId")]
        public RFQQuotation? RFQQuotation { get; set; }
    }
}