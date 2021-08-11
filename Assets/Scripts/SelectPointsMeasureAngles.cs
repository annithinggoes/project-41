using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectPointsMeasureAngles : MonoBehaviour, IMixedRealityPointerHandler
{
    public InputSourceType sourceType = InputSourceType.Hand;
    private Vector3[] points = new Vector3[3];
    private int currentIndex = 0;
    private LineRenderer lineRenderer;
    public TextMeshPro text;
    // Start is called before the first frame update
    void OnEnable()
    {
        CoreServices.InputSystem.Register(gameObject);

    }

    private void OnDisable()
    {
        if (CoreServices.InputSystem != null)
        {
            CoreServices.InputSystem.Unregister(gameObject);
        }
        points = new Vector3[3];
        currentIndex = 0;
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        // if (spawnObject != null && eventData.InputSource.SourceType == sourceType)
        if (eventData.InputSource.SourceType == sourceType)
        {
            // var spawn = Instantiate(spawnObject);
            var result = eventData.Pointer.Result;
            // if (result != null)
            // {
            //     spawn.transform.position = result.Details.Point;
            // }
            // my code
            if (result != null && currentIndex < 3)
            {
                points[currentIndex] = result.Details.Point;
                Debug.Log(points[currentIndex]);

                if (currentIndex == 1)
                {
                    lineRenderer = gameObject.AddComponent<LineRenderer>();
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.widthMultiplier = 0.02f;
                    lineRenderer.positionCount = 2;
                    Vector3[] linePoints = { points[0], points[1] };
                    lineRenderer.SetPositions(linePoints);
                }
                else if (currentIndex == 2)
                {
                    lineRenderer.positionCount = 3;
                    Vector3[] linePoints = { points[0], points[1], points[2] };
                    lineRenderer.SetPositions(linePoints);

                    float angle = calculateAngle(points[0], points[1], points[2]);
                    text.SetText("the angle is " + angle);
                    text.transform.position = points[1];
                    Debug.Log(angle);
                }

                currentIndex++;
            }
        }

    }
    private float calculateAngle(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Vector3 vector1 = pointA - pointB;
        Vector3 vector2 = pointC - pointB;
        float angle = Vector3.Angle(vector1, vector2);
        return angle;
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
    }
}