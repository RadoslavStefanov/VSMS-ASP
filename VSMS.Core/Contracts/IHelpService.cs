using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSMS.Core.Contracts
{
    public interface IHelpService
    {
        public Task<bool> CreateResetRequest(string userName);
    }
}
