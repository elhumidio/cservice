namespace Domain.Enums
{
    public enum OfferStatus
    {
        Active = 1,
        Pending = 2,
        Deleted = 3
    }
    public enum VacancyType
    {
        None = -1,
        Standard = 0,
        Featured = 1,
        Combinada = 2,
        Weekly = 3,
        WelcomeSP = 6,
        StandardNP = 4,
        SimpleMes = 5,
        StandardPT = 7,
        Classic = 8,
        SuperiorPT = 9,
        StandardCredit = 10
    }
    public enum StandardWiseVacancyType
    {
        Standard = 0,
        Combinada = 2,
        StandardNP = 4,
        SimpleMes = 5,
        WelcomeSP = 6,
        StandardCredit = 10
    }

    public enum Regions
    {
        AllCountry = 60,
        Abroad = 61
    }

    public enum RegistrationType
    {
        Classic = 1,
        Express = 2
    }
    public enum EnterpriseStatus
    {
        Active = 1,
        Pending = 2,
        Cancelled = 3
    }
    public enum ResidenceType
    {
        Indiferent = 1,
        VacancyRegion = 2,
        VacancyCountry = 3,
        EuropeanUnion = 4,

    }

    public enum SalaryType
    {
        NotSpecified = 0,
        GrossMonthly = 1,
        GrossAnual = 3,
        UnpaidWork = 5,
        GrossPerHur = 6

    }
}
