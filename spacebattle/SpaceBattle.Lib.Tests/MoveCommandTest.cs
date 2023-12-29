namespace SpaceBattle.Lib.Tests;

public class MoveCommandTest
{
    [Fact]
    public void GameObjectAbleMove()
    {
        var movable = new Mock<IMovable>();

        movable.SetupGet(m => m.Position).Returns(new Vector(12, 5)).Verifiable();
        movable.SetupGet(m => m.Velocity).Returns(new Vector(-7, 3)).Verifiable();

        ICommand moveCommand = new MoveCommand(movable.Object);

        moveCommand.Execute();

        movable.VerifySet(m => m.Position = new Vector(5, 8), Times.Once);
        movable.VerifyAll();
    }
    [Fact]
    public void ImpossibleReadPositionGameObject()
    {
        var movable = new Mock<IMovable>();

        movable.SetupGet(m => m.Position).Throws(() => new Exception()).Verifiable();
        movable.SetupGet(m => m.Velocity).Returns(new Vector(-7, 3)).Verifiable();

        ICommand moveCommand = new MoveCommand(movable.Object);

        Assert.Throws<Exception>(() => moveCommand.Execute());
    }
    [Fact]
    public void AttemptMoveGameObjectInstantaneousVelocityValueCannotBeRead()
    {
        var movable = new Mock<IMovable>();

        movable.SetupGet(m => m.Position).Returns(new Vector(12, 5)).Verifiable();
        movable.SetupGet(m => m.Velocity).Throws(() => new Exception()).Verifiable();

        ICommand moveCommand = new MoveCommand(movable.Object);

        Assert.Throws<Exception>(() => moveCommand.Execute());
    }
    [Fact]
    public void AttemptMoveGameObjectPositionCannotChanged()
    {
        var movable = new Mock<IMovable>();

        movable.SetupGet(m => m.Position).Returns(new Vector(12, 5)).Verifiable();
        movable.SetupGet(m => m.Velocity).Returns(new Vector(-7, 3)).Verifiable();
        movable.SetupSet(m => m.Position = It.IsAny<Vector>()).Throws(() => new Exception()).Verifiable();

        ICommand moveCommand = new MoveCommand(movable.Object);

        Assert.Throws<Exception>(() => moveCommand.Execute());
    }
}
