public interface IObjectPoolDeallocationSignaller
{
    void RegisterAsListener(IObjectPoolDeallocationListener listener);
}
