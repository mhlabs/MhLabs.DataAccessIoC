# MhLabs.DataAccessIoC

Package to help with repository pattern IoC in .NET Core WebApi projects. So far only implementation for AWS backends. Azure and GCP is to be done.

Usage:

`Install-Package MhLabs.DataAccessIoc`

## Repository
### Declarations: 
### AWS repository
`public  class YourRepository : AWSRepositoryBase<IAWSDataStoreType>, IYourRepository` where `IAWSDataStoreType` is derived from `IAmazonServcie`

### Example:
```
public  class ProductRepository : AWSRepositoryBase<IAmazonDynamoDB>, IProductRepository {
        private IDynamoDBContext _dynamoDbContext;
        public ProductRepository(IAmazonDynamoDB dataAccessClient) : base(dataAccessClient)
        {
            // We let AWS generate resoucre names, so we override the table name here instead of using the type name of the entity
            _dynamoDbContext = new DynamoDBContext(DataAccessClient, new DynamoDBOperationConfig { OverrideTableName = ResourceName });
        }

        protected override string AWSResourceKey => "ProductTable";  // <-- the key with which to resolve table name. Typically the name of an environment variable that holds the table name

        public async Task AddAsync(Product product)
        {
            await _dynamoDbContext.SaveAsync(product);
        }

        public async Task<Product> GetAsync(string id)
        {
            return (await _dynamoDbContext.QueryAsync<Product>(id).GetRemainingAsync()).FirstOrDefault();
        }
}

```

## Handler / Service
### Decalaration:
`public class YourHandlerHandler : HandlerBase<IYourRepository>`

### Example:
```
    public class ProductHandler : HandlerBase<IProductRepository>
    {
        private readonly ProductDetailHandler _productDetailHandler;

        public ProductHandler(IProductRepository repository) : base(repository)
        { }
        
        public async Task<Product> AddProductAsync(string id)
        {
            return await Repository.AddAsync(id);
        }
        
        public async Task<Product> GetProductAsync(string id)
        {
            return await Repository.GetAsync(id);
        }
    }
```

### Dependency injection

Startup.cs:
```
        public void ConfigureServices(IServiceCollection services)
        {
            [...]
            var awsOptions = Configuration.GetAWSOptions();
            awsOptions.Region = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION"));
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddAWSRepositories();
            services.AddHandlers();
            [...]
        }
```

Your handler and repository gets automatically registered in the IoC container. Currently only works with Asp.Net's built in ServiceColleton.

## Unit testing:

`Install-Package MhLabs.DataAccessIoC.Xunit`

Base class `HandlerTestBase<THandler>` auto registers your handlers in an IoC container held by the base class. Additionally it creates empty mocks of every repository in the project. Mocks are set up in constructr by calling `base.SetUpMockFor<IRepositoryType>()`

```
public class ProductHandlerTest : HandlerTestBase<ProductHandler>
{
    public ProductHandlerTest()
    {
        SetupMockFor<IProductRepository>(mock => mock.Setup(p => p.GetAsync(It.IsAny<string>())).ReturnsAsync(new Product { Id = "123" }));
    }

    [Fact]
    public async Task GetProductAsync_Test()
    {
        var product = await Handler.GetProductAsync("123");
        Assert.Equal("123", product.Id);
    }
}
```
