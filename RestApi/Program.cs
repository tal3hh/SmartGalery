using DomainLayer.Entities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


#region Identity

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;  //Simvollardan biri olmalidir(@,/,$) 
    opt.Password.RequireLowercase = false;       //Mutleq Kicik herf
    opt.Password.RequireUppercase = false;       //Mutleq Boyuk herf 
    opt.Password.RequiredLength = 4;            //Min. simvol sayi
    opt.Password.RequireDigit = false;

    opt.User.RequireUniqueEmail = false;

    opt.SignIn.RequireConfirmedEmail = false;
    opt.SignIn.RequireConfirmedAccount = false;

    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); //Sifreni 5 defe sehv girdikde hesab 1dk baglanir.
    opt.Lockout.MaxFailedAccessAttempts = 5;                      //Sifreni max. 5defe sehv girmek olar.

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

#endregion


#region Cookie

//builder.Services.ConfigureApplicationCookie(opt =>
//{
//    opt.Cookie.HttpOnly = true;
//    opt.Cookie.SameSite = SameSiteMode.Strict;
//    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
//    opt.Cookie.Name = "AshionIdentity";
//    opt.LoginPath = new PathString("/Account/Login");
//    opt.AccessDeniedPath = new PathString("/Account/AccessDenied");

//});

#endregion


#region Context

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration["ConnectionStrings:Mssql"]);
});

#endregion



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();