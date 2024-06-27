using UnityEngine;

public class ParticleScriptTest : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("PlaySound");
    }
}