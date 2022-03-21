using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSMS.Core.ViewModels
{    
    public class AllUsersListViewModel
    {
        private string username;
        public string UserName 
        {
            get
            { return username; }
            set
            {username = value.Split("@")[0];}
        }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
