using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Classes
{
    public class PlanServices : IPlanServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlanServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<PlanViewModel>>> GetAllPlansAsync(CancellationToken cancellationToken)
        {
           var plans = await _unitOfWork.GetRepositorities<PLan>().GetAllAsync(cancellation: cancellationToken);
           if (plans == null)    return Result<IEnumerable<PlanViewModel>>.NotFound("No plans found");


            var result =  plans.Select(planViewModel => new PlanViewModel()
            {
                Id = planViewModel.Id,
                Name = planViewModel.Name,
                Description = planViewModel.Description,
                DurationInDays = planViewModel.DurationInDays,
                IsActive = planViewModel.IsActive,
                Price = planViewModel.Price
            });

            return Result<IEnumerable<PlanViewModel>>.Ok(result);

        }

        public async Task<Result<PlanViewModel>> GetPlanByIdAsync(int id, CancellationToken cancellationToken)
        {
            var plan = await _unitOfWork.GetRepositorities<PLan>().GetByIdAsync( id , cancellationToken);
            if (plan is null) return Result<PlanViewModel>.NotFound("No plans found");

            else
            {
                var result =  new PlanViewModel()
                {
                    Id = plan.Id,
                    Name = plan.Name,
                    Description = plan.Description,
                    DurationInDays = plan.DurationInDays,
                    Price = plan.Price,
                    IsActive = plan.IsActive,
                };

                return Result<PlanViewModel>.Ok(result);
            }
        }

        public async Task<Result<UpdateToPlanViewModel>> GetPlanToUpdateAsync(int id, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepositorities<PLan>().GetByIdAsync(id, ct);
            if(plan is null || !plan.IsActive) return Result<UpdateToPlanViewModel>.NotFound("Plan Not Found");

            var hasActiveMember = await HasActiveMembershipsAsync(id, ct);
            if (!hasActiveMember.success) return Result<UpdateToPlanViewModel>.NotFound("Plan Not Found"); 
            else 
            {
                var reult =  new UpdateToPlanViewModel()
                {
                    Name = plan.Name,
                    Description = plan.Description,
                    DurationDays = plan.DurationInDays,
                    Price = plan.Price
                };
                return Result<UpdateToPlanViewModel>.Ok(reult);
             }
        }

        public async Task<Result> UpdatePlanAsync(int id, UpdateToPlanViewModel model, CancellationToken cancellationToken)
        {
            var plan = await _unitOfWork.GetRepositorities<PLan>().GetByIdAsync(id, cancellationToken);

            if (plan is null) return Result.NotFound("Plan Not Found");

            //var activeMembership = await _unitOfWork.GetRepositorities<Membership>().AnyAsync(m => m.Id == id && m.EndDate > DateTime.Now);

            var hasActiveMember = await HasActiveMembershipsAsync(id , cancellationToken);
            if (!hasActiveMember.success) 
                return  Result.ValidationError(hasActiveMember.error ?? "Cannot update plan with active memberships");
            ;

            plan.Name = model.Name;
            plan.Price = model.Price;
            plan.Description = model.Description;
            plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepositorities<PLan>().UpdatedAsync(plan, cancellationToken);
            var result = await _unitOfWork.SaveChangAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.ValidationError("Failed To Updated");
        }

        public async Task<Result> HasActiveMembershipsAsync(int planId, CancellationToken ct = default)
        {
            var hasActive = await _unitOfWork.GetRepositorities<Membership>().AnyAsync(x => x.PlanId == planId && x.EndDate > DateTime.Now, ct);

            return hasActive
                ? Result.Fail("Plan has active memberships")
                : Result.Ok();
        }


        public async Task<Result> ToggleActivationAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepositorities<PLan>().GetByIdAsync(planId, ct);
            if (plan is null)
                return Result.NotFound("Plan Not Found");

            if (plan.IsActive)
            {
                var hasActiveResult = await HasActiveMembershipsAsync(planId, ct);
                if (!hasActiveResult.success)
                    return Result.ValidationError(hasActiveResult.error ?? "Cannot deactivate plan with active memberships");
            }

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepositorities<PLan>().UpdatedAsync(plan);
            var result = await _unitOfWork.SaveChangAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to toggle plan activation");
        }
    }
}
