using System.Collections;
using UnityEngine;

public class RottenDonut : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float forceFly = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DonutFly());
    }

    IEnumerator DonutFly() {
        rb.AddForce(transform.up * forceFly, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy")) {
            col.transform.GetChild(0).GetChild(0).GetComponent<AI_Controller>().GoToBath();
            Destroy(gameObject);
        }
    }
}
