using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light m_myLight;
    public float MinIntensity = 0f;
    public float MaxIntensity = 1f;

    [Range(1, 50)]
    public int m_smoothing = 5;

    private Queue<float> m_smoothQueue;
    private float m_lastSum = 0;

    private void Start()
    {
        m_smoothQueue = new Queue<float>(m_smoothing);
        m_myLight = GetComponent<Light>();
    }

    private void Update()
    {
        if (m_myLight == null)
            return;

        while (m_smoothQueue.Count >= m_smoothing)
        {
            m_lastSum -= m_smoothQueue.Dequeue();
        }

        float newVal = Random.Range(MinIntensity, MaxIntensity);
        m_smoothQueue.Enqueue(newVal);
        m_lastSum += newVal;

        m_myLight.intensity = m_lastSum / (float)m_smoothQueue.Count;
    }
}