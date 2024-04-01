using UnityEngine;

public class CameraDisactivator : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [Space(15)]
    [SerializeField] private CamObject camObject;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) {
            if (button.activeInHierarchy) {
                button.SetActive(false);

            }
        }
    }
}
