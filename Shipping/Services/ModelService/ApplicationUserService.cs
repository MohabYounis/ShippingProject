﻿using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;

namespace Shipping.Services.ModelService
{
    public class ApplicationUserService : ServiceGeneric<ApplicationUser>, IApplicationUserService
    {
        public ApplicationUserService(IRepositoryGeneric<ApplicationUser> repository) : base(repository)
        {
        }
    }
}
