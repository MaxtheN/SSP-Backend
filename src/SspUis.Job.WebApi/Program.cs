using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SspUis.Core;
using SspUis.Job.WebApi;
using SspUis.Job.WebApi.Extensions;
using WEBASE.AspNet;
using WEBASE.DependencyInjection;
using WEBASE.Integration.Manuals.Extensions;

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
	options.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
	options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
	options.SerializerSettings.DateFormatString = Constants.DATE_FORMAT;
	options.SerializerSettings.Converters.Add(new DateOnlyJsonConverter());
	options.SerializerSettings.Converters.Add(new TimeOnlyJsonConverter());
	options.SerializerSettings.Converters.Add(new SspUis.Core.StringExtensions.TrimmingStringConverter());
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

builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProviderOptions>(options =>
{
	options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

//Tool Configuration
builder.Services.ConfigureDbServices();
builder.Services.ConfigureRabbitServices(builder.Configuration);
builder.Services.AddMinio(AppSettings.Instance.Minio);

//Custom Configurations
builder.Services.ConfigureConfigs();
builder.Services.ConfigureAuthServices();
builder.Services.AddSingleton<IServiceScopeAccessor, ServiceScopeAccessor>();
builder.Services.AddSingleton<IMimeMappingService, MimeMappingService>(p => new MimeMappingService(new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider()));
builder.Services.ConfigureSimpleServices();
builder.Services.ConfigureQuartzServices();
builder.Services.ConfigureGenericServices();
builder.Services.ConfigureSwaggerServices();
builder.Services.ConfigureCorsServices();
builder.Services.ConfigureIntegrations(builder.Configuration);
builder.Services.ConfigureHangfireServices();

//Middlewares

var app = builder.Build();

// Configure the HTTP request pipeline.
BaseServiceProvider.Initialize(() => app.Services.GetService<IServiceScopeAccessor>().Scope.ServiceProvider);

app.ConfigureSwagger();
app.ConfigureExceptions();
app.ConfigureCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.ConfigureHangfire();
app.MapControllers();
app.UseRouting();
app.Run();
