using Domain.DTO.Products;
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

        public List<ProductUnits> GetProductsAndUnitsByContract(int contractId)
        {
            int[] prods = { 87, 125, 4, 110, 130 };
            var productUnitsList = (
                from contractProduct in _dataContext.ContractProducts
                join regEnterpriseContract in _dataContext.RegEnterpriseContracts on contractProduct.Idcontract equals regEnterpriseContract.Idcontract
                where contractProduct.Idcontract == contractId && prods.Contains(contractProduct.Idproduct) 
                select new ProductUnits
                {
                    Idproduct = contractProduct.Idproduct,
                    Units = regEnterpriseContract.Units
                }
            ).Distinct().ToList();
            return productUnitsList;
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
        
        public List<ContractProduct> GetContractProducts(int contractId)
        {
            var cp = _dataContext.ContractProducts.Where(c => c.Idcontract == contractId).ToList();
            return cp;
        }


            public async Task<int> CreateContractProduct(ContractProduct contractProduct)
            {
                var ret = await _dataContext.Set<ContractProduct>().AddAsync(contractProduct);
                return contractProduct.Idcontract;
            }

        public async Task<bool> Update(ContractProduct contractProduct)
        {
           var ret=  _dataContext.ContractProducts.Update(contractProduct);
            var ans = await _dataContext.SaveChangesAsync();
            return ans > 0;
        }


        public async Task<bool> UpdateContractProductSalesforceId(UpdateContractProductSForceId items)
            {
                var contractProduct = await _dataContext.ContractProducts.Where(c => c.Idcontract == items.ContractId).ToListAsync();
                int ret = -1;
                foreach (var item in contractProduct)
                {
                    var a = items.ContractProductSalesforceIds.Where(i => i.ProductId == item.Idproduct).FirstOrDefault();
                    if (a == null)
                        continue;
                    item.IdsalesForce = a.SalesforceId;
                    ret = await _dataContext.SaveChangesAsync();
                }

                return ret > 0;
            }
        }
    }
