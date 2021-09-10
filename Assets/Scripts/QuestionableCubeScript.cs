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
    public GameObject approveMarker;
    public GameObject rejectMarker;
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

    public void ResetCubeAnnotations()
    {
        string approveMarkerName = approveMarker.name + "(Clone)";
        string rejectMarkerName = rejectMarker.name + "(Clone)";
        foreach (Transform child in transform)
        {
            if (child.Find(approveMarkerName) != null)
            {
                Destroy(child.Find(approveMarkerName).gameObject);
            }
            if (child.Find(rejectMarkerName) != null)
            {
                Destroy(child.Find(rejectMarkerName).gameObject);
            }
            GameObject commentGUI = child.Find("CommentGUI").gameObject;
                commentGUI.SetActive(false);
        }
    }
}
