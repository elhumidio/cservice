using Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IContractRepository ContractRepository { get; }
    IContractProductRepository ContractProductRepository { get; }
    IRegEnterpriseConsumsRepository RegEnterpriseConsumsRepository { get; }
    IRegEnterpriseContractRepository RegEnterpriseContractRepository { get; }
    IEnterpriseUserJobVacRepository EnterpriseUserJobVacRepository { get; }
    ISalesforceTransactionRepository SalesforceTransRepository { get; }
    IContractPublicationRegionRepository ContractPublicationRegionRepository { get;}
    IProductRepository ProductRepository { get; }
    IEnterpriseRepository EnterpriseRepository { get; }
    IProductLineRepository ProductLineRepository { get; }

    void Commit();

    void Rollback();
}
