using UnityEngine;

public class Team : MonoBehaviour
{
    public enum TeamType
    {
        TeamA,
        TeamB,
    }

    public TeamType GetTeamType { get { return teamType; } }

    [SerializeField]
    private TeamType teamType;
}
