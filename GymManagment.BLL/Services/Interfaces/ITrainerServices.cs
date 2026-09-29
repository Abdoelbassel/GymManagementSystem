using GymManagmentSystem.BLL.ViewModels.TrainerViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Interfaces
{
    public interface ITrainerServices
    {
        public Task<IEnumerable<GetTrainerViewModel>> GetALlTrainerAsync(CancellationToken cancellationToken = default);
        public Task<bool> CreateTrainerAsync(CreateTrainerViewModel model ,CancellationToken cancellationToken = default);
        public Task<GetTrainerViewModel> GetTrainerDetailsById(int id, CancellationToken cancellation = default);
        public Task<CreateTrainerViewModel> GetTrainerToUpdate (int id , CancellationToken cancellationToken = default);
        public Task<bool> UpdateTrainerAsync(int id,CreateTrainerViewModel model , CancellationToken cancellationToken = default);
        public Task<bool> DeleteTrainerAsync(int id , CancellationToken cancellationToken = default);


         

        
    }
}
