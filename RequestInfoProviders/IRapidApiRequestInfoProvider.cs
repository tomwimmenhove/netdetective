namespace netdetective.RequestInfoProviders;

public interface IRapidApiRequestInfoProvider : IRequestInfoProvider
{
    string? GetUsername();

    string? GetSecret();
}
