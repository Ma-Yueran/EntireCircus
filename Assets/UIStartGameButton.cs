using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartGameButton : MonoBehaviour
{
    Button _button;

    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(StartGameButton);
    }

    void StartGameButton()
    {
        SceneLoader.instance.OpenGame();
    }
}
