using UnityEngine;

public class AutoSelfDestuct : MonoBehaviour
{
    [SerializeField]
    SelfDestructScreen selfDestructScreen;

    [SerializeField]
    float selfDestructTime = 10;
    float currentTimer = 10;

    bool isSelfDestructing = false;
    SelfDestructType reason = SelfDestructType.Damage;

    public enum SelfDestructType
    {
        Damage,
        Bounds,
    }

    void Start()
    {
        ResetSD();
    }

    void Update()
    {
        if (isSelfDestructing && currentTimer > 0) currentTimer -= Time.deltaTime;
        else if (isSelfDestructing && currentTimer <= 0) SelfDestruct();

        selfDestructScreen.TimeRemainingSecs = currentTimer;
    }

    public void CancelSelfDestruct(SelfDestructType idReason)
    {
        if (reason != idReason) return;

        ResetSD();
    }

    private void ResetSD()
    {
        currentTimer = selfDestructTime;
        isSelfDestructing = false;
        selfDestructScreen.transform.gameObject.SetActive(false);
    }

    public void BeginSelfDestructDamage()
    {
        if (isSelfDestructing) return;

        isSelfDestructing = true;

        reason = SelfDestructType.Damage;

        selfDestructScreen.Reason = "DAMAGE IS CRITICAL!";

        selfDestructScreen.transform.gameObject.SetActive(true);
    }

    public void BeginSelfDestructBounds()
    {
        if (isSelfDestructing) return;

        isSelfDestructing = true;

        reason = SelfDestructType.Bounds;

        selfDestructScreen.Reason = "OUT OF BOUNDS!";

        selfDestructScreen.transform.gameObject.SetActive(true);
    }

    private void SelfDestruct()
    {

        GameManager.Instance.SelfDestruct(Team.TeamType.TeamA);
    }
}
