using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Interfaces
{
    public interface ISessionServices
    {
        public Task<Result<IEnumerable<GetSessionsViewModel>>> GetSessionsAsync(CancellationToken ct = default);
        public Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);

        public Task<Result<IEnumerable<CategorySelectViewModel>>> GetCategoriesFroDropDownList(CancellationToken ct = default);
        public Task<Result<IEnumerable<TrainerSelectViewModel>>> GetTrainersFroDropDownList(CancellationToken ct = default);

        public Task<Result<GetSessionsViewModel>> GetSessionById(int id, CancellationToken ct = default);

        public Task<Result> UpdateToSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default);
        
        public Task<Result<UpdateSessionViewModel>> GetToUpdateSessionAsync(int id, CancellationToken ct = default);
        
        public Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);

    }
    
}
