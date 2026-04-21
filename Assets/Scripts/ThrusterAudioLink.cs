using UnityEngine;

public class ThrusterAudioLink : MonoBehaviour
{
    [SerializeField]
    private Thruster[] thrusters;
    private AudioSource audioSource;

    void Awake()
    {

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        print(GetThrustersAvgMag());

        if (GetThrustersAvgMag() > 0 && !audioSource.isPlaying)
        {
            audioSource.time = UnityEngine.Random.Range(0, audioSource.clip.length);
            audioSource.Play();
            audioSource.volume = Mathf.Lerp(0, 1, GetThrustersAvgMag());
        }
        else if (GetThrustersAvgMag() <= 0 && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private float GetThrustersAvgMag()
    {
        float avg = 0;

        foreach (var thruster in thrusters)
        {
            if (thruster.GetTargetForce() > 0)
                avg += 1;
            else
                avg += 0;
        }

        avg /= thrusters.Length;

        return avg;
    }

}
