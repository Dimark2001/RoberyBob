using UnityEngine;

public class HideSystem : MonoBehaviour
{
    private bool canHide = false;
    private bool onHide = false;

    private PlayerMovement playerMovement;
    private Collider playerCollider;
    private SpriteRenderer playerSpriteRenderer;
    private Rigidbody rb;

    private HideObjectComponent hideObjectComponentData;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("HideItem"))
        {
            canHide = true;
            hideObjectComponentData = col.GetComponent<HideObjectComponent>();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("HideItem"))
        {
            canHide = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!onHide && canHide)
            {
                onHide = true;
                playerMovement.isHide = true;
                playerMovement.enabled = false;
                playerCollider.enabled = false;
                playerSpriteRenderer.enabled = false;
                rb.velocity = Vector3.zero;

                hideObjectComponentData.SwitchState(false);
            }
            else
            {
                onHide = false;
                playerMovement.enabled = true;
                playerMovement.isHide = false;
                playerCollider.enabled = true;
                playerSpriteRenderer.enabled = true;

                if(hideObjectComponentData != null) hideObjectComponentData.SwitchState(true);
            }
        }
    }
}
