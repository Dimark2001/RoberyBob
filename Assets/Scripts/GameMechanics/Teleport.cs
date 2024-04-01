using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private SpriteRenderer teleportRenderer;
    [SerializeField] private Sprite off, on;

    [Space(15)]
    [SerializeField] private Teleport secondTeleport;
    [SerializeField] private GameObject teleportEffect;

    [HideInInspector] public bool cooldown = false;

    private SpriteRenderer playerRenderer;
    private GameObject teleportingPlayerEffect;
    private Rigidbody playerRigidBody;
    private Collider playerCollider;
    private void Start()
    {
        teleportRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        playerRenderer = PlayerMovement.singltone.GetComponent<SpriteRenderer>();
        playerRigidBody = PlayerMovement.singltone.GetComponent<Rigidbody>();
        playerCollider = PlayerMovement.singltone.GetComponent<Collider>();
        teleportingPlayerEffect = PlayerMovement.singltone.teleportingPlayerEffect;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && !cooldown) {
            teleportRenderer.sprite = on;

            cooldown = true;
            secondTeleport.cooldown = true;
            StartCoroutine(Teleporting(col.transform));
        }
    }
    IEnumerator Teleporting(Transform player) {
        GameObject dataEffect = Instantiate(teleportEffect, transform.position, Quaternion.Euler(90f, 0f, 0f));
        Destroy(dataEffect, 1f);

        PlayerMovement.singltone.enabled = false;
        playerRenderer.enabled = false;
        teleportingPlayerEffect.SetActive(true);
        playerRigidBody.velocity = Vector3.zero;
        playerCollider.enabled = false;

        while (Vector3.Distance(secondTeleport.transform.position, player.position) > 0.1f)
        {
            yield return null;
            player.position = Vector3.LerpUnclamped(player.position, secondTeleport.transform.position, Time.deltaTime * 3f);
        }
        player.position = secondTeleport.transform.position;

        PlayerMovement.singltone.enabled = true;
        playerRenderer.enabled = true;
        teleportingPlayerEffect.SetActive(false);
        playerCollider.enabled = true;

        GameObject dataEffect1 = Instantiate(teleportEffect, secondTeleport.transform.position + new Vector3(0f, 0.72f, 0f), Quaternion.Euler(90f, 0f, 0f));
        Destroy(dataEffect1, 1f);

        yield return new WaitForSeconds(1f);
        cooldown = false;
        secondTeleport.cooldown = false;
        teleportRenderer.sprite = off;
    }
}
