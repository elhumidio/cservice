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
    public class GetCompanyInfoById
    {
        public class Query : IRequest<Result<CompanyinfoDto>>
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<CompanyinfoDto>>
        {
            private readonly IEnterpriseUserRepository _enterpriseUserRepository;
            private readonly IBrandRepository _brandRepository;            
            private readonly IEnterpriseRepository _enterpriseRepository;

            public Handler(IEnterpriseUserRepository enterpriseUserRepo, IBrandRepository brandRepo, IEnterpriseRepository enterpriseRepository)
            {
                _enterpriseUserRepository = enterpriseUserRepo;
                _brandRepository = brandRepo;
                
                _enterpriseRepository = enterpriseRepository;
            }

            public async Task<Result<CompanyinfoDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                CompanyinfoDto obj = new()
                {   
                     CompanyId = request.CompanyId, 
                };
                UserInfoDto user = _enterpriseUserRepository.GetIDSUserByCompanyId(obj.CompanyId);
                if (user != null) {
                    obj.IDSUser = user.IDSUser;
                    obj.IDEnterpriseUser = _enterpriseUserRepository.GetCompanyUserIdByUserId(obj.IDSUser);
                    obj.Brands = _brandRepository.GetBrands(obj.CompanyId);
                    obj.UserEmail = user.Email;
                    obj.SiteId = _enterpriseRepository.GetSite(obj.CompanyId);
                    return Result<CompanyinfoDto>.Success(await Task.FromResult(obj));
                }
                else return Result<CompanyinfoDto>.Failure("Such company doesn't exists");

            }
        }
    }
}
