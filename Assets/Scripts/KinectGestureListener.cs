using UnityEngine;
using System.Collections;
using System;
using com.rfilkov.kinect;


public class KinectGestureListener : MonoBehaviour, GestureListenerInterface
{
    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;

    [Tooltip("UI-Text to display gesture-listener messages and gesture information.")]
    public UnityEngine.UI.Text gestureInfo;

    // singleton instance of the class
    private static KinectGestureListener instance = null;

    // whether the needed gesture has been detected or not
    private bool swipeLeft = false;
    private bool swipeRight = false;
    private bool swipeUp = false;
    private bool jump = false;
    private bool psi = false;
    private bool zoomIn = false;
    private bool zoomOut = false;
    private float zoomFactor = 1f;

    private bool wheel = false;
    private float wheelAngle = 0f;


    public static KinectGestureListener Instance
    {
        get
        {
            return instance;
        }
    }

    public bool IsSwipeLeft()
    {
        if (swipeLeft)
        {
            swipeLeft = false;
            return true;
        }

        return false;
    }

    public bool IsSwipeRight()
    {
        if (swipeRight)
        {
            swipeRight = false;
            return true;
        }

        return false;
    }

    public bool IsSwipeUp()
    {
        if (swipeUp)
        {
            swipeUp = false;
            return true;
        }

        return false;
    }

    public bool IsJump()
    {
        if (jump)
        {
            jump = false;
            return true;
        }

        return false;
    }

    public bool IsPsi()
    {
        if (psi)
        {
            psi = false;
            return true;
        }

        return false;
    }

    public bool IsZoomingOut()
    {
        return zoomOut;
    }

    public bool IsZoomingIn()
    {
        return zoomIn;
    }

    public float GetZoomFactor()
    {
        return zoomFactor;
    }

    public bool IsTurningWheel()
    {
        return wheel;
    }

    public float GetWheelAngle()
    {
        return wheelAngle;
    }

    public void UserDetected(ulong userId, int userIndex)
    {
        // the gestures are allowed for the selected user only
        KinectGestureManager gestureManager = KinectManager.Instance.gestureManager;
        if (!gestureManager || (userIndex != playerIndex))
            return;

        // set the gestures to detect
        gestureManager.DetectGesture(userId, GestureType.SwipeLeft);
        gestureManager.DetectGesture(userId, GestureType.SwipeRight);
        gestureManager.DetectGesture(userId, GestureType.SwipeUp);
        gestureManager.DetectGesture(userId, GestureType.Psi);
        gestureManager.DetectGesture(userId, GestureType.ZoomIn);
        gestureManager.DetectGesture(userId, GestureType.ZoomOut);
        gestureManager.DetectGesture(userId, GestureType.Wheel);
        gestureManager.DetectGesture(userId, GestureType.Jump);

        if (gestureInfo != null)
        {
            // gestureInfo.text = "Swipe left, right or up to change the slides.";
        }
    }

    public void UserLost(ulong userId, int userIndex)
    {
        // the gestures are allowed for the selected user only
        if (userIndex != playerIndex)
            return;

        if (gestureInfo != null)
        {
            gestureInfo.text = string.Empty;
        }
    }

    public void GestureInProgress(ulong userId, int userIndex, GestureType gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        // the gestures are allowed for the primary user only
        if (userIndex != playerIndex)
            return;

        if (gesture == GestureType.ZoomOut)
        {
            if (progress > 0.5f)
            {
                zoomOut = true;
                zoomFactor = screenPos.z;

                if (gestureInfo != null)
                {
                    string sGestureText = string.Format("{0} factor: {1:F0}%", gesture, screenPos.z * 100f);
                    gestureInfo.text = sGestureText;
                }
            }
            else
            {
                zoomOut = false;
            }
        }
        else if (gesture == GestureType.ZoomIn)
        {
            if (progress > 0.5f)
            {
                zoomIn = true;
                zoomFactor = screenPos.z;

                if (gestureInfo != null)
                {
                    string sGestureText = string.Format("{0} factor: {1:F0}%", gesture, screenPos.z * 100f);
                    gestureInfo.text = sGestureText;
                }
            }
            else
            {
                zoomIn = false;
            }
        }
        else if (gesture == GestureType.Wheel)
        {
            if (progress > 0.5f)
            {
                wheel = true;
                wheelAngle = screenPos.z;

                if (gestureInfo != null)
                {
                    string sGestureText = string.Format("Wheel angle: {0:F0} deg.", screenPos.z);
                    gestureInfo.text = sGestureText;
                }
            }
            else
            {
                wheel = false;
            }
        }
    }

    public bool GestureCompleted(ulong userId, int userIndex, GestureType gesture, KinectInterop.JointType joint, Vector3 screenPos)
    {
        // the gestures are allowed for the selected user only
        if (userIndex != playerIndex)
            return false;

        if (gestureInfo != null)
        {
            string sGestureText = gesture + " detected";
            gestureInfo.text = sGestureText;
        }

        if (gesture == GestureType.SwipeLeft)
            swipeLeft = true;
        else if (gesture == GestureType.SwipeRight)
            swipeRight = true;
        else if (gesture == GestureType.SwipeUp)
            swipeUp = true;
        else if (gesture == GestureType.Jump)
            jump = true;
        else if (gesture == GestureType.Psi)
            psi = true;
        else if (gesture == GestureType.ZoomOut)
        {
            zoomOut = false;
        }
        else if (gesture == GestureType.ZoomIn)
        {
            zoomIn = false;
        }
        else if (gesture == GestureType.Wheel)
        {
            wheel = false;
        }

        return true;
    }


    public bool GestureCancelled(ulong userId, int userIndex, GestureType gesture, KinectInterop.JointType joint)
    {
        return true;
    }


    void Awake()
    {
        instance = this;
    }

} 
