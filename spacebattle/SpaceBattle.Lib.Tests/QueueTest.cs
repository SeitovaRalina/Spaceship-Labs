namespace SpaceBattle.Lib.Tests;

public class IQueueExample // тесты описывают пример реализации очереди IQueue
{
    [Fact]
    public void IQueueExampleTakeMethod()
    {
        var qReal = new Queue<ICommand>();
        var qMock = new Mock<IQueue>();
        qMock.Setup(q => q.Take()).Returns(qReal.Dequeue); // метод Take извлекает и возвращает первую команду очереди

        var cmd = new Mock<ICommand>();
        qReal.Enqueue(cmd.Object);

        Assert.Equal(cmd.Object, qMock.Object.Take());
    }

    [Fact]
    public void IQueueExampleAddMethod()
    {
        var qReal = new Queue<ICommand>();
        var qMock = new Mock<IQueue>();

        qMock.Setup(q => q.Take()).Returns(qReal.Dequeue);
        qMock.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(
        (ICommand cmd) =>
        {
            qReal.Enqueue(cmd);
        }); // метод Add добавляет команду в очередь

        var cmd = new Mock<ICommand>();
        qMock.Object.Add(cmd.Object);

        Assert.Equal(cmd.Object, qMock.Object.Take());
    }
}
