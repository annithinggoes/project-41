using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCameraPosition : MonoBehaviour
{
    void Start()
    {
        Debug.Log("start");
        if (PlayerPrefs.GetFloat("RotationX", 99999f) != 99999f)
        {
            float rX = PlayerPrefs.GetFloat("RotationX");
            float rY = PlayerPrefs.GetFloat("RotationY");
            float rZ = PlayerPrefs.GetFloat("RotationZ");
            float pX = PlayerPrefs.GetFloat("PositionX");
            float pY = PlayerPrefs.GetFloat("PositionY");
            float pZ = PlayerPrefs.GetFloat("PositionZ");
            Debug.Log(rX + ", " + rY + ", " + rZ + "\n" + pX + "," + pY + "," + pZ);
            Vector3 euler = new Vector3(rX, rY, rZ);
            Camera.main.transform.Rotate(euler, Space.Self);
            Vector3 pos = new Vector3(pX, pY, pZ);
            Camera.main.transform.position = pos;
            Debug.Log(Camera.main.transform.rotation);
            Debug.Log(Camera.main.transform.position);
        }
    }
}
