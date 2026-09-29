using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Interfaces
{
    public interface IPlanServices
    {
        public Task<Result<IEnumerable<PlanViewModel>>> GetAllPlansAsync(CancellationToken cancellationToken = default);
        public Task<Result<PlanViewModel>> GetPlanByIdAsync(int id , CancellationToken cancellationToken = default);
        public Task<Result<UpdateToPlanViewModel>> GetPlanToUpdateAsync(int id, CancellationToken cancellationToken);
        public Task<Result> UpdatePlanAsync(int id, UpdateToPlanViewModel model, CancellationToken cancellationToken);
        Task<Result> ToggleActivationAsync(int planId, CancellationToken ct = default);

    }
}
