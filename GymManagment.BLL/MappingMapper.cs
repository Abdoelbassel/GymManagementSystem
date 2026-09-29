using AutoMapper;
using AutoMapper.Execution;
using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.ViewModels.MemberShipViewModels;
using GymManagmentSystem.BLL.ViewModels.MemberViewModel;
using GymManagmentSystem.BLL.ViewModels.SessionViewModels;
using GymManagmentSystem.BLL.ViewModels.TrainerViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using Member = GymManagment.DAL.Models.Member;

namespace GymManagmentSystem.BLL
{
    public class MappingMapper : Profile
    {
        public MappingMapper()
        {
            MemberMap();

            TrainerMap();

            SessionMap();

            MembershipMap();

            BookingMap();
        }

        private void MemberMap()
        {
            CreateMap<GymManagment.DAL.Models.Member, GetMemberViewModels>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.Street} - {src.Address.BuildingNumber} - {src.Address.City}"))
                .ForMember(dest => dest.DataOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));

            CreateMap<GymManagment.DAL.Models.Member, UpdateToMemberViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<UpdateToMemberViewModel, GymManagment.DAL.Models.Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore());

            CreateMap<HealthRecord, HealthRecordViewModel>();


            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    Street = src.Street,
                    BuildingNumber = src.BuildingNumber,
                    City = src.City
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => new HealthRecord
                {
                    Height = src.HealthRecordViewModel.Height,
                    Weight = src.HealthRecordViewModel.Weight,
                    BloodType = src.HealthRecordViewModel.BloodType
                }));
        }
        private void TrainerMap()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(
                    dest => dest.Specialty,
                    opt => opt.MapFrom(src => src.Specialties)
                )
                .ForMember(
                    dest => dest.Address,
                    opt => opt.MapFrom(src => new Address
                    {
                        Street = src.Street,
                        City = src.City,
                        BuildingNumber = src.BuildingNumber
                    })
                );

            CreateMap<Trainer, GetTrainerViewModel>()
                .ForMember(
                    dest => dest.Address,
                    opt => opt.MapFrom(src => new Address
                    {
                        Street = src.Address.Street,
                        City = src.Address.City,
                        BuildingNumber = src.Address.BuildingNumber
                    })
                );

            CreateMap<Trainer, CreateTrainerViewModel>()
    .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialty))
    .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
    .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
    .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));
        }

        private void SessionMap()
        {
            CreateMap<Session, GetSessionsViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
                
        }

        private void MembershipMap()
        {
            CreateMap<Membership, MembershipViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.member.Name))
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.plan.Name));

            CreateMap<Member, GetMemberForDownListAsync>();
            CreateMap<PLan, GetPlanForDownListAsync>();
        }

        private void BookingMap()
        {
            CreateMap<Booking , GetSessionsViewModel>();
        }
    }
}