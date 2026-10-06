using Fincore.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fincore.Domain.Entity
{
    public class VendorRFQMapping
    {
        [Key]
        public int Id { get; set; }

        public int RFQId { get; set; }

        [ForeignKey("RFQId")]
        public RFQ? RFQ { get; set; }

        public int VendorId { get; set; }

        [ForeignKey("VendorId")]
        public User? Vendor { get; set; }
    }
}
