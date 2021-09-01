using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public GameObject questionCube;
    public AnnotationMaker annotationMaker;
    public GameObject exitMeasureMenu;
    public GameObject plans;
    private SelectPointsMeasureAngles angles;
    private SelectPointsDistance distance;
    private PhotoTaker photo;

    private string[] dialogue = {
        "This tutorial will go through the gestures and functions in this app. Please perform each gesture to move on to the next one. Press Next to begin.",
        "To approve an item, gaze at the cube and perform a Thumbs Up gesture.",
        "To reject an item, gaze at the cube and perform a Thumb Down gesture",
        "To start measuring angles, make an 'L' shape with your thumb and index finger.\nAir tap to select three points to measure the angle between. To exit measuring mode, look at your palm to bring up the menu, and press Back.",
        "To start measuring distance, hold your two index fingers upright and parallel to each other.\nAir tap to select two points to measure distance between. To exit measuring mode, look at your palm to bring up the menu, and press Back.",
        "To take a photo, make a picture frame gesture with both of your hands.",
        "To open the building plans, place your palms side-by-side like an open book.",
        "End of tutorial. Press Next to continue to the main application."
    };
    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        textMeshPro.SetText(dialogue[currentIndex]);
        questionCube.SetActive(false);
        angles = GetComponent<SelectPointsMeasureAngles>();
        angles.enabled = false;
        distance = GetComponent<SelectPointsDistance>();
        distance.enabled = false;
        photo = GetComponent<PhotoTaker>();
    }

    public void ShowNextDialogue()
    {
        currentIndex++;
        switch (currentIndex)
        {
            case 1:
                // approve
                questionCube.SetActive(true);
                break;
            case 3:
                // angle
                questionCube.SetActive(false);
                break;
            case 4:
                // distance
                ExitMeasuringTool();
                break;
            case 5:
                // photo
                ExitMeasuringTool();
                break;
            case 8: 
                SceneManager.LoadScene("SceneWithMR");
                currentIndex--;
                break;
        }
        textMeshPro.SetText(dialogue[currentIndex]);

    }

    public void TriggerGesture()
    {
        switch (currentIndex)
        {
            case 1:
                // approve
                annotationMaker.TutorialApproveHighlightedObject();
                break;
            case 2:
                annotationMaker.TutorialRejectHighlightedObject();
                break;
            case 3:
                angles.enabled = true;
                exitMeasureMenu.SetActive(true);
                break;
            case 4:
                distance.enabled = true;
                exitMeasureMenu.SetActive(true);
                break;
            case 5:
                photo.StartPhotoTimer();
                break;
            case 6: 
                plans.GetComponent<SetPositionFrontOfPerson>().SetPosition();
                plans.SetActive(true);
                break;
        }
    }

    public void ExitMeasuringTool()
    {
        angles.enabled = false;
        distance.enabled = false;
        exitMeasureMenu.SetActive(false);
    }
}
