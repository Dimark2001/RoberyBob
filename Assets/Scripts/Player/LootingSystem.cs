using System.Collections;
using UnityEngine;

public class LootingSystem : MonoBehaviour
{
    private float timer = 0f;

    private Coroutine reservCoroutine;
    private bool wait = false;

    [SerializeField] private LayerMask detectLayers;
    [SerializeField] private GameObject moneyEffect;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Prop")) {
            wait = true;
            StartCoroutine(PickUpAnimationForPropCoroutine(col.transform));
            PlayerCamouflage.singltone.DisableCamouflage();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Prop"))
        {
            wait = false;
        }
    }
    IEnumerator PickUpAnimationForPropCoroutine(Transform prop) {
        LootComponent lootComponent = prop.GetComponent<LootComponent>();
        if (lootComponent.reservCoroutine == null) lootComponent.reservCoroutine = StartCoroutine(lootComponent.FindPlayer());
        timer = 0f;
        while (wait)
        {
            yield return new WaitForSeconds(0.1f);
            if (lootComponent.looting)
            {
                break;
            }
        }
        if (lootComponent.reservCoroutine != null) StopCoroutine(lootComponent.reservCoroutine);
        lootComponent.reservCoroutine = null;
        if (!wait) {
            yield break;
        }
        MoneyManager.singltone.PlusMoney(Random.Range(1, 5));
        Instantiate(moneyEffect, prop.position, moneyEffect.transform.rotation);
        while (Vector3.Distance(prop.position, transform.position) > 0.25f)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            if (prop != null) prop.position = Vector3.Lerp(prop.position, transform.position, timer);
        }
        LootingCounter.singleton.PickUpItem();
        Destroy(prop.gameObject);
    }
}
