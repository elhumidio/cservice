using API.DataContext;
using Domain.Classes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Net.NetworkInformation;

namespace Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int GetUserIdByEmail(string email)
        {
            int userId = 0;
            var user = _dataContext.Users.FirstOrDefault(x => x.Email == email && (bool)x.ChkActive);
            if (user != null)
                userId = user.Idsuser;
            return userId;
        }

        public bool IsAdmin(int userId)
        {
            bool IsAdmin = false;
            var user = _dataContext.Users.Where(u => u.Idsuser == userId && u.ChkActive == true);
            if (user != null && user.Any())
            {
                IsAdmin = user.First().IdstypeUser == (int)UserTypes.AdministradorEmpresa;
            }
            return IsAdmin;
        }

        public Task<List<UserContractExpireSoonData>> GetUsersContractExpireSoon(int days)
        {
            DateTime DateNow = DateTime.Now;
            DateTime DateUntil = DateTime.Now.AddDays(days).Date;

            var query = (from user in _dataContext.Users
                         join enterprise_user in _dataContext.EnterpriseUsers on user.Idsuser equals enterprise_user.Idsuser
                         join contract in _dataContext.Contracts on enterprise_user.IdenterpriseUser equals contract.IdenterpriseUser
                         join enterprise in _dataContext.Enterprises on contract.Identerprise equals enterprise.Identerprise
                         where contract.ChkApproved
                                && user.ChkActive == true
                                && user.IdstypeUser == 4 //admin
                                && enterprise.Name != ""
                                && contract.StartDate < DateTime.Now
                                && contract.FinishDate > DateTime.Now
                                && contract.FinishDate < DateUntil
                         select new UserContractExpireSoonData()
                         {
                             Email = user.Email,
                             CompanyName = enterprise.Name,
                             TypeUser = user.IdstypeUser,
                             ContractConcept = contract.Concept,
                             FinishDate = contract.FinishDate,
                             GrcontactId = user.GrcontactId
                         }).Distinct();

            return query.ToListAsync();
        }

        public Task<List<UserContractAvailableUnitsData>> GetUsersContractAvailableUnits()
        {
            DateTime DateNow = DateTime.Now;

            var query = (from enterprise_user_job_vacs in _dataContext.EnterpriseUserJobVacs
                         let chkPack =
                                    ( from cp in _dataContext.ContractProducts
                                      join p in _dataContext.Products on cp.Idproduct equals p.Idproduct
                                      join pl in _dataContext.ProductLines on p.Idproduct equals pl.Idproduct
                                      where cp.Idcontract == enterprise_user_job_vacs.Idcontract
                                      select p.ChkPack).FirstOrDefault()
                         let maxJobVacancies =
                           (from euj in _dataContext.EnterpriseUserJobVacs
                            where euj.IdenterpriseUser == enterprise_user_job_vacs.IdenterpriseUser
                            && euj.Idcontract == enterprise_user_job_vacs.Idcontract
                            select euj.MaxJobVacancies).Sum()
                         let jobVacUsed =
                           (from euj in _dataContext.EnterpriseUserJobVacs
                            where euj.IdenterpriseUser == enterprise_user_job_vacs.IdenterpriseUser
                            && euj.Idcontract == enterprise_user_job_vacs.Idcontract
                            select euj.JobVacUsed).Sum()
                         join enterprise_user in _dataContext.EnterpriseUsers on enterprise_user_job_vacs.IdenterpriseUser equals enterprise_user.IdenterpriseUser
                         join user in _dataContext.Users on enterprise_user.Idsuser equals user.Idsuser
                         join contract in _dataContext.Contracts on enterprise_user_job_vacs.Idcontract equals contract.Idcontract
                         join enterprise in _dataContext.Enterprises on contract.Identerprise equals enterprise.Identerprise
                         join user_type in _dataContext.TypeUsers on user.IdstypeUser equals user_type.IdstypeUser
                         let unitsConsumedPack =
                           (from jv in _dataContext.JobVacancies
                            where jv.Idstatus == 1 && jv.FinishDate > DateTime.Now && jv.ChkFilled == false && jv.ChkDeleted == false
                            && jv.IdenterpriseUserG == enterprise_user_job_vacs.IdenterpriseUser
                            && jv.Idcontract == enterprise_user_job_vacs.Idcontract
                            && jv.Identerprise == contract.Identerprise
                            select jv).Count()
                         let unitsConsumedWithoutPack =
                           (from jv in _dataContext.JobVacancies
                            where jv.Idstatus != 3
                            && jv.IdenterpriseUserG == enterprise_user_job_vacs.IdenterpriseUser
                            && jv.Idcontract == enterprise_user_job_vacs.Idcontract
                            && jv.Identerprise == contract.Identerprise
                            select jv).Count()
                         where contract.ChkApproved
                                && user.ChkActive == true
                                && enterprise.Name != ""
                                && contract.StartDate < DateTime.Now
                                && contract.FinishDate > DateTime.Now
                                && ((chkPack && (jobVacUsed - unitsConsumedPack) > 0) ||
                                    (!chkPack && (jobVacUsed - unitsConsumedWithoutPack) > 0))
                         select new UserContractAvailableUnitsData()
                         {
                             Email = user.Email,
                             CompanyName = enterprise.Name,
                             TypeUser = user.IdstypeUser,
                             ContractConcept = contract.Concept,
                             FinishDate = contract.FinishDate,
                             ChkPack = chkPack,
                             JobVacAvailable = chkPack == true ? (jobVacUsed - unitsConsumedPack) :
                             (jobVacUsed - unitsConsumedWithoutPack),
                             GrcontactId = user.GrcontactId
                         }).Distinct()
                         .OrderBy(u => u.Email);

            return query.ToListAsync();
        }

        public Task<List<UserContractBegin>> GetUsersContractBegin()
        {
            String DateNowString = DateTime.Now.ToString("dd/MM/yyyy");

            var query = (from user in _dataContext.Users
                         join enterprise_user in _dataContext.EnterpriseUsers on user.Idsuser equals enterprise_user.Idsuser
                         join contract in _dataContext.Contracts on enterprise_user.IdenterpriseUser equals contract.IdenterpriseUser
                         join enterprise in _dataContext.Enterprises on contract.Identerprise equals enterprise.Identerprise
                         where contract.ChkApproved
                                && user.ChkActive == true
                                //&& user.IdstypeUser == 4 //admin
                                && enterprise.Name != ""
                                && contract.StartDate != null
                                && contract.StartDate.Value.Date == DateTime.Now.Date
                         select new UserContractBegin()
                         {
                             Email = user.Email,
                             CompanyName = enterprise.Name,
                             TypeUser = user.IdstypeUser,
                             ContractConcept = contract.Concept,
                             FinishDate = contract.FinishDate,
                             GrcontactId = user.GrcontactId
                         });

            return query.ToListAsync();
        }

        public Task<List<UserGetResponseData>> ListUsersEmptyGetResponse()
        {
            var allCandidates = _dataContext.Users
                .Where(u => u.GrcontactId == null || (u.GrcontactId == "-1" && u.CreationDate > DateTime.UtcNow.AddDays(-5)))
                .Select(x => new UserGetResponseData
                {
                    Email = x.Email,
                    IDUser = x.Idsuser
                }); //.Take(100)


            return allCandidates.ToListAsync();
        }

        public int GetIdsuserByManagerId(int managerId)
        {
            var userId = (from user in _dataContext.Users
                         join eu in _dataContext.EnterpriseUsers on user.Idsuser equals eu.Idsuser
                         where eu.IdenterpriseUser == managerId
                         select eu.Idsuser).FirstOrDefault();

            return userId;  
        }

        public bool UpdateUserGetResponse(User request)
        {
            var users = _dataContext.Users.Where(x => x.Email == request.Email);

            if (users == null)
                return false;

            foreach (var user in users)
            {
                user.GrcontactId = request.GrcontactId;
            }

            _dataContext.SaveChanges();

            return true;
        }
    }
}
