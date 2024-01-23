using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ViewModels
{
    public class DashCategoryProductVM
    {
        public int CategoryId { get; set; }
        public int Take { get; set; }
        public int Page { get; set; }
    }
}
