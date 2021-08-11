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
        cube.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (cube.GetComponent<Renderer>().enabled == true)
        {
            //
        }
        else
        {

        }
        cube.GetComponent<Renderer>().enabled = false;

        Handedness hand = Handedness.Right;

        // Check if hand in view
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out MixedRealityPose pose))
        {
            if (HandPoseUtils.ThumbFingerCurl(hand) <= straightThreshold &&
            HandPoseUtils.IndexFingerCurl(hand) <= straightThreshold &&
            HandPoseUtils.MiddleFingerCurl(hand) > curlThreshold &&
            HandPoseUtils.RingFingerCurl(hand) > curlThreshold &&
            HandPoseUtils.PinkyFingerCurl(hand) > curlThreshold)
            {
                cube.GetComponent<Renderer>().enabled = true;
                cube.transform.position = new Vector3(0, 0, 0.5f);
            }
            Debug.Log("Thumb" + HandPoseUtils.ThumbFingerCurl(hand));
            Debug.Log("Index" + HandPoseUtils.IndexFingerCurl(hand));
        }

    }
}

