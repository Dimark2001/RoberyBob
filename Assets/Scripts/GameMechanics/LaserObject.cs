using System.Collections;
using UnityEngine;

public class LaserObject : MonoBehaviour
{
    private Collider laserCollider;

    [SerializeField] private LineRenderer line;
    [Space(15)]
    [Header("Timer Settings")]
    [SerializeField] private bool activateForTimer = false;
    [SerializeField] private bool firstWait = false;
    [SerializeField] private float timeBetween = 5f;
    private bool work = false;
    private void Awake()
    {
        laserCollider = GetComponent<Collider>();
    }
    void Start()
    {
        if (firstWait) work = true;

        if (activateForTimer)
        {
            StartCoroutine(Activator());
        }
        else {
            work = true;
            StartCoroutine(LaserAnim());
        }
    }
    IEnumerator Activator() { 
        while (true)
        {
            work = !work;
            laserCollider.enabled = work;
            line.enabled = work;
            if (work) StartCoroutine(LaserAnim());
            yield return new WaitForSeconds(timeBetween);
        }
    }
    IEnumerator LaserAnim() { 
        while (work) {
            float timer = 0f;

            while (timer < 0.3f)
            {
                yield return null;
                timer += Time.deltaTime;
                float length = Mathf.Lerp(line.startWidth, 0.05f, Time.deltaTime * 5f);
                line.startWidth = length;
                line.endWidth = length;
            }
            timer = 0f;
            while (timer < 0.3f)
            {
                yield return null;
                timer += Time.deltaTime;
                float length = Mathf.Lerp(line.startWidth, 0.02f, Time.deltaTime * 5f);
                line.startWidth = length;
                line.endWidth = length;
            }
        }
    }
    public void DisableLaser() {
        work = false;
        laserCollider.enabled = work;
        line.enabled = work;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && !PlayerMovement.singltone.playerOnMask) {
            AI_Controller.NoiseAlert(col.gameObject.transform, false);
        }
    }
}
