using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private bool mainElevator = false;

    private AnimatePlayerOnElevator animatePlayerOnElevator;
    private Animator elevatorAnim;
    private Animator canvasAnim;

    [SerializeField] private GameObject elevatorCanvas;

    private void Awake()
    {
        animatePlayerOnElevator = transform.GetChild(0).GetComponent<AnimatePlayerOnElevator>();
        elevatorAnim = GetComponent<Animator>();
        if (mainElevator)
        {
            elevatorAnim.SetBool("Up", true);
            animatePlayerOnElevator.enabled = true;
        }
        else animatePlayerOnElevator.enabled = false;

        canvasAnim = elevatorCanvas.GetComponent<Animator>();
    }
    void Start()
    {
        if(!mainElevator) elevatorCanvas.SetActive(true);
        else elevatorCanvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) {
            canvasAnim.SetBool("Enable", true);

            ExtraButton.singltone.PressAction = null;
            ExtraButton.singltone.PressAction += GoUp;
            ExtraButton.singltone.ActiveButton(2);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            elevatorCanvas.SetActive(true);
            canvasAnim.SetBool("Enable", false);

            ExtraButton.singltone.PressAction = null;
            ExtraButton.singltone.ActiveButton(0);

            elevatorAnim.SetBool("Up", false);
        }
    }
    public void GoUp() {
        elevatorAnim.SetBool("Up", true);
        elevatorCanvas.SetActive(false);
        //animatePlayerOnElevator.StartAnim();
    }
}
