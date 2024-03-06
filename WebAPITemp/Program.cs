using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.OpenApi.Models;
using WebAPITemp.Filters;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using WebAPITemp.Middleware;
using CommonLibrary.Authorization;
using CommonLibrary.Logging;
using CommonLibrary.AutofacHelper;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using CommonLibrary.Helpers;
using CommonLibrary.Dapper.Interfaces;
using CommonLibrary.Dapper;

var builder = WebApplication.CreateBuilder(args);

// 將服務添加到容器中。
#region ORM相關
#region EF Core
builder.Services.AddDbContext<WebAPITemp.DBContexts.EFCore.ProjectDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});
#endregion

#region Dapper
//新增Dapper相關DB連線
var ProjectDBContext = new WebAPITemp.DBContexts.Dapper.ProjectDBContext(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(ProjectDBContext);
//新增Dapper的Repository
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
#endregion
#endregion

#region 添加服務
//設定Autofac作為預設的ServiceProvider
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//設定Autofac的容器
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModuleRegister("WebAPITemp.Services", "Service", Assembly.GetExecutingAssembly())));
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModuleRegister("WebAPITemp.Models.Mapper", "Mapper", Assembly.GetExecutingAssembly())));

//添加自定義的服務
builder.Services.AddScoped<IDbContext, WebAPITemp.DBContexts.Dapper.ProjectDBContext>();
builder.Services.AddScoped<ActionRoleFilter>();
builder.Services.AddScoped<IFileHelper, FileHelper>();
#endregion

#region JWT授權
JWTAuthorizationSetting.JWTSetting(builder);
#endregion

#region 設定JWT授權並啟用Swagger
builder.Services.AddSwaggerGen(options => {
    //設定Swagger產生API文件
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    //設定API版本
    options.SwaggerDoc("v1", new OpenApiInfo { Title = $"WebAPITemp", Version = $"v_{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}" });

    //設定Bearer授權
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        //設定type為http，因為Swagger預設只支援http
        Type = SecuritySchemeType.Http,
        //設定Bearer token
        Scheme = "Bearer",
        //設定Bearer格式為JWT
        BearerFormat = "JWT",
        //設定放置在http request的header中
        In = ParameterLocation.Header,
        //設定描述
        Description = "JWT授權描述"
    });
    //設定Authorize檢查的Attribute
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});
#endregion

#region 設定Serilog日誌
SerilogSettingExtensions.UseSerilogSetting(builder);    //使用Serilog
//UseSerilogForElasticsearchSetting();    //使用Serilog(含Elasticsearch)
#endregion

#region 設定跨域
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

#region 添加中間件
app.UseMiddleware<ErrorHandler>();
app.UseMiddleware<AuthorizationHandler>();
#endregion

#region 執行Migration
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context1 = services.GetRequiredService<WebAPITemp.DBContexts.EFCore.ProjectDBContext>();
        context1.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}
#endregion

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(); //使用跨域

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


#region Serilog For Elasticsearch
void UseSerilogForElasticsearchSetting()
{
    string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{environment}.json",
            optional: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Uri"]))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        })
        .Enrich.WithProperty("Environment", environment!)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
    builder.Host.UseSerilog();
}
#endregion