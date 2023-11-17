namespace SpaceBattle.Lib.Tests;

public class VectorAngleTest
{
    [Fact]
    public void VectorAngleHashCode()
    {
        var angle = new VectorAngle(45);
        var delta_angle = new VectorAngle(45);
        Assert.Equal(angle.GetHashCode(), delta_angle.GetHashCode());
    }

    [Fact]
    public void VectorAngleEqualsNotVectorAngle()
    {
        var angle = new VectorAngle(45);
        Assert.False(angle.Equals(45));
    }

    [Fact]
    public void VectorAngleEqualIsTheSameVectorAngle()
    {
        var angle1 = new VectorAngle(3, 8);
        var angle2 = new VectorAngle(135);
        Assert.True(angle1.Equals(angle2));
    }
}
