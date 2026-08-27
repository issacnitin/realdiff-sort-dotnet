using SortStabilityDemo.Configuration;

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

public static class RuleOrdering
{
    public static IReadOnlyList<DiscountRule> ByPriority(IEnumerable<DiscountRule> rules)
    {
        DiscountRule[] ordered = rules.ToArray();
        bool breakTies = SortConfiguration.ShouldBreakPriorityTiesByCode();
        for (int index = 1; index < ordered.Length; index++)
        {
            DiscountRule current = ordered[index];
            int insertion = index - 1;
            while (insertion >= 0
                && (ordered[insertion].Priority > current.Priority
                    || (breakTies
                        && ordered[insertion].Priority == current.Priority
                        && string.CompareOrdinal(ordered[insertion].Code, current.Code) > 0)))
            {
                ordered[insertion + 1] = ordered[insertion];
                insertion--;
            }
            ordered[insertion + 1] = current;
        }
        return ordered;
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

    public DiscountRule SelectDiscount(decimal listPrice)
    {
        foreach (DiscountRule rule in RuleOrdering.ByPriority(Rules))
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
