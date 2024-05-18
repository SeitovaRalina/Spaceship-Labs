using System.Collections;
using Hwdtech;

namespace SpaceBattle.Lib;

public class PositionIterator : IEnumerator<object>
{
    private readonly IList<Vector> _positions;
    private int _index;
    public PositionIterator(IList<Vector> positions, int index)
    {
        _positions = positions;
        _index = index;
    }
    public bool MoveNext() => ++_index < _positions.Count;
    public object Current => _positions[_index];
    public void Reset() => _index = 0;
    public void Dispose() => throw new NotImplementedException();
}

public class SpaceshipPositions : IEnumerable<object>
{
    private readonly IList<Vector> positionValues = IoC.Resolve<IList<Vector>>("Game.Positions");
    public int startPosition { get; set; } = 0;
    public IEnumerator<object> GetEnumerator() => new PositionIterator(positionValues, startPosition);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
