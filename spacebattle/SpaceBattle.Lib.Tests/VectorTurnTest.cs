﻿namespace SpaceBattle.Lib.Tests;

public class VectorTurnTest
{
    [Fact]
    public void VectorTurnHashCode()
    {
        var angle = new VectorTurn(45);
        var delta_angle = new VectorTurn(45);
        Assert.Equal(angle.GetHashCode(), delta_angle.GetHashCode());
    }

    [Fact]
    public void VectorTurnEqualsNotVectorTurn()
    {
        var angle = new VectorTurn(45);
        Assert.False(angle.Equals(45));
    }

    [Fact]
    public void VectorTurnEqualIsTheSameVectorTurn()
    {
        var angle1 = new VectorTurn(3, 8);
        var angle2 = new VectorTurn(135);
        Assert.True(angle1.Equals(angle2));
    }
}
