using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Common
{
    public enum ResultKind
    {
        Ok,
        NotFound,
        Conflict,
        ValidationError,
        Forbidden,
    }
}
