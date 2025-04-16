using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Models.Models;
using TechProject.Models;

namespace Tech.DataAccess.Repository.IRepository
{
    public interface IDiscountRepository:IRepository<Discount>
    {
        void Update(Discount obj);
       
    }
}
