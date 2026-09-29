using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.ViewModels.TrainerViewModel
{
    public class GetTrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Photo { get; set; }
        public string Email { get; set; } = default!;
        public string Gender { get; set; } = default!;

        public string? DataOfBirth { get; set; } = default!;

        public string Specialties { get; set; } = default!;
        public string? Address { get; set; } = default!;

    }
}
