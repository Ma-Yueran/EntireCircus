using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmoBox : MonoBehaviour
{
    public Image image;

    public void ChangeImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
