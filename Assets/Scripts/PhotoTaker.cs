using System.Collections;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using TMPro;
// modified from https://docs.unity3d.com/2019.4/Documentation/ScriptReference/Windows.WebCam.PhotoCapture.html
public class PhotoTaker : MonoBehaviour
{
    PhotoCapture photoCaptureObject = null;
    Texture2D targetTexture = null;
    public float secondsToWait = 3;
    public TextMeshPro timerText;
    public GameObject quad;
    private float timeRemaining;
    private bool timerIsRunning = false;
    private float showPhototimeRemaining = 2;
    private bool showPhotoTimerIsRunning = false;
    void Start()
    {
        timerIsRunning = false;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.transform.gameObject.SetActive(true);
                timerText.SetText(Math.Ceiling(timeRemaining) + "");
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;

                timerText.SetText("");
                timerText.transform.gameObject.SetActive(false);
                TakePhoto();
            }
        }

        if (showPhotoTimerIsRunning)
        {
            if (showPhototimeRemaining > 0)
            {
                showPhototimeRemaining -= Time.deltaTime;
            }
            else
            {
                showPhototimeRemaining = 0;
                showPhotoTimerIsRunning = false;
                quad.SetActive(false);
            }
        }

    }

    public void StartPhotoTimer()
    {
        timeRemaining = secondsToWait;
        timerIsRunning = true;
    }

    // Use this for initialization
    private void TakePhoto()
    {
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        // Create a PhotoCapture object
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result)
            {
                // Take a picture
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        // Copy the raw image data into our target texture
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);

        // apply our texture to object
        quad.SetActive(true);
        Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        quadRenderer.material = new Material(Shader.Find("Unlit/Texture"));
        quadRenderer.material.SetTexture("_MainTex", targetTexture);
        showPhototimeRemaining = 5;
        showPhotoTimerIsRunning = true;

        // Deactivate our camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown our photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}