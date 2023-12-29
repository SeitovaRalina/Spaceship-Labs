namespace SpaceBattle.Lib;

public class GetHashCodeStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var types = ((IEnumerable<Type>)args[0]).OrderBy(x => x.GetHashCode());
        unchecked
        {
            return types.Aggregate(1, (full, next) => HashCode.Combine(full, next));
        }
    }
}
