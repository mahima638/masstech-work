using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fincore.Domain.Entity
{
   public class Role
    {
        [Key]
        public int rid { get; set; }
        public string rname { get; set; }

        public string isActive { get; set; }
    }
}
