using AutoMapper;
using GymManagment.DAL.Data.Configurtions;
using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.Services.Attachment;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.MemberViewModel;
using GymManagmentSystem.BLL.ViewModels.SessionViewModels;
using GymManagmentSystem.DAL.Repositorities.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork _unitOfWrork;
        private readonly IMapper _mapper;
        private readonly IAttachmentServices _attachmentServices;

        public MemberServices(IUnitOfWork unitOfWrork, IMapper mapper , IAttachmentServices attachmentServices)
        { 
            _unitOfWrork = unitOfWrork;
            _mapper = mapper;
            _attachmentServices = attachmentServices;
        }

        public async Task<Result> CreateMemberAsync(
            CreateMemberViewModel createMemberViewModel,
            CancellationToken cancellationToken = default)
        {
            // Check Email
            var emailExists =
                await _unitOfWrork
                    .GetRepositorities<Member>()
                    .AnyAsync(
                        e => e.Email == createMemberViewModel.Email,
                        cancellationToken);

            // Check Phone
            var phoneExists =
                await _unitOfWrork
                    .GetRepositorities<Member>()
                    .AnyAsync(
                        p => p.Phone == createMemberViewModel.Phone,
                        cancellationToken);

            if (phoneExists || emailExists)
                return Result.ValidationError("Email Or Phone Is Exists Already");

            // Upload Photo
            var storedFile = await _attachmentServices.UploadAsync(
                createMemberViewModel.PhotoFile.OpenReadStream(),
                createMemberViewModel.PhotoFile.FileName,
                "MembersPhoto",
                cancellationToken);

            if (!storedFile.success)
                return Result.Fail(storedFile.error ?? "Failed To Upload");

            // Map Member
            var member = _mapper.Map<Member>(createMemberViewModel);

            member.Photo = storedFile.value;

            // Add
            _unitOfWrork
                .GetRepositorities<Member>()
                .AddAsync(member);

            // Save
            var result = await _unitOfWrork.SaveChangAsync(cancellationToken);

            if (result > 0)
                return Result.Ok();

            // If database failed
            _attachmentServices.DeleteAsync(
                storedFile.value,
                "MembersPhoto",
                cancellationToken);

            return Result.Fail("Failed To Create Member");
        }
        public async Task<Result<IEnumerable<GetMemberViewModels>>> GetMembersAsync(CancellationToken cancellationToken = default)
        {
            var members = await _unitOfWrork.GetRepositorities<Member>().GetAllAsync();

            if (!members.Any()) return Result<IEnumerable<GetMemberViewModels>>.NotFound("No Members Found");

            var memberView = _mapper.Map<IEnumerable<Member>, IEnumerable<GetMemberViewModels>>(members);
            //foreach (var member in memberView)
            //{
            //    member.Photo = _attachmentServices.GetFile(File)
            //}
            return Result<IEnumerable<GetMemberViewModels>>.Ok(memberView);
        }
    

        public async Task<Result<GetMemberViewModels>> GetMemberDetailsById(int MemberId , CancellationToken ct)
        {
            var member = await _unitOfWrork.GetRepositorities<Member>().GetByIdAsync(MemberId);
            if (member == null) return Result<GetMemberViewModels>.NotFound("Member Not Found");

            var model = _mapper.Map<Member, GetMemberViewModels>(member);

            var activeMembership = await _unitOfWrork.GetRepositorities<Membership>().FirstOrDefaultAsync(m => m.Id == MemberId && m.EndDate > DateTime.Now);

            if(activeMembership is not null)
            {
                var activePlan = await _unitOfWrork.GetRepositorities<PLan>().GetByIdAsync(activeMembership.Id);
                model.PlanName = activePlan.Name;
                model.MemberShipStartDate = activeMembership.CreatedAt.ToString();
                model.MemberShipEndDate = activeMembership.ToString();
            }

            return Result<GetMemberViewModels>.Ok(model);

        }

        public async Task<Result<HealthRecordViewModel>> GetHealthRecoerd(int id, CancellationToken cancellationToken = default)
        {
            var healthReacoerd = await _unitOfWrork.GetRepositorities<HealthRecord>().FirstOrDefaultAsync(h => id == h.Id);
            if (healthReacoerd == null) return Result<HealthRecordViewModel>.NotFound();
            var helthrescordMap = _mapper.Map<HealthRecord, HealthRecordViewModel>(healthReacoerd);

            return Result<HealthRecordViewModel>.Ok(helthrescordMap);
        }

        public async Task<Result<UpdateToMemberViewModel>> GetToMemberToUpdate(int id, CancellationToken cancellationToken = default)
        {
            var Member = await _unitOfWrork.GetRepositorities<Member>().GetByIdAsync(id);
            if (Member is null) return Result<UpdateToMemberViewModel>.NotFound();
            else
            {
                var memberMap = _mapper.Map<Member, UpdateToMemberViewModel>(Member);
                return Result<UpdateToMemberViewModel>.Ok(memberMap);
            }
        }

        public async Task<Result> UpdateToMemberDetails(int id, UpdateToMemberViewModel model, CancellationToken cancellationToken)
        {
            var member = await _unitOfWrork.GetRepositorities<Member>().GetByIdAsync(id);
            if (member is null) return Result.NotFound();

            //check email
            var emailExists = await _unitOfWrork.GetRepositorities<Member>().AnyAsync(e => e.Email == model.Email, cancellationToken);

            //checked PhoneNumber
            var PhoneExists = await _unitOfWrork.GetRepositorities<Member>().AnyAsync(p => p.Phone == model.Phone, cancellationToken);

            if (emailExists && PhoneExists) return Result.NotFound("Phone Or Email Is Exists ALready");

          
            _mapper.Map(model, member);

            _unitOfWrork.GetRepositorities<Member>().UpdatedAsync(member);
            var result = await _unitOfWrork.SaveChangAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Updated Member");


        }

        public async Task<Result> DeleteMemberAsync(int id, CancellationToken cancellationToken = default)
        {
            var member = await _unitOfWrork.GetRepositorities<Member>().GetByIdAsync(id, cancellationToken);
            if (member is null) return Result.NotFound();

            var hasFutureBooking = await _unitOfWrork.GetRepositorities<Booking>().AnyAsync(b => b.Id == id && b.Session.StartDate > DateTime.Now);
            if (hasFutureBooking) return Result.NotFound();

             _unitOfWrork.GetRepositorities<Member>().RemoveAsync(member);
            var result =await _unitOfWrork.SaveChangAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Deleted Member");
        }
    }
}
