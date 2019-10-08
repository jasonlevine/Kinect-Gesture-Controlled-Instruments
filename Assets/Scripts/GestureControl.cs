using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.components;
using AudioHelm;

public class GestureControl : MonoBehaviour
{
    public GameObject[] kits;
    public GameObject melody;
    public SequenceGenerator seqGen;
    public HelmController helm;
    public GameObject light;
    public GameObject melodyViz;
    public int selectedKit = 0;
    private KinectGestureListener gestureListener;
    public int mode = 0;

    // Start is called before the first frame update
    void Start()
    {
        // get the gestures listener
        gestureListener = KinectGestureListener.Instance;
        updateMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gestureListener)
            return;

        if (gestureListener.IsJump())
        {
            mode++;
            mode %= 3;
            updateMode();
            Debug.Log("Jump");
        }

        if (mode > 0)
        {
            if (gestureListener.IsSwipeRight())
            {
                selectedKit++;
                selectedKit %= kits.Length;
                selectKit();
                Debug.Log("Swipe left");

            }
            else if (gestureListener.IsSwipeLeft())
            {
                selectedKit--;
                if (selectedKit < 0) selectedKit += kits.Length;
                selectKit();
                Debug.Log("Swipe right");
            }

            if (mode > 1) {
                if (gestureListener.IsSwipeUp())
                {
                    seqGen.Generate();
                    Debug.Log("Swipe up");
                }
                /*
                if (gestureListener.IsZoomingIn() || gestureListener.IsZoomingOut())
                {
                    float resonance = gestureListener.GetZoomFactor() / 4.0f;
                    float distortionOn = gestureListener.GetZoomFactor() / 8.0f + 0.1f;
                    helm.SetParameterPercent(CommonParam.kResonance, resonance);
                    helm.SetParameterPercent(Param.kDistortionOn, distortionOn);
                   // Debug.Log("Zoom by " + gestureListener.GetZoomFactor());
                }
                */
                if (gestureListener.IsTurningWheel())
                {
                    melodyViz.transform.eulerAngles = new Vector3(0, 0, gestureListener.GetWheelAngle());
                    float cutoff = Mathf.InverseLerp(90, -90, gestureListener.GetWheelAngle());
                    helm.SetParameterPercent(CommonParam.kFilterCutoff, cutoff);
                  //  helm.SetParameterPercent(Param.kDelayOn, cutoff * 0.5f);
                   // Debug.Log("Turning " + gestureListener.GetWheelAngle() + " cutoff = " + cutoff);
                }
            }
        }
      
       
    }

    void selectKit()
    {
        for (int i = 0; i < kits.Length; i++)
        {
            if (i == selectedKit)
            {
                kits[i].SetActive(true);
            }
            else
            {
                kits[i].SetActive(false);
            }
        }
    }

    void updateMode()
    {
        if (mode == 0)
        {
            melody.SetActive(false);
            for (int i = 0; i < kits.Length; i++)
            {
                kits[i].SetActive(false);
            }
            light.SetActive(false);
        }
        else if (mode == 1)
        {
            selectKit();
            melody.SetActive(false);
            light.SetActive(true);
        }
        else if (mode == 2)
        {
            selectKit();
            melody.SetActive(true);
            light.SetActive(true);
        }
    }

}
