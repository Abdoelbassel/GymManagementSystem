using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.ViewModels.MemberShipViewModels;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Interfaces
{
    public interface IMembershipServices
    {
        public Task<IEnumerable<MembershipViewModel>> GetMembershipsAsync(CancellationToken ct = default);
        public Task <IEnumerable<GetPlanForDownListAsync>> GetPlanForDownListAsync(CancellationToken ct = default);
        public Task <IEnumerable<GetMemberForDownListAsync>> GetMemberForDownListAsync(CancellationToken ct = default);
        public Task <Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default);
        public Task<Result> DeleteMembershipAsync(int id , CancellationToken ct = default);
    }
}
