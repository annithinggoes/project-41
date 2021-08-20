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
    public float angleCurlThreshold;

    /// <summary>
    /// Threshold for defining a straight finger
    /// </summary>
    public float angleStraightThreshold;
    public float thumbsUpCurlThreshold;
    public float thumbsUpStraightThreshold;

    public TextMeshPro textMeshPro;
    public TextMeshPro textMeshProHit;
    public TextMeshPro textMeshProWrist;
    public TextMeshPro textMeshProDebugger;

    Handedness rightHand = Handedness.Right;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Check if hand in view
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, rightHand, out MixedRealityPose pose))
        {
            if (checkAngle())
            {
                textMeshProHit.SetText("ANGLE");
            }
            else if (checkThumbs())
            {

            }
            else
            {
                textMeshProHit.SetText("NONE");
            }

            // Debug.Log("Thumb" + HandPoseUtils.ThumbFingerCurl(hand));
            // Debug.Log("Index" + HandPoseUtils.IndexFingerCurl(hand));
            textMeshPro.SetText(
                "Thumb " + HandPoseUtils.ThumbFingerCurl(rightHand)
                + "\nIndex " + HandPoseUtils.IndexFingerCurl(rightHand)
                + "\nMiddle " + HandPoseUtils.MiddleFingerCurl(rightHand)
                + "\nRing " + HandPoseUtils.RingFingerCurl(rightHand)
                + "\nPinky " + HandPoseUtils.PinkyFingerCurl(rightHand)
                );

            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, rightHand, out MixedRealityPose wristPose))
            {
                textMeshProWrist.SetText("Wrist\n Position " + wristPose.Position
                + "\nRotation " + wristPose.Rotation);
            }

        }
    }

    bool checkAngle()
    {
        Debug.Log("checkAngle");
        if (HandPoseUtils.ThumbFingerCurl(rightHand) <= angleStraightThreshold &&
        HandPoseUtils.IndexFingerCurl(rightHand) <= angleStraightThreshold &&
        HandPoseUtils.MiddleFingerCurl(rightHand) > angleCurlThreshold &&
        HandPoseUtils.RingFingerCurl(rightHand) > angleCurlThreshold &&
        HandPoseUtils.PinkyFingerCurl(rightHand) > angleCurlThreshold)
        {
            textMeshProHit.SetText("ANGLE");
            Debug.Log("L HIT");
            return true;
        }
        return false;
    }
    bool checkThumbs()
    {
        Debug.Log("checkThumbsUp");

        if (HandPoseUtils.ThumbFingerCurl(rightHand) <= thumbsUpStraightThreshold &&
        HandPoseUtils.IndexFingerCurl(rightHand) > thumbsUpCurlThreshold &&
        HandPoseUtils.MiddleFingerCurl(rightHand) > thumbsUpCurlThreshold &&
        HandPoseUtils.RingFingerCurl(rightHand) > thumbsUpCurlThreshold &&
        HandPoseUtils.PinkyFingerCurl(rightHand) > thumbsUpCurlThreshold)
        {
            if (isThumbUp())
            {
                textMeshProHit.SetText("Thumbs UP");
                Debug.Log("THUMB HIT");
                return true;
            }
            else if (isThumbDown())
            {
                textMeshProHit.SetText("Thumbs DOWN");
                return true;
            }
        }

        return false;
    }
    // 0 - 60, thumbdpwn is 120- 180
    private bool isThumbUp()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, rightHand, out MixedRealityPose thumbTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, rightHand, out MixedRealityPose thumbProximalPose))
        {
            Vector3 thumbDirection = thumbTipPose.Position - thumbProximalPose.Position;
            float thumbCameraAngle = Vector3.Angle(thumbDirection, CameraCache.Main.transform.up);

            return thumbCameraAngle < 60;

        }
        return false;
    }
    private bool isThumbDown()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, rightHand, out MixedRealityPose thumbTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, rightHand, out MixedRealityPose thumbProximalPose))
        {
            Vector3 thumbDirection = thumbTipPose.Position - thumbProximalPose.Position;
            float thumbCameraAngle = Vector3.Angle(thumbDirection, CameraCache.Main.transform.up);

            return thumbCameraAngle > 120;

        }
        return false;
    }
}