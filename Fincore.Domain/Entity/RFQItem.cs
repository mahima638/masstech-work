using Fincore.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fincore.Domain.Entity
{
    public class RFQItem
    {
        [Key]
        public int ItemId { get; set; }

        public int RFQId { get; set; }

        [ForeignKey("RFQId")]
        public RFQ? RFQ { get; set; }

        public string? IndentNo { get; set; }
        public string RFQLineNo { get; set; }

        public string ItemNo { get; set; }

        public string ItemName { get; set; }

        public int ReqQty { get; set; }

        public string UOM { get; set; }

        public DateTime ReqDeliveryDate { get; set; }

        public string? DeliveryLocation { get; set; }

        public string? Description { get; set; }

        public string? FactoryCode { get; set; }

    }
}
