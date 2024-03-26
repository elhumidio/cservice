using Application.ContractProducts.DTO;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Repositories
{
    public class UnitsRepository : IUnitsRepository
    {
        private readonly DataContext _dataContext;
        private IConfiguration _config;

        public UnitsRepository(DataContext dataContext,
            IConfiguration config)
        {
            _dataContext = dataContext;
            _config = config;
        }

        public IQueryable<EnterpriseUserJobVac> GetAssignmentsByContract(int contractId)
        {
            var query = _dataContext.EnterpriseUserJobVacs.Where(a => a.Idcontract == contractId);
            return query;
        }

        IQueryable<EnterpriseUserJobVac> IUnitsRepository.GetAssignmentsByContractAndManager(int contractId, int manager)
        {
            var query = _dataContext.EnterpriseUserJobVacs.Where(a => a.Idcontract == contractId && a.IdenterpriseUser == manager);
            return query;
        }

        public int GetAssignedUnitsMxPtByCompany(int companyId)
        {
            var res = _dataContext.EnterpriseUserJobVacs
          .Join(_dataContext.Contracts, c => new { c.Idcontract }, eujv => new { eujv.Idcontract },
              (jv, c) => new { jv, c })
          .Where(o => o.c.Identerprise == companyId
          && (o.jv.Idproduct == 110 || o.jv.Idproduct == 115) && o.c.FinishDate >= DateTime.Today)
          .Select(o => o.jv).ToList();
            return res.Sum(a => a.JobVacUsed);
        }

        public async Task<Dictionary<int, List<int>>> GetAssignedContractsForManagers(List<int> managers)
        {
            Dictionary<int, List<int>> dic = new();

            foreach (var manager in managers)
            {
                var res =await _dataContext.ManagersVisibilities.Where(m => m.EnterpriseUserId == manager && m.IsVisible == true).ToListAsync();
                List<int> listContracts = new List<int>();
                foreach (var visibilities in res)
                {
                    if (!listContracts.Contains(visibilities.ContractId))
                        listContracts.Add(visibilities.ContractId);
                }
                dic.Add(manager, listContracts);                
            }
            return dic;
        }

        public bool AssignUnitToManager(int contractId, VacancyType type, int ownerId)
        {
            var assignment = _dataContext.EnterpriseUserJobVacs.Where(a => a.IdjobVacType == (int)type
                && a.Idcontract == contractId
                && a.IdenterpriseUser == ownerId).FirstOrDefault();
            if (assignment != null)
            {
                assignment.JobVacUsed++;
                _dataContext.SaveChanges();
            }
            return true;
        }

        public bool TakeUnitFromManager(int contractId, VacancyType type, int ownerId)
        {
            var assignment = _dataContext.EnterpriseUserJobVacs.Where(a => a.IdjobVacType == (int)type
                && a.Idcontract == contractId
                && a.IdenterpriseUser == ownerId).FirstOrDefault();
            if (assignment != null)
            {
                assignment.JobVacUsed--;
                _dataContext.SaveChanges();
            }
            return true;
        }

        public List<CreditsPerProductDto> GetCreditsPerProduct(int enterpriseId)
        {
            var contracts = _dataContext.Contracts
             .Join(_dataContext.ContractPayments,a => a.Idcontract,cp => cp.Idcontract,(a, cp) => new { Contract = a, ContractPayment = cp })
             .Where(x => x.Contract.Identerprise == enterpriseId &&
                         x.Contract.FinishDate > DateTime.Today &&
                         x.ContractPayment.Finished != null &&
                         x.ContractPayment.Finished.Value)
             .Select(x => x.Contract)
             .ToList();
            var products = _dataContext.ContractProducts
                .Join(_dataContext.Products, p => new { p.Idproduct }, cp => new { cp.Idproduct },
                        (cp, p) => new { cp, p })
                .Where(a => contracts.Select(b => b.Idcontract).Contains(a.cp.Idcontract)).ToList();


            var productLines = _dataContext.ProductLines.Where(a => products.Select(b => b.cp.Idproduct).Contains(a.Idproduct)).ToList();
            var units = _dataContext.RegEnterpriseContracts.Where(a => contracts.Select(b => b.Idcontract).Contains(a.Idcontract)).ToList();

            var results = new List<CreditsPerProductDto>();
            List<int> allowedProductIds = _config["ConfigurationVariables:ProductsAvailable"].Split(',').Select(Int32.Parse).ToList();

            foreach( var contract in contracts )
            {
                foreach( var product in products.Where(prod => prod.cp.Idcontract == contract.Idcontract))
                {
                    if (!allowedProductIds.Contains(product.cp.Idproduct))
                        continue;
                    var productType = productLines.First(a => a.Idproduct == product.cp.Idproduct);
                    var unitsObj = units.First( a => a.Idcontract == contract.Idcontract && a.IdjobVacType == productType.IdjobVacType);

                    results.Add(new CreditsPerProductDto()
                    {
                        ContractId = contract.Idcontract,
                        ContractStartDate = contract.StartDate ?? DateTime.MinValue,
                        ContractFinishDate = contract.FinishDate ?? DateTime.MinValue,
                        ProductId = product.p.Idproduct,
                        ProductName = product.p.BaseName,
                        TotalCredits = unitsObj.Units,
                        ConsumedCredits = unitsObj.UnitsUsed,
                        RemainingCredits = unitsObj.Units - unitsObj.UnitsUsed
                    });
                }
            }

            return results;

            /*//Needs left joins, but that's Deprecated Soon TM...
            return _dataContext.ContractProducts
                .Join(_dataContext.Contracts, cp => cp.Idcontract, con => con.Idcontract, (cp, con) => new { IDProduct = cp.Idproduct, IDContract = con.Idcontract, IDEnterprise = con.Identerprise, FinishDate = con.FinishDate }).Distinct()
                .Join(_dataContext.ProductLines, cont => cont.IDProduct, pl => pl.Idproduct, (cont, pl) => new { IDProduct = cont.IDProduct, IDContract = cont.IDContract, IDEnterprise = cont.IDEnterprise, JobVacType = pl.IdjobVacType, FinishDate = cont.FinishDate }).Distinct()
                .Join(_dataContext.RegEnterpriseContracts,
                cont => new { id1 = cont.IDContract, id2 = cont.JobVacType ?? 0 },
                reg => new { id1 = reg.Idcontract, id2 = reg.IdjobVacType },
                (cont, reg) => new { IDProduct = cont.IDProduct, IDContract = cont.IDContract, IDEnterprise = cont.IDEnterprise, JobVacType = cont.JobVacType, RegEnterpriseContract = reg, FinishDate = cont.FinishDate })
                .Where(a => a.IDEnterprise == enterpriseId && a.FinishDate > DateTime.Today )
                .Select(a => new CreditsPerProductDto()
                {
                    ContractId = a.IDContract,
                    ProductId = a.IDProduct,
                    TotalCredits = a.RegEnterpriseContract.Units,
                    ConsumedCredits = a.RegEnterpriseContract.UnitsUsed,
                    RemainingCredits = a.RegEnterpriseContract.Units - a.RegEnterpriseContract.UnitsUsed
                }).ToList();*/
        }
    }
}
