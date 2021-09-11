using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class EnableCubes : MonoBehaviour
{
    // Start is called before the first frame update
    public bool cubesEnabled = false;
    void Start()
    {
        QuestionableCubeScript cubes = FindObjectOfType<QuestionableCubeScript>();
        if (cubes != null){
            cubes.SetCubesActive(cubesEnabled);
        } else {
            Debug.Log("could not find cubes");
        } 
    }

    public void callResetCubes() {
        QuestionableCubeScript cubes = FindObjectOfType<QuestionableCubeScript>();
        if (cubes != null){
            cubes.ResetCubeAnnotations();
        } else {
            Debug.Log("could not find cubes");
        } 

    }
}
