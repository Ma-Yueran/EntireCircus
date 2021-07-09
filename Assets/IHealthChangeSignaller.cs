public interface IHealthChangeSignaller
{
    void RegisterAsHealthChangeListener(IHealthChangeListener listener);
}
