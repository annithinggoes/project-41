using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{

    public TextMeshPro textMeshPro;
    private string[] dialogue = {
        "This tutorial will go through the gestures and functions in this app. Please perform each gesture to move on to the next one. Press Next to begin.",
        "To approve an item, gaze at the cube and perform a Thumbs Up gesture.",
        "To reject an item, gaze at the cube and perform a Thumb Down gesture",
        "To start measuring angles, make an 'L' shape with your thumb and index finger",
        "To start measuring distance, hold your two index fingers upright and parallel to each other.",
        "To take a photo, make a picture frame gesture with both of your hands.",
        "To open the building plans, place your palms side-by-side like an open book."
    };
    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        textMeshPro.SetText(dialogue[currentIndex]);
        currentIndex++;

    }

    // Update is called once per frame
    public void ShowNextDialogue()
    {
        textMeshPro.SetText(dialogue[currentIndex]);
        currentIndex++;
        currentIndex = currentIndex % dialogue.Length;
    }
}
