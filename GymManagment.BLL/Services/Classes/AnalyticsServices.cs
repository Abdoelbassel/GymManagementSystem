using AutoMapper.Configuration.Annotations;
using AutoMapper.Execution;
using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.AnalyticsViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Member = GymManagment.DAL.Models.Member;

namespace GymManagmentSystem.BLL.Services.Classes
{
    public class AnalyticsServices : IAnalyticsServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AnalyticsViewModel> GetAnalyticsAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepositorities<Member>().CountAsync();
            var trsiners = await _unitOfWork.GetRepositorities<Trainer>().CountAsync();
            var CompletedSessions = await _unitOfWork.GetRepositorities<Session>().CountAsync(s => s.EndDate < DateTime.Now);
            var OnGoingSession = await _unitOfWork.GetRepositorities<Session>().CountAsync(so => so.StartDate <= DateTime.Now && so.EndDate > DateTime.Now);
            var UpcommingSession = await _unitOfWork.GetRepositorities<Session>().CountAsync(su => su.StartDate > DateTime.Now);
            var memberShips = await _unitOfWork.GetRepositorities<Membership>().CountAsync(ms => ms.EndDate > DateTime.Now);


            return new AnalyticsViewModel
            {
                TotalMembers = members,
                ActiveMembers = memberShips,
                TotalTrainers = trsiners,
                CompletedSessions = CompletedSessions,
                UpcomingSessions = UpcommingSession,
                OngoingSessions = OnGoingSession
            };
        }
    }
}
