using ServiceStack;
using locodepostgres.ServiceModel;

namespace locodepostgres.ServiceInterface;

public class MyServices : Service
{
    public object Any(Hello request)
    {
        return new HelloResponse { Result = $"Hello, {request.Name}!" };
    }
}