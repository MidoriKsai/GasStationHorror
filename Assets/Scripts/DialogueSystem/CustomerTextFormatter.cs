using System.Collections.Generic;

public static class CustomerTextFormatter
{
    public static string ApplyData(string text, CustomerData data)
    {
        if (data == null)
            return text;

        return text
            .Replace("{pump}", (data.petrolPumpNumber + 1).ToString())
            .Replace("{liters}", data.literQuantity.ToString())
            .Replace("{fuel}", data.fuelType)
            .Replace("{foodOrder}", BuildFoodOrderText(data))
            .Replace("{food}", BuildFoodText(data))
            .Replace("{coffeeCount}", data.coffeeCount.ToString())
            .Replace("{frenchDogCount}", data.frenchDogCount.ToString())
            .Replace("{readyCoffee}", data.readyCoffee.ToString())
            .Replace("{readyFrenchDogs}", data.readyFrenchDogs.ToString());
    }

    public static string BuildFoodOrderText(CustomerData data)
    {
        string food = BuildFoodText(data);

        if (string.IsNullOrEmpty(food))
            return "";

        return $" Ещё {food}.";
    }

    public static string BuildFoodText(CustomerData data)
    {
        if (data.coffeeCount <= 0 && data.frenchDogCount <= 0)
            return "";

        var parts = new List<string>();

        if (data.coffeeCount > 0)
            parts.Add($"{data.coffeeCount} кофе");

        if (data.frenchDogCount > 0)
            parts.Add($"{data.frenchDogCount} {GetFrenchDogWord(data.frenchDogCount)}");

        return string.Join(" и ", parts);
    }

    private static string GetFrenchDogWord(int count)
    {
        return count == 1 ? "френчдог" : "френчдога";
    }
}