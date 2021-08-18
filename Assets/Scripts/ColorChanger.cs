using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Color main;

    void Start() {
        main = gameObject.GetComponent<Renderer>().material.color;
    }
    public void changeFocusColor()
    {
        var renderer = gameObject.GetComponent<Renderer>();

       //Call SetColor using the shader property name "_Color" and setting the color to red
       renderer.material.SetColor("_Color", Color.cyan);
    }
    public void changeToMainColor()
    {
        var renderer = gameObject.GetComponent<Renderer>();

       //Call SetColor using the shader property name "_Color" and setting the color to red
       renderer.material.SetColor("_Color", main);
    }
}
