using Domain.DTO;
using Domain.DTO.Products;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

        public bool DeleteContractInfo(int contractId, bool HasToDeleteContract)
        {
            // Obtener las entidades
            var contract = _dataContext.Contracts.FirstOrDefault(c => c.Idcontract == contractId);
            var contractProduct = _dataContext.ContractProducts.FirstOrDefault(c => c.Idcontract == contractId);
            var regenterprisecontracts = _dataContext.RegEnterpriseContracts.FirstOrDefault(c => c.Idcontract == contractId);
            var enterpriseuserjobvac = _dataContext.EnterpriseUserJobVacs.FirstOrDefault(c => c.Idcontract == contractId);
            var contractpayment = _dataContext.ContractPayments.FirstOrDefault(c => c.Idcontract == contractId);

            // Eliminar las entidades si existen
            if (contract != null && HasToDeleteContract)
            {
                _dataContext.Contracts.Remove(contract);
            }

            if (contractProduct != null)
            {
                _dataContext.ContractProducts.Remove(contractProduct);
            }

            if (regenterprisecontracts != null)
            {
                _dataContext.RegEnterpriseContracts.Remove(regenterprisecontracts);
            }

            if (enterpriseuserjobvac != null)
            {
                _dataContext.EnterpriseUserJobVacs.Remove(enterpriseuserjobvac);
            }

            if (contractpayment != null)
            {
                _dataContext.ContractPayments.Remove(contractpayment);
            }

            var ret = _dataContext.SaveChanges();

            return ret > 0;

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

        public DateTime GetStartDateByContract(int idcontract)
        {
            var date = _dataContext.Contracts.FirstOrDefault(c => c.Idcontract == idcontract)?.StartDate ?? DateTime.Now;
            return date;
        }

        public bool IsValidContract(int contractId)
        {
            var contracts = _dataContext.Contracts
                .Where(c => c.FinishDate >= DateTime.Today
                && (c.StartDate.HasValue) ? DateTime.Now.Date >= ((DateTime)c.StartDate).Date : DateTime.Now.Date >= (c.StartDate ?? DateTime.MaxValue).Date
                && c.ChkApproved
                && c.Idcontract == contractId
                && !c.ChkCancel)
                .OrderBy(con => con.FinishDate);
            return contracts != null && contracts.Any();
        }

        public async Task<bool> IsAllowedContractForSeeingFilters(int contractId)
        {
            var contract = await _dataContext.Contracts.FirstOrDefaultAsync(c => c.Idcontract == contractId);
            return contract != null && contract.FinishDate >= DateTime.Now.Date.AddDays(-30);
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
                .Join(_dataContext.ContractPayments, cp => cp.pl.p.c.Idcontract, cp => cp.Idcontract, (cp, cpayment) => new { cp, cpayment })
                .Where(a => a.cp.ppl.IdserviceType == null
                            && a.cp.pl.p.c.Identerprise == companyId
                            && a.cp.pl.p.c.ChkApproved
                            && a.cp.pl.p.c.FinishDate >= DateTime.Now.Date
                            && a.cp.pl.p.c.StartDate <= DateTime.Now.Date
                            && a.cp.ppl.Idsite == siteId && a.cp.ppl.Idslanguage == langId
                            && a.cp.pl.pr.Idsite == siteId && a.cp.pl.pr.Idslanguage == langId
                            && a.cpayment.Finished == true
                )
                .Select(res => new ContractsDistDto
                {
                    BaseName = res.cp.pl.pr.BaseName,
                    ContractId = res.cp.pl.p.c.Idcontract,
                    ProductId = res.cp.pl.pr.Idproduct,
                    FinishDate = res.cp.pl.p.c.FinishDate != null ? Convert.ToDateTime(res.cp.pl.p.c.FinishDate).ToString("dd/MM/yyyy") : string.Empty,
                    StartDate = res.cp.pl.p.c.StartDate != null ? Convert.ToDateTime(res.cp.pl.p.c.StartDate) : DateTime.Now,
                    IdJobVacType = res.cp.ppl.IdjobVacType ?? -1,
                })
                .Distinct()
                .ToListAsync();


            return list;
        }


        public async Task<List<ContractsDistDto>> GetValidContractsByProduct(int companyId, int productId, int siteId, int langId)
        {
            bool isPack = false;
            var list = await _dataContext.Contracts
                .Join(_dataContext.ContractProducts, c => new { c.Idcontract }, cp => new { cp.Idcontract }, (c, cp) => new { c, cp })
                .Join(_dataContext.Products, p => p.cp.Idproduct, pr => pr.Idproduct, (p, pr) => new { p, pr })
                .Join(_dataContext.ProductLines, pl => pl.pr.Idproduct, ppl => ppl.Idproduct, (pl, ppl) => new { pl, ppl })
                .Join(_dataContext.ContractPayments, cp => cp.pl.p.c.Idcontract, cp => cp.Idcontract, (cp, cpayment) => new { cp, cpayment })
                .Where(a => a.cp.ppl.IdserviceType == null
                            && a.cp.pl.p.c.Identerprise == companyId
                            && a.cp.pl.p.c.ChkApproved
                            && a.cp.pl.p.c.FinishDate >= DateTime.Now.Date
                            && a.cp.pl.p.c.StartDate <= DateTime.Now.Date
                            && a.cp.ppl.Idsite == siteId && a.cp.ppl.Idslanguage == langId
                            && a.cp.pl.pr.Idsite == siteId && a.cp.pl.pr.Idslanguage == langId
                            && a.cp.pl.pr.Idproduct == productId
                            && a.cpayment.Finished == true
                )
                .Select(res => new ContractsDistDto
                {
                    BaseName = res.cp.pl.pr.BaseName,
                    ContractId = res.cp.pl.p.c.Idcontract,
                    ProductId = res.cp.pl.pr.Idproduct,
                    FinishDate = res.cp.pl.p.c.FinishDate != null ? Convert.ToDateTime(res.cp.pl.p.c.FinishDate).ToString("dd/MM/yyyy") : string.Empty,
                    StartDate = res.cp.pl.p.c.StartDate != null ? Convert.ToDateTime(res.cp.pl.p.c.StartDate) : DateTime.Now,
                    IdJobVacType = res.cp.ppl.IdjobVacType ?? -1,
                })
                .Distinct()
                .ToListAsync();


            return list;
        }

        public async Task<IReadOnlyList<KeyValueResponse>> GetValidContractsByCompaniesIds(List<int> companiesIds)
        {
            var filteredQuery = _dataContext.Contracts
                .Where(a => a.ChkApproved
                && a.FinishDate >= DateTime.Now.Date
                && a.StartDate <= DateTime.Now.Date
                && a.SalesforceId != null
                && a.Identerprise > 0 && companiesIds.Contains(a.Identerprise))
                .GroupBy(a => a.Identerprise)
                .Select(group => new KeyValueResponse
                {
                    Id = group.Key,
                    Value = group.Count()
                });

            var result = await filteredQuery.ToListAsync();

            return result;
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

        public async Task<int> CreateContract(Contract contract)
        {
            try
            {
                var a = _dataContext.Add(contract).Entity;
                var saving = await _dataContext.SaveChangesAsync();
                return contract.Idcontract;
            }
            catch (Exception ex)
            {
                var a = ex;
                return -1;
            }
        }

        public async Task<bool> UpdateContractSalesforceId(int contractId, string salesforceId)
        {
            var contract = await _dataContext.Contracts.FirstOrDefaultAsync(c => c.Idcontract == contractId);
            contract.SalesforceId = salesforceId;
            var ret = await _dataContext.SaveChangesAsync();
            return ret > 0;
        }

        public async Task<bool> UpdateContract(Contract contract)
        {
            var currentContract = _dataContext.Contracts.First(c => c.Idcontract == contract.Idcontract);

            _dataContext.Contracts.Update(currentContract);            
            currentContract.IdenterpriseUser = contract.IdenterpriseUser;
            currentContract.IdcontractParent = contract.IdcontractParent;
            currentContract.IdpayMethod = contract.IdpayMethod;
            currentContract.Idcurrency = contract.Idcurrency;
            currentContract.ContractDate = contract.ContractDate;
            currentContract.StartDate = contract.StartDate;
            currentContract.FinishDate = contract.FinishDate;
            currentContract.Comment = contract.Comment;
            currentContract.ChkApproved = true;
            currentContract.Concept = contract.Concept;
            currentContract.Discount = contract.Discount;
            currentContract.Price = contract.Price;
            currentContract.CorporateNameInvoicing = contract.CorporateNameInvoicing;
            currentContract.ContactInvoicing = contract.ContactInvoicing;
            currentContract.CompanyTaxCodeInvoicing = contract.CompanyTaxCodeInvoicing;
            currentContract.AddressInvoicing = contract.AddressInvoicing;
            currentContract.PhoneNumberInvoicing = contract.PhoneNumberInvoicing;
            currentContract.FaxNumberInvoicing = contract.FaxNumberInvoicing;
            currentContract.ShopOnline = contract.ShopOnline;
            currentContract.IdbackOfUser = contract.IdbackOfUser;
            currentContract.FinalPrice = contract.FinalPrice;
            currentContract.ApprovedDate = contract.ApprovedDate;
            currentContract.CancelDate = contract.CancelDate;
            currentContract.IdcancelReason = contract.IdcancelReason;
            currentContract.ChkCancel = contract.ChkCancel;
            currentContract.ChkPayFract = contract.ChkPayFract;
            currentContract.ChkContratExtension = contract.ChkContratExtension;
            currentContract.ChkPromotionalCode = contract.ChkPromotionalCode;
            currentContract.Idcode = contract.Idcode;
            currentContract.DiscPromotion = contract.DiscPromotion;
            currentContract.SalesforceId = contract.SalesforceId;
            currentContract.Sftimestamp = contract.Sftimestamp;
            currentContract.SiteId = contract.SiteId;
            var ret = _dataContext.SaveChanges();

            return ret > 0;
        }

        public bool DisableContract(int contractId)
        {
            var currentContract = _dataContext.Contracts.First(c => c.Idcontract == contractId);

            _dataContext.Contracts.Update(currentContract);
            currentContract.ChkApproved = false;
            var ret = _dataContext.SaveChanges();

            return ret > 0;
        }


        public int GetOlderContractFromList(List<int> contracts)
        {
            var cs = _dataContext.Contracts.Where(c => contracts.Contains(c.Idcontract)).OrderByDescending(d => d.StartDate).FirstOrDefault();
            if (cs == null)
                return -1;
            return cs.Idcontract;
        }

        public DateTime GetContractFinishDate(List<ProductUnits> productUnits, int countryId = 40)
        {
            var maxDate = DateTime.MinValue;
            var currentDate = DateTime.Now;
            int daysDuration = 0;
            foreach (var item in productUnits)
            {
                var durations = _dataContext.ContractDurationByProducts.Where(c => c.ProductId == item.Idproduct
                && c.CountryId == countryId && item.Units >= c.From && item.Units <= c.To).FirstOrDefault();
                if(durations != null)
                    daysDuration = durations.Duration ?? 0;
                var finishDate = currentDate.AddDays(daysDuration);               
                maxDate = finishDate > maxDate ? finishDate : maxDate;
            }

            return maxDate;
        }

        public async Task<IReadOnlyList<EnterpriseListContractsIdsDto>> GetContractsByCompaniesIds(List<int> companiesIds)
        {
            var filteredQuery = _dataContext.Contracts
                .Where(c => companiesIds.Contains(c.Identerprise)
                && c.FinishDate >= DateTime.Today)
                .GroupBy(c => c.Identerprise)
                .Select(x => new EnterpriseListContractsIdsDto { Id = x.Key, Value = x.Select(c => c.Idcontract).ToList() });

            return await filteredQuery.ToListAsync();
        }

        public async Task<IReadOnlyList<KeyValueDateTimeDto>> GetFinishDateContractClosingExpiringByCompaniesIds(List<int> companiesIds)
        {
            var filteredQuery = _dataContext.Contracts
                .Where(c => companiesIds.Contains(c.Identerprise)
                && c.FinishDate >= DateTime.Today)
                .GroupBy(c => c.Identerprise)
                .Select(group => new KeyValueDateTimeDto
                {
                    Id = group.Key,
                    Value = group.OrderBy(c => c.FinishDate).First().FinishDate
                });

            var result = await filteredQuery.ToListAsync();

            return result;
        }

        public async Task<Contract> GetContractByStripeSessionId(string stripeSessionId)
        {
            //get contract by checkoutsessionid
            var contract = await _dataContext.Contracts.Where(c => c.CheckoutSessionId.Contains(stripeSessionId)).FirstOrDefaultAsync();
            return contract;
        }


    }
}
