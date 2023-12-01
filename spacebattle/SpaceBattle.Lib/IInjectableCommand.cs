namespace SpaceBattle.Lib;

public interface IInjectableCommand
{
    public void Inject(ICommand obj);
}