using AutoMapper;
using Shipping.DTOs;
using Shipping.Models;

namespace Shipping.MapperConfig
{
    public class RoleConfig : Profile
    {
        public RoleConfig()
        {
            CreateMap<ApplicationRole, ApplicationRoleDTO>().AfterMap((src, dest) =>
            {
                dest.Id = src.Id;
                dest.Name = src.Name;
                dest.NormalizedName = src.NormalizedName;
                dest.IsDeleted = src.IsDeleted;
            }).ReverseMap();

            CreateMap<ApplicationRoleDTO, ApplicationRole>().AfterMap((src, dest) =>
            {
                dest.Id = src.Id;
                dest.Name = src.Name;
                dest.NormalizedName = src.NormalizedName;
                dest.IsDeleted = src.IsDeleted;
            }).ReverseMap();

        }
    }
}
