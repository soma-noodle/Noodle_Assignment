var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.UseCommercetoolsApi(builder.Configuration,
                new List<string> { "Client", "ImportApiClient", "BerlinStoreClient", "MeClient" },
                TokenProviderExtension.CreateTokenProvider);

// Add Services to the container
builder.Services.AddScoped<IDummyExerciseServie, DummyExerciseService>();
builder.Services.AddScoped<ICreateService, CreateService>();
builder.Services.AddScoped<IUpdateGroupService, UpdateGroupService>();
builder.Services.AddScoped<IStateMachineService, StateMachineService>();
builder.Services.AddScoped<ICheckOutService, CheckOutService>();
builder.Services.AddScoped<ICartMergingService, CartMergingService>();
builder.Services.AddScoped<IInStoreService, InStoreService>();
builder.Services.AddScoped<IMeService, MeService>();
builder.Services.AddScoped<IProductSelectionsService, ProductSelectionsService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IPagedQueryService, PagedQueryService>();
builder.Services.AddScoped<IGraphQLService, GraphQLService>();
builder.Services.AddScoped<ICustomTypesService, CustomTypesService>();
builder.Services.AddScoped<ICustomObjectsService, CustomObjectsService>();
builder.Services.AddScoped<IApiExtensionService, ApiExtensionService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IErrorHandlingService, ErrorHandlingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
