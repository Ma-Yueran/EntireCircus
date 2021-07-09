using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Tent tent;

    public List<KeyCode> ammoSelectionHotkeys = new List<KeyCode>();
    int _leftMouseButton = 0;
    int _rightMouseButton = 1;

    void Update()
    {
        // Update pointer direction
        tent.UpdateMousePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        // State transitions between charging and firing
        if (Input.GetMouseButtonDown(_leftMouseButton))
        {
            tent.BeginChargingAttack();
        }
        else if (Input.GetMouseButtonUp(_leftMouseButton))
        {
            tent.FireChargedAttack();
        }

        if (Input.GetMouseButtonDown(_rightMouseButton))
        {
            tent.CancelChargingAttack();
        }

        // Ammo selection
        bool hotkeyDown = false;
        int selectedAmmo = 0;

        for (int i = 0; i < ammoSelectionHotkeys.Count; i++)
        {
            if (Input.GetKeyDown(ammoSelectionHotkeys[i]))
            {
                hotkeyDown = true;
                selectedAmmo = i;
                break;
            }
        }

        if (hotkeyDown)
        {
            tent.SelectAmmo(selectedAmmo);
        }
    }
}
