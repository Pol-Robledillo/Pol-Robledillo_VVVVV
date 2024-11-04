using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public MainCamera instance;
    private void Awake()
    {
        if (FindObjectsOfType<MainCamera>().Count() > 1)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
