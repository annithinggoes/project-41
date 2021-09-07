using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    private static ResetPlayerPrefs instance;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
            PlayerPrefs.DeleteAll();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
