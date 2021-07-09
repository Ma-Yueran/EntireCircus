using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPowerBar : MonoBehaviour
{
    public GameObject powerBarContent;

    RectTransform _powerBarRectTransform;
    Vector2 _powerBarOriginalRectDelta;

    void Start()
    {
        _powerBarRectTransform = powerBarContent.GetComponent<RectTransform>();
        _powerBarOriginalRectDelta = _powerBarRectTransform.sizeDelta;
        _powerBarRectTransform.sizeDelta = new Vector2(0f, _powerBarOriginalRectDelta.y);
    }

    public void ResizePowerBar(float chargedPower, float maximumChargedPower)
    {
        float chargeBarWidth = _powerBarOriginalRectDelta.x * chargedPower / maximumChargedPower;
        _powerBarRectTransform.sizeDelta = new Vector2(chargeBarWidth, _powerBarOriginalRectDelta.y);
    }
}
