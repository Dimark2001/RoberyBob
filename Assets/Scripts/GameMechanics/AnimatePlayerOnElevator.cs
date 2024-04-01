using System.Collections;
using UnityEngine;

public class AnimatePlayerOnElevator : MonoBehaviour
{
    [SerializeField] private Transform elevatorFloor;
    private Transform player;
    private SpriteRenderer playerSpriteRenderer;

    private void Start()
    {
        player = PlayerMovement.singltone.transform;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        StartAnim();
    }
    public void StartAnim() => StartCoroutine(AnimPlayerOnElewatorCoroutine());
    IEnumerator AnimPlayerOnElewatorCoroutine() {
        float timer = 0f;
        playerSpriteRenderer.color = Color.black;
        while (timer < 2f)
        {
            yield return null;
            timer += Time.deltaTime;
            player.position = new Vector3(player.position.x, -elevatorFloor.localPosition.z + 0.1f, player.position.z);
            playerSpriteRenderer.color = Color.Lerp(Color.black, Color.white, timer);
        }
        playerSpriteRenderer.color = Color.white;
        player.position = new Vector3(player.position.x, 0.1f, player.position.z);
    }
}
