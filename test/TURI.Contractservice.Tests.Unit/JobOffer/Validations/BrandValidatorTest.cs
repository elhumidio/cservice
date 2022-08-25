using Application.JobOffer.Commands;
using Application.JobOffer.Validations;
using Domain.Repositories;
using Moq;
using NUnit.Framework;
using Shouldly;
using TURI.Contractservice.Tests.Unit.Mocks;
namespace TURI.Contractservice.Tests.Unit.JobOffer.Validations
{

    public class BrandValidatorTest
    {
        private BrandValidator _validator;
        private readonly Mock<IBrandRepository> _brandRepositoryMock;

        public BrandValidatorTest()
        {
            _brandRepositoryMock = MockBrandRepository.GetBrandRepository(true);
            _validator = new BrandValidator(_brandRepositoryMock.Object);
        }
        [Test]
        public void NotNegativeOrEmptyValue()
        {
            // Given
            var cmd = new CreateOfferCommand();


            // When
            var result = _validator.Validate(cmd);

            // Then
            result.Errors.ShouldBeEmpty();


        }



    }
}
