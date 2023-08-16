using Domain.Repositories;
using Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dbContext;

    public IContractRepository ContractRepository { get; set; }

    public IContractProductRepository ContractProductRepository { get; set; }

    public IRegEnterpriseConsumsRepository RegEnterpriseConsumsRepository { get; set; }

    public IRegEnterpriseContractRepository RegEnterpriseContractRepository { get; set; }

    public IEnterpriseUserJobVacRepository EnterpriseUserJobVacRepository { get; set; }
    public ISalesforceTransactionRepository SalesforceTransRepository { get; set; }

    public IContractPublicationRegionRepository ContractPublicationRegionRepository { get;set; }

    public IProductRepository ProductRepository { get; set; }

    public IEnterpriseRepository EnterpriseRepository { get; set; }

    public IProductLineRepository ProductLineRepository { get; set; }

    public UnitOfWork(DataContext dbContext, IContractRepository contractRepository,
        IContractProductRepository contractProductRepository,
        IRegEnterpriseContractRepository regEnterpriseContract,
        IRegEnterpriseConsumsRepository regEnterpriseConsums,
        IEnterpriseUserJobVacRepository enterpriseUserJobVacRepository,
        ISalesforceTransactionRepository salesforceTransactionRepository,
        IProductRepository productRepository,
        IEnterpriseRepository enterpriseRepository,
        IProductLineRepository productLineRepository)
    {
        _dbContext = dbContext;
        ContractRepository = contractRepository;
        ContractProductRepository = contractProductRepository;
        RegEnterpriseContractRepository = regEnterpriseContract;
        RegEnterpriseConsumsRepository = regEnterpriseConsums;
        EnterpriseUserJobVacRepository = enterpriseUserJobVacRepository;
        SalesforceTransRepository = salesforceTransactionRepository;
        ProductRepository = productRepository;
        EnterpriseRepository = enterpriseRepository;
        ProductLineRepository = productLineRepository;
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }

    public void Commit()
    {
        _dbContext.SaveChanges();
    }

    public void Rollback()
    {
        _dbContext.Dispose();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
