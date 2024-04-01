using UnityEngine;

public class LaserDisactivator : MonoBehaviour
{
    [SerializeField] private LaserObject laser;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) {
            laser.DisableLaser();
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
