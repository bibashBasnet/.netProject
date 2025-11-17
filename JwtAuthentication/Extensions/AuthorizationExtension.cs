namespace JwtAuthentication.Extensions
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddAuthorization(this IServiceCollection service, IConfiguration config)
        {
            service.AddAuthorization(options => {
                options.AddPolicy("AdminAccess", policy =>
                {
                    policy.RequireRole("Admin");
                    policy.RequireUserName("Ram");
                });

                options.AddPolicy("NormalAccess", policy =>
                {
                    policy.RequireRole("Admin", "User");
                });
            });
            return service;
        }
    }
}
