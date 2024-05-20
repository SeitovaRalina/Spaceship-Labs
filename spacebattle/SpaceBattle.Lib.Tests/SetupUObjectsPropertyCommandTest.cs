using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class SetupObjectsPropertyCommandTest
{
    public SetupObjectsPropertyCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObject.SetProperty",
            (object[] args) => new SetUObjectPropertyStrategy().Init(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObjects.SetProperty",
            (object[] args) => new SetupObjectsPropertyCommand((IEnumerable<int>)args[0], (string)args[1])
        ).Execute();
    }
    [Fact]
    public void SuccessfulArrangementOfShips()
    {
        var positionEnumerable = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Positions",
            (object[] args) => new List<Vector>{
                new(new int[] {0, 0}),
                new(new int[] {0, 1}),
                new(new int[] {1, 0}),
                new(new int[] {1, 1}) }
        ).Execute();
        positionEnumerable.Setup(s => s.Init("position")).Returns(new SpaceshipPositions()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Enumerable.GetByPropertyName",
            (object[] args) => positionEnumerable.Object.Init(args)
        ).Execute();

        var enemyShip1 = new Mock<IUObject>();
        enemyShip1.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();
        var enemyShip2 = new Mock<IUObject>();
        enemyShip2.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();
        var enemyShip3 = new Mock<IUObject>();
        enemyShip3.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();
        var ships = new Dictionary<int, IUObject>()
        {
            [1] = enemyShip1.Object,
            [2] = enemyShip2.Object,
            [3] = enemyShip3.Object,
        };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObjects.Dictionary",
            (object[] args) => ships
        ).Execute();

        IoC.Resolve<ICommand>("Game.UObjects.SetProperty", ships.Keys, "position").Execute();

        positionEnumerable.Verify();

        enemyShip1.Verify(x => x.SetProperty("position", new Vector(new int[] { 0, 0 })), Times.Once());
        enemyShip2.Verify(x => x.SetProperty("position", new Vector(new int[] { 0, 1 })), Times.Once());
        enemyShip3.Verify(x => x.SetProperty("position", new Vector(new int[] { 1, 0 })), Times.Once());
    }
    [Fact]
    public void SuccessfulSetupFuelVolumesForShips()
    {
        var fuelEnumerable = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Fuels",
            (object[] args) => new List<int> { 100, 200, 300, 400 }
        ).Execute();
        fuelEnumerable.Setup(s => s.Init("fuel")).Returns(new SpaceshipFuels()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Enumerable.GetByPropertyName",
            (object[] args) => fuelEnumerable.Object.Init(args)
        ).Execute();

        var enemyShip1 = new Mock<IUObject>();
        enemyShip1.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();
        var enemyShip2 = new Mock<IUObject>();
        enemyShip2.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();
        var enemyShip3 = new Mock<IUObject>();
        enemyShip3.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();
        var ships = new Dictionary<int, IUObject>()
        {
            [1] = enemyShip1.Object,
            [2] = enemyShip2.Object,
            [3] = enemyShip3.Object,
        };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObjects.Dictionary",
            (object[] args) => ships
        ).Execute();

        IoC.Resolve<ICommand>("Game.UObjects.SetProperty", ships.Keys, "fuel").Execute();

        fuelEnumerable.Verify();

        enemyShip1.Verify(x => x.SetProperty("fuel", 100), Times.Once());
        enemyShip2.Verify(x => x.SetProperty("fuel", 200), Times.Once());
        enemyShip3.Verify(x => x.SetProperty("fuel", 300), Times.Once());
    }
}
