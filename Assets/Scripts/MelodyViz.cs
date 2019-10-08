using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class MelodyViz : MonoBehaviour
{
    // Start is called before the first frame update
    public Material mat;
    public GameObject[] spheres;
    private float[] intensities;
    private float[] rates;
    public float sphereSize = 0.4f;
    public Color sphereColor;

    void Start()
    {
        spheres = new GameObject[12];
        intensities = new float[12];
        rates = new float[12];
       // sphereColor = new Color(0.8f, 0.45f, 0.2f);
        float radius = 0.8f;

        for (int i = 0; i < 12; i++)
        {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            float theta = (float)i / 12.0f * Mathf.PI * 2.0f;
            float x = Mathf.Sin(theta) * radius;
            float y = Mathf.Cos(theta) * radius;// + 0.8f;
            spheres[i].transform.localPosition = new Vector3(x, y, 3);
            spheres[i].transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
            spheres[i].transform.SetParent(gameObject.transform, false);
            spheres[i].GetComponent<Renderer>().material = mat;
            intensities[i] = 1.0f;
            rates[i] = 0.99f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 12; i++)
        {
            if (intensities[i] < 0.5f) continue;

            intensities[i] *= rates[i];
            float newSize = sphereSize * intensities[i];
            spheres[i].transform.localScale = new Vector3(newSize, newSize, newSize);
            spheres[i].GetComponent<Renderer>().material.SetColor("_BaseColor", sphereColor * intensities[i]);
        }
    }

    public void NoteOn(Note note)
    {
        int index = note.note % 12;

        intensities[index] = 1.5f;
        rates[index] = 0.99f;
    }

    public void NoteOff(Note note)
    {
        int index = note.note % 12;

        //intensities[index] = 1.0f;
        rates[index] = 0.9f;
    }
}
