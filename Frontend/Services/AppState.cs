namespace Aspiregregator.Frontend.Services;

public sealed class AppState
{
    public event Action? StateChanged;

    internal void AppStateChanged() => StateChanged?.Invoke();
}
