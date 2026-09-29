using AutoMapper;
using GymManagment.DAL.Models;
using GymManagment.DAL.Models.Enums;
using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Classes
{
    public class SessionServices : ISessionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.ValidationError("EndDate Must Be After StartDate");
            if (model.StartDate <= DateTime.Now) return Result.ValidationError("StartDate Must Be In The Future");
            if (model.Capacity < 1 || model.Capacity > 25) return Result.ValidationError("Capacity Must Be Between 1 and 25");

            var trainer = await _unitOfWork.GetRepositorities<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer is null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepositorities<Category>().GetByIdAsync(model.CategoryId);
            if (category is null) return Result.NotFound("Category Not Found");

            var isValid = Enum.TryParse<Specialties>(category.CategoryName, true, out var CategorySpecialty);
            if (!isValid || trainer.Specialty != CategorySpecialty) return Result.ValidationError("Can Not Create This Session To This Trainer");

            var session = _mapper.Map<CreateSessionViewModel, Session>(model);

            _unitOfWork.GetRepositorities<Session>().AddAsync(session);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create Session");
        }


        public async Task<Result<IEnumerable<GetSessionsViewModel>>> GetSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork._SessionRepo.GetAllSessionsAsync();

            if (sessions is null || !sessions.Any()) return Result<IEnumerable<GetSessionsViewModel>>.NotFound("No Sessions Found");

            var mappedSessions = sessions.Select(s => new GetSessionsViewModel
            {
                Id = s.Id,
                Capacity = s.Capacity,
                CategoryName = s.Category.CategoryName,
                TrainerName = s.Trainer.Name,
                Description = s.Description,
                EndDate = s.EndDate,
                StartDate = s.StartDate,
                
            });
            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork._SessionRepo.GetCountOfBookedSlotsAsync(session.Id);
            }
            return Result<IEnumerable<GetSessionsViewModel>>.Ok(mappedSessions);

        }


        public async Task<Result<IEnumerable<CategorySelectViewModel>>> GetCategoriesFroDropDownList(CancellationToken ct = default)
        {
            var Categories = await _unitOfWork.GetRepositorities<Category>().GetAllAsync();
            var mappedCategories = _mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(Categories);

            return Result<IEnumerable<CategorySelectViewModel>>.Ok(mappedCategories);
        }
        public async Task<Result<IEnumerable<TrainerSelectViewModel>>> GetTrainersFroDropDownList(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepositorities<Trainer>().GetAllAsync();
            var mappadTrainers = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(trainers);

            return Result<IEnumerable<TrainerSelectViewModel>>.Ok(mappadTrainers);
        }

        public async Task<Result<GetSessionsViewModel>> GetSessionById(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork._SessionRepo.GetSessionByIdAsync(id, ct);
            if (session is null) return Result<GetSessionsViewModel>.NotFound("Session Not Found");

            var sessionMappped = _mapper.Map<GetSessionsViewModel>(session);
            sessionMappped.AvailableSlots = session.Capacity - await _unitOfWork._SessionRepo.GetCountOfBookedSlotsAsync(session.Id, ct);

            return Result<GetSessionsViewModel>.Ok(sessionMappped);
        }

        public async Task<Result> UpdateToSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork._SessionRepo.GetSessionByIdAsync(id, ct);

            if (session == null)
                return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.ValidationError("Invalid Start Date");

            if(model.EndDate <= DateTime.Now)
                return Result.ValidationError("Invalid End Date");

            var bookedSlots = await _unitOfWork._SessionRepo.GetCountOfBookedSlotsAsync(id, ct);
            if (bookedSlots > 0)
                return Result.Fail("Session Is Fully Booked", ResultKind.Conflict);

            if(model.StartDate <= DateTime.Now)
                return Result.ValidationError("Invalid Start Date");

            var trainer = session.Trainer;
            if(trainer == null)
                return Result.NotFound("Trainer Not Found");

            var category = session.Category;
            if (category == null)
                return Result.NotFound("Category Not Found");

            Enum.TryParse<Specialties>(category.CategoryName, true, out var CategorySpecialty);
            if(trainer.Specialty != CategorySpecialty)
                return Result.ValidationError("Trainer Specialty Does Not Match Session Category");

            _mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;

            _unitOfWork._SessionRepo.UpdatedAsync(session, ct);
            var resylt = await _unitOfWork.SaveChangAsync(ct);

            return resylt > 0 ? Result.Ok() : Result.Fail("Failed To Update Session");

        }

        public async Task<Result<UpdateSessionViewModel>> GetToUpdateSessionAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork._SessionRepo.GetSessionByIdAsync(id, ct);
            if (session is null) return Result<UpdateSessionViewModel>.NotFound("Session Not Found");

            if(session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.ValidationError("Cannot Update Session That Has Already Started");

            var bookedSlots = await _unitOfWork._SessionRepo.GetCountOfBookedSlotsAsync(id, ct);
            if(bookedSlots > 0)
                return Result<UpdateSessionViewModel>.Fail("Cannot Update Session That Has Booked Slots", ResultKind.Conflict);
        
            var sessionMappped = _mapper.Map<UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.Ok(sessionMappped);
        }

        public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork._SessionRepo.GetSessionByIdAsync(id, ct);
            if(session == null) return Result.NotFound("Session Not Found");

            if(session.EndDate <= DateTime.Now) return Result.ValidationError("Cannot Delete Session That Has Already Ended");

            var bookedSlots = await _unitOfWork._SessionRepo.GetCountOfBookedSlotsAsync(id, ct);

            if(bookedSlots > 0) return Result.Fail("Cannot Delete Session That Has Booked Slots", ResultKind.Conflict);

            _unitOfWork._SessionRepo.RemoveAsync(session, ct);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Delete Session");
        }

        
    }
}
