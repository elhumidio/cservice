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
                .Must(DescriptionMaxLength)
                .Must(IsNotShortDescriptionNull)
                .Must(RequirementsMaxLength);
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
            if (string.IsNullOrEmpty(cmd.Description))
            {
                return false;
            }
            else {
                htmldoc.LoadHtml(cmd.Description);
                if (cmd.Description.Length > 2900)
                {
                    if (htmldoc.DocumentNode.InnerText.Length <= 2900)
                        cmd.Description = htmldoc.DocumentNode.InnerText;
                    else
                        cmd.Description = htmldoc.DocumentNode.InnerText.Substring(0, 2900);
                }
                return true;
            }
           
        }

        private bool IsNotShortDescriptionNull(CreateOfferCommand cmd)
        {
            if (string.IsNullOrEmpty(cmd.ShortDescription)) {
                cmd.ShortDescription = string.Empty;
            }
            return true;    
        }


        private bool RequirementsMaxLength(CreateOfferCommand cmd)
        {
            if (string.IsNullOrEmpty(cmd.Requirements))
                return true;
            else
            {
                htmldoc.LoadHtml(cmd.Requirements);
                if (cmd.Requirements.Length > 2900)
                {
                    if (htmldoc.DocumentNode.InnerText.Length <= 2900)
                        cmd.Requirements = htmldoc.DocumentNode.InnerText;
                    else
                        cmd.Requirements = htmldoc.DocumentNode.InnerText.Substring(0, 2900);
                }
                return true;
            }
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
                .Must(DescriptionMaxLength)
                .Must(IsNotShortDescriptionNull)
                .Must(RequirementsMaxLength);
        }

        private bool IsNotShortDescriptionNull(UpdateOfferCommand cmd)
        {
            if (string.IsNullOrEmpty(cmd.ShortDescription))
            {
                cmd.ShortDescription = string.Empty;
            }
            return true;
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

        private bool RequirementsMaxLength(UpdateOfferCommand cmd)
        {
            if (string.IsNullOrEmpty(cmd.Requirements))
            {
                return true;
            }
            else
            {
                htmldoc.LoadHtml(cmd.Requirements);
                if (cmd.Requirements.Length > 2900)
                {
                    if (htmldoc.DocumentNode.InnerText.Length <= 2900)
                        cmd.Requirements = htmldoc.DocumentNode.InnerText;
                    else
                        cmd.Requirements = htmldoc.DocumentNode.InnerText.Substring(0, 2900);
                }
                return true;
            }
        }

        private bool DescriptionMaxLength(UpdateOfferCommand cmd)
        {
            if (string.IsNullOrEmpty(cmd.Description))
            {
                return false;
            }
            else {
                htmldoc.LoadHtml(cmd.Description);
                if (cmd.Description.Length > 2900)
                {
                    if (htmldoc.DocumentNode.InnerText.Length <= 2900)
                        cmd.Description = htmldoc.DocumentNode.InnerText;
                    else
                        cmd.Description = htmldoc.DocumentNode.InnerText.Substring(0, 2900);
                }
                return true;
            }
            
        }
    }
}
