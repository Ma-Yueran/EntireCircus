using System.Collections.Generic;

public interface IAmmoViewChangeListener 
{
    void ReceiveAmmoViewChangeSignal(List<AmmoData> playerAmmoStock);
}
