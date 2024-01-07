using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ViewModels
{
    public class PaginationVM
    {
        public int CategoryId { get; set; }
        public int Page { get; set; }
        public int Take { get; set; }
    }
}
