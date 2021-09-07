using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void changeScene(string sceneName)
    {
        PlayerPrefs.SetFloat("RotationX", Camera.main.transform.eulerAngles.x);
        PlayerPrefs.SetFloat("RotationY", Camera.main.transform.eulerAngles.y);
        PlayerPrefs.SetFloat("RotationZ", Camera.main.transform.eulerAngles.z);
        PlayerPrefs.SetFloat("PositionX", Camera.main.transform.position.x);
        PlayerPrefs.SetFloat("PositionY", Camera.main.transform.position.y);
        PlayerPrefs.SetFloat("PositionZ", Camera.main.transform.position.z);
        Debug.Log(Camera.main.transform.rotation);
        Debug.Log(Camera.main.transform.position);
        SceneManager.LoadScene(sceneName);
    }
}
