using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSMS.Core.ViewModels
{
    public class EditUserViewModel
    {
        public string OldEmail { get; set; }
        public string EmailChange { get; set; }
        public string RoleChange { get; set; }
        public string PasswordChange { get; set; }
    }
}
