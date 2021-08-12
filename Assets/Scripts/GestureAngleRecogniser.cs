using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class GestureAngleRecogniser : MonoBehaviour
{
    /// <summary>
    /// Threshold for defining a curled finger
    /// </summary>
    public float curlThreshold;

    /// <summary>
    /// Threshold for defining a straight finger
    /// </summary>
    public float straightThreshold;

    public TextMeshPro textMeshPro;
    public TextMeshPro textMeshProHit;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
                textMeshProHit.SetText(
                    "Thumb" + HandPoseUtils.ThumbFingerCurl(hand)
                + "\nIndex" + HandPoseUtils.IndexFingerCurl(hand)
                + "\nMiddle" + HandPoseUtils.MiddleFingerCurl(hand)
                + "\nRing" + HandPoseUtils.RingFingerCurl(hand)
                + "\nPinky" + HandPoseUtils.PinkyFingerCurl(hand)
                + "\nHIT");
                Debug.Log("HIT");
            }
            Debug.Log("Thumb" + HandPoseUtils.ThumbFingerCurl(hand));
            Debug.Log("Index" + HandPoseUtils.IndexFingerCurl(hand));
            textMeshPro.SetText(
                "Thumb" + HandPoseUtils.ThumbFingerCurl(hand)
                + "\nIndex" + HandPoseUtils.IndexFingerCurl(hand)
                + "\nMiddle" + HandPoseUtils.MiddleFingerCurl(hand)
                + "\nRing" + HandPoseUtils.RingFingerCurl(hand)
                + "\nPinky" + HandPoseUtils.PinkyFingerCurl(hand)
                );
        }


    }
}

