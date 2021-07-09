public interface IHealthChangeListener
{
    void ReceiveHealthChangeSignal(Alignment alignment, int currentHealth, int maxHealth);
}
