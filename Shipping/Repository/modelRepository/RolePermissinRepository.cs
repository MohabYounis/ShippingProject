using Microsoft.EntityFrameworkCore;
using Shipping.Enums;
using Shipping.Models;
using Shipping.Repository.ImodelRepository;
using Shipping.UnitOfWorks;

namespace Shipping.Repository.modelRepository
{
    public class RolePermissinRepository : RepositoryGeneric<RolePermission>, IRolePermissinRepository
    {
        public RolePermissinRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }



        //get one row by ids
        public async Task<RolePermission> GetRolePermissin(string role_id, int permission_id)
        {
           return await   unitOfWork.Context.Set<RolePermission>().SingleOrDefaultAsync(rp => rp.Role_Id == role_id && rp.Permission_Id == permission_id);
        }


        //add role permission
        public async Task<AddResult> AddRolePermissin(RolePermission rolePermission)
        {
            //check not exist
            var roleExists = await unitOfWork.Context.Set<ApplicationRole>().AnyAsync(r => r.Id == rolePermission.Role_Id);
            var permissionExists = await unitOfWork.Context.Set<Permission>().AnyAsync(p => p.Id == rolePermission.Permission_Id);

            if (!roleExists || !permissionExists)
                return AddResult.NotFound;

            // check if exist
            var existingRolePermission = await unitOfWork.Context.Set<RolePermission>()
                .FirstOrDefaultAsync(rp => rp.Role_Id == rolePermission.Role_Id && rp.Permission_Id == rolePermission.Permission_Id);

            if (existingRolePermission != null)
                return AddResult.AlreadyExists;

            //  add 
            await unitOfWork.Context.Set<RolePermission>().AddAsync(rolePermission);
            return AddResult.AddedSuccessfully; 

        }



        //update

        public async Task<UpdateREsult> UpdateRolePermissin(RolePermission rolePermission)
        {
            var existingRolePermission = await unitOfWork.Context.Set<RolePermission>()
         .FirstOrDefaultAsync(rp => rp.Role_Id == rolePermission.Role_Id && rp.Permission_Id == rolePermission.Permission_Id);

            //check not exist

            if (existingRolePermission == null)
                return UpdateREsult.NotFound;
            //check if deleted
            if (existingRolePermission.IsDeleted)
                return UpdateREsult.AlreadyDeleted;
      
            unitOfWork.Context.Update(rolePermission);
            return UpdateREsult.UpdatedSuccessfully;
        }





        //delte

        public async Task<DeleteResult> DeleteRolePermissin(string role_id, int permission_id)
        {
            var rolePermission = await GetRolePermissin(role_id, permission_id);
            if (rolePermission == null) return DeleteResult.NotFound;
            if( rolePermission.IsDeleted) return DeleteResult.AlreadyDeleted;
            //delte and return success
            else
            {
                rolePermission.IsDeleted = true;
                unitOfWork.Context.Update(rolePermission);
                return DeleteResult.DeletedSuccessfully;
            }


        }



    }
}
