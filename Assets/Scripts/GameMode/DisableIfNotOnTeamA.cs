using UnityEngine;

public class DisableIfNotOnTeamA : MonoBehaviour
{
    [SerializeField]
    Team team;

    [SerializeField]
    Team.TeamType teamType;

    [SerializeField]
    GameObject[] gameObjectToDisable;

    void Start()
    {
        if (team.GetTeamType != teamType)
        {
            foreach (var go in gameObjectToDisable)
            {
                go.SetActive(false);
            }
        }
    }
}
