using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSMS.Core.Contracts
{
    public interface IProductsService
    {
        public void Delete(string arg);
        public void Create();
    }
}
