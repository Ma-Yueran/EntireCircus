public interface IObjectPoolDeallocationListener
{
    void ReceiveDeallocationSignal(int indexToDeallocate);
}
