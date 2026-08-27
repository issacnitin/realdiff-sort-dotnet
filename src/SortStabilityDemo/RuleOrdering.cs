namespace SortStabilityDemo.Sorting;

public static class RuleOrdering
{
    public static IReadOnlyList<DiscountRule> ByPriority(IEnumerable<DiscountRule> rules)
    {
        List<DiscountRule> ordered = rules.ToList();
        ordered.Sort((left, right) => left.Priority.CompareTo(right.Priority));
        return ordered;
    }
}