# RealDiff .NET sort-stability demo

RealDiff runs the same tests on both sides of this pull request and reports the runtime values that changed.

## How it works

1. Check out the base and pull-request revisions.
2. Build both with .NET method instrumentation woven in.
3. Run `dotnet test` on both, recording observed method arguments and return values.
4. Diff those execution traces instead of inferring behavior from the source diff.

This is not mutation testing, static analysis, or coverage. No production code or test is mutated, RealDiff does not generate tests, and it observes only code this test suite executes.

## Worked example

The pull request tries to avoid the array allocated by `OrderBy`. In this block, `-` is the stable base implementation, `+` is the proposed implementation, and the important added line is `ordered.Sort(...)`:

```diff
-return rules.OrderBy(rule => rule.Priority).ToArray();
+List<DiscountRule> ordered = rules.ToList();
+ordered.Sort((left, right) => left.Priority.CompareTo(right.Priority));
+return ordered;
```

Both versions sort by priority, so the edit looks like a local allocation/performance refactor. But `OrderBy` is stable and `List.Sort` is not. `A_SEASONAL` and `Z_CLEARANCE` both have priority 10; the stable base keeps `A_SEASONAL` first, while the proposed sort puts `Z_CLEARANCE` first.

The following block labels the exact values RealDiff observed before and after the edit:

```text
BASE  DiscountEngine.SelectDiscount(100) -> A_SEASONAL
PR    DiscountEngine.SelectDiscount(100) -> Z_CLEARANCE
BASE  CheckoutTotals.Compute(100) -> 85
PR    CheckoutTotals.Compute(100) -> 60
```

Neither `SelectDiscount` nor `Compute` is in the diff; only `RuleOrdering.cs` changed. All three tests execute the pricing path. `DiscountIsApplied` still passes because 60 is below 100, and `TotalNeverExceedsListPrice` still passes because 60 is not above 100. Only `SeasonalDiscountWinsCurrentTies`, which asserts the exact selected code, reacts.

## Why the finding is focused

RealDiff runs the base more than once and subtracts observations that disagree with themselves, so timestamps, GUIDs, and other run-to-run variation do not become findings.

The changed discount propagates through callers, but RealDiff collapses that blast radius. It reports the first changed behavior in unedited `Pricing.cs`, where the difference originates, rather than every caller above it.

## Run it

The command below runs the demo's three tests:

```bash
dotnet test
```
