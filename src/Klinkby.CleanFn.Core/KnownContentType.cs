namespace Klinkby.CleanFn.Core;

internal static class KnownContentType
{
    public const string ApplicationHealthJson = $"application/health+json{Utf8PostFix}";
    public const string ApplicationJson = $"application/json{Utf8PostFix}";
    public const string ApplicationProblemJson = $"application/problem+json{Utf8PostFix}";
    public const string TextYaml = $"text/vnd.yaml{Utf8PostFix}";

    private const string Utf8PostFix = "; charset=utf-8";
}
