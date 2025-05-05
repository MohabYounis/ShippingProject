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
        //
        IServiceGeneric<Branch> branchService;
        public EmployeeService(IUnitOfWork unitOfWork, IServiceGeneric<Branch> branchService, UserManager<ApplicationUser> userManager, IApplicationRoleService roleCacheService) : base(unitOfWork)
        {
            this.userManager= userManager;
            this.roleCacheService = roleCacheService;
            this.branchService = branchService;
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var query = unitOfWork.GetRepository<Employee>().GetAllAsync();
            var employees = await query;
            return employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToList();
        }

        public override async Task<IEnumerable<Employee>> GetAllExistAsync()
        {
            var query = unitOfWork.GetRepository<Employee>().GetAllExistAsync();
            var employees = await query;
            return employees
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToList();
        }

        public async Task<EmployeeDTO> GetByIdAsync(int id)
        {

            var employeeQuery = await unitOfWork.GetRepository<Employee>().GetAllAsync();

            var employee = await employeeQuery
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return null;
            }

            //  roleDictionary
            var roleDictionary = await roleCacheService.GetRoleDictionaryAsync();

            // record
            var roleDto = await roleCacheService.GetRoleByUserIdAsync(employee.AppUser_Id);

            //  mapping
            var employeeDto = new EmployeeDTO
            {
                Id = employee.Id,
                IsDeleted = employee.IsDeleted,
                userId = employee.AppUser_Id,
                Name = employee.ApplicationUser.UserName,
                Phone = employee.ApplicationUser?.PhoneNumber,
                Address = employee.ApplicationUser?.Address,
                Email = employee.ApplicationUser?.Email,
                RoleId = roleDto?.Id,
                Role = roleDictionary.TryGetValue(roleDto?.Id ?? "", out var role) ? role : "no role",
                branchId = employee.Branch_Id,
                BranchName = employee.Branch.Name
            };

            return employeeDto;
        }

        //
        public async Task<EmployeeDTO> UpdateAsync(int id, UpdateEmployeeDTO employeeDto)
        {
            //get from db
            var employeeQuery = await unitOfWork.GetRepository<Employee>().GetAllAsync();
            var employee = await employeeQuery
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return null;
            }
            //
            var branch = await branchService.GetByIdAsync(employeeDto.branchId);
            if (branch == null)
            {
                throw new Exception("Branch not found");
            }
            //
            var appUser = employee.ApplicationUser;
            appUser.UserName = employeeDto.Name;
            appUser.Email = employeeDto.Email;
            appUser.PhoneNumber = employeeDto.Phone;
            appUser.Address = employeeDto.Address;

            var result = await userManager.UpdateAsync(appUser);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            employee.Branch_Id = branch.Id;

            await unitOfWork.SaveChangesAsync();

            //
            var roleDto = await roleCacheService.GetRoleByUserIdAsync(employee.AppUser_Id);


            //mapping for output
            var updatedEmployeeDto = new EmployeeDTO
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                Address = employeeDto.Address,
                branchId = employeeDto.branchId,

                Id = employee.Id,
                IsDeleted = employee.IsDeleted,
                userId = employee.AppUser_Id,
                RoleId = roleDto?.Id,
                Role = roleDto?.Name ?? "no role",
                BranchName = branch.Name
            };

            return updatedEmployeeDto;
        }
        public async Task<EmployeeDTO> AddAsync(CreateEmployeeDTO employeeDto)
        {
            //check role starts with employee
            if (!employeeDto.Role.ToLower().StartsWith("employee"))
            {
                throw new Exception("Role must start with 'Employee'");
            }

            var role = await roleCacheService.GetByNameAsync(employeeDto.Role.Trim());
            if (role == null)
            {
                throw new Exception($"Role {employeeDto.Role} not found");
            }

            var branch = await branchService.GetByIdAsync(employeeDto.branchId);
            if (branch == null)
            {
                throw new Exception("Branch not found");
            }

            var appUser = new ApplicationUser
            {
                UserName = employeeDto.Name,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.Phone,
                Address = employeeDto.Address
            };

            var result = await userManager.CreateAsync(appUser, employeeDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(appUser, role.Name);

            var employee = new Employee
            {
                Branch_Id = branch.Id,
                AppUser_Id = appUser.Id,
                ApplicationUser = appUser
            };

            await unitOfWork.GetRepository<Employee>().AddAsync(employee);
            await unitOfWork.SaveChangesAsync();

            var createdEmployeeDto = new EmployeeDTO
            {
                Id = employee.Id,
                IsDeleted = employee.IsDeleted,
                userId = employee.AppUser_Id,
                Name = employee.ApplicationUser.UserName,
                Phone = employee.ApplicationUser?.PhoneNumber,
                Address = employee.ApplicationUser?.Address,
                Email = employee.ApplicationUser?.Email,
                RoleId = role.Id,
                Role = role.Name,
                branchId = employee.Branch_Id,
                BranchName = branch.Name
            };

            return createdEmployeeDto;
        }
        public async Task<string> DeleteAsync(int id)
        {
            try
            {
                var employeeQuery = await unitOfWork.GetRepository<Employee>().GetAllAsync();
                var employee = await employeeQuery
                    .Include(e => e.ApplicationUser)
                    .Include(e => e.Branch)
                    .Where(e => e.Id == id)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    return $"Employee with ID {id} not found";
                }

                if (employee.IsDeleted)
                {
                    return $"Employee with ID {id} is already deleted";
                }

                var roleDto = await roleCacheService.GetRoleByUserIdAsync(employee.AppUser_Id);

                var employeeName = employee.ApplicationUser?.UserName ?? "Unknown";
                var roleName = roleDto?.Name ?? "no role";
                var branchName = employee.Branch?.Name ?? "Unknown";

                if (employee.ApplicationUser != null)
                {
                    employee.ApplicationUser.IsDeleted = true;
                    await userManager.UpdateAsync(employee.ApplicationUser);
                }


                unitOfWork.GetRepository<Employee>().Delete(employee);

                await unitOfWork.SaveChangesAsync();

                return $"Employee '{employeeName}' (ID: {id}, Role: {roleName}, Branch: {branchName}) deleted successfully";
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete employee with ID {id}: {ex.Message}");
            }
        }

        //search
        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesBySearch(string term, bool includeDeleted = true)
        {

            IQueryable<Employee> query;
            if (!includeDeleted)
            {
                query = await unitOfWork.GetRepository<Employee>().GetAllExistAsync();
            }
            else
            {
                query = await unitOfWork.GetRepository<Employee>().GetAllAsync();
            }

            var employees = await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .Where(e => EF.Functions.Like(e.ApplicationUser.UserName, $"%{term}%"))
                .ToListAsync();
            //no emps
            if (!employees.Any())
            {
                return new List<EmployeeDTO>();
            }
            //
            var roleDictionary = await roleCacheService.GetRoleDictionaryAsync();
            //
            var employeeDtos = new List<EmployeeDTO>();
            foreach (var e in employees)
            {
                var roleDto = await roleCacheService.GetRoleByUserIdAsync(e.AppUser_Id);

                employeeDtos.Add(new EmployeeDTO
                {
                    Id = e.Id,
                    IsDeleted = e.IsDeleted,
                    userId = e.AppUser_Id,
                    Name = e.ApplicationUser.UserName,
                    Phone = e.ApplicationUser?.PhoneNumber,
                    Address = e.ApplicationUser?.Address,
                    Email = e.ApplicationUser?.Email,

                    RoleId = roleDto?.Id,
                    Role = roleDictionary.TryGetValue(roleDto?.Id ?? "", out var role) ? role : "no role",

                    branchId = e.Branch_Id,
                    BranchName = e.Branch.Name
                });
            }

            return employeeDtos;
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
        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByRole(string roleName)
        {

            //get useids by roles
            var userIds = (await userManager.GetUsersInRoleAsync(roleName))
                            .Select(u => u.Id)
                            .ToList();

            //filter emps by userids
            var query = await unitOfWork.GetRepository<Employee>().GetAllExistAsync();
            var employees = await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .Where(e => userIds.Contains(e.ApplicationUser.Id))
                .ToListAsync();
          
            //no emps
            if (!employees.Any())
            {
                return new List<EmployeeDTO>();
            }

            //  roleDictionary
            var roleDictionary = await roleCacheService.GetRoleDictionaryAsync();

            //  mapping
            var employeeDtos = new List<EmployeeDTO>();
            foreach (var e in employees)
            {
                //
                var roleDto = await roleCacheService.GetRoleByUserIdAsync(e.AppUser_Id);

                employeeDtos.Add(new EmployeeDTO
                {
                    Id = e.Id,
                    IsDeleted = e.IsDeleted,
                    userId = e.AppUser_Id,
                    Name = e.ApplicationUser.UserName,
                    Phone = e.ApplicationUser?.PhoneNumber,
                    Address = e.ApplicationUser?.Address,
                    Email = e.ApplicationUser?.Email,

                    RoleId = roleDto?.Id,
                    Role = roleDictionary.TryGetValue(roleDto?.Id ?? "", out var role) ? roleName : "no role",
                   
                    branchId = e.Branch_Id,
                    BranchName = e.Branch.Name
                });
            }

            return employeeDtos;
        }





        //transaction

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {

              return    await  unitOfWork.Context.Database.BeginTransactionAsync();


        }
        //




      



        



      
    }
}
