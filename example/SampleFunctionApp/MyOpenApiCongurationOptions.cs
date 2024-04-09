using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace SampleFunctionApp;

#if DEBUG

public class MyOpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
{
    public override OpenApiInfo Info { get; set; } = new()
    {
        Version = "1.0.0",
        Title = "Swagger Petstore",
        Description = "This is a sample server Petstore API designed by [http://swagger.io](http://swagger.io).",
        TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
        Contact = new OpenApiContact
        {
            Name = "Enquiry",
            Email = "azfunc-openapi@microsoft.com",
            Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues")
        },
        License = new OpenApiLicense { Name = "MIT", Url = new Uri("http://opensource.org/licenses/MIT") }
    };

    public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
}

#endif
