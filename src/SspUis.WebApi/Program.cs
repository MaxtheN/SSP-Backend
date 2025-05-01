using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SspUis.Core;
using SspUis.WebApi;
using WEBASE.AspNet;
using WEBASE.DependencyInjection;
using WEBASE.Integration.Manuals.Extensions;
using static SspUis.Core.StringExtensions;

var builder = WebApplication.CreateBuilder(args);
AppSettings.Init(builder.Configuration.Get<AppSettings>());

// ASP.NET Configurations
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new SspUis.BizLogicLayer.DateTimeBinder.DateTimeModelBinderProvider());
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = true });
    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
    options.SerializerSettings.DateFormatString = Constants.DATE_FORMAT;
    options.SerializerSettings.Converters.Add(new HDateTimeConverter());
    options.SerializerSettings.Converters.Add(new DateTimeConverter());
    options.SerializerSettings.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerSettings.Converters.Add(new DateOnlyNullableJsonConverter());
    options.SerializerSettings.Converters.Add(new TimeOnlyJsonConverter());
    options.SerializerSettings.Converters.Add(new TimeOnlyNullableJsonConverter());
    options.SerializerSettings.Converters.Add(new TrimmingStringConverter());
});
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});
builder.Services.Configure<RequestLocalizationOptions>(
        opts =>
        {
            var supportedCultures = new List<CultureInfo> { new CultureInfo("ru") };
            opts.DefaultRequestCulture = new RequestCulture("ru");
            // Formatting numbers, dates, etc.
            opts.SupportedCultures = supportedCultures;
            // UI strings that we have localized.
            opts.SupportedUICultures = supportedCultures;
        });

// Tool Configuration
builder.Services.ConfigureDbServices();
builder.Services.ConfigureAuthServices();
builder.Services.AddMinio(AppSettings.Instance.Minio);
// Custom Configuration
builder.Services.ConfigureConfigs();
builder.Services.AddSingleton<IServiceScopeAccessor, ServiceScopeAccessor>();
builder.Services.AddSingleton<IMimeMappingService, MimeMappingService>(p => new MimeMappingService(new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider()));
builder.Services.ConfigureSimpleServices();
builder.Services.ConfigureGenericServices();
builder.Services.ConfigureCorsServices();
builder.Services.ConfigureHangfireServices();
builder.Services.ConfigureSwaggerServices();
builder.Services.ConfigureIntegrations(builder.Configuration);
builder.Services.ConfigureMspdServices(AppSettings.Instance.Integration.Mspd);
builder.Services.ConfigureRabbitServices(builder.Configuration);

// Middlewares
var app = builder.Build();

BaseServiceProvider.Initialize(() => app.Services.GetService<IHttpContextAccessor>()?.HttpContext?.RequestServices);

app.ConfigureSwagger();
app.ConfigureExceptions();
app.ConfigureCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseRouting();

app.Run();