﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ViewModels
{
    public class HomeOrderAddVM
    {
        public string? Username { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
