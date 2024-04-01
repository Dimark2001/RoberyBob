using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LootingCounter : MonoBehaviour 
{
    public static LootingCounter singleton { get; private set; }

    [HideInInspector] public int countItems;
    private int collectedItems = 0;
    [SerializeField] private Image itemCounter;
    [SerializeField] private Sprite fullCounterSprite;
    //effects
    [SerializeField] private GameObject pickUpEffect;
    [SerializeField] private GameObject fullCounterEffect;

    [SerializeField] private Animator animCounter;

    private LevelConditions levelConditions;

    private void Awake()
    {
        singleton = this;
    }
    void Start()
    {
        itemCounter.fillAmount = 0;

        levelConditions = FindObjectOfType<LevelConditions>();
    }
    public void PickUpItem() {
        collectedItems++;
        StartCoroutine(ItemCounterFillAmountAnimCoroutine());
        pickUpEffect.SetActive(true);

        if (collectedItems == countItems) {
            itemCounter.sprite = fullCounterSprite;
            fullCounterEffect.SetActive(true);
            animCounter.SetTrigger("FullCounterTrigger");
            
            levelConditions.lootStar = 1;
        }
    }
    IEnumerator ItemCounterFillAmountAnimCoroutine() { 
        while (itemCounter.fillAmount < 1f / countItems * collectedItems)
        {
            yield return null;
            itemCounter.fillAmount = Mathf.Lerp(itemCounter.fillAmount, 1f / countItems * collectedItems, 5f * Time.deltaTime);
        }
        itemCounter.fillAmount = 1f / countItems * collectedItems;
    }
}
