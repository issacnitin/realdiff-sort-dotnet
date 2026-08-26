# BehaviorDiff .NET sort-stability demo

A three-test fixture for a stable-sort tie-break change. The pull request edits only `SortConfiguration.cs`; the behavior frontier remains in unedited pricing code, and only the exact tie-winner assertion reacts.

Run locally with `dotnet test`.
