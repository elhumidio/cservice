using Application.JobOffer.Commands;
using Application.JobOffer.Validations;
using NUnit.Framework;
using Shouldly;

namespace TURI.Contractservice.Tests.Unit.JobOffer.Validations
{
    public class EmailFormatValidatorTest
    {
        private EmailFormatValidator _validator;


        public EmailFormatValidatorTest()
        {

            _validator = new EmailFormatValidator();
        }
        [Test]
        public void RightFormatOrEmptyValue()
        {
            // Given
            var cmd = new CreateOfferCommand();
            cmd.IntegrationData.ApplicationEmail = "damian.blanc@turij@obs.com";

            // When
            var result = _validator.Validate(cmd);

            // Then
            result.Errors.Count().ShouldBeGreaterThan(0);

        }

    }
}
