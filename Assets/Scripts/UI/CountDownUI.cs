using UnityEngine;
using TMPro;
using System.Collections;

public class CountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDown;
    private float time;
    private float repeatingTime;
    [SerializeField] private Color colorPink, colorRed;
    [SerializeField] AudioSource countDownAudio;


    private void OnEnable()
    {
        countDown = GetComponent<TextMeshProUGUI>();
        countDownAudio = GetComponent<AudioSource>();
        time = countDownAudio.clip.length;
        repeatingTime = 0.01f;
        countDown.text = Mathf.Floor(time).ToString();
        //countDown.color = R
        InvokeRepeating(nameof(IncreaseUIScaleCountDown), 0f, repeatingTime);
    }

    private void Update()
    {
        //StartCoroutine(CountDown());
    }

    private void IncreaseUIScaleCountDown()
    {
        Debug.Log("CountDown");
        Vector3 temp = countDown.rectTransform.localScale;

        if (temp.x >= 1f)
        {
            countDown.rectTransform.localScale = Vector3.zero;
            time -= 1;
            if (time != 0) countDown.text = Mathf.Floor(time).ToString();
            return;
        }
        
        temp.x += repeatingTime;
        temp.y += repeatingTime;
        temp.z += repeatingTime;
        countDown.rectTransform.localScale = temp;

        if (time < 1f && time >= 0f) countDown.text = "GO";
        if (time < 0f)
        {
            countDown.rectTransform.localScale = Vector3.zero;
            gameObject.SetActive(false);
            CancelInvoke();
        }
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(repeatingTime);
        IncreaseUIScaleCountDown();
    }
}
