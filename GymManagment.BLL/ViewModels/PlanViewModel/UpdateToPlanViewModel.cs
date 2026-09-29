using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagmentSystem.BLL.ViewModels.PlanViewModel
{
    public class UpdateToPlanViewModel


{
        public string Name { get; set; }
        [Required(ErrorMessage ="Description Is Required")]
        public string Description { get; set; }
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
    }
}
