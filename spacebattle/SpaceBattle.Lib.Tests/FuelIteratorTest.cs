using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class FuelIteratorTest
{
    public FuelIteratorTest()
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
    public void SuccessfulFuelVolumeIterationProcedure()
    {
        var fuelVolumes = new List<int> { 40, 60 };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Fuels",
            (object[] args) => fuelVolumes
        ).Execute();

        var iterator = new SpaceshipFuels().GetEnumerator();

        Assert.Equal(fuelVolumes[0], iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(fuelVolumes[1], iterator.Current);
        Assert.False(iterator.MoveNext());

        iterator.Reset();
        Assert.Equal(fuelVolumes[0], iterator.Current);

        Assert.Throws<NotImplementedException>(iterator.Dispose);
    }
    [Fact]
    public void PositionIteratingThrowsOutOfRangeException()
    {
        var fuelVolumes = new List<int>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Fuels",
            (object[] args) => fuelVolumes
        ).Execute();

        var iterator = ((IEnumerable)new SpaceshipFuels()).GetEnumerator();

        Assert.Empty(fuelVolumes);
        Assert.False(iterator.MoveNext());
        Assert.Throws<ArgumentOutOfRangeException>(() => iterator.Current);
    }
}
