﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSMS.Infrastructure.Data.Models
{
    public class SalesProducts
    {
        [StringLength(64)]
        public string SaleId { get; set; }

        [ForeignKey(nameof(SaleId))]
        public Sales Sale { get; set; }



        [StringLength(64)]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Products Product { get; set; }
    }
}