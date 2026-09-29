using AutoMapper;
using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.MemberViewModel;
using GymManagmentSystem.BLL.ViewModels.TrainerViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Classes
{
    public class TrainerServices : ITrainerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel createTrainerView, CancellationToken cancellationToken = default)
        {
            // check Email
            var emailExists = await _unitOfWork.GetRepositorities<Trainer>().AnyAsync(t => t.Email == createTrainerView.Email);

            // check Phone 
            var phoneExists = await _unitOfWork.GetRepositorities<Trainer>().AnyAsync(t => t.Phone == createTrainerView.Phone);

            if (emailExists || phoneExists) return false;

            var trainer = _mapper.Map<Trainer>(createTrainerView);

            _unitOfWork.GetRepositorities<Trainer>().AddAsync(trainer);
            var result = await _unitOfWork.SaveChangAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> DeleteTrainerAsync(int id, CancellationToken cancellationToken = default)
        {
            var trainer = await _unitOfWork.GetRepositorities<Trainer>().GetByIdAsync(id);
            if (trainer is null) return false;

            var HasfutureSession = await _unitOfWork.GetRepositorities<Session>().AnyAsync(s => s.TrainerId == id && s.StartDate > DateTime.Now);
            if (HasfutureSession) return false;

            _unitOfWork.GetRepositorities<Trainer>().RemoveAsync(trainer);
            var result = await _unitOfWork.SaveChangAsync(cancellationToken);
            return result > 0;
    
        }

        public async Task<IEnumerable<GetTrainerViewModel>> GetALlTrainerAsync(CancellationToken cancellationToken =default)
        {
            var trainer = await _unitOfWork.GetRepositorities<Trainer>().GetAllAsync(cancellation: cancellationToken);
            if (!trainer.Any()) return [];

            var trainerView = trainer.Select(t => new GetTrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialty.ToString()
                
            }).ToList();

            return trainerView;
        }

        public async Task<GetTrainerViewModel> GetTrainerDetailsById(int id, CancellationToken cancellation = default)
        {
            var trainer = await _unitOfWork.GetRepositorities<Trainer>().GetByIdAsync(id);
            if (trainer is null) return null;

            return new GetTrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DataOfBirth = trainer.DateOfBirth.ToString(),
                Address = $"{trainer.Address.City} - {trainer.Address.Street} - {trainer.Address.BuildingNumber}",
                Specialties= trainer.Specialty.ToString()
            };
        }

        public async Task<CreateTrainerViewModel> GetTrainerToUpdate(int id, CancellationToken cancellationToken = default)
        {
            var trainer = await _unitOfWork.GetRepositorities<Trainer>().GetByIdAsync(id);
            if (trainer is null) return null;

            return _mapper.Map<CreateTrainerViewModel>(trainer);
        }

        public async Task<bool> UpdateTrainerAsync(int id, CreateTrainerViewModel model, CancellationToken cancellationToken = default)
        {
            var trainer = await _unitOfWork.GetRepositorities<Trainer>().GetByIdAsync(id, cancellationToken);
            if(trainer is null) return false;
            // check phone
            var phoneExists = await _unitOfWork.GetRepositorities<Trainer>().AnyAsync(t => t.Phone == model.Phone && t.Id != id);

            // check Email
            var emailExists = await _unitOfWork.GetRepositorities<Trainer>().AnyAsync(t => t.Email == model.Email && t.Id != id);

            if (emailExists && phoneExists) return false;

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Specialty = model.Specialties;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.UpdatedAt = DateTime.Now;

           _unitOfWork.GetRepositorities<Trainer>().UpdatedAsync(trainer);
            var result = await _unitOfWork.SaveChangAsync(cancellationToken);
            return result > 0;

        }


    }
}
