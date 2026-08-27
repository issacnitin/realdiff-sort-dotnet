using SortStabilityDemo.Sorting;

namespace SortStabilityDemo;

public readonly struct DiscountRule
{
    public DiscountRule(string code, int priority, decimal minimumTotal, decimal rate)
    {
        Code = code;
        Priority = priority;
        MinimumTotal = minimumTotal;
        Rate = rate;
    }

    public readonly string Code;
    public readonly int Priority;
    public readonly decimal MinimumTotal;
    public readonly decimal Rate;
}

public sealed class DiscountEngine
{
    private readonly DiscountRule[] rules;

    public DiscountEngine()
    {
        rules =
        [
            new("INELIGIBLE_00", 10, 1000m, 0.05m),
            new("A_SEASONAL", 10, 50m, 0.15m),
            new("Z_CLEARANCE", 10, 50m, 0.40m),
            new("INELIGIBLE_03", 10, 1000m, 0.05m),
            new("INELIGIBLE_04", 10, 1000m, 0.05m),
            new("INELIGIBLE_05", 10, 1000m, 0.05m),
            new("INELIGIBLE_06", 10, 1000m, 0.05m),
            new("INELIGIBLE_07", 10, 1000m, 0.05m),
            new("INELIGIBLE_08", 10, 1000m, 0.05m),
            new("INELIGIBLE_09", 10, 1000m, 0.05m),
            new("INELIGIBLE_10", 10, 1000m, 0.05m),
            new("INELIGIBLE_11", 10, 1000m, 0.05m),
            new("INELIGIBLE_12", 10, 1000m, 0.05m),
            new("INELIGIBLE_13", 10, 1000m, 0.05m),
            new("INELIGIBLE_14", 10, 1000m, 0.05m),
            new("INELIGIBLE_15", 10, 1000m, 0.05m),
            new("INELIGIBLE_16", 10, 1000m, 0.05m)
        ];
    }

    public DiscountRule SelectDiscount(decimal listPrice)
    {
        foreach (DiscountRule rule in RuleOrdering.ByPriority(rules))
        {
            if (listPrice >= rule.MinimumTotal)
            {
                return rule;
            }
        }
        throw new InvalidOperationException("No eligible discount rule.");
    }
}

public sealed class CheckoutTotals
{
    public string? SelectedCode;

    public decimal Compute(decimal listPrice)
    {
        var selected = new DiscountEngine().SelectDiscount(listPrice);
        SelectedCode = selected.Code;
        return listPrice * (1m - selected.Rate);
    }
}
