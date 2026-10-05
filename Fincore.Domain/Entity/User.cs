using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fincore.Domain.Entity
{
    public class User
    {


        [Key]
        public int eid { get; set; }
        public  string email { get; set; }
        public string pass { get; set; }

        [ForeignKey("rid")]
        public int rid { get; set; }
        public Role role { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public string? TwoFactorSecret { get; set; }


    }
}
