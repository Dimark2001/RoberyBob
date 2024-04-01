using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoiseMaker : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Transform noisePos;
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI timerText;
    [Space(15)]
    [SerializeField] private AI_Controller noisedAI;

    private bool atWork = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) {
            anim.SetBool("isButtonActive", true);
            ExtraButton.singltone.PressAction = null;
            ExtraButton.singltone.PressAction += StartNoiseTimer;
            ExtraButton.singltone.ActiveButton(1);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isButtonActive", false);
            ExtraButton.singltone.PressAction = null;
            ExtraButton.singltone.ActiveButton(0);
        }
    }
    public void StartNoiseTimer() => StartCoroutine(NoiseCoroutine());

    private IEnumerator NoiseCoroutine() {
        if (atWork) { yield break; }
        atWork = true;
        float timer = 5f;
        anim.SetBool("isButtonActive", false);
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            timerImage.fillAmount = 1f / 5f * timer;
            timerText.text = Mathf.Round(timer).ToString();

            yield return null;
        }
        timerText.text = "";
        anim.SetTrigger("shake");
        noisedAI.GoToNoiseWithoutDistance(noisePos);
        yield return new WaitForSeconds(3f);
        timerImage.fillAmount = 1f;
        atWork = false;
    }
}
