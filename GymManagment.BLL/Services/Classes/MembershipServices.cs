using AutoMapper;
using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.MemberShipViewModels;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Classes
{
    public class MembershipServices : IMembershipServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetMemberForDownListAsync>> GetMemberForDownListAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepositorities<Member>().GetAllAsync();
            return _mapper.Map<IEnumerable<GetMemberForDownListAsync>>(members);
        }
        public async Task<IEnumerable<GetPlanForDownListAsync>> GetPlanForDownListAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepositorities<PLan>().GetAllAsync();
            return _mapper.Map<IEnumerable<GetPlanForDownListAsync>>(plans);
        }
        public async Task<IEnumerable<MembershipViewModel>> GetMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await _unitOfWork._MembershipRepo.GetAllMembershipsAsync(m => m.EndDate > DateTime.Now);
            return _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);
        }

        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            var memberExists = await _unitOfWork.GetRepositorities<Member>().AnyAsync(m => m.Id == model.MemberId);
            if(!memberExists) return Result.Fail("Member not found", ResultKind.NotFound);

            var plans = await _unitOfWork.GetRepositorities<PLan>().GetByIdAsync(model.PlanId);
            if (plans is null) return Result.NotFound("Plan not found");

            var membership = await _unitOfWork._MembershipRepo.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate >= DateTime.Now);
            if(membership) return Result.Fail("Member already has an active membership", ResultKind.Conflict);

            var entity = new Membership
            {
                MemberId = model.MemberId,
                PlanId = model.PlanId,
                CreatedAt = DateTime.Now,
                EndDate = (model.StartDate ?? DateTime.Now).AddDays(plans.DurationInDays)
            };
            
            _unitOfWork._MembershipRepo.AddAsync(entity);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to create membership", ResultKind.ValidationError);
        }

        public async Task<Result> DeleteMembershipAsync(int id, CancellationToken ct = default)
        {
            var membership =await _unitOfWork._MembershipRepo.FirstOrDefaultAsync(m => m.MemberId == id && m.EndDate > DateTime.Now, true, ct);
            if(membership is null) return Result.NotFound("Membership not found");

            _unitOfWork._MembershipRepo.RemoveAsync(membership);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to delete membership", ResultKind.ValidationError);
        }
    }
}
