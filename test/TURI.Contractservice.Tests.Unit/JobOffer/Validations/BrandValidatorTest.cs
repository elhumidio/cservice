using Application.JobOffer.Commands;
using Application.JobOffer.Validations;
using Domain.Repositories;
using Moq;
using NUnit.Framework;
using TURI.Contractservice.Tests.Unit.Mocks;

namespace TURI.Contractservice.Tests.Unit.JobOffer.Validations
{

    public class BrandValidatorTest
    {
        private BrandValidator _validator;
        private readonly Mock<IBrandRepository> _brandRepositoryMock;

        public BrandValidatorTest()
        {
            _brandRepositoryMock = MockBrandRepository.GetBrandRepository();
            _validator = new BrandValidator(_brandRepositoryMock.Object);
        }
        [Test]
        public void NotNegativeOrEmptyValue()
        {
            // Given
            var cmd = new CreateOfferCommand();
            cmd.Idbrand = -10;
            cmd.Identerprise = 10175;

            // When
            var result = _validator.Validate(cmd);

            // Then

            Assert.That(result.Errors.Any(o => o.PropertyName == "IDBrand"), Is.False);
        }



    }
}
