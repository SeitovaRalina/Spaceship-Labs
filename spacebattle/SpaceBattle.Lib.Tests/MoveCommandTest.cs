using Moq;

namespace SpaceBattle.Lib.Tests;

public class MoveCommandTest
{

    // 1. Всё работает
    // 2. Не читается Position
    // 3. Не читается Velocity
    // 4. Не пишется в Position

    [Fact]
    public void MoveCommandPositive()
    {
        // pre
        var movable = new Mock<IMovable>();

        movable.SetupGet(m => m.Position).Returns(new Vector(new int[] { 12, 5 })).Verifiable();
        movable.SetupGet(m => m.Velocity).Returns(new Vector(new int[] { -7, 3 })).Verifiable();

        ICommand moveCommand = new MoveCommand(movable.Object);

        // action
        moveCommand.Execute();

        //post
        // movable.Position is correct
        movable.Verify();
        movable.VerifySet(m => m.Position = new Vector(new int[] { 5, 8 }), Times.Once);
        
        movable.VerifyAll(); // !!!
    }
}