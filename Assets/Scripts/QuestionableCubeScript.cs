using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class QuestionableCubeScript : MonoBehaviour, IMixedRealityPointerHandler
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

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        Debug.Log("onpointer down" + gameObject);
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        Debug.Log("onpointer dragged" + gameObject);
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        Debug.Log("onpointer up" + gameObject);
    }
    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
    }
}
