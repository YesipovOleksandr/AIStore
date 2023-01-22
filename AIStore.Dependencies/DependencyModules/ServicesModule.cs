using Microsoft.Extensions.DependencyInjection;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Abstract.Repository;
using AIStore.DAL.Repository;
using AIStore.BLL.Services;
using AIStore.Domain.Abstract.Services.Mail;
using AIStore.Domain.Abstract.Services.Verifier;
using AIStore.BLL.Services.Verifier;
using AIStore.Domain.Abstract.Services.RecoverPassword;
using AIStore.BLL.Services.RecoverPassword;

namespace AIStore.Dependencies.DependencyModules
{
    public static class ServicesModule
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.AddScoped<IMailService, MailService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthExternalService, AuthExternalService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVerifyRepository, VerifyRepository>();
            services.AddScoped<IVerifierService, VerifierService>();

            services.AddScoped<IRecoverPasswordRepository, RecoverPasswordRepository>();
            services.AddScoped<IRecoverPasswordService, RecoverPasswordService>();
            


        }
    }
}
