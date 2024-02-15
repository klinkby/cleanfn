using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace Klinkby.CleanFn.Core.Services;

/// <summary>
///     Mediator with declarative data annotations validation.
/// </summary>
internal class DataAnnotationsValidatorMediator(
    IServiceProvider serviceProvider,
    ILogger<DataAnnotationsValidatorMediator> logger)
    : Mediator(serviceProvider, logger)
{
    protected override void Validate(object request)
        => Validator.ValidateObject(request, new ValidationContext(request), true);
}