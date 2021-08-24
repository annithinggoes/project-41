using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
public class SetPositionFrontOfPerson : MonoBehaviour
{
    public float distance = 1f;
    public void SetPosition()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position = CameraCache.Main.transform.position + CameraCache.Main.transform.forward*distance;
    }
}
