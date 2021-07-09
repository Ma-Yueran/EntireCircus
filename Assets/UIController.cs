using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IAmmoViewChangeListener, IHealthChangeListener
{
    public Alignment playerToDisplayFor = Alignment.PlayerOne;

    public Tent playerOneTent;
    public Tent playerTwoTent;

    public List<GameObject> ammoStockBoxObjects;
    public UIPowerBar powerBar;

    public UIHealthBar playerOneHealthBar;

    public UIHealthBar playerTwoHealthBar;
 
    bool _initialized = false;
    List<UIAmmoBox> _uiAmmoBoxes = new List<UIAmmoBox>();

    void Start()
    {
        if (_initialized) return;

        Tent tentToDisplayUIFor = playerToDisplayFor == Alignment.PlayerOne ? playerOneTent : playerTwoTent;

        tentToDisplayUIFor.playerAmmo.RegisterAsAmmoViewChangeListener(this);

        playerOneTent.RegisterAsHealthChangeListener(this);
        playerTwoTent.RegisterAsHealthChangeListener(this);

        foreach (GameObject gameObject in ammoStockBoxObjects)
        {
            _uiAmmoBoxes.Add(gameObject.GetComponent<UIAmmoBox>());
        }
        _initialized = true;
    }

    public void ReceiveAmmoViewChangeSignal(List<AmmoData> playerAmmoStock)
    {
        for (int i = 0; i < playerAmmoStock.Count; i++)
        {
            _uiAmmoBoxes[i].ChangeImage(playerAmmoStock[i].sprite);
        }    
    }

    public void ReceiveHealthChangeSignal(Alignment alignment, int currentHealth, int maxHealth)
    {
        UIHealthBar healthBarToUpdate = alignment == Alignment.PlayerOne ? playerOneHealthBar : playerTwoHealthBar;
        healthBarToUpdate.UpdateHealthValue(currentHealth, maxHealth);
    }
}
