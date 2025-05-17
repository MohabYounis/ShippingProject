using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Identity;
using Shipping.DTOs.Employee;
using AutoMapper;
using Shipping.DTOs;
using static Dapper.SqlMapper;

namespace Shipping.Services.ModelService
{
    public class EmployeeService : ServiceGeneric<Employee>, IEmployeeService
    {
        UserManager<ApplicationUser> userManager;
        RoleManager<ApplicationRole> roleManager;
        IServiceGeneric<Employee> serviceGeneric;
        IUnitOfWork unitOfWork;
        IMapper mapper;
        public EmployeeService(IUnitOfWork unitOfWork, IServiceGeneric<Employee> serviceGeneric, UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager, IMapper mapper) : base(unitOfWork)
        {
            this.userManager= userManager;
            this.serviceGeneric = serviceGeneric;
            this.unitOfWork = unitOfWork;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public new async Task<IEnumerable<EmployeeGetDTO>> GetAllAsync()
        {
            var query = await unitOfWork.GetRepository<Employee>().GetAllAsync();
            var employees = query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToList();

            var employeesDTO = mapper.Map<List<EmployeeGetDTO>>(employees);

            foreach(var employee in employeesDTO)
            {
                var user = await userManager.FindByEmailAsync(employee.Email);
                var roleNames = await userManager.GetRolesAsync(user);

                var rolesDict = new Dictionary<string, string>();

                foreach (var roleName in roleNames)
                {
                    var role = await roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        rolesDict[role.Id] = role.Name;
                    }
                }

                employee.Roles = rolesDict;
            }

            return employeesDTO;
        }
        

        public new async Task<IEnumerable<EmployeeGetDTO>> GetAllExistAsync()
        {
            var query = await unitOfWork.GetRepository<Employee>().GetAllExistAsync();
            var employees =  query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .ToList();

            var employeesDTO = mapper.Map<List<EmployeeGetDTO>>(employees);

            foreach (var employee in employeesDTO)
            {
                var user = await userManager.FindByEmailAsync(employee.Email);
                var roleNames = await userManager.GetRolesAsync(user);

                var rolesDict = new Dictionary<string, string>();

                foreach (var roleName in roleNames)
                {
                    var role = await roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        rolesDict[role.Id] = role.Name;
                    }
                }

                employee.Roles = rolesDict;
            }

            return employeesDTO;
        }


        public async Task<EmployeeGetDTO> GetByIdAsync(int id)
        {

            var query = await unitOfWork.GetRepository<Employee>().GetAllExistAsync();
            var employee = await query
                .Include(e => e.ApplicationUser)
                .Include(e => e.Branch)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) throw new Exception("Not Found!.");

            var employeeDTO = mapper.Map<EmployeeGetDTO>(employee);
            
            var user = await userManager.FindByEmailAsync(employeeDTO.Email);
            var roleNames = await userManager.GetRolesAsync(user);

            var rolesDict = new Dictionary<string, string>();
            foreach (var roleName in roleNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    rolesDict[role.Id] = role.Name;
                }
            }

            employeeDTO.Roles = rolesDict;

            return employeeDTO;
        }


        public async Task AddAsync(CreateEmployeeDTO employeeFromReq)
        {
            var newUser = mapper.Map<ApplicationUser>(employeeFromReq);
            var result = await userManager.CreateAsync(newUser, employeeFromReq.Password);

            if (!result.Succeeded) throw new Exception("Failed to create Employee!");

            foreach (var roleId in employeeFromReq.Roles_Id)
            {
                var role = await roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    throw new Exception($"Role with ID '{roleId}' does not exist.");
                }
                else
                {
                    await userManager.AddToRoleAsync(newUser, role.Name);
                }
            }
        }


        public async Task UpdateAsync(int id, UpdateEmployeeDTO employeeFromReq)
        {
            var employee = await unitOfWork.GetRepository<Employee>().GetByIdAsync(id);
            if (employee == null) throw new Exception("Not Found!");

            employee.ApplicationUser ??= new ApplicationUser();
            employee.ApplicationUser.UserName = employeeFromReq.Name;
            employee.ApplicationUser.Email = employeeFromReq.Email;
            employee.ApplicationUser.PhoneNumber = employeeFromReq.Phone;
            employee.ApplicationUser.Address = employeeFromReq.Address;
            employee.Branch_Id = employeeFromReq.Branch_Id;
            employee.IsDeleted = employeeFromReq.IsDeleted;

            foreach (var roleId in employeeFromReq.Roles_Id)
            {
                var role = await roleManager.FindByIdAsync(roleId);
                var isRoleExist = await roleManager.RoleExistsAsync(role.Name);
                if (!isRoleExist)
                {
                    throw new Exception($"Role with ID '{roleId}' does not exist.");
                }
                else
                {
                    var employeeExistRoles = await userManager.GetRolesAsync(employee.ApplicationUser);
                    if (!employeeExistRoles.Contains(role.Name))
                        await userManager.AddToRoleAsync(employee.ApplicationUser, role.Name);
                }
            }

            if (!string.IsNullOrWhiteSpace(employeeFromReq.CurrentPassword) &&
                !string.IsNullOrWhiteSpace(employeeFromReq.NewPassword))
            {
                var user = await userManager.FindByIdAsync(employee.ApplicationUser.Id.ToString());

                if (user != null)
                {
                    var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, employeeFromReq.CurrentPassword);
                    if (!isCurrentPasswordValid) throw new Exception("Current password is incorrect.");

                    var passwordResult = await userManager.ChangePasswordAsync(user, employeeFromReq.CurrentPassword, employeeFromReq.NewPassword);
                    if (!passwordResult.Succeeded)
                    {
                        string errors = string.Join("; ", passwordResult.Errors.Select(e => e.Description));
                        throw new Exception(errors);
                    }
                }
            }

            await serviceGeneric.UpdateAsync(employee);
            await unitOfWork.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            await serviceGeneric.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }


        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return    await  unitOfWork.Context.Database.BeginTransactionAsync();
        }
        




      



        



      
    }
}
