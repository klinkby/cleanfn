using Klinkby.CleanFn.Abstractions;
using Klinkby.CleanFn.Core.HealthChecks;

namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Request for <see cref="HealthResponse"/>
/// </summary>
/// <seealso cref="MemoryHealthCheck"/>
public record HealthRequest : IRequest<HealthResponse>
// ReSharper disable once RedundantTypeDeclarationBody
{
}
