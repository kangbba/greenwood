using Cysharp.Threading.Tasks;

public abstract class Element
{
    public abstract UniTask ExecuteAsync();

    public abstract void ExecuteInstantly();

    public virtual void Execute()
    {
        ExecuteAsync().Forget();
    }
}
