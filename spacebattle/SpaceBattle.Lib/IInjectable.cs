namespace SpaceBattle.Lib;

public interface IInjectable<T>
{
    public void Inject(T obj);
}