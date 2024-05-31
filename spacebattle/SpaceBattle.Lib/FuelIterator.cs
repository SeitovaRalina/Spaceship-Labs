using System.Collections;
using Hwdtech;

namespace SpaceBattle.Lib;

public class FuelIterator : IEnumerator<object>
{
    private readonly IList<int> _fuels;
    private int _index = 0;
    public FuelIterator(IList<int> fuels)
    {
        _fuels = fuels;
    }
    public bool MoveNext() => ++_index < _fuels.Count;
    public object Current => _fuels[_index];
    public void Reset() => _index = 0;
    public void Dispose() => GC.SuppressFinalize(this);
}

public class SpaceshipFuels : IEnumerable<object>
{
    private readonly IList<int> _fuelVolumes = IoC.Resolve<IList<int>>("Game.Fuels");
    public IEnumerator<object> GetEnumerator() => new FuelIterator(_fuelVolumes);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
