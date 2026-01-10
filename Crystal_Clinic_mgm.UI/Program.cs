
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Application.Common.Jobs;
using Crystal_Clinic_Mgm.Application.Common.SignalR;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Persistence.Initializers;
using Crystal_Clinic_Mgm.UI.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);
//add services to container
//-----add-Swagger-Service
builder.Services.AddSwaggerAPIHeader(builder.Configuration);
//------Add Application-Services
builder.Services.AddApplicationServices(builder.Configuration);
//------Add User Services
builder.Services.AddIdentityProvider(builder.Configuration);
//----------------------------------------------------------

//---for handling file size limit in byts --Allow 1500 MB Size

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 1500 * 1024 * 1024);
//---for Localization string--1
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
var supportedCultures = new[] { "en", "ps-AF", "fa-IR" };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{

    options.AddSupportedUICultures(supportedCultures);
    options.SetDefaultCulture(supportedCultures[0]);
    //.AddSupportedCultures(supportedCultures)//---For Converting Date and Number
});
builder.Services.AddHostedService<DailyJob>();
var app = builder.Build();
//--For Localization string--2
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
//-----Use for Development Environments
//app.UseSwagger();
//app.UseSwaggerUI(option =>
//{
//    option.SwaggerEndpoint("/swagger/MEW_ERP/swagger.json", "MEW-API-V2");
//    //--this line is use for Collapes Contoller Name
//    option.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
//    option.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
Constants.CheckPassword = true;
//});
//app.UseRouting();
//app.UseCors("AllowAll");
//app.UseCors("CorsPolicy");
//app.UseStaticFiles();
//app.UseDefaultFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/Crystal_Clinic_Mgm/swagger.json", "Crystal_Clinic-Market-V1");
        //--this line is use for Collapes Contoller Name
        option.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        option.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
        Constants.CheckPassword = false;
    });
    app.UseRouting();
    app.UseCors("AllowAll");
    app.UseStaticFiles();
    app.UseDefaultFiles();
    app.UseCors("CorsPolicy");
}
else
{
    app.UseRouting();
    //---Use For Client app to be used here
    //---Use for third Party(React Project))
    app.UseDefaultFiles();
    app.UseStaticFiles();
}
app.UseDeveloperExceptionPage();
//-----------------------
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<MessageHub>("/signalr");
app.UseHttpsRedirection();
//----Use for third Party(React Project))
if (!app.Environment.IsDevelopment())
{
    app.MapFallbackToController("Index", "Fallback");//For Using client-app in API
}
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
//if the database was not created it will create it for us
try
{
    var context = services.GetRequiredService<ERP_DbContext>();
    var umscontext = services.GetRequiredService<UMS_DbContext>();
    var pmiscontext = services.GetRequiredService<ERP_DbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    if (context.Database.IsSqlServer())
    {
        context.Database.Migrate();
    }
    if (umscontext.Database.IsSqlServer())
    {
        umscontext.Database.Migrate();
    }
    //////---Seeding Data-------------------------------
    await Crystal_Clinic_Initializer.InitilizeCrystal_Clinic(context);
    await UMSintializer.InitializeUMS(userManager);
    await NotificationInitializer.InitializeNotifications(umscontext);
    await PermissionInitializer.InitializePermissions(umscontext);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}
await app.RunAsync();