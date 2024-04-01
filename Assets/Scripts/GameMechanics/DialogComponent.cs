using UnityEngine;
using TMPro;
using System.Collections;

public class DialogComponent : MonoBehaviour
{
    enum typePersonOnDialog { 
        bob, spy
    }
    private int dialogNow = -1;
    [SerializeField] private float speedText = 1f;
    [Space(5)]
    [SerializeField][TextArea]private string[] texts;
    [SerializeField] private typePersonOnDialog[] typePersonOnDialogs;
    [Space(15)]
    [SerializeField] private TextMeshProUGUI textObject;
    [SerializeField] private GameObject spyObject;
    [SerializeField] private GameObject bobObject;

    private void OnValidate()
    {
        if (texts != null && typePersonOnDialogs.Length != texts.Length) typePersonOnDialogs = new typePersonOnDialog[texts.Length];
    }
    private void Start()
    {
        Time.timeScale = 0f;
        ClickOnDialog();
    }
    public void ClickOnDialog() {
        dialogNow++;
        if (dialogNow >= texts.Length) {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            return;
        }

        if (typePersonOnDialogs[dialogNow] == typePersonOnDialog.spy) { 
            spyObject.SetActive(true);
            bobObject.SetActive(false);
        }
        if (typePersonOnDialogs[dialogNow] == typePersonOnDialog.bob)
        {
            bobObject.SetActive(true);
            spyObject.SetActive(false);
        }
        StopAllCoroutines();
        StartCoroutine(TextAnimation(texts[dialogNow]));
    }
    IEnumerator TextAnimation(string text) {
        string textOnScreen = "";
        int simbolNow = 0;
        while (simbolNow < text.Length) {
            yield return new WaitForSecondsRealtime(1f / speedText);
            textOnScreen += text[simbolNow];
            simbolNow++;
            textObject.text = textOnScreen;
        }
    }
}
