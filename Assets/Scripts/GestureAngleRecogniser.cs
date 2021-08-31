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
    public float pointCurlThreshold;
    public float pointStraightThreshold;

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
            else if (checkDistance())
            {
                textMeshProHit.SetText("DISTANCE");
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

            // if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, rightHand, out MixedRealityPose wristPose))
            // {
            //     textMeshProWrist.SetText("Wrist\n Position " + wristPose.Position
            //     + "\nRotation " + wristPose.Rotation);
            // }

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
    bool checkDistance()
    {
        Debug.Log("checkDistance");
        Handedness leftHand = Handedness.Left;

        if (isIndexPointed(rightHand) && isIndexPointed(leftHand))
        {
            if (isFacingTowardsCentre(rightHand) && isFacingTowardsCentre(leftHand))
            {
                // Check they are relatively same height
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, rightHand, out MixedRealityPose rightIndexTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, leftHand, out MixedRealityPose leftIndexTipPose))
                {
                    bool isSameHeight = Mathf.Abs(rightIndexTipPose.Position.y - leftIndexTipPose.Position.y) < 0.05;
                    // Check right is on the right, and left is on the left
                    bool isCorrectPosition = (rightIndexTipPose.Position.x - leftIndexTipPose.Position.x) > 0;

                    return isSameHeight && isCorrectPosition;
                }

            }
        }

        return false;
    }

    private bool isIndexPointed(Handedness hand)
    {
        if (HandPoseUtils.ThumbFingerCurl(hand) > pointCurlThreshold &&
            HandPoseUtils.IndexFingerCurl(hand) <= pointStraightThreshold &&
            HandPoseUtils.MiddleFingerCurl(hand) > pointCurlThreshold &&
            HandPoseUtils.RingFingerCurl(hand) > pointCurlThreshold &&
            HandPoseUtils.PinkyFingerCurl(hand) > pointCurlThreshold)
        {
            return true;
        }
        return false;
    }

    private bool isFacingTowardsCentre(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out MixedRealityPose indexKnucklePose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out MixedRealityPose ringKnucklePose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, hand, out MixedRealityPose wristPose))
        {
            var handNormal = Vector3.Cross(indexKnucklePose.Position - wristPose.Position,
                                                  ringKnucklePose.Position - wristPose.Position).normalized;

            return Vector3.Angle(CameraCache.Main.transform.right, handNormal) < 30;
        }
        return false;
    }
}

