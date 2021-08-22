﻿using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class SelectPointsDistance : MonoBehaviour, IMixedRealityPointerHandler
{

    public InputSourceType sourceType = InputSourceType.Hand;
    private Vector3[] points = new Vector3[2];
    private int currentIndex = 0;
    private LineRenderer lineRenderer;
    public TextMeshPro text;
    // Start is called before the first frame update
    void OnEnable()
    {
        // CoreServices.InputSystem.Register(gameObject);
        CoreServices.InputSystem.RegisterHandler<IMixedRealityPointerHandler>(this);
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.01f;
    }

    private void OnDisable()
    {
        if (CoreServices.InputSystem != null)
        {
            CoreServices.InputSystem.UnregisterHandler<IMixedRealityPointerHandler>(this);
        }
        points = new Vector3[2];
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
            if (result != null && currentIndex < 2)
            {
                points[currentIndex] = result.Details.Point;

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

                    float distance = Vector3.Distance(points[0], points[1]);
                    text.SetText(Math.Round(distance, 2) + " meters");

                    Vector3 textPosition = points[0] + (points[1] - points[0]) / 2;
                    text.transform.position = textPosition;

                    currentIndex = -1;
                }
                currentIndex++;
            }
        }
    }
    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
    }
}
