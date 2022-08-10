using Application.Core;
using Application.EnterpriseContract.DTO;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class GetCompanyInfo
    {

        public class Query : IRequest<Result<CompanyinfoDTO>>
        {
            public string Email { get; set; }

        }

        public class Handler : IRequestHandler<Query, Result<CompanyinfoDTO>>
        {

            private readonly IEnterpriseUserRepository _enterpriseUserRepository;
            private readonly IBrandRepository _brandRepository;
            private readonly IUserRepository _userRepository;
            private readonly IEnterpriseRepository _enterpriseRepository;

            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IEnterpriseUserRepository enterpriseUserRepo, IBrandRepository brandRepo, IUserRepository userRepo, IEnterpriseRepository enterpriseRepository)
            {
                _mapper = mapper;
                _enterpriseUserRepository = enterpriseUserRepo;
                _brandRepository = brandRepo;
                _userRepository = userRepo;
                _enterpriseRepository = enterpriseRepository;
            }

            public async Task<Result<CompanyinfoDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                CompanyinfoDTO obj = new()
                {
                    IDSUser = _userRepository.GetUserIdByEmail(request.Email)
                };
                obj.CompanyId = _enterpriseUserRepository.GetCompanyIdByUserId(obj.IDSUser);
                obj.IDEnterpriseUser = _enterpriseUserRepository.GetCompanyUserIdByUserId(obj.IDSUser);
                obj.Brands = _brandRepository.GetBrands(obj.CompanyId);
                obj.UserEmail = request.Email;
                obj.SiteId = _enterpriseRepository.GetSite(obj.CompanyId);
                return Result<CompanyinfoDTO>.Success(await Task.FromResult(obj));
            }


        }
    }
}
