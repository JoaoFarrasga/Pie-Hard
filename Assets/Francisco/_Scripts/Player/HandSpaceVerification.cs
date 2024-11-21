using UnityEngine;

public class HandSpaceVerification : MonoBehaviour
{
    [SerializeField] private bool isEmpty;

    private void Start()
    {
        isEmpty = true;
    }

    public bool VerifySpaceState() { return isEmpty; }
    public void ChangeSpaceState() { isEmpty = !isEmpty; }
}
