using Domain.Repositories;

namespace Persistence.Repositories
{
    public class ContractProductRepository : IContractProductRepository
    {
        private readonly DataContext _dataContext;

        public ContractProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsPack(int contractId)
        {
            var res = _dataContext.ContractProducts
            .Join(_dataContext.Products, p => new { p.Idproduct }, cp => new { cp.Idproduct },
                (p, cp) => new { p, cp })
            .Join(_dataContext.ProductLines, ppl => ppl.p.Idproduct, pl => pl.Idproduct, (ppl, pl) => new { ppl, pl })
            .Where(o => o.ppl.p.Idcontract == contractId && o.pl.IdjobVacType != null)
            .Select(o => o.ppl.cp.ChkPack)
            .First();
            return res;
        }

        public int GetIdProductByContract(int contractId)
        {
            var res = _dataContext.ContractProducts
            .Join(_dataContext.Products, p => new { p.Idproduct }, cp => new { cp.Idproduct },
                (p, cp) => new { p, cp })
            .Join(_dataContext.ProductLines, ppl => ppl.p.Idproduct, pl => pl.Idproduct, (ppl, pl) => new { ppl, pl })
            .Where(o => o.ppl.p.Idcontract == contractId && o.pl.IdjobVacType != null)
            .Select(o => o.ppl.cp.Idproduct)
            .First();
            return res;
        }
    }
}
