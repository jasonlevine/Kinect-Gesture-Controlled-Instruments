using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.components;

public class getForegroundTex : MonoBehaviour
{
    public Texture foregroundTexture;
    public BackgroundRemovalManager backgroundRemovalManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (foregroundTexture == null || backgroundRemovalManager == null) return;
        if (backgroundRemovalManager.GetForegroundTex() == null) return;

        foregroundTexture = backgroundRemovalManager.GetColorTex();
        //foregroundTexture = backgroundRemovalManager.GetForegroundTex();
        Debug.Log("assigning foreground tex");
    }
}
