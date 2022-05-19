using Orleans;

namespace Grains;

public interface ITestConcurrencyGrain : IGrainWithStringKey
{
    Task Send(int i);
}

public class TestConcurrencyGrain : Grain, ITestConcurrencyGrain
{
    public async Task Send(int i)
    {
        string id = Guid.NewGuid().ToString().Substring(0, 7);//this.GetPrimaryKeyString();
        Console.WriteLine($"{i} - {id} - Start");
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        Console.WriteLine($"{i} - {id} - End");
    }
}
