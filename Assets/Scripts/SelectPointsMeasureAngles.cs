﻿using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class SelectPointsMeasureAngles : MonoBehaviour, IMixedRealityPointerHandler
{
    public InputSourceType sourceType = InputSourceType.Hand;
    private Vector3[] points = new Vector3[3];
    private int currentIndex = 0;
    private LineRenderer lineRenderer;
    public TextMeshPro text;

    void OnEnable()
    {
        // CoreServices.InputSystem.Register(gameObject);
        CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(this);
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (!lineRenderer)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.01f;
        lineRenderer.positionCount = 0;
    }

    private void OnDisable()
    {
        if (CoreServices.InputSystem != null)
        {
            CoreServices.InputSystem.UnregisterHandler<IMixedRealityPointerHandler>(this);
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
        if (eventData.InputSource.SourceType == sourceType)
        {
            var result = eventData.Pointer.Result;
            if (result != null && currentIndex < 3)
            {
                points[currentIndex] = result.Details.Point;
                // Debug.Log(points[currentIndex]);

                if (currentIndex == 0)
                {
                    lineRenderer.positionCount = 0;
                    text.SetText("");
                }
                else if (currentIndex == 1)
                {
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
                    text.SetText(Math.Round(angle, 2) + " degrees");
                    text.transform.position = points[1];
                    // Debug.Log(angle);
                    currentIndex = -1;
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