using System.ComponentModel.DataAnnotations;
using Klinkby.CleanFn.Abstractions;

namespace SampleDomainLogic;

public class FooGetRequest : IRequest<FooGetResponse>
{
    [Required] [Range(1, int.MaxValue)] public required int Id { get; init; }
}