using ServiceStack;
using ServiceStack.Auth;

[assembly: HostingStartup(typeof(locodepostgres.ConfigureAutoQuery))]

namespace locodepostgres
{
    public class ConfigureAutoQuery : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder) => builder
            .ConfigureAppHost(appHost =>
            {
                var skipTables = new List<string>();
                skipTables.Add(nameof(AppUser));
                skipTables.Add(nameof(UserAuthDetails));
                skipTables.Add(nameof(UserAuthRole));
                appHost.Plugins.Add(new AutoQueryFeature {
                    MaxLimit = 1000,
                    //IncludeTotal = true,
                    GenerateCrudServices = new GenerateCrudServices
                    {
                        AutoRegister = true,
                        ServiceFilter = (op, req) =>
                        {
                            if (op.IsCrud())
                            {
                                op.Request.AddAttributeIfNotExists(new ValidateRequestAttribute("IsAuthenticated"),
                                    x => x.Validator == "IsAuthenticated");
                            }
                        },
                        TypeFilter = (type, req) =>
                        {
                        }, 
                        IncludeService = op => !skipTables.Any(table => op.ReferencesAny(table))
                        
                    }
                });
            });
    }
}