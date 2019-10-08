using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class DrumViz : MonoBehaviour
{
    public Sampler[] samplers;
    //public float  waveScale = 1.0f;
    public GestureControl controller;
    //public LineRenderer line;
    private int selected = -1;
    private float[] intensities;
    private float[] rates;
    public float boxSize = 0.2f;
    public Color boxColor;

    public Material boxMaterial;
    public GameObject[] samplerBoxes;
    // Start is called before the first frame update
    void Start()
    {
        // line = GetComponent<LineRenderer>();
        intensities = new float[11];
        rates = new float[11];
        samplerBoxes = new GameObject[11];

        for (int i = 0; i < 11; i++)
        {
            samplerBoxes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            samplerBoxes[i].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            samplerBoxes[i].transform.position = new Vector3(0, i * 0.3f - 2.0f, 3);
            samplerBoxes[i].GetComponent<Renderer>().material = boxMaterial;
            samplerBoxes[i].transform.SetParent(gameObject.transform, false);
            intensities[i] = 1.0f;
            rates[i] = 0.99f;
        }
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 11; i++)
        {
            if (intensities[i] < 0.5f) continue;

            intensities[i] *= rates[i];
            float newSize = boxSize * intensities[i];
            samplerBoxes[i].transform.localScale = new Vector3(newSize, newSize, newSize);
            samplerBoxes[i].GetComponent<Renderer>().material.SetColor("_BaseColor", boxColor * intensities[i]);
        }
    }

    public void NoteOn(Note note)
    {
        int index = note.note % 11;

        intensities[index] = 1.0f;
        rates[index] = 0.99f;
    }

    public void NoteOff(Note note)
    {
        int index = note.note % 11;

        //intensities[index] = 1.0f;
        rates[index] = 0.9f;
    }

    /*
    // Update is called once per frame
    void Update()
    {
        if (controller.selectedKit == -1) return;


        // var notes = samplers[controller.selectedKit].GetActiveNotes();
        
        
        for (int i = 0; i < 11; i++)
        {
            int note = i + 60;
            if (samplers[controller.selectedKit].IsNoteOn(note))
            {
                //  Debug.Log("note is on! " + note + " at " + Time.realtimeSinceStartup);
                //samplerBoxes[i].GetComponent<Renderer>().material.SetFloat("_EmissiveIntensity", 2.0f);
                samplerBoxes[i].GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(0.7f, 0.05f, 0.7f) * 1.5f);
                samplerBoxes[i].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }
            else
            {
                //samplerBoxes[i].GetComponent<Renderer>().material.SetFloat("_EmissiveIntensity", 0.0f);
                samplerBoxes[i].GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(0.7f, 0.05f, 0.7f));
                samplerBoxes[i].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }

        }

        if (samplers[controller.selectedKit].IsNoteOn(60))
        {
              Debug.Log("note is on! " + 60 + " at " + Time.realtimeSinceStartup);
        }
        else
        {
              Debug.Log("note is off! " + 60 + " at " + Time.realtimeSinceStartup);
        }
        

        //int bufferSize = 512;
        //float[] samples = new float[bufferSize];
        //AudioSource audioSource = samplers[controller.selectedKit].GetNextAudioSource();
        //audioSource.GetOutputData(samples, 0);
       // Debug.Log(samples[0]);

      /*  Vector3[] positions = new Vector3[bufferSize];

        for (int i = 0; i < bufferSize; i++)
        {
            positions[i].x = samples[i] * waveScale;
            positions[i].y = i;
            positions[i].z = 0;
        }

        line.SetPositions(positions);
        
    }*/
}
