using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] int playerID;

    public int GetID()
    {
        return playerID;
    }
}
