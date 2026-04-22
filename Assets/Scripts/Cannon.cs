using UnityEngine;

public class CannonData
{
    public int CurrentAmmo;
    public int MaxAmmo;
}

public class Cannon : ComponentBase, IDataReadable
{
    [SerializeField]
    GameObject bulletProjectilePrefab;

    [SerializeField]
    Transform bulletSpawnPoint;

    [SerializeField]
    float fireRate = 10;

    [SerializeField]
    int maxAmmo = 999;

    int currentAmmo;

    float currentFireWaitTime = 0f;

    private bool isFiring = false;

    public const float BULLET_SPEED = 800f;

    [SerializeField]
    Rigidbody shipRB;

    [SerializeField]
    AudioSource audioSource;

    void Start()
    {
        Rearm();
    }

    void Update()
    {
        if (currentFireWaitTime > 0) currentFireWaitTime -= Time.deltaTime;

        if (destroyed)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            return;
        }

        if (isFiring)
        {
            FireBullet();

            if (currentAmmo > 0)
            {
                if (!audioSource.isPlaying) audioSource.Play();
            }
            else
            {
                if (audioSource.isPlaying) audioSource.Stop();
            }

        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }
    }

    public string ReadData()
    {
        if (destroyed) return string.Empty;

        return JsonUtility.ToJson(new CannonData()
        {
            CurrentAmmo = currentAmmo,
            MaxAmmo = maxAmmo,
        });
    }

    public void SetFireState(bool fireState)
    {
        isFiring = fireState;
    }

    public void Rearm()
    {
        currentAmmo = maxAmmo;
    }

    private void FireBullet()
    {
        if (currentAmmo <= 0) return;
        // if (currentAmmo <= 0) Rearm();

        currentAmmo--;

        currentFireWaitTime = 1f / fireRate;

        GameObject bullet = Instantiate(bulletProjectilePrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * BULLET_SPEED + shipRB.linearVelocity, ForceMode.VelocityChange);
    }
}
