using UnityEngine;

public class СamouflageObject : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) {
            anim.SetBool("Enable", true);

            ExtraButton.singltone.PressAction = null;
            ExtraButton.singltone.PressAction += EnableCamouflage;
            ExtraButton.singltone.ActiveButton(3);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Enable", false);

            ExtraButton.singltone.PressAction = null;
            ExtraButton.singltone.ActiveButton(0);
        }
    }

    public void EnableCamouflage() { 
        
    }
}
