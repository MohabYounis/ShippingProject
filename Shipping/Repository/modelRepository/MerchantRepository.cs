using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.ImodelRepository;
using Shipping.Models;
using Microsoft.EntityFrameworkCore;
using Shipping.Repository;
using Shipping.UnitOfWorks;

namespace Shipping.modelRepository
{
    public class MerchantRepository : RepositoryGeneric<Merchant>, IMerchantRepository
    {
        public MerchantRepository (UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

    }
}

