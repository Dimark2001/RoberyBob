using System.Collections;
using UnityEngine;

public class WindUpToy : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float speed = 1f;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private Animator toyAnim;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        toyAnim = GetComponent<Animator>();
        StartCoroutine(RideCoroutine());
    }

    IEnumerator RideCoroutine() {
        float timer = 0f;
        while (timer < 10f)
        {
            yield return null;
            timer += Time.deltaTime;

            rb.velocity = transform.up * speed;

            Ray ray = new Ray(transform.position, transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Time.deltaTime * speed + 0.15f, collisionMask))
            {
                Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
                float rot = 90f - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(90f, rot, 0);
                toyAnim.SetTrigger("ricochet");

                AI_Controller.NoiseAlert(transform);
            }
        }
        Destroy(gameObject);
    }
}
