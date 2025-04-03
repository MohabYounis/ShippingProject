using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Identity;
using Shipping.DTOs.pagination;
using Shipping.DTOs.Employee;

namespace Shipping.Services.ModelService
{
    public class EmployeeService : ServiceGeneric<Employee>, IEmployeeService
    {
        UserManager<ApplicationUser> userManager;

        public EmployeeService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork)
        {
            this.userManager= userManager;
        }

        public  async Task<GenericPagination<EmployeeDTO>> GetAllAsync(int pageIndex=1, int pageSize=10)
        {
            var query =await unitOfWork.GetRepository<Employee>().GetAllAsync();
           var employees=  await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)

                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            //maping

            var epmloyeesDto = employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.ApplicationUser?.UserName,
                Address = e.ApplicationUser?.Address,
                userId = e.ApplicationUser?.Id,
                branchId = e.Branch.Id,
                IsDeleted = e.IsDeleted
            }).ToList();

            return new  GenericPagination<EmployeeDTO>
            {
                pageIndex = pageIndex,
                pageSize = pageSize,
                totalCount = query.Count(),
                Items = epmloyeesDto
            };


        }

        public async Task<GenericPagination<EmployeeDTO>> GetAllExistAsync(int pageIndex = 1, int pageSize = 10)
        {
            var query =await unitOfWork.GetRepository<Employee>().GetAllExistAsync();
            var employees = await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //mapping

            var epmloyeesDto = employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.ApplicationUser?.UserName,
                Address = e.ApplicationUser?.Address,
                userId = e.ApplicationUser?.Id,
                branchId = e.Branch.Id,
                IsDeleted = e.IsDeleted
            }).ToList();

            return new GenericPagination<EmployeeDTO>
            {
                pageIndex = pageIndex,
                pageSize = pageSize,
                totalCount = query.Count(),
                Items = epmloyeesDto
            };




        }

        //search
        public async Task<IEnumerable<Employee>> GetEmployeesBySearch(string term, bool includeDelted = true)
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
                .Where(e => EF.Functions.Like(e.ApplicationUser.UserName, $"%{term}%"))
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
