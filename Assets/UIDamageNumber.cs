using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDamageNumber : MonoBehaviour, IObjectPoolDeallocationSignaller
{    
    public int damageNumberIndex = -1;
    float _riseSpeed = 40f;
    float _timeToLive = 1f;
    float _timeLived = 0f;

    List<IObjectPoolDeallocationListener> _listeners = new List<IObjectPoolDeallocationListener>();

    TextMeshProUGUI _textMesh;

    void Start()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();

        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + _riseSpeed * Time.deltaTime);

        _timeLived += Time.deltaTime;

        if (_timeLived > _timeToLive)
        {
            NotifyListeners();
        }
    }

    public void RegisterAsListener(IObjectPoolDeallocationListener listener)
    {
        _listeners.Add(listener);
    }

    public void ShowDamageNumber(int damageAmount, Vector2 positionToSpawn, RectTransform uiCanvasRectTransform)
    {
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(positionToSpawn);
        Vector2 proportionalPosition = new Vector2(viewportPosition.x * uiCanvasRectTransform.sizeDelta.x, viewportPosition.y * uiCanvasRectTransform.sizeDelta.y);

        gameObject.SetActive(true);
        _textMesh.text = "-" + damageAmount;
        _timeLived = 0f;

        transform.position = proportionalPosition;
    }

    public void DestroyDamageNumber()
    {
        gameObject.SetActive(false);
    }

    void NotifyListeners()
    {
        foreach (IObjectPoolDeallocationListener listener in _listeners)
        {
            listener.ReceiveDeallocationSignal(damageNumberIndex);
        }
    }
}
