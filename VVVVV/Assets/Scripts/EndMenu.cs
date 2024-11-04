using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    public Button restartButton;
    public Button quitButton;
    void Start()
    {
        if (GameManager.instance != null)
        {
            restartButton.onClick.AddListener(GameManager.instance.LoadLevel1);
            quitButton.onClick.AddListener(GameManager.instance.Exit);
        }
    }
}
