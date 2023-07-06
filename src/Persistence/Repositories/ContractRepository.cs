using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly DataContext _dataContext;

        public ContractRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Contract> Get(int contractId)
        {
            var query = _dataContext.Contracts.Where(a => a.Idcontract == contractId);
            return query;
        }

        public IQueryable<Contract> GetContracts(int companyId)
        {
            var query = _dataContext.Contracts.Where(c => c.Identerprise == companyId
            && c.ChkApproved
            && !c.ChkCancel
            && c.FinishDate >= DateTime.Today
            && DateTime.Now.Date >= c.StartDate).OrderByDescending(con => con.StartDate);
            return query;
        }

        public async Task<List<RegEnterpriseContract>> GetWithReg(int contractId)
        {
            try
            {
                var contracts = await _dataContext.Contracts
                    .Join(_dataContext.RegEnterpriseContracts, reg => new { reg.Idcontract }, c => new { c.Idcontract }, (reg, c) => new { reg, c })
                    .Where(creg => creg.c.Idcontract == contractId)
                    .Select(res => res.c).ToListAsync();
                return contracts;
            }
            catch (Exception ex)
            {
                var a = ex;
                return null;
            }
        }

        public bool IsValidContract(int contractId)
        {
            var contracts = _dataContext.Contracts
                .Where(c => c.FinishDate >= DateTime.Today
                && DateTime.Now.Date >= c.StartDate
                && c.ChkApproved
                && c.Idcontract == contractId
                && !c.ChkCancel)
                .OrderBy(con => con.FinishDate);
            return contracts != null && contracts.Any();
        }

        public async Task<List<ContractProductShortDto>> GetAllProductsByContract(int contractId, int lang, int site)
        {
            var list = await _dataContext.Contracts
                .Join(_dataContext.ContractProducts, c => new { c.Idcontract }, cp => new { cp.Idcontract }, (c, cp) => new { c, cp })
                .Join(_dataContext.Products, p => p.cp.Idproduct, pr => pr.Idproduct, (p, pr) => new { p, pr })
                .Where(p => p.p.cp.Idcontract == contractId && p.pr.Idslanguage == lang && p.pr.Idsite == site)
                .Select(res => new ContractProductShortDto
                {
                    ProductId = res.p.cp.Idproduct,
                    ProductName = res.pr.BaseName
                }).ToListAsync();

            return list;
        }

        public async Task<List<ContractsDistDto>> GetValidContracts(int companyId, int siteId, int langId)
        {
            bool isPack = false;
            var list = await _dataContext.Contracts
                .Join(_dataContext.ContractProducts, c => new { c.Idcontract }, cp => new { cp.Idcontract }, (c, cp) => new { c, cp })
                .Join(_dataContext.Products, p => p.cp.Idproduct, pr => pr.Idproduct, (p, pr) => new { p, pr })
                .Join(_dataContext.ProductLines, pl => pl.pr.Idproduct, ppl => ppl.Idproduct, (pl, ppl) => new { pl, ppl })
                .Where(a => a.ppl.IdserviceType == null
                && a.pl.p.c.Identerprise == companyId
                && a.pl.p.c.ChkApproved
                && a.pl.p.c.FinishDate >= DateTime.Now.Date
                && a.pl.p.c.StartDate <= DateTime.Now.Date
                && a.ppl.Idsite == siteId && a.ppl.Idslanguage == langId
                && a.pl.pr.Idsite == siteId && a.pl.pr.Idslanguage == langId
                && a.pl.p.c.SalesforceId != null)
                .Select(res => new ContractsDistDto
                {
                    BaseName = res.pl.pr.BaseName,
                    ContractId = res.pl.p.c.Idcontract,
                    ProductId = res.pl.pr.Idproduct,
                    FinishDate = res.pl.p.c.FinishDate != null ? Convert.ToDateTime(res.pl.p.c.FinishDate).ToString("dd/MM/yyyy") : string.Empty,
                }).Distinct().ToListAsync();

            return list;
        }

        public IQueryable<ServiceTypeDto> GetServiceTypes(int contractId)
        {
            IQueryable<ServiceTypeDto> list;
            list = _dataContext.Contracts
                .Join(_dataContext.ContractProducts, c => new { c.Idcontract }, cp => new { cp.Idcontract }, (c, cp) => new { c, cp })
                .Join(_dataContext.Products, p => p.cp.Idproduct, pr => pr.Idproduct, (p, pr) => new { p, pr })
                .Join(_dataContext.ProductLines, pl => pl.pr.Idproduct, ppl => ppl.Idproduct, (pl, ppl) => new { pl, ppl })
                .Where(a => a.ppl.IdserviceType != null && a.pl.p.c.Idcontract == contractId)
                .Select(res => new ServiceTypeDto { IDContract = res.pl.p.c.Idcontract, ServiceType = (int)res.ppl.IdserviceType }).Distinct().AsQueryable();

            return list;
        }
    }
}
