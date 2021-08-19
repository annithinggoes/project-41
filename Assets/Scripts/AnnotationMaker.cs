using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class AnnotationMaker : MonoBehaviour
{
    public GameObject approveMarker;
    public GameObject rejectMarker;
    public void approveHighlightedObject()
    {
        annotateAndShowGUI(approveMarker, rejectMarker);
    }
    public void rejectHighlightedObject()
    {
        annotateAndShowGUI(rejectMarker, approveMarker);
    }

    private void annotateAndShowGUI(GameObject addedMarker, GameObject removedMarker)
    {
        string addedMarkerName = addedMarker.name + "(Clone)";
        string removedMarkerName = removedMarker.name + "(Clone)";
        if (CoreServices.InputSystem.GazeProvider.GazeTarget)
        {
            GameObject target = CoreServices.InputSystem.GazeProvider.GazeTarget;
            Transform questionableObject;
            if (!target.name.Contains("Questionable"))
            {
                questionableObject = target.transform.parent;
            }
            else
            {
                questionableObject = target.transform;
            }
            Debug.Log(target);
            if (questionableObject.name.Contains("Questionable") && questionableObject.transform.Find(addedMarkerName) == null)
            {
                if (questionableObject.transform.Find(removedMarkerName) != null)
                {
                    Destroy(questionableObject.transform.Find(removedMarkerName).gameObject);
                }
                GameObject clone;
                Vector3 offset = new Vector3(0, 0.08f, 0);
                clone = Instantiate(addedMarker, target.transform.position + offset, target.transform.rotation);
                clone.transform.parent = questionableObject;
                questionableObject.transform.Find("CommentGUI").gameObject.SetActive(true);
            }
        }
    }
}
