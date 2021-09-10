using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class QuestionableCubeScript : MonoBehaviour
{
    private static QuestionableCubeScript cubeScriptInstance;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (cubeScriptInstance == null)
        {
            cubeScriptInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCubesActive(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);

        }
    }
}
