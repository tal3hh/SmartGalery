﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ViewModels
{
    public class DashProductSearchVM
    {
        public string? Search { get; set; }
        public int Page { get; set; }
        public int Take { get; set; }
    }
}