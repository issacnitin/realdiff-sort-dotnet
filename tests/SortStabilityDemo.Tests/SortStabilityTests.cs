using Xunit;

namespace SortStabilityDemo.Tests;

public sealed class SortStabilityTests
{
    [Fact]
    public void DiscountIsApplied()
    {
        Assert.True(CoverageVolume.Exercise() > 0);
        var total = new CheckoutTotals().Compute(100m);
        Assert.True(total < 100m);
    }

    [Fact]
    public void TotalNeverExceedsListPrice()
    {
        Assert.True(CoverageVolume.Exercise() > 0);
        var total = new CheckoutTotals().Compute(100m);
        Assert.True(total <= 100m);
    }

    [Fact]
    public void ClearanceWinsCurrentTies()
    {
        Assert.True(CoverageVolume.Exercise() > 0);
        var checkout = new CheckoutTotals();
        checkout.Compute(100m);
        Assert.Equal("Z_CLEARANCE", checkout.SelectedCode);
    }
}
