using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace GymManagmentSystem.BLL.ViewModels.SessionViewModels
{
    public class CategorySelectViewModel
    {
         public int Id { get; set; }
         public string CategoryName { get; set; } = default!;
    }
}
