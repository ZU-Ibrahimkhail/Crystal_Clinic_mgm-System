using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Application.Common.Configuration;
using Crystal_Clinic_Mgm.Application.Common.Identity;
using Crystal_Clinic_Mgm.Application.Common.MappingProfiles;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.Services.Repositories;
using Crystal_Clinic_Mgm.Application.Common.SignalR;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Newtonsoft.Json;
using Crystal_Clinic_Mgm.Application.Accounting.Repositories;
using Crystal_Clinic_Mgm.Application.BranchStock;

namespace Crystal_Clinic_Mgm.UI.Providers
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //----Configuration Settings
            services.Configure<FinancialSettings>(config.GetSection("FinancialSettings"));
            
            //----Database Connections     
            //----ERP
            services.AddDbContext<ERP_DbContext>(options =>
                 options.UseSqlServer(config.GetConnectionString("ERPDbConnection"),
                 opt => opt.EnableRetryOnFailure()),
                 ServiceLifetime.Transient);
            //----UMS
            services.AddDbContext<UMS_DbContext>(options =>
                 options.UseSqlServer(config.GetConnectionString("UMSDbConnection"), opt => opt.EnableRetryOnFailure()), ServiceLifetime.Transient);

            //----For Token
            services.AddScoped<TokenProvider>();
            services.AddControllers().AddNewtonsoftJson();
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddAutoMapper(typeof(PMISMappingProfile));
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
                options.ValueCountLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
            });

            //Inject IHttpcontexAccessor for language cookies accessing and also used to access Mapping Profiles.
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                cfg.AddProfile(new HRMappingProfile(httpContextAccessor));
                cfg.AddProfile(new GeneralMappingProfile(httpContextAccessor));
                cfg.AddProfile(new UMSMappingProfile(httpContextAccessor));
            }).CreateMapper());

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:7865", "http://localhost:3000", "http://localhost:9000")
                    .AllowCredentials();
                });
            });
            services.AddHttpContextAccessor();
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddSignalR();
            // services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddMediatR(typeof(MyCommandHandler).Assembly);
            //services.AddControllersWithViews(options => options.Filters.Add<ErrorHandlerMiddlewere>())
            //.AddFluentValidation(x => x.AutomaticValidationEnabled = false);
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            // services.AddValidatorsFromAssembly(typeof(Startup3).Assembly);
            services.AddBrowserDetection();
            services.AddRazorPages();
            services.AddScoped<IMessage, Message>();
            services.AddScoped<IMessageHubClient, MessageHub>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ILoggedInUser, LoggedInUser>();
            services.AddTransient(typeof(IGenericRepositoryAsync<,>), typeof(GenericRepositoryAsync<,>));
            services.AddTransient(typeof(IGenericDashboardRepository<,>), typeof(GenericDashboardRepository<,>));
            services.AddTransient(typeof(IGenericDashboardRepository<>), typeof(GenericDashboardRepository<>));
            services.AddTransient(typeof(IGeneralHelperRepositoryAsync<,>), typeof(GeneralHelperRepositoryAsync<,>));
            services.AddTransient(typeof(IGeneralHelperRepositoryAsync<>), typeof(GeneralHelperRepositoryAsync<>));
            services.AddTransient(typeof(IGeneralHelperRepositoryAsync), typeof(GeneralHelperRepositoryAsync));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountingRepository, AccountingRepository>();
            services.AddScoped<IFinancialConfigurationService, FinancialConfigurationService>();
            services.AddScoped<IEquityService, EquityService>();
            services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<ISalesInvoiceService, SalesInvoiceService>();
            services.AddScoped<IProcurementService, ProcurementService>();
            services.AddScoped<IBankReconciliationService, BankReconciliationService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IAuditTrailService, AuditTrailService>();
            services.AddScoped<IForecastingService, ForecastingService>();
            services.AddScoped<IChartOfAccountsService, ChartOfAccountsService>();
            services.AddScoped<IFinancialService, FinancialService>();
            services.AddScoped<IValuationService, ValuationService>();
            //--For Email-----
            services.AddTransient<IMailRepositoy, MailRepositoy>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }); return services;
        }
    }
}
