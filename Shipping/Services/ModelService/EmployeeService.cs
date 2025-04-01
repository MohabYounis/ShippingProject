using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Identity;

namespace Shipping.Services.ModelService
{
    public class EmployeeService : ServiceGeneric<Employee>, IEmployeeService
    {
        UserManager<ApplicationUser> userManager;

        public EmployeeService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork)
        {
            this.userManager= userManager;
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var query =await unitOfWork.GetRepository<Employee>().GetAllAsync();
            return await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Employee>> GetAllExistAsync()
        {
            var query =await unitOfWork.GetRepository<Employee>().GetAllExistAsync();
            return await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToListAsync();
        }

        //search
        public  async Task<IEnumerable<Employee>> GetEmployeeBySearch(string term, bool includeDelted = true)
        {
            IQueryable<Employee> query = null;
            if (!includeDelted)
            {
                query = await unitOfWork.GetRepository<Employee>().GetAllExistAsync();
            }

             query = await unitOfWork.GetRepository<Employee>().GetAllAsync();

            return await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .Where(e => e.ApplicationUser.UserName.Contains(term))
                .ToListAsync();
        }



        // get employees by role 
        public async Task<IEnumerable<Employee>> GetEmployeesByRole(string roleName)
        {
            //get ids of users
            var userIds = (await userManager.GetUsersInRoleAsync(roleName))
                            .Select(u => u.Id)
                            .ToList();

            //get employees with ids 
            var query =await  unitOfWork.GetRepository<Employee>().GetAllExistAsync();

            return  await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .Where(e => userIds.Contains(e.ApplicationUser.Id))
                .ToListAsync();

        }



        


        //transaction

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {

              return    await  unitOfWork.Context.Database.BeginTransactionAsync();


        }
        //




      



        



      
    }
}
