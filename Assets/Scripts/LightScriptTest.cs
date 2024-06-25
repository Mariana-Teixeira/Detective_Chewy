using System.Collections.Generic;
using UnityEngine;

public class LightScriptTest : MonoBehaviour
{
    [Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
    private Light myLight;
    [Tooltip("Minimum random light intensity")]
    public float minIntensity = 0f;
    [Tooltip("Maximum random light intensity")]
    public float maxIntensity = 1f;
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    [Range(1, 50)]
    public int smoothing = 5;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueue;
    float lastSum = 0;

    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        if (myLight == null)
            return;

        // pop off an item if too big
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        // Calculate new smoothed average
        myLight.intensity = lastSum / (float)smoothQueue.Count;
    }

}