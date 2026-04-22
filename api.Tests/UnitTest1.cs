namespace api.Tests;

public class DiscoveryTests
{
    [Fact]
    public void Tests_Work()
    {
        var calculator = Substitute.For<ICalculator>();
        calculator.Add(1, 1).Returns(2);

        int result = calculator.Add(1, 1);
        result.Should().Be(2);
    }
}

public interface ICalculator
{
    int Add(int a, int b);
}
