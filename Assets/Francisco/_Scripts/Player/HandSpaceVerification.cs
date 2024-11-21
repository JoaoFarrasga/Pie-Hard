using UnityEngine;

public class HandSpaceVerification : MonoBehaviour
{
    [SerializeField] private bool isEmpty;

    private void Start()
    {
        isEmpty = true;
    }

    public bool VerifySpaceState() { return isEmpty; } //returns variable
    public void ChangeSpaceState() { isEmpty = !isEmpty; }//alternates between empty and not empty as the game proceeds
}
