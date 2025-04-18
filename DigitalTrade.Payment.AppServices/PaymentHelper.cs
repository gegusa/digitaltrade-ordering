namespace DigitalTrade.Payment.AppServices;

internal static class PaymentHelper
{
    private static readonly ThreadLocal<Random> ThreadLocalRandom = 
        new(() => new Random());

    public static int GetRandomNumber(int min, int max)
    {
        return ThreadLocalRandom.Value.Next(min, max);
    }
}