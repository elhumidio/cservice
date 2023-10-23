using Application.Core;
using Application.EnterpriseContract.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.GetCompanyInfo.Queries
{
    public class GetCompanyInfo
    {
        public class Query : IRequest<Result<CompanyinfoDto>>
        {
            public string Email { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<CompanyinfoDto>>
        {
            private readonly IEnterpriseUserRepository _enterpriseUserRepository;
            private readonly IBrandRepository _brandRepository;
            private readonly IUserRepository _userRepository;
            private readonly IEnterpriseRepository _enterpriseRepository;            

            public Handler(IEnterpriseUserRepository enterpriseUserRepo, IBrandRepository brandRepo, IUserRepository userRepo, IEnterpriseRepository enterpriseRepository)
            {                
                _enterpriseUserRepository = enterpriseUserRepo;
                _brandRepository = brandRepo;
                _userRepository = userRepo;
                _enterpriseRepository = enterpriseRepository;
            }

            public async Task<Result<CompanyinfoDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                CompanyinfoDto obj = new()
                {
                    IDSUser = _userRepository.GetUserIdByEmail(request.Email)
                };
                
                obj.CompanyId = _enterpriseUserRepository.GetCompanyIdByUserId(obj.IDSUser);
                var company = _enterpriseRepository.Get(obj.CompanyId);
                obj.IDEnterpriseUser = _enterpriseUserRepository.GetCompanyUserIdByUserId(obj.IDSUser);
                obj.Brands = _brandRepository.GetBrands(obj.CompanyId);
                obj.UserEmail = request.Email;
                obj.SiteId = _enterpriseRepository.GetSite(obj.CompanyId);
                obj.AccountStatus = company.AccountStatus;
                return Result<CompanyinfoDto>.Success(await Task.FromResult(obj));
            }
        }
    }
}
