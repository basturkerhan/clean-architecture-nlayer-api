using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayerApp.API.Filters;
using NLayerApp.API.Middlewares;
using NLayerApp.API.Modules;
using NLayerApp.Repository;
using NLayerApp.Service.Mappings;
using NLayerApp.Service.Validation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// sen ProductDtoValidatorun oldu�u konuma git ve o assembly i�erisinde yer alan di�er validatorlerle birlikte hepsini al diyoruz
builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(new ValidateFilterAttribute());
    })
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CreateProductDtoValidator>());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // varsay�lan filter throw d�n���n� devre d��� b�rak�yoruz
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalSqlConnection"), options =>
    {
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        //options.MigrationsAssembly("NLayerApp.Repository");
    });
});

builder.Services.AddScoped(typeof(NotFoundFilter<,>));
builder.Services.AddAutoMapper(typeof(MapProfile));
// AutoFac ile eklenenler
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
