public interface IGameManager
{
    public ManagerStatus Status { get; }

    void Startup();
}
    
public enum ManagerStatus
{
    Shutdown,
    Initializing,
    Started
}