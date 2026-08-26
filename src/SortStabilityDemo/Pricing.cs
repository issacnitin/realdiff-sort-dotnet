namespace SortStabilityDemo;

public readonly record struct DiscountRule(string Code, int Priority, decimal MinimumTotal, decimal Rate);

public static class RuleOrdering
{
    public static IReadOnlyList<DiscountRule> ByPriority(IEnumerable<DiscountRule> rules)
    {
        var ordered = rules.OrderBy(rule => rule.Priority);
        return SortConfiguration.BreakPriorityTiesByCode
            ? ordered.ThenBy(rule => rule.Code, StringComparer.Ordinal).ToArray()
            : ordered.ToArray();
    }
}

public sealed class DiscountEngine
{
    private static readonly DiscountRule[] Rules =
    [
        new("Z_CLEARANCE", 10, 50m, 0.40m),
        new("A_SEASONAL", 10, 50m, 0.15m),
        new("INELIGIBLE", 10, 1000m, 0.05m)
    ];

    public DiscountRule SelectDiscount(decimal listPrice) =>
        RuleOrdering.ByPriority(Rules).First(rule => listPrice >= rule.MinimumTotal);
}

public sealed class CheckoutTotals
{
    public string? SelectedCode { get; private set; }

    public decimal Compute(decimal listPrice)
    {
        var selected = new DiscountEngine().SelectDiscount(listPrice);
        SelectedCode = selected.Code;
        return listPrice * (1m - selected.Rate);
    }
}

public static class CoverageVolume
{
    public static int Exercise()
    {
        var total = 0;
        for (var index = 0; index < 40; index++)
        {
            total += Normalize(index);
        }
        return total;
    }

    private static int Normalize(int value) => value + 1;
}
