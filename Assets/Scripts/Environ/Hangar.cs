using System.Collections;
using UnityEngine;

public class Hangar : MonoBehaviour
{
    private bool hasShip { get => ship != null; }

    private GameObject ship;

    [SerializeField]
    Transform dockingPoint;

    [SerializeField]
    Transform exitPoint;

    Vector3 origPos;
    Quaternion origRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnShipEnter(GameObject ship)
    {
        if (hasShip) return;

        this.ship = ship;

        StartCoroutine(StartRepairSequence());
    }

    IEnumerator StartRepairSequence()
    {
        yield return new WaitForFixedUpdate();

        origPos = ship.transform.position;
        origRot = ship.transform.rotation;

        ship.GetComponent<Rigidbody>().isKinematic = true;


        float timer = 0;

        while (timer < 1)
        {
            ship.transform.position = Vector3.Lerp(origPos, dockingPoint.position, timer);
            ship.transform.rotation = Quaternion.Slerp(origRot, dockingPoint.rotation, timer);

            timer += Time.fixedDeltaTime * (1f / 1f);
            yield return new WaitForFixedUpdate();
        }

        ComponentBase[] allComps = ship.GetComponentsInChildren<ComponentBase>();

        foreach (ComponentBase comp in allComps)
        {
            comp.ResetComponent();
        }

        WeaponBase[] weaponBases = ship.GetComponentsInChildren<WeaponBase>();

        foreach (WeaponBase weapon in weaponBases)
        {
            weapon.Rearm();
        }

        yield return new WaitForSeconds(1f);
        yield return new WaitForFixedUpdate();

        timer = 0;

        while (timer < 1)
        {
            ship.transform.position = Vector3.Lerp(dockingPoint.position, exitPoint.position, timer);
            ship.transform.rotation = Quaternion.Slerp(dockingPoint.rotation, exitPoint.rotation, timer);

            timer += Time.fixedDeltaTime * (1f / 1f);
            yield return new WaitForFixedUpdate();
        }

        ship.GetComponent<Rigidbody>().isKinematic = false;

        ship = null;

        print("SHIP FIXED AND REARMED");
    }
}
