using GymManagmentSystem.BLL.ViewModels.AnalyticsViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Interfaces
{
    public interface IAnalyticsServices
    {
        public Task<AnalyticsViewModel> GetAnalyticsAsync(CancellationToken cancellationToken = default);
    }
}
