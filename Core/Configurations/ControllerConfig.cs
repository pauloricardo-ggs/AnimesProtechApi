using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Core.Configurations;

public static class ControllerConfig
{
    public static void AddControllersWithRoutePrefix(this IServiceCollection service, string prefix)
    {
        service.AddControllers(options =>
            options.Conventions.Add(new RoutePrefixConvention(prefix)));
    }
}

public class RoutePrefixConvention(string routePrefix) : IControllerModelConvention
{
    private readonly string _routePrefix = routePrefix;

    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
        {
            if (selector.AttributeRouteModel == null)
            {
                selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(_routePrefix));
                continue;
            }
            
            selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
                new AttributeRouteModel(new RouteAttribute(_routePrefix)),
                selector.AttributeRouteModel
            );
        }
    }
}