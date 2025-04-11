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
        // cashed roles 
        readonly IApplicationRoleService roleCacheService;
        public EmployeeService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IApplicationRoleService roleCacheService) : base(unitOfWork)
        {
            this.userManager= userManager;
            this.roleCacheService = roleCacheService;
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
            //get roles from cache
            var roleDictionary = await roleCacheService.GetRoleDictionaryAsync();
           
            
            //mapping
            var epmloyeesDto =  employees.Select( e => 
            {
                //get role id
                var roleId = e.ApplicationUser?.UserRoles?.FirstOrDefault()?.RoleId;

                return new EmployeeDTO { 
                Id = e.Id,
                IsDeleted = e.IsDeleted,

                userId = e.AppUser_Id,
                Name = e.ApplicationUser.UserName,
                Phone = e.ApplicationUser?.PhoneNumber,
                Address = e.ApplicationUser?.Address,
                    Email = e.ApplicationUser?.Email,
                    // using roleId 
                    RoleId = roleId,
                    Role = roleDictionary.TryGetValue(roleId ?? "", out var roleName) ? roleName : " no role",
                branchId = e.Branch_Id,
                BranchName = e.Branch.Name
                };
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

            //get roles from cache
            var roleDictionary = await roleCacheService.GetRoleDictionaryAsync();


            //mapping
            var employeeDtosTasks = employees.Select(async e =>
            {
                //get role id
                var roleDto =await roleCacheService.GetRoleByUserIdAsync(e.AppUser_Id);

                return new EmployeeDTO
                {
                    Id = e.Id,
                    IsDeleted = e.IsDeleted,

                    userId = e.AppUser_Id,
                    Name = e.ApplicationUser.UserName,
                    Phone = e.ApplicationUser?.PhoneNumber,
                    Address = e.ApplicationUser?.Address,
                    Email = e.ApplicationUser?.Email,
                    // using roleId 
                    RoleId = roleDto.Id,
                    Role = roleDictionary.TryGetValue(roleDto.Name ?? "", out var roleName) ? roleName : " no role",
                    branchId = e.Branch_Id,
                    BranchName = e.Branch.Name
                };
            }).ToList();



            //await all tasks to complete
            var employeeDtos = await Task.WhenAll(employeeDtosTasks);


            return new GenericPagination<EmployeeDTO>
            {
                pageIndex = pageIndex,
                pageSize = pageSize,
                totalCount = query.Count(),
                Items = employeeDtos
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

      
    }
}
