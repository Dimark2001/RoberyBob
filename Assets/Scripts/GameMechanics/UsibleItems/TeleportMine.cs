using UnityEngine;

public class TeleportMine : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy")) {
            col.transform.GetChild(0).GetChild(0).GetComponent<AI_Controller>().Teleportating(transform);
        }
    }
}
