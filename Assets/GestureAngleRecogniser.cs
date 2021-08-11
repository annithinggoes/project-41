using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class GestureAngleRecogniser : MonoBehaviour
{
    public GameObject angleMarker;

    /// <summary>
    /// Threshold for defining a curled finger
    /// </summary>
    public float curlThreshold;

    /// <summary>
    /// Threshold for defining a straight finger
    /// </summary>
    public float straightThreshold;

    GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        cube = Instantiate(angleMarker, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        cube.GetComponent<Renderer>().enabled = false;

        Handedness hand = Handedness.Right;

        if (HandPoseUtils.ThumbFingerCurl(hand) < straightThreshold &&
        HandPoseUtils.IndexFingerCurl(hand) < straightThreshold &&
        HandPoseUtils.MiddleFingerCurl(hand) > curlThreshold &&
        HandPoseUtils.RingFingerCurl(hand) > curlThreshold &&
        HandPoseUtils.PinkyFingerCurl(hand) > curlThreshold)
        {
            cube.GetComponent<Renderer>().enabled = true;
            cube.transform.position = new Vector3(0,0,0.5f);
        }
    }
}
