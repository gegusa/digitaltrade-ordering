namespace DigitalTrade.Ordering.AppServices;

internal static class OrderingHelper
{
    private static readonly ThreadLocal<Random> ThreadLocalRandom = 
        new(() => new Random());

    public static int GetRandomNumber(int min, int max)
    {
        return ThreadLocalRandom.Value.Next(min, max);
    }
}