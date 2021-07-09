using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberObjectPool : MonoBehaviour, IObjectPoolDeallocationListener
{
    public static int poolSize = 40;
    public GameObject damageNumberBase;

    public GameObject uiCanvas;

    public static DamageNumberObjectPool instance;

    List<UIDamageNumber> _damageNumberPool = new List<UIDamageNumber>();
    Stack<int> _availableDamageNumbers = new Stack<int>();

    RectTransform _uiCanvasRectTransform;

    bool _initialized = false;

    void Start()
    {
        if (_initialized) return;

        if (instance != null)
        {
            Debug.Log("ProjectileObjectPool: Attempting to create second ProjectileObjectPool!");
            return;
        }

        instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject damageNumberObject = Instantiate(damageNumberBase, new Vector3(-5f, 0, 0), Quaternion.identity, transform);

            UIDamageNumber damageNumber = damageNumberObject.GetComponent<UIDamageNumber>();
            damageNumber.damageNumberIndex = i;
            _damageNumberPool.Add(damageNumber);

            damageNumber.RegisterAsListener(this);

            _availableDamageNumbers.Push(i);
        }

        _uiCanvasRectTransform = uiCanvas.GetComponent<RectTransform>();

        _initialized = true;
    }

    public void ShowDamageNumber(int damageAmount, Vector2 positionToSpawn)
    {
        if (_availableDamageNumbers.Count == 0)
        {
            Debug.Log("DamageNumberObjectPool: Maximum capacity reached!");
            return;
        }

        int damageNumberToUse = _availableDamageNumbers.Pop();
        _damageNumberPool[damageNumberToUse].ShowDamageNumber(damageAmount, positionToSpawn, _uiCanvasRectTransform);
    }

    public void ReceiveDeallocationSignal(int indexToDeallocate)
    {
        if (indexToDeallocate < 0) {
            Debug.Log("DamageNumberObjectPool: Negative-indexed damage number detected!");
            return;
        }

        _damageNumberPool[indexToDeallocate].DestroyDamageNumber();
        _availableDamageNumbers.Push(indexToDeallocate);
    }
}
