namespace netdetective.IpTests;

public interface IIpQuerier
{
    Task<IpInfoResults> Query(string ipAddress, CancellationToken token);
}
