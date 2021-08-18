using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class AnnotationMaker : MonoBehaviour
{
    public GameObject approveMarker;
    public GameObject rejectMarker;
    public void approveHighlightedObject()
    {
        if (CoreServices.InputSystem.GazeProvider.GazeTarget)
        {
            GameObject target = CoreServices.InputSystem.GazeProvider.GazeTarget;
            Transform targetParent = target.transform.parent;
            if (targetParent.name.Contains("Questionable") && targetParent.transform.Find("ApprovalMarker(Clone)") == null)
            {
                if (targetParent.transform.Find("RejectionMarker(Clone)") != null)
                {
                    Destroy(targetParent.transform.Find("RejectionMarker(Clone)").gameObject);
                }
                GameObject clone;
                Vector3 offset = new Vector3(0, 0.06f, 0);
                clone = Instantiate(approveMarker, target.transform.position + offset, target.transform.rotation);
                clone.transform.parent = targetParent;

            }

        }
    }
    public void rejectHighlightedObject()
    {
        if (CoreServices.InputSystem.GazeProvider.GazeTarget)
        {
            GameObject target = CoreServices.InputSystem.GazeProvider.GazeTarget;
            Transform targetParent = target.transform.parent;
            if (targetParent.name.Contains("Questionable") && targetParent.transform.Find("RejectionMarker(Clone)") == null)
            {
                if (targetParent.transform.Find("ApprovalMarker(Clone)") != null)
                {
                    Destroy(targetParent.transform.Find("ApprovalMarker(Clone)").gameObject);
                }
                GameObject clone;
                Vector3 offset = new Vector3(0, 0.06f, 0);
                clone = Instantiate(rejectMarker, target.transform.position + offset, target.transform.rotation);
                clone.transform.parent = targetParent;

            }

        }
    }
}
