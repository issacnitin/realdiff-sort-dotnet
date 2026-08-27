namespace SortStabilityDemo.Sorting;

public static class RuleOrdering
{
    public static IReadOnlyList<DiscountRule> ByPriority(IEnumerable<DiscountRule> rules) =>
        rules.OrderBy(rule => rule.Priority).ToArray();
}