using Domain.DTO.Requests;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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
            bool isPack = false;
            var res = _dataContext.ContractProducts
            .Join(_dataContext.Products, p => new { p.Idproduct }, cp => new { cp.Idproduct },
                (p, cp) => new { p, cp })
            .Join(_dataContext.ProductLines, ppl => ppl.p.Idproduct, pl => pl.Idproduct, (ppl, pl) => new { ppl, pl })
            .Where(o => o.ppl.p.Idcontract == contractId && o.pl.IdjobVacType != null && o.ppl.p.Idproduct != 110)
            .Select(o => o.ppl.cp.ChkPack);

            if (res.Any() && res != null)
                isPack = res.First();
            return isPack;
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

        public async Task<int> CreateContractProduct(ContractProduct contractProduct)
        {
            var ret = await _dataContext.Set<ContractProduct>().AddAsync(contractProduct);            
            return contractProduct.Idcontract;
        }

        public async Task<bool> UpdateContractProductSalesforceId(UpdateContractProductSForceId items)
        {
            
            var contractProduct = await _dataContext.ContractProducts.Where(c => c.Idcontract == items.ContractId).ToListAsync();
            int ret = -1;
            foreach (var item in contractProduct)
            {
                var a =  items.ContractProductSalesforceIds.Where(i => i.ProductId == item.Idproduct).FirstOrDefault();
                if (a == null)
                    continue;
                item.IdsalesForce = a.SalesforceId;
                ret = await _dataContext.SaveChangesAsync();
            }
            
           
            return ret > 0;
        }
    }
}
