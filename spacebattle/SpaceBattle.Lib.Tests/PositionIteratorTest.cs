using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class PositionIteratorTest
{
    public PositionIteratorTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();
    }
    [Fact]
    public void SuccessfulPositionIterationProcedure()
    {
        var positionValues = new List<Vector>{
            new(new int[] {0, 0}),
            new(new int[] {1, 1}),
            new(new int[] {2, 2})
        };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Positions",
            (object[] args) => positionValues
        ).Execute();

        var positions = new SpaceshipPositions
        {
            StartPosition = 1
        };

        var iterator = positions.GetEnumerator();

        Assert.Equal(positionValues[1], iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(positionValues[2], iterator.Current);
        Assert.False(iterator.MoveNext());

        iterator.Reset();
        Assert.Equal(positionValues[1], iterator.Current);

        Assert.Throws<NotImplementedException>(iterator.Dispose);
    }
    [Fact]
    public void PositionIteratingThrowsOutOfRangeException()
    {
        var positionValues = new List<Vector>{
            new(new int[] {3, 3}),
        };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Positions",
            (object[] args) => positionValues
        ).Execute();

        var positions = new SpaceshipPositions();
        var iterator = ((IEnumerable)positions).GetEnumerator();

        Assert.Equal(0, positions.StartPosition);

        Assert.Equal(positionValues[0], iterator.Current);
        Assert.False(iterator.MoveNext());

        Assert.Throws<ArgumentOutOfRangeException>(() => iterator.Current);
    }
}
