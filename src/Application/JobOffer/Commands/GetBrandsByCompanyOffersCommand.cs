using Application.Core;
using Domain.DTO.ManageJobs;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Commands
{
    public class GetBrandsByCompanyOffersCommand
    {
        public class Handler : IRequestHandler<GetBrandsQuery, Result<BrandsByOfferCompany>>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IJobOfferRepository _jobOfferRepository;

            public Handler(
                IBrandRepository brandRepository, IJobOfferRepository jobOfferRepository
                )
            {
                _brandRepository = brandRepository;
                _jobOfferRepository = jobOfferRepository;
            }

            public async Task<Result<BrandsByOfferCompany>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
            {
                BrandsByOfferCompany response = new BrandsByOfferCompany();
                var offers = _jobOfferRepository.GetAllOffersByCompany(request.CompanyId);
                var brands = _brandRepository.GetListBrands(request.CompanyId);

                Dictionary<int, string> brandsDic = new();
     

                foreach (var brand in brands)
                {
                    if (!brandsDic.ContainsKey(brand.Idbrand) && offers.Where(o => o.Idbrand == brand.Idbrand).Any())
                    {
                        brandsDic.Add(brand.Idbrand, brand.Name);
                    }
                }
                return Result<BrandsByOfferCompany>.Success(new  BrandsByOfferCompany {  Brands = brandsDic });
            }
        }
    }
}
