using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.ViewModels.PlanViewModel
{
    public class PlanViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int DurationInDays { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }

    }
}
