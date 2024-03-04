using System.ComponentModel.DataAnnotations;
using Klinkby.CleanFn.Abstractions;
using Microsoft.Extensions.Logging;

namespace Klinkby.CleanFn.Core;

/// <summary>
///     Mediator with declarative data annotations <see cref="Validator" />.
/// </summary>
public class DataAnnotationsValidatorMediator(
    IServiceProvider serviceProvider,
    ILogger<DataAnnotationsValidatorMediator> logger)
    : Mediator(serviceProvider, logger)
{
    /// <summary>
    ///     Validate the request object using <see cref="Validator" />.
    /// </summary>
    /// <param name="request"></param>
    protected override void Validate(IRequestBase request)
    {
        Validator.ValidateObject(request, new ValidationContext(request), true);
    }
}
