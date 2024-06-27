using Microsoft.Extensions.Options;
using MongoDb.UserInterface.AutoMapper.EntityDtoMappers;
using MongoDb.UserInterface.Services.Abstractions.CartService;
using MongoDb.UserInterface.Services.Abstractions.CategoryServices;
using MongoDb.UserInterface.Services.Abstractions.CustomerServices;
using MongoDb.UserInterface.Services.Abstractions.OrderService;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;
using MongoDb.UserInterface.Services.Concretions.CartService;
using MongoDb.UserInterface.Services.Concretions.CategoryServices;
using MongoDb.UserInterface.Services.Concretions.CustomerServices;
using MongoDb.UserInterface.Services.Concretions.OrderService;
using MongoDb.UserInterface.Services.Concretions.ProductServices;
using MongoDb.UserInterface.Settings.MongoDb.Context;
using MongoDb.UserInterface.Settings.MongoDb.NewContext;
using MongoDb.UserInterface.Settings.MongoDb.OldContext;
using QuestPDF.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

#region Db Settings For DbContext. I developed.

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddScoped<IMongoDbContext, MongoDbContext>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoDbContext(settings);
});

#endregion

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

#region Db Settings For Services. This is the Murat teacher's configuration. This is using only for  the product.

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

#endregion

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
