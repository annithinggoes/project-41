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
    public AnnotationMaker annotationMaker;
    public GameObject exitMeasureMenu;
    public GameObject plans;
    private SelectPointsMeasureAngles angles;
    private SelectPointsDistance distance;
    private PhotoTaker photo;
    private Stopwatch stopwatch;

    private List<string> times;
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
        stopwatch = new Stopwatch();
        times = new List<string>();
    }

    public void ShowNextDialogue()
    {
        currentIndex++;
        switch (currentIndex)
        {
            case 1:
                // approve
                questionCube.SetActive(true);
                stopwatch.Start();
                break;
            case 2:
                // reject
                setStopwatch("Approve");
                break;
            case 3:
                // angle
                questionCube.SetActive(false);
                setStopwatch("Reject");
                break;
            case 4:
                // distance
                ExitMeasuringTool();
                setStopwatch("Angle");
                break;
            case 5:
                // photo
                ExitMeasuringTool();
                setStopwatch("Distance");
                break;
            case 6:
                // plans
                setStopwatch("Photo");
                break;
            case 7:
                // end
                plans.SetActive(false);
                stopwatch.Stop();
                times.Add(String.Format("Show Plans time: {0}", stopwatch.Elapsed));
                UnityEngine.Debug.Log(String.Format("Show Plans time: {0}", stopwatch.Elapsed));
                writeTimesToFile();
                break;
            case 8:
                SceneManager.LoadScene("SceneWithMR");
                currentIndex--;
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
            case 1:
                // approve
                if (checkThumbs("Up"))
                {
                    TriggerGesture();
                }
                break;
            case 2:
                // reject
                if (checkThumbs("Down"))
                {
                    TriggerGesture();
                }
                break;
            case 3:
                // angle
                if (checkAngle(rightHand) || checkAngle(leftHand))
                {
                    TriggerGesture();
                }
                break;
            case 4:
                // distance
                if (checkDistance())
                {
                    TriggerGesture();
                }
                break;
            case 5:
                // photo
                if (checkPhoto())
                {
                    TriggerGesture();
                }
                break;
            case 6:
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
