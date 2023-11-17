namespace SpaceBattle.Lib.Tests;

public class TurnCommandTest
{
    // корабль, который находится под углом к горизонту в 45 градусов 
    // имеет угловую скорость 90 градусов. 
    // В результате поворота корабль оказывается под углом 135 градусов к горизонту.
    [Fact]
    public void TurnCommandPositive()
    {
        var turnable = new Mock<ITurnable>();

        turnable.SetupGet(t => t.Angle).Returns(new VectorTurn(45)).Verifiable();
        turnable.SetupGet(t => t.DeltaAngle).Returns(new VectorTurn(90)).Verifiable();

        ICommand turnCommand = new TurnCommand(turnable.Object);

        turnCommand.Execute();

        turnable.Verify();
        turnable.VerifySet(t => t.Angle = new VectorTurn(135), Times.Once);

        turnable.VerifyAll();
    }
    // попытка сдвинуть корабль, у которого невозможно прочитать значение угла наклона к горизонту, приводит к ошибке.
    [Fact]
    public void TurnCommandCantDefineAngle()
    {
        var turnable = new Mock<ITurnable>();

        turnable.SetupGet(t => t.Angle).Throws(() => new Exception()).Verifiable();
        turnable.SetupGet(t => t.DeltaAngle).Returns(new VectorTurn(100)).Verifiable();

        ICommand turnCommand = new TurnCommand(turnable.Object);

        Assert.Throws<Exception>(() => turnCommand.Execute());
    }
    // попытка сдвинуть корабль, у которого невозможно прочитать значение угловой скорости, приводит к ошибке.
    [Fact]
    public void TurnCommandCantDefineDeltaAngle()
    {
        var turnable = new Mock<ITurnable>();

        turnable.SetupGet(t => t.Angle).Returns(new VectorTurn(45)).Verifiable();
        turnable.SetupGet(t => t.DeltaAngle).Throws(() => new Exception()).Verifiable();

        ICommand turnCommand = new TurnCommand(turnable.Object);
        
        Assert.Throws<Exception>(() => turnCommand.Execute());
    } 
    // попытка сдвинуть корабль, у которого невозможно изменить угол наклона к горизонту, приводит к ошибке.
    [Fact]
    [Obsolete]
    public void TurnCommandCantChangeAngle()
    {
        var turnable = new Mock<ITurnable>();

        turnable.SetupGet(t => t.Angle).Returns(new VectorTurn(120)).Verifiable();
        turnable.SetupGet(t => t.DeltaAngle).Returns(new VectorTurn(90)).Verifiable();
        turnable.SetupSet(t => t.Angle).Throws(() => new Exception()).Verifiable();

        ICommand turnCommand = new TurnCommand(turnable.Object);

        Assert.Throws<Exception>(() => turnCommand.Execute());
    }
}