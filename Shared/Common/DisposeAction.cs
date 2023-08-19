namespace Shared.Common;

public class DisposeAction : IDisposable
{
    private readonly Action action;

    public DisposeAction(Action action)
    {
        this.action = action;
    }
    
    public void Dispose()
    {
        action();
    }
}