using System;
using UnityEngine;

public class ExtraButton : MonoBehaviour
{
    public static ExtraButton singltone;

    [SerializeField] private GameObject[] buttons;

    [HideInInspector] public Action PressAction;

    private void Awake()
    {
        singltone = this;
    }
    private void Start()
    {
        ActiveButton(0);
    }
    public void ActiveButton(int buttonID) { 
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
        PlayerMovement.singltone.UpRunButton();

        buttons[buttonID].SetActive(true);
    }
    public void PressButton() => PressAction.Invoke();
}
