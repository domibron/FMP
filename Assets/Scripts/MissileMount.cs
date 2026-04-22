using UnityEngine;

public class MissileMountData
{
    public bool hasMissile;
}

public class MissileMount : ComponentBase, IDataReadable, IActivateable
{
    [SerializeField, Header("Missile Mount")]
    GameObject missilePrefab;

    [SerializeField]
    Transform missileMountPoint;

    [SerializeField]
    Antenna antenna;

    Missile missile;

    [SerializeField]
    AudioSource audioSource;

    void Start()
    {
        Rearm();
    }

    public void Rearm()
    {
        if (destroyed) return;

        if (missile != null) return; // Dont need to spawn a new missile if we already have one.

        GameObject missileGO = Instantiate(missilePrefab, missileMountPoint.position, missileMountPoint.rotation);
        missileGO.transform.parent = transform;
        missile = missileGO.GetComponent<Missile>();
        missile.SetUpMissile(antenna);
    }

    public void Activate()
    {
        if (destroyed) return;

        if (missile == null) return;

        audioSource.Play();


        missile.Activate();
        missile = null;
    }

    public string ReadData()
    {
        if (destroyed) return string.Empty;

        return JsonUtility.ToJson(new MissileMountData()
        {
            hasMissile = missile != null,
        });// TODO add data
    }




}
