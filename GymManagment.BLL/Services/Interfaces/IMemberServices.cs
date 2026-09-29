using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.ViewModels.MemberViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Interfaces
{
    public interface IMemberServices
    {
        public Task<Result<IEnumerable<GetMemberViewModels>>> GetMembersAsync(CancellationToken cancellationToken = default);
        public Task<Result> CreateMemberAsync(CreateMemberViewModel createMemberViewModel ,CancellationToken cancellationToken = default);
        public Task<Result<GetMemberViewModels>> GetMemberDetailsById(int id, CancellationToken ct);
        public Task<Result<HealthRecordViewModel>> GetHealthRecoerd(int id , CancellationToken cancellationToken = default);
        public Task<Result<UpdateToMemberViewModel>> GetToMemberToUpdate (int id , CancellationToken cancellationToken = default);
        public Task<Result> UpdateToMemberDetails(int id, UpdateToMemberViewModel model, CancellationToken cancellationToken);
        public Task<Result> DeleteMemberAsync(int id , CancellationToken cancellationToken = default);

    }
}
