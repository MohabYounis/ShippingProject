using Microsoft.EntityFrameworkCore;
using Shipping.Enums;
using Shipping.Models;
using Shipping.Repository.ImodelRepository;
using Shipping.Services.IModelService;
using Shipping.UnitOfWorks;
using SHIPPING.Services;

namespace Shipping.Services.ModelService
{
    public class RolePermissionService: ServiceGeneric<RolePermission>,IRolePermissionService
    {
        IRolePermissinRepository rolePermissinRepository;
        public RolePermissionService(IUnitOfWork unitOfWork , IRolePermissinRepository rolePermissinRepository) :base(unitOfWork)
        {
            this.rolePermissinRepository = rolePermissinRepository;
        }

        //overide get all method

        public override async Task<IEnumerable<RolePermission>> GetAllAsync()
        {
            var query = await rolePermissinRepository.GetAllAsync();

            return query
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .ToList();


        }
        // overide get all exist method
        public override async Task<IEnumerable<RolePermission>> GetAllExistAsync()
        {
            var query = await rolePermissinRepository.GetAllExistAsync();
            return query
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .ToList();
        }

    




        //get one row by ids
        public  async Task<RolePermission> GetRolePermissin(string role_id, int permission_id)
        {

            return await rolePermissinRepository.GetRolePermissin(role_id, permission_id);
            
        }



        //add role permission
        public async Task<AddResult> AddRolePermission(RolePermission rolePermission)
        {
          return  await rolePermissinRepository.AddRolePermissin(rolePermission);
        }


        //update role permission


        public async Task<UpdateREsult> UpdateRolePermissin(RolePermission rolePermission)
        {
            return await rolePermissinRepository.UpdateRolePermissin(rolePermission);
        }


        //delete by ids

        public async Task<DeleteResult> DeleteRolePermissin(string role_id, int permission_id)
        {
            return await rolePermissinRepository.DeleteRolePermissin(role_id, permission_id);
        }




    }
}
