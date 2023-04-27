namespace netdetective.IpTests;

public interface IIpTest
{
    Task<IpInfoResults> Test(string ipAddress, CancellationToken token);
}
