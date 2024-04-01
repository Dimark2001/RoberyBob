using System.Collections;
using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(MoneyAnim());
    }
    IEnumerator MoneyAnim() {
        float timer = 0f;
        while (timer < 0.35f) {
            transform.Translate(Vector3.up * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        while (timer < 1f)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 0.8f);
            timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
