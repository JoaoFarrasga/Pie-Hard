using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 1f;
    MeshRenderer mat;

    void Start()
    {
        mat = GetComponent<MeshRenderer>();
        StartCoroutine(FadeOut());
        //Destroy(gameObject, lifetime);
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(lifetime);
        while(mat.material.color.a > 0)
        { 
            //Decreases the alpha until transparency
            mat.material.color = new Color(mat.material.color.r, mat.material.color.g, mat.material.color.b, mat.material.color.a - Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }

    //Just a Test
        private IEnumerator FadeOutTest()
    {
        yield return new WaitForSeconds(lifetime);

        Color color = mat.material.color;
        float startAlpha = color.a;
;       float progress = 0.0f;

        while (progress < 1.0f)
        {
            color.a = Mathf.Lerp(startAlpha, 0, progress);
            mat.material.color = color;
            progress += Time.deltaTime;

            Debug.Log("Fading");

            yield return null; // Wait for the next frame
        }

        color.a = 0;
        mat.material.color = color;
    }
}