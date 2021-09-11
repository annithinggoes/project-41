using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Tutorial : GestureRecogniser
{
    public TextMeshPro textMeshPro;
    public GameObject questionCube;
    public GameObject handMenu;
    public AnnotationMaker annotationMaker;
    public GameObject exitMeasureMenu;
    public GameObject plans;
    private SelectPointsMeasureAngles angles;
    private SelectPointsDistance distance;
    private PhotoTaker photo;
    private ChangeScene sceneChanger;
    private Stopwatch stopwatch;

    private List<string> times;
    private string[] dialogue = {
        "This tutorial will go through the gestures and functions in this app. Please perform each gesture to move on to the next one. Press Next to begin.",
        "To open the menu, bring up your right palm with the palm facing you. It will follow your hand. You can place it down by turning your palm away from you. Press the X button to close it.",
        "To approve an item, gaze at the cube and perform a Thumbs Up gesture.",
        "To reject an item, gaze at the cube and perform a Thumb Down gesture",
        "To start measuring angles, make an 'L' shape with your thumb and index finger.\nAir tap to select three points to measure the angle between. To exit measuring mode, look at your right palm to bring up the menu, and press Back.",
        "To start measuring distance, hold your two index fingers upright and parallel to each other.\nAir tap to select two points to measure distance between. To exit measuring mode, look at your right palm to bring up the menu, and press Back.",
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
        stopwatch = new Stopwatch();
        times = new List<string>();
        sceneChanger = GetComponent<ChangeScene>();
    }

    public void ShowNextDialogue()
    {
        currentIndex++;
        switch (currentIndex)
        {
            case 2:
                // approve
                questionCube.SetActive(true);
                stopwatch.Start();
                handMenu.SetActive(false);
                break;
            case 3:
                // reject
                setStopwatch("Approve");
                break;
            case 4:
                // angle
                questionCube.SetActive(false);
                setStopwatch("Reject");
                break;
            case 5:
                // distance
                ExitMeasuringTool();
                setStopwatch("Angle");
                break;
            case 6:
                // photo
                ExitMeasuringTool();
                setStopwatch("Distance");
                break;
            case 7:
                // plans
                setStopwatch("Photo");
                break;
            case 8:
                // end
                plans.SetActive(false);
                stopwatch.Stop();
                times.Add(String.Format("Show Plans time: {0}", stopwatch.Elapsed));
                UnityEngine.Debug.Log(String.Format("Show Plans time: {0}", stopwatch.Elapsed));
                writeTimesToFile();
                break;
            case 9:
                currentIndex--;
                sceneChanger.changeScene("SceneWithMR");
                break;
        }
        textMeshPro.SetText(dialogue[currentIndex]);

    }
    private void setStopwatch(string function)
    {
        stopwatch.Stop();
        times.Add(String.Format(function + " time: {0}", stopwatch.Elapsed));
        UnityEngine.Debug.Log(String.Format(function + " time: {0}", stopwatch.Elapsed));
        stopwatch.Reset();
        stopwatch.Start();
    }

    private void writeTimesToFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "LearningGestureTimes_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt");
        UnityEngine.Debug.Log(path);
        using (TextWriter writer = File.CreateText(path))
        {
            foreach (string s in times)
            {
                writer.WriteLine(s);
            }
        }
    }
    override protected void gestureRecogniser()
    {
        switch (currentIndex)
        {
            case 2:
                // approve
                if (checkThumbs("Up"))
                {
                    TriggerGesture();
                }
                break;
            case 3:
                // reject
                if (checkThumbs("Down"))
                {
                    TriggerGesture();
                }
                break;
            case 4:
                // angle
                if (checkAngle(rightHand) || checkAngle(leftHand))
                {
                    TriggerGesture();
                }
                break;
            case 5:
                // distance
                if (checkDistance())
                {
                    TriggerGesture();
                }
                break;
            case 6:
                // photo
                if (checkPhoto())
                {
                    TriggerGesture();
                }
                break;
            case 7:
                // plans
                if (checkPlans())
                {
                    TriggerGesture();
                }
                break;
        }
    }
    public void TriggerGesture()
    {
        switch (currentIndex)
        {
            case 2:
                // approve
                annotationMaker.TutorialApproveHighlightedObject();
                break;
            case 3:
                //reject
                annotationMaker.TutorialRejectHighlightedObject();
                break;
            case 4:
                //angles
                angles.enabled = true;
                exitMeasureMenu.SetActive(true);
                break;
            case 5:
                // distance
                distance.enabled = true;
                exitMeasureMenu.SetActive(true);
                break;
            case 6:
                // photo
                photo.StartPhotoTimer();
                break;
            case 7:
                // plans
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
