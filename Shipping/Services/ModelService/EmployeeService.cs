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

            var query = await unitOfWork.GetRepository<Employee>().GetAllAsync();
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
            if (employee == null) throw new Exception("Employee not found.");

            var user = await userManager.FindByIdAsync(employee.AppUser_Id.ToString());
            if (user == null) throw new Exception("User not found.");

            // ✅ 1. Check for unique email
            var existingUserWithEmail = await userManager.FindByEmailAsync(employeeFromReq.Email);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != user.Id)
                throw new Exception("Email already exists.");

            // ✅ 2. Check for unique username
            var existingUserWithName = await userManager.FindByNameAsync(employeeFromReq.Name);
            if (existingUserWithName != null && existingUserWithName.Id != user.Id)
                throw new Exception("Username already exists.");

            // ✅ 3. Update basic fields
            user.UserName = employeeFromReq.Name;
            user.Email = employeeFromReq.Email;
            user.PhoneNumber = employeeFromReq.Phone;
            user.Address = employeeFromReq.Address;

            employee.Branch_Id = employeeFromReq.Branch_Id;
            employee.IsDeleted = employeeFromReq.IsDeleted;

            // ✅ 4. Update roles (remove missing, add new)
            var currentRoles = await userManager.GetRolesAsync(user);
            var requestedRoles = new List<string>();

            foreach (var roleId in employeeFromReq.Roles_Id)
            {
                var role = await roleManager.FindByIdAsync(roleId);
                if (role == null)
                    throw new Exception($"Role with ID '{roleId}' does not exist.");

                requestedRoles.Add(role.Name);
            }

            var rolesToAdd = requestedRoles.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(requestedRoles);

            if (rolesToAdd.Any())
                await userManager.AddToRolesAsync(user, rolesToAdd);

            if (rolesToRemove.Any())
                await userManager.RemoveFromRolesAsync(user, rolesToRemove);

            // ✅ 5. Change password if requested
            if (!string.IsNullOrWhiteSpace(employeeFromReq.CurrentPassword) &&
                !string.IsNullOrWhiteSpace(employeeFromReq.NewPassword))
            {
                var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, employeeFromReq.CurrentPassword);
                if (!isCurrentPasswordValid) throw new Exception("Current password is incorrect.");

                var passwordResult = await userManager.ChangePasswordAsync(user, employeeFromReq.CurrentPassword, employeeFromReq.NewPassword);
                if (!passwordResult.Succeeded)
                    throw new Exception(string.Join("; ", passwordResult.Errors.Select(e => e.Description)));
            }

            // ✅ 6. Save all changes
            await userManager.UpdateAsync(user);
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
