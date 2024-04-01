using UnityEngine;

public class HideObjectComponent : MonoBehaviour
{
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject effectObject;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SwitchState(bool openState) {
        if (!openState)
        {
            spriteRenderer.sprite = closeSprite;
            effectObject.SetActive(true);
        }
        else {
            spriteRenderer.sprite = openSprite;
            effectObject.SetActive(false);
        }
    }
}
