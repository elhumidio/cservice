using Application.JobOffer.Commands;
using FluentValidation;
using HtmlAgilityPack;

namespace Application.JobOffer.Validations
{
    public class EmptyStringValidator : AbstractValidator<CreateOfferCommand>
    {
        private HtmlDocument htmldoc = new();

        public EmptyStringValidator()
        {
            RuleFor(command => command).NotEmpty().WithMessage("Title is mandatory field.\n").Must(HasNotHtml);
            RuleFor(command => command).NotEmpty().WithMessage("Description is mandatory field.\n")
                .Must(HasBeCleanHtml)
                .Must(DescriptionMaxLength);
                
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

        private bool DescriptionMaxLength(CreateOfferCommand cmd)
        {
            htmldoc.LoadHtml(cmd.Description);
            if (cmd.Description.Length > 2499)
            {
                if (htmldoc.DocumentNode.InnerText.Length <= 2499)
                    cmd.Description = htmldoc.DocumentNode.InnerText;
                else
                    cmd.Description = htmldoc.DocumentNode.InnerText.Substring(0, 2490);
            }
            return true;
        }


    }

    public class EmptyStringValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private HtmlDocument htmldoc = new();

        public EmptyStringValidatorUp()
        {
            RuleFor(command => command).NotEmpty().WithMessage("Title is mandatory field.\n").Must(HasNotHtml);
            RuleFor(command => command).NotEmpty().WithMessage("Description is mandatory field.\n")
                .Must(HasBeCleanHtml)
                .Must(DescriptionMaxLength);
                
        }

        private bool HasNotHtml(UpdateOfferCommand cmd)
        {
            htmldoc.LoadHtml(cmd.Title);
            cmd.Title = htmldoc.DocumentNode.InnerText;
            return true;
        }

        private bool HasBeCleanHtml(UpdateOfferCommand cmd)
        {
            htmldoc.LoadHtml(cmd.Description);
            if (htmldoc.ParseErrors.Any())
                cmd.Description = htmldoc.DocumentNode.InnerText;
            return true;
        }

        private bool DescriptionMaxLength(UpdateOfferCommand cmd)
        {
            htmldoc.LoadHtml(cmd.Description);
            if (cmd.Description.Length > 2499)
            {
                if (htmldoc.DocumentNode.InnerText.Length <= 2499)
                    cmd.Description = htmldoc.DocumentNode.InnerText;
                else
                    cmd.Description = htmldoc.DocumentNode.InnerText.Substring(0, 2490);
            }
            return true;
        }

    }
}
