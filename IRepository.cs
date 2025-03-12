using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.ImodelRepository
{
    public interface IRepository
    {
        List<> GetAll();
         GetById(int id);
        int Insert( entity);
        int Update( entity);
        int Delete(int id);
    }
}
