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
            RuleFor(command => command.Title).NotEmpty().WithMessage("Title is mandatory field.\n").Must(HasNotHtml);
            RuleFor(command => command.Description).NotEmpty().WithMessage("Description is mandatory field.\n").Must(HasBeCleanHtml);

        }
        private bool HasNotHtml(string _title)
        {
            htmldoc.LoadHtml(_title);
            _title = htmldoc.DocumentNode.InnerText;
            return true;
        }

        private bool HasBeCleanHtml(string _text)
        {
            htmldoc.LoadHtml(_text);
            if (htmldoc.ParseErrors.Count() > 0)
                _text = htmldoc.DocumentNode.InnerText;
            return true;
        }

    }
}
