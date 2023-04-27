namespace netdetective.IpTests;

public interface IIpTestFactory
{
    Task<IIpTest> GetInstance();
}
