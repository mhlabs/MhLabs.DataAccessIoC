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
            _dynamoDbContext = new DynamoDBContext(DataAccessClient);
        }

        protected override string AWSResourceKey => "ProductTable";  // <-- the key with which to resolve table name. Typically the name of an environment variable that holds the table name

        public async Task AddAsync(Product product)
        {
            await _dynamoDbContext.SaveAsync(product, new DynamoDBOperationConfig { OverrideTableName = ResourceName });
        }

        public async Task<Product> GetAsync(string id)
        {
            return await _dynamoDbContext.LoadAsync<Product>(id, new DynamoDBOperationConfig { OverrideTableName = ResourceName });
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

        public async Task AddProductAsync(Product product)
        {
            return await Repository.AddAsync(product);
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
            services.AddAWSRepositories<Startup>();
            services.AddHandlers<Startup>();
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
## Pushing a new version
Set the `Version` number in the <a href="https://github.com/mhlabs/MhLabs.DataAccessIoC/blob/master/MhLabs.DataAccessIoC/MhLabs.DataAccessIoC.csproj"> .csproj-file</a> before pushing. If an existing version is pushed the <a href="https://github.com/mhlabs/MhLabs.DataAccessIoC/actions">build will fail</a>.

## Publish pre-release packages on branches to allow us to test the package without merging to master
1. Create a new branch
2. Update `Version` number and add `-beta` postfix (can have .1, .2 etc. at the end)
3. Make any required changes updating the version as you go
4. Test beta package in solution that uses package
5. Create PR and get it reviewed
6. Check if there are any changes on the branch you're merging into. If there are you need to rebase those changes into yours and check that it still builds
7. As the final thing before merging update version number and remove post fix