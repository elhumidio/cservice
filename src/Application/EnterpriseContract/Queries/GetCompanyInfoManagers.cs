using Application.Core;
using Application.EnterpriseContract.DTO;
using Domain.DTO;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EnterpriseContract.Queries
{
    public class GetCompanyInfoManagers
    {
        public class Query : IRequest<Result<CompanyinfoDto>>
        {
            public GetCompanyRequest Params { get; set; }
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
                    IDSUser = _userRepository.GetUserIdByEmail(request.Params.Email)
                };
                obj.CompanyId = _enterpriseUserRepository.GetCompanyIdByUserId(obj.IDSUser);

                //Here new logic
                obj.IDEnterpriseUser = _enterpriseUserRepository.GetCompanyUserIdByUserId(obj.IDSUser);


                obj.Brands = _brandRepository.GetBrands(obj.CompanyId);
                obj.UserEmail = request.Params.Email;


                obj.SiteId = _enterpriseRepository.GetSite(obj.CompanyId);
                return Result<CompanyinfoDto>.Success(await Task.FromResult(obj));
            }
        }
    }
}
