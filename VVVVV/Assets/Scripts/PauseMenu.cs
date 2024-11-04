using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    private void Awake()
    {
        if (PauseMenu.instance == null)
        {
            PauseMenu.instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }
}
