using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmo : MonoBehaviour, IAmmoViewChangeSignaller
{
    public static readonly int playerAmmoStockSize = 5;

    public float reloadTime = 2f;

    public AmmoDispenser ammoDispenser;
    public List<AmmoData> playerAmmoStock = new List<AmmoData>();
    List<float> _ammoReloadTimer = new List<float>();

    bool ammoLoaded = false;

    List<IAmmoViewChangeListener> _listeners = new List<IAmmoViewChangeListener>();

    void Start()
    {
        if (ammoLoaded) return;

        for (int i = 0; i < playerAmmoStockSize; i++)
        {
            playerAmmoStock.Add(ammoDispenser.GetAmmoData());
            _ammoReloadTimer.Add(-1f);
        }

        AlertListeners();

        ammoLoaded = true;
    }

    void Update()
    {
        bool shouldRedrawUI = false;

        for (int i = 0; i < playerAmmoStockSize; i++)
        {
            if (_ammoReloadTimer[i] < 0)
            {
                continue;
            }

            _ammoReloadTimer[i] -= Time.deltaTime;

            if (_ammoReloadTimer[i] <= 0)
            {
                playerAmmoStock[i] = ammoDispenser.GetAmmoData();
                _ammoReloadTimer[i] = -1;
                shouldRedrawUI = true;
            }
        }

        if (shouldRedrawUI)
        {
            AlertListeners();
        }
    }

    public bool AmmoAtIndexFireable(int selectedAmmo)
    {
        if (selectedAmmo < 0) return false;
        if (playerAmmoStock[selectedAmmo].fireable) return true;
        return false;
    }

    public AmmoData FireAmmoAtIndex(int selectedAmmo)
    {
        AmmoData ammoToFire = playerAmmoStock[selectedAmmo];
        playerAmmoStock[selectedAmmo] = ammoDispenser.nullAmmoData;
        _ammoReloadTimer[selectedAmmo] = reloadTime; 

        foreach (IAmmoViewChangeListener listener in _listeners) 
        {
            listener.ReceiveAmmoViewChangeSignal(playerAmmoStock);
        }

        return ammoToFire;
    }

    public void RegisterAsAmmoViewChangeListener(IAmmoViewChangeListener listener)
    {
        _listeners.Add(listener);
    }

    void AlertListeners()
    {
        foreach (IAmmoViewChangeListener listener in _listeners) 
        {
            listener.ReceiveAmmoViewChangeSignal(playerAmmoStock);
        }
    }
}
