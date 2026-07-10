using Microsoft.EntityFrameworkCore;
using BankSystem.Models;
using BankSystem.Data;
using BankSystem.Services;
using BankSystem.Hubs;
using BankSystem.Services.Interfaces;
using BankSystem.Models.Identities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, UserRole>().AddEntityFrameworkStores<AppDbContext>().AddUserStore<UserStore<User, UserRole, AppDbContext, string>>().
    AddRoleStore<RoleStore<UserRole, AppDbContext, string>>();

builder.Services.AddScoped<IFrontendService, FrontendService>();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

});// this Enforce the Authorization policy( user must authorize) for all the actions methods.

builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/Account/Index";
});// if the user is not authenticated request will hit this route.


builder.Services.AddSignalR();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default",pattern: "{controller=Account}/{action=Index}/{id?}");
app.MapHub<NotificationHub>("/notificationHub");
app.Run();
