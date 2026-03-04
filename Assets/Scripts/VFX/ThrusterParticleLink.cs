using UnityEngine;

public class ThrusterParticleLink : MonoBehaviour
{
    [SerializeField]
    Thruster thruster;

    [SerializeField]
    ParticleSystem particle;

    void Update()
    {
        if (thruster.GetThrusterForce() > 0)
        {
            if (!particle.isPlaying) particle.Play();
        }
        else
        {
            if (particle.isPlaying) particle.Stop();
        }
    }
}
