using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SHIPPING.Services;
using Microsoft.AspNetCore.Identity;
using Shipping.DTOs.DeliveryDTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Shipping.Services.ModelService
{
    public class DeliveryService : ServiceGeneric<Delivery>, IDeliveryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public DeliveryService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.userManager = userManager;
        }

 
        public async Task<IEnumerable<Government>> GetAllGovernmentExist(List<int> governorateIds)
        {
            if (governorateIds == null || !governorateIds.Any())
            {
                return Enumerable.Empty<Government>(); 
            }

            var governmentRepository = unitOfWork.GetRepository<Government>();
            var allGovernments = await governmentRepository.GetAllAsync(); // إحضار جميع المحافظات
            var existingGovernments = allGovernments.Where(g => governorateIds.Contains(g.Id)).ToList(); // تصفية القيم المطابقة

            return existingGovernments;
        }

       
        public async Task<bool> AddDeliveryAsync(DeliveryDTO deliveryDTO)
        {
            try
            {
                await unitOfWork.Context.Database.BeginTransactionAsync(); // ✅ بدء المعاملة

                var governmentRepository = unitOfWork.GetRepository<Government>();
                var branchRepository = unitOfWork.GetRepository<Branch>();
                var deliveryRepository = unitOfWork.GetRepository<Delivery>();

                // ✅ التحقق من صحة بيانات المحافظات
                var allGovernmentsIdExist = await GetAllGovernmentExist(deliveryDTO.GovernmentsId);
                var invalidIds = deliveryDTO.GovernmentsId.Except(allGovernmentsIdExist.Select(g => g.Id)).ToList();
                if (invalidIds.Any())
                {
                    throw new Exception($"Invalid government IDs: {string.Join(", ", invalidIds)}");
                }

                // ✅ التحقق من الفرع
                var branch = await branchRepository.GetByIdAsync(deliveryDTO.BranchId);
                if (branch == null)
                {
                    throw new Exception("Branch not found.");
                }

                // ✅ التحقق مما إذا كان المستخدم موجودًا بالفعل
                var existingUser = await userManager.FindByEmailAsync(deliveryDTO.Email);
                if (existingUser != null)
                {
                    throw new Exception("A user with this email already exists.");
                }

                // ✅ إنشاء مستخدم جديد
                var user = new ApplicationUser
                {
                    UserName = deliveryDTO.Name,
                    Email = deliveryDTO.Email,
                    PhoneNumber = deliveryDTO.Phone,
                    Address = deliveryDTO.Address,
                };
                // ✅ إضافة المستخدم إلى قاعدة البيانات
                //حطيت هنا password علشان يهيشهولي 
                IdentityResult Result = await userManager.CreateAsync(user, deliveryDTO.Password);

                if (!Result.Succeeded)
                {
                    throw new Exception($"Failed to create user: {string.Join(", ", Result.Errors.Select(e => e.Description))}");
                }
                await userManager.AddToRoleAsync(user, "delivery");

                // ✅ إنشاء كيان التوصيل وربطه بالمستخدم الجديد
                var delivery = new Delivery
                {
                    AppUser_Id = user.Id,
                    Branch_Id = deliveryDTO.BranchId,
                    IsDeleted =false

                };
                delivery.DeliveryGovernments.AddRange(deliveryDTO.GovernmentsId.Select(id => new DeliveryGovernment { Government_Id = id }).ToList());

                await deliveryRepository.AddAsync(delivery);
                await unitOfWork.SaveChangesAsync(); // ✅ حفظ البيانات

                await unitOfWork.Context.Database.CommitTransactionAsync(); // ✅ تأكيد المعاملة

                return true;
            }
            catch (Exception ex)
            {
                await unitOfWork.Context.Database.RollbackTransactionAsync(); // ❌ التراجع عند الخطأ
                throw new Exception($"An error occurred while adding the delivery: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveryWithGovernmentsAsync()
        {
            return await unitOfWork.Context.Set<Delivery>()
            .Include(d => d.ApplicationUser)
            .Include(d => d.Branch)
            .Include(d => d.DeliveryGovernments)
             .ThenInclude(dg => dg.Government)
            .ToListAsync();
        }

        public async Task<bool> UpdateDeliveryAsync(int deliveryId, DeliveryDTO deliveryDTO)
        {
            try
            {
                await unitOfWork.Context.Database.BeginTransactionAsync();

                var deliveryRepository = unitOfWork.GetRepository<Delivery>();
                var delivery = await deliveryRepository.GetByIdAsync(deliveryId);

                if (delivery == null)
                {
                    throw new Exception("Delivery not found.");
                }

                // 🟢 تحديث بيانات المستخدم المرتبط بالتوصيل
                if (delivery.AppUser_Id != null)
                {
                    var user = await userManager.FindByIdAsync(delivery.AppUser_Id);

                    if (user != null)
                    {
                        user.UserName = deliveryDTO.Name;
                        user.Email = deliveryDTO.Email;
                        user.PhoneNumber = deliveryDTO.Phone;
                        user.Address = deliveryDTO.Address;
                        var result = await userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            throw new Exception($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                // 🟢 تحديث بيانات التوصيل
                delivery.Branch_Id = deliveryDTO.BranchId;
                delivery.IsDeleted = false;

                // جلب جميع المحافظات
                var deliveryGovernmentRepo = unitOfWork.GetRepository<DeliveryGovernment>();

                //جلب المحافظات القديمه من deliveryGovernment 
                var oldGovernments = (await deliveryGovernmentRepo.GetAllAsync())
                    .Where(dg => dg.Delivery_Id == delivery.Id).ToList();
                var oldGovernmentIds = oldGovernments.Select(dg => dg.Government_Id).ToList();

                // 🔹 المحافظات التي يجب إضافتها (الجديدة فقط)
                var toAdd = deliveryDTO.GovernmentsId.Except(oldGovernmentIds).ToList();
                foreach (var govId in toAdd)
                {
                    await deliveryGovernmentRepo.AddAsync(new DeliveryGovernment
                    {
                        Delivery_Id = delivery.Id,
                        Government_Id = govId
                    });
                }

                // 🔹 المحافظات التي يجب حذفها (الغير موجودة في القائمة الجديدة)
                var toDelete = oldGovernments.Where(dg => !deliveryDTO.GovernmentsId.Contains(dg.Government_Id)).ToList();
                foreach (var gov in toDelete)
                {
                    deliveryGovernmentRepo.Delete(gov);
                }

                 unitOfWork.SaveChangesAsync();
                 unitOfWork.Context.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await unitOfWork.Context.Database.RollbackTransactionAsync();
                throw new Exception($"An error occurred while updating the delivery: {ex.Message}");
            }
        }

        public async Task<Delivery> GetDeliveryByIdAsync(int deliveryId)
        {
            var deliveryRepository = unitOfWork.GetRepository<Delivery>();

            var delivery = await unitOfWork.Context.Set<Delivery>()
                .Include(d => d.ApplicationUser)
                .Include(d => d.Branch)
                .Include(d => d.DeliveryGovernments)
                    .ThenInclude(dg => dg.Government)
                .FirstOrDefaultAsync(d => d.Id == deliveryId);

            return delivery;
        }



    }
}
