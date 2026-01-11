using AutoMapper;
using Shipping.DTOs.Employee;
using Shipping.DTOs.MerchantDTOs;
using Shipping.DTOs.NewFolder1;
using Shipping.Models;

namespace Shipping.MapperConfig
{
    public class EmployeeConfig : Profile
    {
        public EmployeeConfig()
        {
            CreateMap<Employee, EmployeeGetDTO>().AfterMap((src, dest) =>
            {
                dest.Name = src.ApplicationUser.UserName;
                dest.Email = src.ApplicationUser.Email;
                dest.Phone = src.ApplicationUser.PhoneNumber;
                dest.Address = src.ApplicationUser.Address;
                dest.CreatedDate = src.ApplicationUser.CreatedDate?.ToString("dd MMM yyyy");
                dest.BranchName = src.Branch.Name;
            }).ReverseMap();

            CreateMap<CreateEmployeeDTO, ApplicationUser>().AfterMap((src, dest) =>
            {
                if (dest.Employee == null) dest.Employee = new Employee();

                dest.UserName = src.Name;
                dest.Email = src.Email;
                dest.PhoneNumber = src.Phone;
                dest.Address = src.Address;
                dest.Employee.Branch_Id = src.Branch_Id;
            }).ReverseMap();
        }
    }
}
