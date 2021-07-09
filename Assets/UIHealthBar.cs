using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHealthBar : MonoBehaviour
{
    public TextMeshProUGUI remainingHealth;

    public GameObject healthBarContent;

    RectTransform _healthBarRectTransform;
    Vector2 _healthBarOriginalRectDelta;

    void Start()
    {
        _healthBarRectTransform = healthBarContent.GetComponent<RectTransform>();
        _healthBarOriginalRectDelta = _healthBarRectTransform.sizeDelta;
    }

    public void UpdateHealthValue(int currentHealth, int maxHealth)
    {
        remainingHealth.text = currentHealth.ToString();
        float healthBarWidth = _healthBarOriginalRectDelta.x * currentHealth / maxHealth;
        _healthBarRectTransform.sizeDelta = new Vector2(healthBarWidth, _healthBarOriginalRectDelta.y);
    }
}
