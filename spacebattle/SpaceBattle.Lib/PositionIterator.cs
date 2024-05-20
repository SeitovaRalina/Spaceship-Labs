using System.Collections;
using Hwdtech;

namespace SpaceBattle.Lib;

public class PositionIterator : IEnumerator<object>
{
    private readonly IList<Vector> _positions;
    private readonly int _indexStart;
    private int _index;
    public PositionIterator(IList<Vector> positions, int indexStart)
    {
        _positions = positions;
        _indexStart = indexStart;

        _index = indexStart;
    }
    public bool MoveNext() => ++_index < _positions.Count;
    public object Current => _positions[_index];
    public void Reset() => _index = _indexStart;
    public void Dispose() => throw new NotImplementedException();
}

public class SpaceshipPositions : IEnumerable<object>
{
    private readonly IList<Vector> _positionValues = IoC.Resolve<IList<Vector>>("Game.Positions");
    public int StartPosition { get; set; } = 0;
    public IEnumerator<object> GetEnumerator() => new PositionIterator(_positionValues, StartPosition);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
