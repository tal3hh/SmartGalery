using ServiceLayer.Dtos.Order;
using ServiceLayer.Utilities;
using ServiceLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Paginate<HomeOrderItemDto>> DashProductOrderFilter(DashOrderDateVM vm);
    }
}
