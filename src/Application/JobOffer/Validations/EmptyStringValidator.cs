using Application.JobOffer.Commands;
using FluentValidation;
using HtmlAgilityPack;

namespace Application.JobOffer.Validations
{
    public class EmptyStringValidator : AbstractValidator<CreateOfferCommand>
    {
        HtmlDocument htmldoc = new();
        public EmptyStringValidator()
        {
            RuleFor(command => command).NotEmpty().WithMessage("Title is mandatory field.\n").Must(HasNotHtml);
            RuleFor(command => command).NotEmpty().WithMessage("Description is mandatory field.\n").Must(HasBeCleanHtml);

        }
        private bool HasNotHtml(CreateOfferCommand cmd)
        {
            htmldoc.LoadHtml(cmd.Title);
            cmd.Title = htmldoc.DocumentNode.InnerText;
            return true;
        }

        private bool HasBeCleanHtml(CreateOfferCommand cmd)
        {
            htmldoc.LoadHtml(cmd.Description);
            if (htmldoc.ParseErrors.Any())
                cmd.Description = htmldoc.DocumentNode.InnerText;
            return true;
        }

    }
}
