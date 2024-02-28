using Application.ContractProducts.DTO;
using Application.Core;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Newtonsoft.Json;

namespace Application.ContractProducts.Queries
{
    public class DistributionForCredits
    {
        public class Query : IRequest<Result<UnitsContainer>>
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<UnitsContainer>>
        {
            private const int SITE = 6;
            private const int LANGUAGE = 7;
            private readonly IContractProductRepository _contProdRepo;
            private readonly IContractRepository _contractRepository;
            private readonly IJobOfferRepository _jobOfferRepo;
            private readonly IUserRepository _userRepository;
            private readonly IRegEnterpriseContractRepository _regEnterpriseContract;
            private readonly IEnterpriseUserJobVacRepository _enterpriseUserJobVacRepository;

            public Handler(IContractProductRepository contractProductRepository,
                IContractRepository contractRepository,
                IJobOfferRepository jobOfferRepository,
                IUserRepository userRepository,
                IRegEnterpriseContractRepository regEnterpriseContract,
                IEnterpriseUserJobVacRepository enterpriseUserJobVacRepository)
            {
                _contProdRepo = contractProductRepository;
                _contractRepository = contractRepository;
                _jobOfferRepo = jobOfferRepository;
                _userRepository = userRepository;
                _regEnterpriseContract = regEnterpriseContract;
                _enterpriseUserJobVacRepository = enterpriseUserJobVacRepository;
            }

            public async Task<Result<UnitsContainer>> Handle(Query request, CancellationToken cancellationToken)
            {
                UnitsContainer unitsContainer = new UnitsContainer();
                VacancyTypesCredits[] vacancyTypes = { VacancyTypesCredits.Basic, VacancyTypesCredits.Superior, VacancyTypesCredits.Premium, VacancyTypesCredits.PremiumInternational, VacancyTypesCredits.Internship };
                //Get all valid contracts for the company
                var contracts = await _contractRepository.GetValidContracts(request.CompanyId);
                var json = JsonConvert.SerializeObject(contracts);
                var users = _userRepository.GetCompanyValidUsers(request.CompanyId);
                var purchasedBasic = 0;
                var purchasedSuperior = 0;
                var purchasedPremium = 0;
                var purchasedPremiumInternational = 0;
                var purchasedInternship = 0;

                var consumedBasic = 0;
                var consumedSuperior = 0;
                var consumedPremium = 0;
                var consumedPremiumInternational = 0;
                var consumedInternship = 0;

                foreach (var c in contracts)
                {
                    if(c.IdJobVacType == (int)VacancyTypesCredits.Basic)
                    {
                        purchasedBasic+= _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Basic);
                    }
                    if(c.IdJobVacType == (int)VacancyTypesCredits.Superior)
                    {
                        purchasedSuperior += _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Superior);
                    }
                    if(c.IdJobVacType == (int)VacancyTypesCredits.Premium)
                    {
                        purchasedPremium += _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Premium);
                    }
                    if(c.IdJobVacType == (int)VacancyTypesCredits.PremiumInternational)
                    {
                        purchasedPremiumInternational += _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.PremiumInternational);
                    }
                    if(c.IdJobVacType == (int)VacancyTypesCredits.Internship)
                    {
                        purchasedInternship += _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Internship);
                    }
                    if (c.IdJobVacType == (int)VacancyTypesCredits.StandardWelcome)
                    {
                        purchasedInternship += _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.StandardWelcome);
                    }
                }

                foreach (var c in contracts)
                {
                    if(c.IdJobVacType == (int)VacancyTypesCredits.Basic)
                    {
                        consumedBasic += _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, (int)VacancyTypesCredits.Basic).Count();
                    }
                    if(c.IdJobVacType == (int)VacancyTypesCredits.Superior)
                    {
                        consumedSuperior += _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, (int)VacancyTypesCredits.Superior).Count();
                    }
                    if(c.IdJobVacType == (int)VacancyTypesCredits.Premium)
                    {
                        consumedPremium += _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, (int)VacancyTypesCredits.Premium).Count();
                    }
                    if(c.IdJobVacType == (int)VacancyTypesCredits.PremiumInternational)
                    {
                        consumedPremiumInternational += _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, (int)VacancyTypesCredits.PremiumInternational).Count();
                    }
                    if(c.IdJobVacType == (int)VacancyTypesCredits.Internship)
                    {
                        consumedInternship += _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, (int)VacancyTypesCredits.Internship).Count();
                    }
                    if (c.IdJobVacType == (int)VacancyTypesCredits.StandardWelcome)
                    {
                        consumedInternship += _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, (int)VacancyTypesCredits.StandardWelcome).Count();
                    }

                }
                

                var assignedBasic = 0;
                var assignedSuperior = 0;
                var assignedPremium = 0;
                var assignedPremiumInternational = 0;
                var assignedInternship = 0;

                //pass the nested foreach to linq


                foreach(var c in contracts)
                {
                    foreach(var u in users)
                    {

                        var assBasic = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)VacancyTypesCredits.Basic, c.ContractId);
                        assignedBasic += assBasic.Sum(b => b.JobVacUsed);
                        var assSuperior = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)VacancyTypesCredits.Superior, c.ContractId);
                        assignedSuperior += assSuperior.Sum(b => b.JobVacUsed);
                        var assPremium = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)VacancyTypesCredits.Premium, c.ContractId);
                        assignedPremium += assPremium.Sum(b => b.JobVacUsed);
                        var assPremiumInternational = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)VacancyTypesCredits.PremiumInternational, c.ContractId);
                        assignedPremiumInternational = assPremiumInternational.Sum(b => b.JobVacUsed);
                        var assInternship = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)VacancyTypesCredits.Internship, c.ContractId);
                        assignedInternship += assInternship.Sum(b => b.JobVacUsed);
                    }
                }
                foreach (var u in users)
                {
                    AssignedUnitsByUserAndProduct userUnitsInfo = new()
                    {
                        UserId = u.Idsuser,
                        IDEnterpriseUser = u.IdEnterpriseUser,
                        Email = u.Email
                    };
                    foreach (var type in vacancyTypes)
                    {
                        var AssignedUnitsBasic = 0;
                        var AssignedUnitsSuperior = 0;
                        var AssignedUnitsPremium = 0;
                        var AssignedUnitsPremiumInternational = 0;
                        var AssignedUnitsInternship = 0;
                        var AssignedUnitsWelcome = 0;
                        foreach (var c in contracts)
                        {
                            if(c.IdJobVacType == (int) VacancyTypesCredits.Basic)
                            {
                                var a = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)type, c.ContractId);
                                AssignedUnitsBasic += a.Sum(b => b.JobVacUsed);
                            }
                            if(c.IdJobVacType == (int) VacancyTypesCredits.Superior)
                            {
                                var a = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)type, c.ContractId);
                                AssignedUnitsSuperior += a.Sum(b => b.JobVacUsed);
                            }
                            if(c.IdJobVacType == (int) VacancyTypesCredits.Premium)
                            {
                                var a = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)type, c.ContractId);
                                AssignedUnitsPremium += a.Sum(b => b.JobVacUsed);
                            }
                            if(c.IdJobVacType == (int) VacancyTypesCredits.PremiumInternational)
                            {
                                var a = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)type, c.ContractId);
                                AssignedUnitsPremiumInternational += a.Sum(b => b.JobVacUsed);
                            }
                            if(c.IdJobVacType == (int) VacancyTypesCredits.Internship)
                            {
                                var a = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)type, c.ContractId);
                                AssignedUnitsInternship += a.Sum(b => b.JobVacUsed);
                            }
                            if(c.IdJobVacType == (int) VacancyTypesCredits.StandardWelcome)
                            {
                                var a = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(u.IdEnterpriseUser, (int)type, c.ContractId);
                                AssignedUnitsWelcome += a.Sum(b => b.JobVacUsed);
                            }

                            
                        }

                        userUnitsInfo.UnitsInfoByProduct.Add(new ProductUnitsDistribution
                        {
                            ProductName = type == VacancyTypesCredits.Basic ? "Basic"
                            : type == VacancyTypesCredits.Premium ? "Premium"
                            : type == VacancyTypesCredits.Superior ? "Superior"
                            : type == VacancyTypesCredits.PremiumInternational ? "Premium International"
                            : type == VacancyTypesCredits.Internship ? "Internship" : "",

                            Type = type,
                            UnitsPurchasedAndValid = type  == VacancyTypesCredits.Basic ?  purchasedBasic
                            : type == VacancyTypesCredits.Premium ? purchasedPremium
                            : type == VacancyTypesCredits.Superior ? purchasedSuperior
                            : type == VacancyTypesCredits.PremiumInternational ? purchasedPremiumInternational
                            : type == VacancyTypesCredits.Internship ? purchasedInternship : 0,


                            ConsumedUnits = type == VacancyTypesCredits.Basic ? consumedBasic
                            : type == VacancyTypesCredits.Premium ? consumedPremium
                            : type == VacancyTypesCredits.Superior ? consumedSuperior
                            : type == VacancyTypesCredits.PremiumInternational ? consumedPremiumInternational
                            : type == VacancyTypesCredits.Internship ? consumedInternship : 0,
                            AssignedUnits = type == VacancyTypesCredits.Basic  ?  AssignedUnitsBasic
                            : type == VacancyTypesCredits.Superior ? AssignedUnitsSuperior
                            : type == VacancyTypesCredits.Premium ? AssignedUnitsPremium
                            : type==VacancyTypesCredits.PremiumInternational  ? AssignedUnitsPremiumInternational
                            : type== VacancyTypesCredits.Internship ? AssignedUnitsInternship
                            : type == VacancyTypesCredits.StandardWelcome ? AssignedUnitsWelcome :  0,
                            AvailableUnits = type == VacancyTypesCredits.Basic ? purchasedBasic - consumedBasic
                            : type == VacancyTypesCredits.Premium ? purchasedPremium - consumedPremium
                            : type == VacancyTypesCredits.Superior ? purchasedSuperior - consumedSuperior
                            : type == VacancyTypesCredits.PremiumInternational ? purchasedPremiumInternational - consumedPremiumInternational
                            : type == VacancyTypesCredits.Internship ? purchasedInternship - consumedInternship : 0

                           
                        });
                    }


                        unitsContainer.UnitsInfoByUser.Add(userUnitsInfo);

                }


                unitsContainer.PurchasedUnitsBasic = purchasedBasic;
                unitsContainer.PurchasedUnitsPremium = purchasedPremium;
                unitsContainer.PurchasedUnitsPremiumInternational = purchasedPremiumInternational;
                unitsContainer.PurchasedUnitsSuperior = purchasedSuperior;
                unitsContainer.PurchasedUnitsInternship = purchasedInternship;

                unitsContainer.ConsumedUnitsBasic = consumedBasic;
                unitsContainer.ConsumedUnitsInternship = consumedInternship;
                unitsContainer.ConsumedUnitsSuperior = consumedSuperior;
                unitsContainer.ConsumedUnitsPremium = consumedPremium;
                unitsContainer.ConsumedUnitsPremiumInternational = consumedPremiumInternational;

                unitsContainer.AvailableUnitsInternship = purchasedInternship - consumedInternship;
                unitsContainer.AvailableUnitsBasic = purchasedBasic - consumedBasic;
                unitsContainer.AvailableUnitsPremium = purchasedPremium - consumedPremium;
                unitsContainer.AvailableUnitsPremiumInternational = purchasedPremiumInternational - consumedPremiumInternational;
                unitsContainer.AvailableUnitsSuperior = purchasedSuperior - consumedSuperior;

                unitsContainer.AssignedUnitsBasic = assignedBasic;
                unitsContainer.AssignedUnitsPremium = assignedPremium;
                unitsContainer.AssignedUnitsPremiumInternational = assignedPremiumInternational;
                unitsContainer.AssignedUnitsSuperior = assignedSuperior;
                unitsContainer.AssignedUnitsInternship = assignedInternship;

                unitsContainer.AvailableToAssignBasic = purchasedBasic - consumedBasic - assignedBasic;
                unitsContainer.AvailableToAssignPremium = purchasedPremium - consumedPremium - assignedPremium;
                unitsContainer.AvailableToAssignPremiumInternational = purchasedPremiumInternational - consumedPremiumInternational - assignedPremiumInternational;
                unitsContainer.AvailableToAssignSuperior = purchasedSuperior - consumedSuperior - assignedSuperior;
                unitsContainer.AvailableToAssignInternship = purchasedInternship - consumedInternship - assignedInternship;



                return Result<UnitsContainer>.Success(unitsContainer);
            }
        }
    }
}
