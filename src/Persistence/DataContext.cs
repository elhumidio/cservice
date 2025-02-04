using API.DataContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EquestCountryState> EquestCountryStates { get; set; } = null!;
        public virtual DbSet<ContractPayment> ContractPayments { get; set; } = null!;
        public virtual DbSet<EquestDegreeEquivalent> EquestDegreeEquivalents { get; set; } = null!;
        public virtual DbSet<EquestIndustry> EquestIndustries { get; set; } = null!;
        public virtual DbSet<RegJobVacMatching> RegJobVacMatchings { get; set; } = null!;
        public virtual DbSet<CountryIso> CountryIsos { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<ContractPublicationRegion> ContractPublicationRegions { get; set; } = null!;
        public virtual DbSet<Degree> Degrees { get; set; } = null!;
        public virtual DbSet<EnterpriseBlind> EnterpriseBlinds { get; set; } = null!;
        public virtual DbSet<EnterpriseUser> EnterpriseUsers { get; set; } = null!;
        public virtual DbSet<JobCategory> JobCategories { get; set; } = null!;
        public virtual DbSet<Region> Regions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<JobContractType> JobContractTypes { get; set; } = null!;
        public virtual DbSet<ResidenceType> ResidenceTypes { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<ContractProduct> ContractProducts { get; set; } = null!;
        public virtual DbSet<EnterpriseUserJobVac> EnterpriseUserJobVacs { get; set; } = null!;
        public virtual DbSet<JobExpYear> JobExpYears { get; set; } = null!;
        public virtual DbSet<JobVacType> JobVacTypes { get; set; } = null!;
        public virtual DbSet<JobVacancy> JobVacancies { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<RegEnterpriseContract> RegEnterpriseContracts { get; set; } = null!;
        public virtual DbSet<Salary> Salaries { get; set; } = null!;
        public virtual DbSet<SalaryType> SalaryTypes { get; set; } = null!;
        public virtual DbSet<Currency> Currencies { get; set; } = null!;
        public virtual DbSet<Field> Fields { get; set; } = null!;
        public virtual DbSet<TsturijobsLang> TsturijobsLangs { get; set; } = null!;
        public virtual DbSet<TsubArea> TsubAreas { get; set; } = null!;
        public virtual DbSet<ZipCode> ZipCodes { get; set; } = null!;
        public virtual DbSet<ProductLine> ProductLines { get; set; } = null!;
        public virtual DbSet<Enterprise> Enterprises { get; set; } = null!;
        public virtual DbSet<Site> Sites { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<FeaturedJob> FeaturedJobs { get; set; } = null!;
        public virtual DbSet<Logo> Logos { get; set; } = null!;
        public virtual DbSet<ProductCountryPrice> ProductCountryPrices { get; set; } = null!;
        public virtual DbSet<RegJobVacWorkPermit> RegJobVacWorkPermits { get; set; } = null!;
        public virtual DbSet<JobVacancyLanguage> JobVacancyLanguages { get; set; } = null!;
        public virtual DbSet<AtsmanagerAdminRegion> AtsmanagerAdminRegions { get; set; } = null!;
        public virtual DbSet<AimwelCreationError> AimwelCreationErrors { get; set; } = null!;
        public virtual DbSet<TypeUser> TypeUsers { get; set; } = null!;
        public virtual DbSet<JobTitle> Titles { get; set; } = null!;
        public virtual DbSet<TitleLang> TitleLangs { get; set; } = null!;
        public virtual DbSet<TitlesRelationship> TitlesRelationships { get; set; } = null!;
        public virtual DbSet<CampaignsManagement> CampaignManagements { get; set; } = null!;
        public virtual DbSet<CampaignSetting> CampaignSettings { get; set; } = null!;
        public virtual DbSet<ManagersVisibility> ManagersVisibilities { get; set; } = null!;
        public virtual DbSet<OigSafety> OigSafeties { get; set; } = null!;
        public virtual DbSet<CampaignsUpdatingCheck> CampaignsUpdatingChecks { get; set; } = null!;
        public virtual DbSet<FeedsAggregatorsLog> FeedsAggregatorsLogs { get; set; } = null!;
        public virtual DbSet<RegEnterpriseConsum> RegEnterpriseConsums { get; set; } = null!;
        public virtual DbSet<SalesforceTransaction> SalesforceTransactions { get; set; } = null!;
        public virtual DbSet<Culture> Cultures { get; set; } = null!;
        public virtual DbSet<WP_CategoryOfferRelation> WP_CategoryOfferRelations { get; set; } = null!;
        public virtual DbSet<ContractDurationByProduct> ContractDurationByProducts { get; set; } = null!;
        public virtual DbSet<Discount> Discounts { get; set; } = null!;
        //public DbSet<NextSqlValueFeedLog> NextSeqFeedLog { get; set; }

        public virtual DbSet<ZoneUrl> ZoneUrls { get; set; } = null!;
        public virtual DbSet<InternationalDiffusionCountry> InternationalDiffusionCountries { get; set; } = null!;

        public virtual DbSet<JobTitleArea> JobTitlesAreas { get; set; } = null!;
        public virtual DbSet<JobTitleDenomination> JobTitlesDenominations { get; set; } = null!;
        public virtual DbSet<JobTitlesRelationship> JobTitlesRelationships { get; set; } = null!;

        public virtual DbSet<Benefit> Benefits { get; set; } = null!;
        public virtual DbSet<CompanyBenefit> CompanyBenefits { get; set; } = null!;
        public virtual DbSet<TranslationsWeb> TranslationsWebs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Modern_Spanish_CI_AS");

            modelBuilder.HasSequence("GetNextSequenceValueFeedsLog");

            

            modelBuilder.Entity<ContractPayment>(entity =>
            {
                entity.HasKey(e => e.IdcontractPayment);

                entity.ToTable("TContractPayment");

                entity.Property(e => e.IdcontractPayment).HasColumnName("IDContractPayment");

                entity.Property(e => e.DataPayment).HasColumnType("datetime");

                entity.Property(e => e.Idcontract).HasColumnName("IDContract");

                entity.Property(e => e.Payment).HasColumnType("decimal(11, 2)");
                entity.Property(e => e.CouponDiscount).HasColumnType("decimal(11, 2)");
                entity.Property(e => e.TaxAmount).HasColumnType("decimal(11, 2)");
                entity.Property(e => e.ConvertRate).HasColumnType("decimal(11, 2)");
                entity.Property(e => e.Currency).HasColumnType("nvarchar(10)");
                entity.Property(e => e.PaymentWithoutTax)
                            .HasColumnType("decimal(11, 2)")
                            .HasColumnName("PaymentWithoutTAX");
            });

            modelBuilder.Entity<RegEnterpriseConsum>(entity =>
            {
                entity.HasKey(e => new { e.Identerprise, e.Idcontract, e.Idproduct });

                entity.ToTable("TRegEnterpriseConsums");

                entity.Property(e => e.Identerprise).HasColumnName("IDEnterprise");

                entity.Property(e => e.Idcontract).HasColumnName("IDContract");

                entity.Property(e => e.Idproduct).HasColumnName("IDProduct");

                entity.HasOne(d => d.IdcontractNavigation)
                    .WithMany(p => p.RegEnterpriseConsums)
                    .HasForeignKey(d => d.Idcontract)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRegEnterpriseConsums_TContract");
            });

            modelBuilder.Entity<SalesforceTransaction>(entity =>
            {
                entity.HasKey(e => e.IdsalesforceTransaction);

                entity.ToTable("TSalesforceTransaction");

                entity.Property(e => e.IdsalesforceTransaction).HasColumnName("IDSalesforceTransaction");

                entity.Property(e => e.FinishDate).HasColumnType("datetime");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.ObjectTypeName).HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TurijobsId).HasColumnName("TurijobsID");
            });

            modelBuilder.Entity<FeedsAggregatorsLog>(entity =>
            {
                entity.HasKey(e => e.AutoId)
                    .HasName("PK_Feeds_Aggregators_Logs_1");

                entity.ToTable("Feeds_Aggregators_Logs");

                entity.Property(e => e.AutoId).HasColumnName("autoId");

                entity.Property(e => e.AreaId).HasColumnName("area_id");

                entity.Property(e => e.AreaName)
                    .HasMaxLength(50)
                    .HasColumnName("area_name");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.FeedName)
                    .HasMaxLength(50)
                    .HasColumnName("feed_name");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RegionId).HasColumnName("region_id");

                entity.Property(e => e.RegionName)
                    .HasMaxLength(50)
                    .HasColumnName("region_name");

                entity.Property(e => e.TotalOffers).HasColumnName("total_offers");
            });

            modelBuilder.Entity<ZoneUrl>(entity =>
            {
                entity.HasKey(e => e.Url);

                entity.ToTable("TZoneURL");

                entity.HasIndex(e => new { e.Idcity, e.Idregion }, "ix_TZoneURL_IDCity_IDRegion")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Idcountry, e.Idregion, e.Idcity }, "ix_TZoneURL_IDCountry_IDRegion_IDCity_includes")
                    .HasFillFactor(90);

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .HasColumnName("URL");

                entity.Property(e => e.Idcity).HasColumnName("IDCity");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.Idregion).HasColumnName("IDRegion");
            });
            modelBuilder.Entity<AimwelCreationError>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<OigSafety>(entity =>
            {
                entity.ToTable("OIG_safety");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Column0)
                    .HasColumnType("text")
                    .HasColumnName("Column 0");

                entity.Property(e => e.Column1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Column 1");
            });

            modelBuilder.Entity<TypeUser>(entity =>
            {
                entity.HasKey(e => new { e.IdstypeUser, e.Idsite, e.Idslanguage });

                entity.ToTable("TSTypeUser");

                entity.Property(e => e.IdstypeUser).HasColumnName("IDSTypeUser");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(50);

                entity.Property(e => e.Idbduser)
                    .HasMaxLength(7)
                    .HasColumnName("IDBDUser");
            });

            modelBuilder.Entity<CampaignsManagement>(entity =>
            {
                entity.ToTable("CampaignManagement");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Budget).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ExternalCampaignId).HasMaxLength(150);

                entity.Property(e => e.IdjobVacancy).HasColumnName("IDjobVacancy");

                entity.Property(e => e.LastModificationDate).HasColumnType("datetime");

                entity.Property(e => e.Provider).HasMaxLength(50);
            });

            modelBuilder.Entity<CampaignsUpdatingCheck>(entity =>
            {
                entity.ToTable("CampaignsUpdatingCheck");

                entity.Property(e => e.Campaign)
                    .HasMaxLength(30)
                    .IsFixedLength();
            });

            modelBuilder.Entity<CampaignSetting>(entity =>
            {
                entity.Property(e => e.Budget).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.SiteId).HasColumnName("SiteId");
            });

            modelBuilder.Entity<TitleLang>(entity =>
            {
                entity.ToTable("TitleLangs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Label)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LABEL");

                entity.Property(e => e.LanguageId).HasColumnName("LANGUAGE_ID");

                entity.Property(e => e.TitleId).HasColumnName("TITLE_ID");

                entity.HasOne(d => d.Title)
                    .WithMany()
                    .HasForeignKey(d => d.TitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TitleLangs_Titles");
            });

            modelBuilder.Entity<AtsmanagerAdminRegion>(entity =>
            {
                entity.HasKey(e => new { e.CompanyId, e.ManagerId, e.RegionId, e.CountryId });

                entity.ToTable("ATSManagerAdminRegions");

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");
            });

            modelBuilder.Entity<Culture>(entity =>
            {
                entity.ToTable("TCulture");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Actualidad).HasMaxLength(50);

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CultureName).HasMaxLength(10);

                entity.Property(e => e.Cursos).HasMaxLength(50);

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.Iso639xValue)
                    .HasMaxLength(3)
                    .HasColumnName("ISO639xValue");

                entity.Property(e => e.JobVacancyDirName).HasMaxLength(50);

                entity.Property(e => e.Language).HasMaxLength(50);

                entity.Property(e => e.Path).HasMaxLength(150);

                entity.Property(e => e.ShortName).HasMaxLength(2);

                entity.Property(e => e.UrlBase).HasMaxLength(100);
            });

            modelBuilder.Entity<ContractDurationByProduct>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.From).HasColumnName("FROM");

                entity.Property(e => e.To).HasColumnName("TO");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.From).HasColumnName("FROM");

                entity.Property(e => e.To).HasColumnName("TO");
                entity.Property(e => e.WelcomeSpecialPrice).HasColumnName("WelcomeSpecialPrice");

            });


            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Idcity)
                    .HasName("PK__TCity__36D35083D6278DDF");

                entity.ToTable("TCity");

                entity.HasIndex(e => new { e.Idcountry, e.Idregion }, "ix_TCity_IDCountry_IDRegion_includes")
                    .HasFillFactor(90);

                entity.Property(e => e.Idcity).HasColumnName("IDCity");

                entity.Property(e => e.Cmun)
                    .HasMaxLength(3)
                    .HasColumnName("CMUN");

                entity.Property(e => e.Cpro)
                    .HasMaxLength(2)
                    .HasColumnName("CPRO");

                entity.Property(e => e.Dc)
                    .HasMaxLength(1)
                    .HasColumnName("DC");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.Idregion).HasColumnName("IDRegion");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<RegJobVacWorkPermit>(entity =>
            {
                entity.HasKey(e => new { e.IdjobVacancy, e.IdworkPermit });

                entity.ToTable("TRegJobVacWorkPermit");

                entity.Property(e => e.IdjobVacancy).HasColumnName("IDJobVacancy");

                entity.Property(e => e.IdworkPermit).HasColumnName("IDWorkPermit");
            });

            modelBuilder.Entity<Logo>(entity =>
            {
                entity.HasKey(e => e.Idlogo);

                entity.ToTable("TLogo");

                entity.Property(e => e.Idlogo).HasColumnName("IDLogo");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Idbrand).HasColumnName("IDBrand");

                entity.Property(e => e.Identerprise).HasColumnName("IDEnterprise");

                entity.Property(e => e.OldIdlogo).HasColumnName("OLD_IDLogo");

                entity.Property(e => e.UrlImgBig).HasMaxLength(100);

                entity.Property(e => e.UrlImgSmall).HasMaxLength(100);
            });

            modelBuilder.Entity<EquestCountryState>(entity =>
            {
                entity.HasKey(e => e.IdcountryState);

                entity.ToTable("TEQuestCountryState");

                entity.Property(e => e.IdcountryState)
                    .HasMaxLength(10)
                    .HasColumnName("IDCountryState");

                entity.Property(e => e.EquivalentId).HasColumnName("EquivalentID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<EquestDegreeEquivalent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEQuestDegreeEquivalent");

                entity.Property(e => e.Iddegree).HasColumnName("IDDegree");

                entity.Property(e => e.IdequestDegree).HasColumnName("IDEQuestDegree");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");
            });

            modelBuilder.Entity<EquestIndustry>(entity =>
            {
                entity.HasKey(e => e.IdindustryCode)
                    .HasName("PK_TEQuestIndustryCode");

                entity.ToTable("TEQuestIndustry");

                entity.Property(e => e.IdindustryCode)
                    .ValueGeneratedNever()
                    .HasColumnName("IDIndustryCode");

                entity.Property(e => e.EquivalentId).HasColumnName("EquivalentID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<RegJobVacMatching>(entity =>
            {
                entity.HasKey(e => e.IdjobMatching);

                entity.ToTable("TRegJobVacMatching");

                entity.Property(e => e.IdjobMatching).HasColumnName("IDJobMatching");

                entity.Property(e => e.ExternalId)
                    .HasMaxLength(255)
                    .HasColumnName("ExternalID");

                entity.Property(e => e.Identerprise).HasColumnName("IDEnterprise");

                entity.Property(e => e.Idintegration).HasColumnName("IDIntegration");

                entity.Property(e => e.IdjobVacancy).HasColumnName("IDJobVacancy");
            });

            modelBuilder.Entity<Enterprise>(entity =>
            {
                entity.HasKey(e => e.Identerprise);

                entity.ToTable("TEnterprise");

                entity.Property(e => e.Identerprise).HasColumnName("IDEnterprise");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.AddressInvoicing).HasMaxLength(100);

                entity.Property(e => e.Ats)
                    .HasColumnName("ATS")
                    .HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.Atsname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ATSName");

                entity.Property(e => e.Ban)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BAN");

                entity.Property(e => e.ChkActive)
                    .IsRequired()
                    .HasColumnName("chkActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.CityInvoicing).HasMaxLength(100);

                entity.Property(e => e.CityOld)
                    .HasMaxLength(200)
                    .HasColumnName("City_Old");

                entity.Property(e => e.CompanyTaxCode).HasMaxLength(24);

                entity.Property(e => e.CompanyTaxCodeInvoicing).HasMaxLength(24);

                entity.Property(e => e.ContactInvoicing).HasMaxLength(100);

                entity.Property(e => e.CorporateName).HasMaxLength(100);

                entity.Property(e => e.CorporateNameInvoicing).HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.DescriptionMeet).HasMaxLength(200);

                entity.Property(e => e.Employees).HasComment("Employees Number");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.ExpiryReason).HasMaxLength(260);

                entity.Property(e => e.FaxNumber).HasMaxLength(20);

                entity.Property(e => e.FaxNumberInvoicing).HasMaxLength(20);

                entity.Property(e => e.Iban)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("IBAN");

                entity.Property(e => e.IdbackOfUser).HasColumnName("IDBackOfUser");

                entity.Property(e => e.Idcentral).HasColumnName("IDCentral");

                entity.Property(e => e.Idcity).HasColumnName("IDCity");

                entity.Property(e => e.IdcityInvoicing).HasColumnName("IDCityInvoicing");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.IdcountryInvoicing).HasColumnName("IDCountryInvoicing");

                entity.Property(e => e.IdenterpriseOld).HasColumnName("IDEnterpriseOld");

                entity.Property(e => e.IdenterpriseType).HasColumnName("IDEnterpriseType");

                entity.Property(e => e.Iderp).HasColumnName("IDERP");

                entity.Property(e => e.IdexpiryReason)
                    .HasColumnName("IDExpiryReason")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Idfield).HasColumnName("IDField");

                entity.Property(e => e.IdhowMeet).HasColumnName("IDHowMeet");

                entity.Property(e => e.IdlegalForm).HasColumnName("IDLegalForm");

                entity.Property(e => e.IdlegalFormInvoicing).HasColumnName("IDLegalFormInvoicing");

                entity.Property(e => e.Idregion).HasColumnName("IDRegion");

                entity.Property(e => e.IdregionInvoicing).HasColumnName("IDRegionInvoicing");

                entity.Property(e => e.Idstatus).HasColumnName("IDStatus");

                entity.Property(e => e.IdsubField).HasColumnName("IDSubField");

                entity.Property(e => e.IdzipCode).HasColumnName("IDZipCode");

                entity.Property(e => e.IdzipCodeInvoicing).HasColumnName("IDZipCodeInvoicing");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(80);

                entity.Property(e => e.NameInvoicing).HasMaxLength(80);

                entity.Property(e => e.OldIdenterprise).HasColumnName("OLD_IDEnterprise");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.PhoneNumberInvoicing).HasMaxLength(50);

                entity.Property(e => e.PrefixPhoneNumber).HasMaxLength(3);

                entity.Property(e => e.PrefixPhoneNumberInvoicing).HasMaxLength(3);

                entity.Property(e => e.Region).HasMaxLength(100);

                entity.Property(e => e.RegionInvoicing).HasMaxLength(100);

                entity.Property(e => e.SalesforceId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Sftimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("SFTimestamp");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.UrlWeb).HasMaxLength(100);

                entity.Property(e => e.Urlturijobs)
                    .HasMaxLength(50)
                    .HasColumnName("URLTurijobs");

                entity.Property(e => e.ZipCode).HasMaxLength(50);

                entity.Property(e => e.ZipCodeInvoicing).HasMaxLength(50);
            });

            modelBuilder.Entity<CountryIso>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TCountryISO");

                entity.Property(e => e.Cee).HasColumnName("CEE");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.IdcountryIso)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("IDCountryISO");

                entity.Property(e => e.Iso)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ISO");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductLine>(entity =>
            {
                entity.HasKey(e => new { e.IdproductLine, e.Idsite, e.Idslanguage });

                entity.ToTable("TProductLine");

                entity.HasIndex(e => new { e.Idslanguage, e.Idproduct, e.IdserviceType }, "ix_TProductLine_IDSLanguage_IDProduct_IDServiceType_includes")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Idslanguage, e.IdserviceType }, "ix_TProductLine_IDSLanguage_IDServiceType_includes")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Idsite, e.Idslanguage, e.IdserviceType }, "ix_TProductLine_IDSite_IDSLanguage_IDServiceType_includes")
                    .HasFillFactor(90);

                entity.Property(e => e.IdproductLine)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("IDProductLine");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.ChkConsumable).HasColumnName("chkConsumable");

                entity.Property(e => e.ChkMain)
                    .IsRequired()
                    .HasColumnName("chkMain")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdgroupType).HasColumnName("IDGroupType");

                entity.Property(e => e.IdjobVacType).HasColumnName("IDJobVacType");

                entity.Property(e => e.Idproduct).HasColumnName("IDProduct");

                entity.Property(e => e.IdserviceType).HasColumnName("IDServiceType");
            });

            modelBuilder.Entity<ProductCountryPrice>(entity =>
            {
                entity.ToTable("ProductCountryPrice");

                entity.Property(e => e.Discount).HasColumnType("int");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.Idproduct).HasColumnName("IDProduct");

                entity.Property(e => e.Price).HasColumnType("int");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.Idbrand);

                entity.ToTable("TBrand");

                entity.Property(e => e.Idbrand).HasColumnName("IDBrand");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.Identerprise).HasColumnName("IDEnterprise");

                entity.Property(e => e.Name).HasMaxLength(80);

                entity.Property(e => e.OldIdbrand).HasColumnName("OLD_IDBrand");
            });

            modelBuilder.Entity<ContractPublicationRegion>(entity =>
            {
                entity.HasKey(e => new { e.Idcontract, e.Idproduct, e.Idsite, e.Idregion });

                entity.ToTable("TContractPublicationRegion");

                entity.Property(e => e.Idcontract).HasColumnName("IDContract");

                entity.Property(e => e.Idproduct).HasColumnName("IDProduct");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idregion).HasColumnName("IDRegion");

                entity.Property(e => e.ChkActive).HasColumnName("chkActive");

                entity.Property(e => e.CreationBouserId)
                    .HasMaxLength(450)
                    .HasColumnName("CreationBOUserId");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DeactivationBouserId)
                    .HasMaxLength(450)
                    .HasColumnName("DeactivationBOUserId");

                entity.Property(e => e.DeactivationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Degree>(entity =>
            {
                entity.HasKey(e => new { e.Iddegree, e.Idsite, e.Idslanguage });

                entity.ToTable("TDegree");

                entity.Property(e => e.Iddegree).HasColumnName("IDDegree");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(50);

                entity.Property(e => e.ShortName)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<EnterpriseBlind>(entity =>
            {
                entity.HasKey(e => e.Identerprise);

                entity.ToTable("TEnterpriseBlind");

                entity.Property(e => e.Identerprise)
                    .ValueGeneratedNever()
                    .HasColumnName("IDEnterprise");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Idcity).HasColumnName("IDCity");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.Idfield).HasColumnName("IDField");

                entity.Property(e => e.Idregion).HasColumnName("IDRegion");

                entity.Property(e => e.IdsubField).HasColumnName("IDSubField");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Web).HasMaxLength(200);
            });

            modelBuilder.Entity<EnterpriseUser>(entity =>
            {
                entity.HasKey(e => e.IdenterpriseUser);

                entity.ToTable("TEnterpriseUser");

                entity.Property(e => e.IdenterpriseUser).HasColumnName("IDEnterpriseUser");

                entity.Property(e => e.ChkMarketing)
                    .HasColumnName("chkMarketing")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChkNotifications)
                    .HasColumnName("chkNotifications")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChkPromos)
                    .HasColumnName("chkPromos")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ContactName).HasMaxLength(100);

                entity.Property(e => e.ContactPhone).HasMaxLength(50);

                entity.Property(e => e.ContactPosition).HasMaxLength(50);

                entity.Property(e => e.ContactSurname)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Identerprise).HasColumnName("IDEnterprise");

                entity.Property(e => e.Idperiod)
                    .HasColumnName("IDPeriod")
                    .HasDefaultValueSql("((7))");

                entity.Property(e => e.Idsuser).HasColumnName("IDSUser");

                entity.Property(e => e.PrefixContactPhone).HasMaxLength(3);

                entity.Property(e => e.SalesforceId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Sftimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("SFTimestamp");
            });

            modelBuilder.Entity<JobCategory>(entity =>
            {
                entity.HasKey(e => new { e.IdjobCategory, e.Idsite, e.Idslanguage })
                    .HasName("PK_TWorkCategory");

                entity.ToTable("TJobCategory");

                entity.Property(e => e.IdjobCategory).HasColumnName("IDJobCategory");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(100);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => new { e.Idregion, e.Idcountry, e.Idsite, e.Idslanguage });

                entity.ToTable("TRegion");

                entity.HasIndex(e => new { e.Idcountry, e.Idsite, e.Idslanguage, e.BaseName }, "ix_TRegion_IDCountry_IDSite_IDSLanguage_BaseName")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Idcountry, e.Idsite, e.Idslanguage, e.BaseName }, "ix_TRegion_IDCountry_IDSite_IDSLanguage_BaseName_includes")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Idcountry, e.Idsite, e.Idslanguage, e.Idregion }, "ix_TRegion_IDCountry_IDSite_IDSLanguage_IDRegion_includes")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Idcountry, e.Idsite, e.Idslanguage }, "ix_TRegion_IDCountry_IDSite_IDSLanguage_includes")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Idsite, e.Idregion }, "ix_TRegion_IDSite_IDRegion_includes")
                    .HasFillFactor(90);

                entity.Property(e => e.Idregion).HasColumnName("IDRegion");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(50);

                entity.Property(e => e.Ccaa)
                    .HasMaxLength(50)
                    .HasColumnName("CCAA");

                entity.Property(e => e.ChkActive).HasColumnName("chkActive");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Idsuser);

                entity.ToTable("TSUser");

                entity.HasIndex(e => e.Email, "IDX_TSUser_mail");

                entity.HasIndex(e => e.Idslanguage, "IX_TSUser_IDSLanguage");

                entity.HasIndex(e => e.ChkActive, "IX_TSUser_chkActive");

                entity.HasIndex(e => new { e.Idsuser, e.ChkActive }, "_dta_index_TSUser_6_536388980__K1_K12");

                entity.HasIndex(e => new { e.ChkActive, e.CreationDate }, "ix_TSUser_chkActive_CreationDate_includes")
                    .HasFillFactor(90);

                entity.Property(e => e.Idsuser).HasColumnName("IDSUser");

                entity.Property(e => e.Campaign).HasMaxLength(100);

                entity.Property(e => e.ChkActive)
                    .IsRequired()
                    .HasColumnName("chkActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChkEmailMarketing)
                    .IsRequired()
                    .HasColumnName("chkEmailMarketing")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChkNewsLetterCand).HasColumnName("chkNewsLetterCand");

                entity.Property(e => e.ChkNewsLetterEnt).HasColumnName("chkNewsLetterEnt");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.EncryptionAlgorithm)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.IdexpiryReason).HasColumnName("IDExpiryReason");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.IdslanguagePref).HasColumnName("IDSLanguagePref");

                entity.Property(e => e.IdstypeUser).HasColumnName("IDSTypeUser");

                entity.Property(e => e.Keyword).HasMaxLength(100);

                entity.Property(e => e.LastLoggin).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Medium).HasMaxLength(100);

                entity.Property(e => e.NumberRetry).HasDefaultValueSql("((3))");

                entity.Property(e => e.OldIdsuser).HasColumnName("OLD_IDSUser");

                entity.Property(e => e.OptoutHash)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.Source).HasMaxLength(100);

                entity.Property(e => e.WordOfPass).HasMaxLength(160);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => new { e.Idcountry, e.Idsite, e.Idslanguage });

                entity.ToTable("TCountry");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(50);

                entity.Property(e => e.FormatCandidateDoc1).HasMaxLength(12);

                entity.Property(e => e.FormatZipCode)
                    .HasMaxLength(12)
                    .IsFixedLength();

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.PhoneIntPref).HasMaxLength(5);
            });

            modelBuilder.Entity<JobContractType>(entity =>
            {
                entity.HasKey(e => new { e.IdjobContractType, e.Idsite, e.Idslanguage });

                entity.ToTable("TJobContractType");

                entity.Property(e => e.IdjobContractType).HasColumnName("IDJobContractType");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(30);
            });

            modelBuilder.Entity<ResidenceType>(entity =>
            {
                entity.HasKey(e => new { e.IdresidenceType, e.Idsite, e.Idslanguage });

                entity.ToTable("TResidenceType");

                entity.Property(e => e.IdresidenceType).HasColumnName("IDResidenceType");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(50);
            });

            //     OnModelCreatingPartial(modelBuilder);
            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.Idcontract);

                entity.ToTable("TContract");

                entity.Property(e => e.Idcontract).HasColumnName("IDContract");

                entity.Property(e => e.AddressInvoicing).HasMaxLength(500);

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CancelDate).HasColumnType("datetime");

                entity.Property(e => e.ChkApproved).HasColumnName("chkApproved");

                entity.Property(e => e.ChkCancel).HasColumnName("chkCancel");

                entity.Property(e => e.ChkContratExtension).HasColumnName("chkContratExtension");

                entity.Property(e => e.ChkPayFract).HasColumnName("chkPayFract");

                entity.Property(e => e.ChkPromotionalCode).HasColumnName("chkPromotionalCode");

                entity.Property(e => e.Comment).HasMaxLength(1000);

                entity.Property(e => e.CompanyTaxCodeInvoicing).HasMaxLength(20);

                entity.Property(e => e.Concept).HasMaxLength(1000);

                entity.Property(e => e.ContactInvoicing).HasMaxLength(100);

                entity.Property(e => e.ContractDate).HasColumnType("datetime");

                entity.Property(e => e.CorporateNameInvoicing).HasMaxLength(100);

                entity.Property(e => e.DiscPromotion)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.FaxNumberInvoicing).HasMaxLength(20);

                entity.Property(e => e.FinalPrice).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.FinishDate).HasColumnType("datetime");

                entity.Property(e => e.IdbackOfUser)
                    .HasColumnName("IDBackOfUser")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdcancelReason).HasColumnName("IDCancelReason");

                entity.Property(e => e.Idcode).HasColumnName("IDCode");

                entity.Property(e => e.IdcontractParent).HasColumnName("IDContractParent");

                entity.Property(e => e.Idcurrency).HasColumnName("IDCurrency");

                entity.Property(e => e.Identerprise).HasColumnName("IDEnterprise");

                entity.Property(e => e.IdenterpriseUser).HasColumnName("IDEnterpriseUser");

                entity.Property(e => e.IdpayMethod).HasColumnName("IDPayMethod");

                entity.Property(e => e.OldIdcontract).HasColumnName("OLD_IDContract");

                entity.Property(e => e.PhoneNumberInvoicing).HasMaxLength(20);

                entity.Property(e => e.Price).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.SalesforceId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Sftimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("SFTimestamp");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.CheckoutSessionId).HasColumnName("CheckoutSessionId");

                entity.HasOne(d => d.IdcontractParentNavigation)
                    .WithMany(p => p.InverseIdcontractParentNavigation)
                    .HasForeignKey(d => d.IdcontractParent)
                    .HasConstraintName("FK_TContract_TContractParent");
            });

            modelBuilder.Entity<ContractProduct>(entity =>
            {
                entity.HasKey(e => new { e.Idcontract, e.Idproduct })
                    .HasName("PK_TContractProductPack");

                entity.ToTable("TContractProduct");

                entity.Property(e => e.Idcontract).HasColumnName("IDContract");

                entity.Property(e => e.Idproduct).HasColumnName("IDProduct");

                entity.Property(e => e.ChkAllEnteprise).HasColumnName("chkAllEnteprise");

                entity.Property(e => e.CommercialDiscount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CouponDiscount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Idpromotion).HasColumnName("IDPromotion");

                entity.Property(e => e.IdsalesForce)
                    .HasMaxLength(100)
                    .HasColumnName("IDSalesForce");

                entity.Property(e => e.Price).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.ProductContractDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdcontractNavigation)
                    .WithMany(p => p.ContractProducts)
                    .HasForeignKey(d => d.Idcontract)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TContractProduct_TContract");
            });

            modelBuilder.Entity<EnterpriseUser>(entity =>
            {
                entity.HasKey(e => e.IdenterpriseUser);

                entity.ToTable("TEnterpriseUser");

                entity.Property(e => e.IdenterpriseUser).HasColumnName("IDEnterpriseUser");

                entity.Property(e => e.ChkMarketing)
                    .HasColumnName("chkMarketing")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChkNotifications)
                    .HasColumnName("chkNotifications")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChkPromos)
                    .HasColumnName("chkPromos")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ContactName).HasMaxLength(100);

                entity.Property(e => e.ContactPhone).HasMaxLength(50);

                entity.Property(e => e.ContactPosition).HasMaxLength(50);

                entity.Property(e => e.ContactSurname)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Identerprise).HasColumnName("IDEnterprise");

                entity.Property(e => e.Idperiod)
                    .HasColumnName("IDPeriod")
                    .HasDefaultValueSql("((7))");

                entity.Property(e => e.Idsuser).HasColumnName("IDSUser");

                entity.Property(e => e.PrefixContactPhone).HasMaxLength(3);

                entity.Property(e => e.SalesforceId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Sftimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("SFTimestamp");
            });

            modelBuilder.Entity<ManagersVisibility>(entity =>
            {
                entity.ToTable("ManagersVisibility");
            });

            modelBuilder.Entity<EnterpriseUserJobVac>(entity =>
            {
                entity.HasKey(e => new { e.IdenterpriseUser, e.IdjobVacType, e.Idproduct, e.Idcontract })
                    .HasName("PK__TEnterpr__29D0DE14BC2D1AC7");

                entity.ToTable("TEnterpriseUserJobVac");

                entity.Property(e => e.IdenterpriseUser).HasColumnName("IDEnterpriseUser");

                entity.Property(e => e.IdjobVacType).HasColumnName("IDJobVacType");

                entity.Property(e => e.Idproduct).HasColumnName("IDProduct");

                entity.Property(e => e.Idcontract).HasColumnName("IDContract");

                entity.Property(e => e.IdjobVacTypeComp)
                    .HasColumnName("IDJobVacTypeComp")
                    .HasComputedColumnSql("(case when [IDJobVacType]=(1) then [IDJobVacType] when [IDJobVacType]=(0) then [IDJobVacType] when [IDJobVacType]=(2) then (0) when [IDJobVacType]=(4) then (0) when [IDJobVacType]=(5) then (0) when [IDJobVacType]=(6) then (0) when [IDJobVacType]=(7) then (0) when [IDJobVacType]=(8) then (0) when [IDJobVacType]=(9) then (1) when [IDJobVacType]=(10) then (0)  end)", false);
            });

            modelBuilder.Entity<JobExpYear>(entity =>
            {
                entity.HasKey(e => new { e.IdjobExpYears, e.Idsite, e.Idslanguage });

                entity.ToTable("TJobExpYears");

                entity.Property(e => e.IdjobExpYears).HasColumnName("IDJobExpYears");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(50);

                entity.Property(e => e.BaseNameShort).HasMaxLength(20);

                entity.HasOne(d => d.Ids)
                    .WithMany(p => p.TjobExpYears)
                    .HasForeignKey(d => new { d.Idslanguage, d.Idsite })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TJobExpYears_TSTuriJobsLang");
            });

            modelBuilder.Entity<JobVacType>(entity =>
            {
                entity.HasKey(e => new { e.IdjobVacType, e.Idsite, e.Idslanguage });

                entity.ToTable("TJobVacType");

                entity.Property(e => e.IdjobVacType).HasColumnName("IDJobVacType");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(50);

                entity.HasOne(d => d.Ids)
                    .WithMany(p => p.TjobVacTypes)
                    .HasForeignKey(d => new { d.Idslanguage, d.Idsite })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TJobVacType_TSTuriJobsLang");
            });

            modelBuilder.Entity<JobVacancy>(entity =>
            {
                entity.HasKey(e => e.IdjobVacancy);

                entity.ToTable("TJobVacancy");

                entity.HasIndex(e => e.Idarea, "IX_TJobVacancy_IDArea");

                entity.HasIndex(e => e.Idbrand, "IX_TJobVacancy_IDBrand");

                entity.HasIndex(e => e.Idcontract, "IX_TJobVacancy_IDContract");

                entity.HasIndex(e => e.Identerprise, "IX_TJobVacancy_IDEnterprise");

                entity.HasIndex(e => e.IdenterpriseUserG, "IX_TJobVacancy_IDEnterpriseUserG");

                entity.HasIndex(e => e.IdenterpriseUserLastMod, "IX_TJobVacancy_IDEnterpriseUserLastMod");

                entity.HasIndex(e => e.IdjobExpYears, "IX_TJobVacancy_IDJobExpYears");

                entity.HasIndex(e => e.IdjobVacType, "IX_TJobVacancy_IDJobVacType");

                entity.HasIndex(e => e.Idregion, "IX_TJobVacancy_IDRegion");

                entity.HasIndex(e => new { e.Identerprise, e.IdjobVacancy, e.Idquest }, "_dta_index_TJobVacancy_6_1115203073__K3_K1_K8");

                entity.HasIndex(e => new { e.Idquest, e.IdjobVacancy, e.Identerprise }, "_dta_index_TJobVacancy_6_1115203073__K8_K1_K3");

                entity.HasIndex(e => new { e.Idarea, e.ChkFilled, e.ChkDeleted, e.FinishDate }, "ix_TJobVacancy_IDArea_chkFilled_chkDeleted_FinishDate_includes")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Idarea, e.ChkFilled, e.ChkDeleted, e.Idstatus, e.IdjobVacancy, e.FinishDate }, "ix_TJobVacancy_IDArea_chkFilled_chkDeleted_IDStatus_IDJobVacancy_FinishDate_includes")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Identerprise, e.ChkBlindVac, e.ChkFilled, e.ChkDeleted, e.Idstatus, e.FinishDate }, "ix_TJobVacancy_IDEnterprise_chkBlindVac_chkFilled_chkDeleted_IDStatus_FinishDate")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Identerprise, e.ChkDeleted }, "ix_TJobVacancy_IDEnterprise_chkDeleted")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Identerprise, e.ChkFilled, e.ChkDeleted, e.Idstatus }, "ix_TJobVacancy_IDEnterprise_chkFilled_chkDeleted_IDStatus")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.IdjobVacType, "ix_TJobVacancy_IDJobVacType_includes")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.Idregion, e.Idarea, e.ChkFilled }, "ix_TJobVacancy_IDRegion_IDArea_chkFilled")
                    .HasFillFactor(90);

                entity.HasIndex(e => e.PublicationDate, "ix_TJobVacancy_PublicationDate_includes")
                    .HasFillFactor(90);

                entity.HasIndex(e => new { e.ChkBlindVac, e.ChkFilled, e.ChkDeleted, e.FinishDate }, "ix_TJobVacancy_chkBlindVac_chkFilled_chkDeleted_FinishDate_includes")
                    .HasFillFactor(90);

                entity.Property(e => e.IdjobVacancy).HasColumnName("IDJobVacancy");

                entity.Property(e => e.ChkBlindSalary).HasColumnName("chkBlindSalary");

                entity.Property(e => e.ChkBlindVac).HasColumnName("chkBlindVac");

                entity.Property(e => e.ChkColor).HasColumnName("chkColor");

                entity.Property(e => e.ChkDeleted).HasColumnName("chkDeleted");

                entity.Property(e => e.ChkDestacarAreaProv).HasColumnName("chkDestacarAreaProv");

                entity.Property(e => e.ChkDisability)
                    .HasColumnName("chkDisability")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ChkEnterpriseVisible)
                    .IsRequired()
                    .HasColumnName("chkEnterpriseVisible")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChkFilled).HasColumnName("chkFilled");

                entity.Property(e => e.ChkPack).HasColumnName("chkPack");

                entity.Property(e => e.ChkSynerquia).HasColumnName("chkSynerquia");

                entity.Property(e => e.ChkUpdateDate).HasColumnName("chkUpdateDate");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.CityOld)
                    .HasMaxLength(200)
                    .HasColumnName("City_Old");

                entity.Property(e => e.DailyJobFinishDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(2500);

                entity.Property(e => e.ExtensionDays).HasDefaultValueSql("((0))");

                entity.Property(e => e.ExternalUrl)
                    .HasMaxLength(1000)
                    .HasColumnName("externalURL");

                entity.Property(e => e.FilledDate).HasColumnType("datetime");

                entity.Property(e => e.FinishDate).HasColumnType("datetime");

                entity.Property(e => e.ForeignZipCode).HasMaxLength(20);

                entity.Property(e => e.Idarea).HasColumnName("IDArea");

                entity.Property(e => e.Idbrand).HasColumnName("IDBrand");

                entity.Property(e => e.Idcity).HasColumnName("IDCity");

                entity.Property(e => e.Idcontract).HasColumnName("IDContract");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.Iddegree).HasColumnName("IDDegree");

                entity.Property(e => e.Identerprise).HasColumnName("IDEnterprise");

                entity.Property(e => e.IdenterpriseUserG).HasColumnName("IDEnterpriseUserG");

                entity.Property(e => e.IdenterpriseUserLastMod).HasColumnName("IDEnterpriseUserLastMod");

                entity.Property(e => e.IdjobCategory).HasColumnName("IDJobCategory");

                entity.Property(e => e.IdjobContractType).HasColumnName("IDJobContractType");

                entity.Property(e => e.IdjobExpYears).HasColumnName("IDJobExpYears");

                entity.Property(e => e.IdjobRegType)
                    .HasColumnName("IDJobRegType")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdjobVacState).HasColumnName("IDJobVacState");

                entity.Property(e => e.IdjobVacType).HasColumnName("IDJobVacType");

                entity.Property(e => e.IdjobVacancyOld).HasColumnName("IDJobVacancyOld");

                entity.Property(e => e.Idquest).HasColumnName("IDQuest");

                entity.Property(e => e.IdquestRegistState).HasColumnName("IDQuestRegistState");

                entity.Property(e => e.Idregion).HasColumnName("IDRegion");

                entity.Property(e => e.IdresidenceType).HasColumnName("IDResidenceType");

                entity.Property(e => e.IdsalaryType).HasColumnName("IDSalaryType");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idstatus).HasColumnName("IDStatus");

                entity.Property(e => e.IdsubArea).HasColumnName("IDSubArea");

                entity.Property(e => e.IdworkDayType).HasColumnName("IDWorkDayType");

                entity.Property(e => e.IdworkPermit).HasColumnName("IDWorkPermit");

                entity.Property(e => e.IdzipCode).HasColumnName("IDZipCode");

                entity.Property(e => e.JobLocation).HasMaxLength(100);

                entity.Property(e => e.LastVisitorDate).HasColumnType("datetime");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.PublicationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Requirements)
                    .HasMaxLength(2000)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SalaryMax).HasMaxLength(20);

                entity.Property(e => e.SalaryMin).HasMaxLength(20);

                entity.Property(e => e.ScheduleTime).HasMaxLength(200);

                entity.Property(e => e.ShortDescription).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(300);

                entity.Property(e => e.UpdatingDate).HasColumnType("datetime");

                entity.Property(e => e.Isco).HasColumnName("Isco");
                entity.Property(e => e.TitleId).HasColumnName("TitleId");
                entity.Property(e => e.TitleDenominationId).HasColumnName("TitleDenominationId");

                entity.Property(e => e.NationalDiffusion).HasColumnName("NationalDiffusion");
                entity.Property(e => e.InternationalDiffusion).HasColumnName("InternationalDiffusion");

                entity.Property(e => e.Latitude).HasColumnType("decimal(10, 7)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(10, 7)");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => new { e.Idproduct, e.Idsite, e.Idslanguage });

                entity.ToTable("TProduct");

                entity.Property(e => e.Idproduct).HasColumnName("IDProduct");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.Atsrestricted).HasColumnName("ATSRestricted");

                entity.Property(e => e.BaseName).HasMaxLength(100);

                entity.Property(e => e.ChkAvailable)
                    .IsRequired()
                    .HasColumnName("chkAvailable")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChkEmptyService).HasColumnName("chkEmptyService");

                entity.Property(e => e.ChkExtension).HasColumnName("chkExtension");

                entity.Property(e => e.ChkMainForShop).HasColumnName("chkMainForShop");

                entity.Property(e => e.ChkOfferForService).HasColumnName("chkOfferForService");

                entity.Property(e => e.ChkPack).HasColumnName("chkPack");

                entity.Property(e => e.ChkPostAjob).HasColumnName("chkPostAJob");

                entity.Property(e => e.ChkService).HasColumnName("chkService");

                entity.Property(e => e.ChkShopCv).HasColumnName("chkShopCV");

                entity.Property(e => e.ChkShopOnline).HasColumnName("chkShopOnline");

                entity.Property(e => e.ChkWelcome)
                    .HasColumnName("chkWelcome")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.Idcurrency).HasColumnName("IDCurrency");

                entity.Property(e => e.IdgroupForShop).HasColumnName("IDGroupForShop");

                entity.Property(e => e.Idregion).HasColumnName("IDRegion");

                entity.Property(e => e.Price).HasColumnType("decimal(7, 2)");

                entity.HasOne(d => d.Ids)
                    .WithMany(p => p.Tproducts)
                    .HasForeignKey(d => new { d.Idslanguage, d.Idsite })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TProduct_TSTuriJobsLang");
            });

            modelBuilder.Entity<RegEnterpriseContract>(entity =>
            {
                entity.HasKey(e => new { e.Identerprise, e.Idcontract, e.IdjobVacType });

                entity.ToTable("TRegEnterpriseContract");

                entity.Property(e => e.Identerprise).HasColumnName("IDEnterprise");

                entity.Property(e => e.Idcontract).HasColumnName("IDContract");

                entity.Property(e => e.IdjobVacType).HasColumnName("IDJobVacType");

                entity.Property(e => e.IdjobVacTypeComp)
                    .HasColumnName("IDJobVacTypeComp")
                    .HasComputedColumnSql("(case when [IDJobVacType]=(1) then [IDJobVacType] when [IDJobVacType]=(0) then [IDJobVacType] when [IDJobVacType]=(2) then (0) when [IDJobVacType]=(4) then (0) when [IDJobVacType]=(5) then (0) when [IDJobVacType]=(6) then (0) when [IDJobVacType]=(7) then (0) when [IDJobVacType]=(8) then (0) when [IDJobVacType]=(9) then (1) when [IDJobVacType]=(10) then (0)  end)", false);

                entity.HasOne(d => d.IdcontractNavigation)
                    .WithMany(p => p.RegEnterpriseContracts)
                    .HasForeignKey(d => d.Idcontract)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRegEnterpriseContract_TContract");
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.HasKey(e => new { e.Idsalary, e.Idsite, e.Idslanguage });

                entity.ToTable("TSalary");

                entity.Property(e => e.Idsalary)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("IDSalary");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.ChkActive).HasColumnName("chkActive");

                entity.Property(e => e.Idcurrency).HasColumnName("IDCurrency");

                entity.Property(e => e.IdsalaryType).HasColumnName("IDSalaryType");

                entity.Property(e => e.Value).HasMaxLength(20);

                entity.HasOne(d => d.Ids)
                    .WithMany(p => p.Tsalaries)
                    .HasForeignKey(d => new { d.IdsalaryType, d.Idsite, d.Idslanguage })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TSalary_TSalaryType");
            });

            modelBuilder.Entity<SalaryType>(entity =>
            {
                entity.HasKey(e => new { e.IdsalaryType, e.Idsite, e.Idslanguage });

                entity.ToTable("TSalaryType");

                entity.Property(e => e.IdsalaryType).HasColumnName("IDSalaryType");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(30);

                entity.Property(e => e.ChkActive).HasColumnName("chkActive");

                entity.HasOne(d => d.Ids)
                    .WithMany(p => p.TsalaryTypes)
                    .HasForeignKey(d => new { d.Idslanguage, d.Idsite })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TSalaryType_TSTuriJobsLang");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => new { e.IDCurrency, e.IDSite, e.IDSLanguage });

                entity.ToTable("TCurrency");

                entity.Property(e => e.IDCurrency);

                entity.Property(e => e.IDSite);

                entity.Property(e => e.IDSLanguage);

                entity.Property(e => e.BaseName).HasMaxLength(30);
            });

            modelBuilder.Entity<Field>(entity =>
            {
                entity.HasKey(e => new { e.IDField, e.IDSite, e.IDSLanguage });

                entity.ToTable("TField");

                entity.Property(e => e.IDField);

                entity.Property(e => e.IDSite);

                entity.Property(e => e.IDSLanguage);

                entity.Property(e => e.BaseName).HasMaxLength(30);

                entity.Property(e => e.IDMacroSector);
            });

            modelBuilder.Entity<TsturijobsLang>(entity =>
            {
                entity.HasKey(e => new { e.IdsturiJobsLang, e.Idsite });

                entity.ToTable("TSTurijobsLang");

                entity.Property(e => e.IdsturiJobsLang).HasColumnName("IDSTuriJobsLang");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.LangName).HasMaxLength(50);

                entity.Property(e => e.SysMsgLangId).HasColumnName("sysMsgLangId");
            });

            modelBuilder.Entity<TsubArea>(entity =>
            {
                entity.HasKey(e => new { e.IdsubArea, e.Idsite, e.Idslanguage });

                entity.ToTable("TSubArea");

                entity.Property(e => e.IdsubArea).HasColumnName("IDSubArea");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(50);

                entity.Property(e => e.Idarea).HasColumnName("IDArea");

                entity.HasOne(d => d.Id)
                    .WithMany(p => p.TsubAreas)
                    .HasForeignKey(d => new { d.Idarea, d.Idsite, d.Idslanguage })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TSubArea_TArea");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Idsuser);

                entity.ToTable("TSUser");

                entity.HasIndex(e => e.Email, "IDX_TSUser_mail");

                entity.HasIndex(e => e.Idslanguage, "IX_TSUser_IDSLanguage");

                entity.HasIndex(e => e.ChkActive, "IX_TSUser_chkActive");

                entity.HasIndex(e => new { e.Idsuser, e.ChkActive }, "_dta_index_TSUser_6_536388980__K1_K12");

                entity.HasIndex(e => new { e.ChkActive, e.CreationDate }, "ix_TSUser_chkActive_CreationDate_includes")
                    .HasFillFactor(90);

                entity.Property(e => e.Idsuser).HasColumnName("IDSUser");

                entity.Property(e => e.Campaign).HasMaxLength(100);

                entity.Property(e => e.ChkActive)
                    .IsRequired()
                    .HasColumnName("chkActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChkEmailMarketing)
                    .IsRequired()
                    .HasColumnName("chkEmailMarketing")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ChkNewsLetterCand).HasColumnName("chkNewsLetterCand");

                entity.Property(e => e.ChkNewsLetterEnt).HasColumnName("chkNewsLetterEnt");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.EncryptionAlgorithm)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.IdexpiryReason).HasColumnName("IDExpiryReason");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.IdslanguagePref).HasColumnName("IDSLanguagePref");

                entity.Property(e => e.IdstypeUser).HasColumnName("IDSTypeUser");

                entity.Property(e => e.Keyword).HasMaxLength(100);

                entity.Property(e => e.LastLoggin).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Medium).HasMaxLength(100);

                entity.Property(e => e.NumberRetry).HasDefaultValueSql("((3))");

                entity.Property(e => e.OldIdsuser).HasColumnName("OLD_IDSUser");

                entity.Property(e => e.OptoutHash)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.Source).HasMaxLength(100);

                entity.Property(e => e.WordOfPass).HasMaxLength(160);
            });

            modelBuilder.Entity<JobVacancyLanguage>(entity =>
            {
                entity.HasKey(e => new { e.IdjobVacancy, e.Idlanguage })
                    .HasName("PK__TJobVaca__31F90E4541FFF41D");

                entity.ToTable("TJobVacancyLanguage");

                entity.Property(e => e.IdjobVacancy).HasColumnName("IDJobVacancy");

                entity.Property(e => e.Idlanguage)
                    .HasColumnName("IDLanguage")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdlangLevel)
                    .HasColumnName("IDLangLevel")
                    .HasDefaultValueSql("((-1))");
            });

            modelBuilder.Entity<ZipCode>(entity =>
            {
                entity.HasKey(e => e.IdzipCode);

                entity.ToTable("TZipCode");

                entity.Property(e => e.IdzipCode).HasColumnName("IDZipCode");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Idcity).HasColumnName("IDCity");

                entity.Property(e => e.Idcountry).HasColumnName("IDCountry");

                entity.Property(e => e.Idregion).HasColumnName("IDRegion");

                entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Zip).HasMaxLength(20);
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.HasKey(e => new { e.Idsite });

                entity.ToTable("TSite");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.BaseName).HasMaxLength(20);
            });

            modelBuilder.Entity<FeaturedJob>(entity =>
            {
                entity.ToTable("TFeaturedJob");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Url).HasColumnName("URL");
            });

            modelBuilder.Entity<WP_CategoryOfferRelation>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.RelationId });

                entity.ToTable("WP_CategoryOfferRelation");

                entity.Property(e => e.CategoryId);

                entity.Property(e => e.RelationId);

                entity.Property(e => e.RelationTypeId);
            });

            modelBuilder.Entity<InternationalDiffusionCountry>(entity =>
            {
                entity.ToTable("InternationalDiffusionCountry");
            });

            modelBuilder.Entity<Benefit>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.TranslationId).HasColumnName("translation_id");

                entity.HasOne(d => d.Translation)
                    .WithMany(p => p.Benefits)
                    .HasForeignKey(d => d.TranslationId)
                    .HasConstraintName("FK_translation_id");
            });

            modelBuilder.Entity<CompanyBenefit>(entity =>
            {
                entity.HasKey(e => new { e.CompanyId, e.BenefitId });

                entity.ToTable("Company_Benefits");

                entity.HasIndex(e => e.CompanyId, "IX_Company_Benefits");

                entity.Property(e => e.CompanyId).HasColumnName("Company_Id");

                entity.Property(e => e.BenefitId).HasColumnName("Benefit_Id");

                entity.HasOne(d => d.Benefit)
                    .WithMany(p => p.CompanyBenefits)
                    .HasForeignKey(d => d.BenefitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Company_Benefits_Benefits");
            });

            modelBuilder.Entity<TranslationsWeb>(entity =>
            {
                entity.ToTable("TranslationsWeb");

                entity.HasIndex(e => new { e.TextName, e.TextScope }, "unique_constraint_name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TextName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TextScope)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            }); modelBuilder.Entity<JobTitle>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.FkTranslationDescription).HasColumnName("FK_TranslationDescription");

                entity.Property(e => e.Isco08)
                    .HasMaxLength(20)
                    .HasColumnName("ISCO08")
                    .IsFixedLength();

                entity.Property(e => e.Isco88)
                    .HasMaxLength(20)
                    .HasColumnName("ISCO88")
                    .IsFixedLength();
            });

            modelBuilder.Entity<JobTitleArea>(entity =>
            {
                entity.HasKey(e => new { e.FkJobTitleId, e.FkAreaId });

                entity.ToTable("JobTitles_Areas");

                entity.Property(e => e.FkJobTitleId).HasColumnName("FK_JobTitleID");

                entity.Property(e => e.FkAreaId).HasColumnName("FK_AreaID");

                entity.Property(e => e.FkIdsite).HasColumnName("FK_IDSite");

                entity.Property(e => e.FkIdslanguage).HasColumnName("FK_IDSLanguage");

                entity.HasOne(d => d.FkJobTitle)
                    .WithMany(p => p.JobTitlesAreas)
                    .HasForeignKey(d => d.FkJobTitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobTitles_Areas_JobTitles");

                entity.HasOne(d => d.Fk)
                    .WithMany(p => p.JobTitlesAreas)
                    .HasForeignKey(d => new { d.FkAreaId, d.FkIdsite, d.FkIdslanguage })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobTitles_Areas_TArea");
            });

            modelBuilder.Entity<JobTitleDenomination>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Denomination).HasMaxLength(80);

                entity.Property(e => e.FkJobTitle).HasColumnName("FK_JobTitle");

                entity.Property(e => e.LanguageId).HasDefaultValueSql("((7))");

                entity.HasOne(d => d.FkJobTitleNavigation)
                    .WithMany(p => p.JobTitlesDenominations)
                    .HasForeignKey(d => d.FkJobTitle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobTitlesDenominations_JobTitles");
            });

            modelBuilder.Entity<JobTitlesRelationship>(entity =>
            {
                entity.HasKey(e => new { e.FkJobTitleId, e.FkJobTitleRelatedId });

                entity.Property(e => e.FkJobTitleId).HasColumnName("FK_JobTitleID");

                entity.Property(e => e.FkJobTitleRelatedId).HasColumnName("FK_JobTitleRelatedID");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => new { e.Idarea, e.Idsite, e.Idslanguage });

                entity.ToTable("TArea");

                entity.Property(e => e.Idarea).HasColumnName("IDArea");

                entity.Property(e => e.Idsite).HasColumnName("IDSite");

                entity.Property(e => e.Idslanguage).HasColumnName("IDSLanguage");

                entity.Property(e => e.BaseName).HasMaxLength(50);

                entity.Property(e => e.ChkActive).HasColumnName("chkActive");

                entity.Property(e => e.FkTranslationId).HasColumnName("FK_TranslationID");

                entity.Property(e => e.Subdomain).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
