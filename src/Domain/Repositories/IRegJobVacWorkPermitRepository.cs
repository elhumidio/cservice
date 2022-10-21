using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IRegJobVacWorkPermitRepository
    {
        public Task<int> Add(RegJobVacWorkPermit permit);
        public Task<int> Delete(int jobId);
    }
}
