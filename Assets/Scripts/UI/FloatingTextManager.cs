using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    private void Update()
    {
        if (transform.localScale.x <= 0.1f) Destroy(gameObject);
    }
}
