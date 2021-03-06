using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class GestureRecogniser : MonoBehaviour
{
    private bool isDoneExecuting = true;

    /// <summary>
    /// Threshold for defining a curled finger
    /// </summary>
    public float angleCurlThreshold = 0.55f;

    /// <summary>
    /// Threshold for defining a straight finger
    /// </summary>
    public float angleStraightThreshold = 0.25f;
    public float thumbsUpCurlThreshold = 0.65f;
    public float thumbsUpStraightThreshold = 0.25f;
    public float pointCurlThreshold = 0.55f;
    public float pointStraightThreshold = 0.1f;
    public float facingCameraTrackingThreshold = 60.0f;
    public float facingAwayFromCameraTrackingThreshold = 120.0f;
    public float flatHandThreshold = 45.0f;

    public GameObject menu;
    public GameObject scripts;
    public GameObject buildingPlansGUI;
    public TextMeshPro textMeshProHit;

    protected Handedness rightHand = Handedness.Right;
    protected Handedness leftHand = Handedness.Left;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Check if hand in view
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Both, out MixedRealityPose pose))
        {
            if (isDoneExecuting)
            {
                isDoneExecuting = false;
                gestureRecogniser();
                isDoneExecuting = true;
            }
        }
    }
    protected virtual void gestureRecogniser()
    {
        if (checkPhoto())
        {
            Debug.Log("HIT");
            PhotoTaker photoTaker = scripts.GetComponent<PhotoTaker>();
            photoTaker.StartPhotoTimer();
            textMeshProHit.SetText("PHOTO");
        }
        else if (checkPlans())
        {
            toggleMenu(false);
            buildingPlansGUI.SetActive(true);
            SetPositionFrontOfPerson positionSetter = buildingPlansGUI.GetComponent<SetPositionFrontOfPerson>();
            positionSetter.SetPosition();
            textMeshProHit.SetText("PLANS");
        }
        else if (checkThumbs("Up"))
        {
            AnnotationMaker annotationMaker = scripts.GetComponent<AnnotationMaker>();
            annotationMaker.approveHighlightedObject();
            textMeshProHit.SetText("Thumbs UP");
        }
        else if (checkThumbs("Down"))
        {
            AnnotationMaker annotationMaker = scripts.GetComponent<AnnotationMaker>();
            annotationMaker.rejectHighlightedObject();
            textMeshProHit.SetText("Thumbs DOWN");
        }
        else if (checkDistance())
        {
            ChangeScene changeScene = scripts.GetComponent<ChangeScene>();
            changeScene.changeScene("Distance");
            textMeshProHit.SetText("DISTANCE");
        }
        else if (checkAngle(rightHand) || checkAngle(leftHand))
        {
            ChangeScene changeScene = scripts.GetComponent<ChangeScene>();
            changeScene.changeScene("Angle");
            textMeshProHit.SetText("ANGLE");
        }
        else
        {
            textMeshProHit.SetText("NONE");
        }
    }
    protected bool checkAngle(Handedness hand)
    {
        if (checkL(hand) && checkPalmFacingConstraint(hand, false, false))
        {
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out MixedRealityPose indexTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out MixedRealityPose indexKnucklePose))
            {
                Vector3 indexDirection = indexTipPose.Position - indexKnucklePose.Position;
                float indexCameraAngle = Vector3.Angle(indexDirection, CameraCache.Main.transform.up);

                return indexCameraAngle < 60;

            }
        }
        return false;

    }
    protected bool checkL(Handedness hand)
    {
        if (HandPoseUtils.ThumbFingerCurl(hand) <= angleStraightThreshold &&
            HandPoseUtils.IndexFingerCurl(hand) <= angleStraightThreshold &&
            HandPoseUtils.MiddleFingerCurl(hand) > angleCurlThreshold &&
            HandPoseUtils.RingFingerCurl(hand) > angleCurlThreshold &&
            HandPoseUtils.PinkyFingerCurl(hand) > angleCurlThreshold)
        {
            return true;
        }
        return false;
    }
    protected bool checkThumbs(string direction)
    {
        if (HandPoseUtils.ThumbFingerCurl(rightHand) <= thumbsUpStraightThreshold &&
            HandPoseUtils.IndexFingerCurl(rightHand) > thumbsUpCurlThreshold &&
            HandPoseUtils.MiddleFingerCurl(rightHand) > thumbsUpCurlThreshold &&
            HandPoseUtils.RingFingerCurl(rightHand) > thumbsUpCurlThreshold &&
            HandPoseUtils.PinkyFingerCurl(rightHand) > thumbsUpCurlThreshold)
        {
            if (direction == "Up")
            {
                return isThumbUp(rightHand);
            }
            else if (direction == "Down")
            {
                return isThumbDown(rightHand);
            }
        }

        return false;
    }
    // 0 - 60, thumbdpwn is 120- 180
    private bool isThumbUp(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out MixedRealityPose thumbTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, hand, out MixedRealityPose thumbProximalPose))
        {
            Vector3 thumbDirection = thumbTipPose.Position - thumbProximalPose.Position;
            float thumbCameraAngle = Vector3.Angle(thumbDirection, CameraCache.Main.transform.up);

            return thumbCameraAngle < 60;

        }
        return false;
    }
    private bool isThumbDown(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out MixedRealityPose thumbTipPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, hand, out MixedRealityPose thumbProximalPose))
        {
            Vector3 thumbDirection = thumbTipPose.Position - thumbProximalPose.Position;
            float thumbCameraAngle = Vector3.Angle(thumbDirection, CameraCache.Main.transform.up);

            return thumbCameraAngle > 120;

        }
        return false;
    }
    protected bool checkDistance()
    {
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

    protected bool checkPhoto()
    {
        if (checkL(rightHand) && checkL(leftHand))
        {
            if (isThumbUp(leftHand) && isThumbDown(rightHand))
            {
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, leftHand, out MixedRealityPose leftPalmPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, rightHand, out MixedRealityPose rightPalmPose))
                {
                    return checkPalmFacingConstraint(leftHand, true, false) && checkPalmFacingConstraint(rightHand, false, false);
                }
            }
        }
        return false;
    }
    protected bool checkPlans()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, leftHand, out MixedRealityPose leftPalmPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, rightHand, out MixedRealityPose rightPalmPose))
        {
            if (Vector3.Distance(leftPalmPose.Position, rightPalmPose.Position) > 0.15)
            {
                return false;
            }

            return checkPalmFacingConstraint(rightHand, true, true) && checkPalmFacingConstraint(leftHand, true, true);
        }
        return false;
    }

    private bool checkPalmFacingConstraint(Handedness hand, bool inwards, bool flatHand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out MixedRealityPose palmPose))
        {
            float palmCameraAngle = Vector3.Angle(palmPose.Up, CameraCache.Main.transform.forward);

            if (flatHand)
            {
                // Check if the triangle's normal formed from the palm, to index, to ring finger tip roughly matches the palm normal.
                MixedRealityPose indexTipPose, ringTipPose;

                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out indexTipPose) &&
                    HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, hand, out ringTipPose))
                {
                    var handNormal = Vector3.Cross(indexTipPose.Position - palmPose.Position,
                                                   ringTipPose.Position - indexTipPose.Position).normalized;
                    handNormal *= (hand == Handedness.Right) ? 1.0f : -1.0f;

                    if (Vector3.Angle(palmPose.Up, handNormal) > flatHandThreshold)
                    {
                        return false;
                    }
                }
            }
            // Check if the palm angle meets the prescribed threshold for the direction
            if (inwards)
            {
                return palmCameraAngle < facingCameraTrackingThreshold;
            }
            else
            {
                return palmCameraAngle > facingAwayFromCameraTrackingThreshold;
            }
        }
        return false;
    }

    public void callMenu()
    {
        if (!checkPlans())
        {
            toggleMenu(true);
        } else {
            toggleMenu(false);
        }
    }

    private void toggleMenu(bool active)
    {
        GameObject btnClose = menu.transform.Find("ButtonClose").gameObject;
        btnClose.SetActive(active);
        GameObject backplate = menu.transform.Find("Backplate").gameObject;
        backplate.SetActive(active);
        GameObject btnCollection = menu.transform.Find("ButtonCollection").gameObject;
        btnCollection.SetActive(active);
        GameObject gravVisualCue = menu.transform.Find("GravVisualCue").gameObject;
        gravVisualCue.SetActive(active);
    }
}
