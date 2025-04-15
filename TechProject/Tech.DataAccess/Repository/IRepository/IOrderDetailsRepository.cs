using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechProject.Models;
using TechProject.DataAccess.Data;
using Tech.Models.Models;
namespace Tech.DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails> 
    {
        void Update(OrderDetails obj);
    }
}
