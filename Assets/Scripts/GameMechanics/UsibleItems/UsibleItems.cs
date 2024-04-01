using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UsibleItems : MonoBehaviour
{
    private int[] itemsCount = new int[5];

    private int itemNow = -1;

    [SerializeField] private Image itemButtonImage;
    [SerializeField] private Sprite[] itemsSprites;
    [SerializeField] private GameObject[] itemsPrefabs;

    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private GameObject stepsEffect;


    private void Start()
    {
        LoadItemsFromShop();
        FindItemNow();
    }
    private void LoadItemsFromShop() {
        itemsCount[0] = Bufs.rottenDonutCount;
        itemsCount[1] = Bufs.windUpToyCount;
        itemsCount[2] = Bufs.invisibilityPotionCount;
        itemsCount[3] = Bufs.teleportMineCount;
        itemsCount[4] = Bufs.lockPickCount;
    }
    private void FindItemNow() {
        bool finded = false;
        for (int i = 0; i < 5; i++)
        {
            if (itemsCount[i] > 0) {
                itemButtonImage.sprite = itemsSprites[i];
                itemNow = i;
                finded = true;
                itemButtonImage.gameObject.SetActive(true);
                break;
            }
        }
        if (!finded) {
            itemNow = -1;
        }
    }
    public void UseItem() {
        if (itemNow == -1) { itemButtonImage.gameObject.SetActive(false); return; }

        if (itemNow == 2)
        {
            StartCoroutine(UseInvisibilityPotion());
        }
        else if (itemNow == 4)
        {
            OpenLockedDoor();
        }
        else
        {
            Instantiate(itemsPrefabs[itemNow], PlayerMovement.singltone.transform.position, PlayerMovement.singltone.transform.rotation);
        }
        itemsCount[itemNow]--;

        switch (itemNow)
        {
            case 0:
                Bufs.rottenDonutCount--;
                PlayerPrefs.SetInt(Bufs.rottenDonutCount_SaveKey, Bufs.rottenDonutCount);
                break;
            case 1:
                Bufs.windUpToyCount--;
                PlayerPrefs.SetInt(Bufs.windUpToyCount_SaveKey, Bufs.windUpToyCount);
                break;
            case 2:
                Bufs.invisibilityPotionCount--;
                PlayerPrefs.SetInt(Bufs.invisibilityPotionCount_SaveKey, Bufs.invisibilityPotionCount);
                break;
            case 3:
                Bufs.teleportMineCount--;
                PlayerPrefs.SetInt(Bufs.teleportMineCount_SaveKey, Bufs.teleportMineCount);
                break;
            case 4:
                Bufs.lockPickCount--;
                PlayerPrefs.SetInt(Bufs.lockPickCount_SaveKey, Bufs.lockPickCount);
                break;
        }

        if (itemsCount[itemNow] <= 0) {
            itemButtonImage.gameObject.SetActive(false);
            itemsCount[itemNow] = 0;
            FindItemNow();
        }
        
    }
    private IEnumerator UseInvisibilityPotion()
    {
        stepsEffect.SetActive(true);
        PlayerMovement.singltone.playerOnInvise = true;
        playerRenderer.color = new Color(1f, 1f, 1f, 0.2f);
        yield return new WaitForSeconds(7f);
        stepsEffect.SetActive(false);
        playerRenderer.color = new Color(1f, 1f, 1f, 1f);
        PlayerMovement.singltone.playerOnInvise = false;
    }
    private void OpenLockedDoor() {
        if (DoorComponent.lockedDoorComponents.Count == 0) return;
        float minDistance = 100000f;
        int nearestDoorID = -1;
        for (int i = 0; i < DoorComponent.lockedDoorComponents.Count; i++)
        {
            float dataDistance = Vector3.Distance(PlayerMovement.singltone.transform.position, DoorComponent.lockedDoorComponents[i].transform.position);
            if (dataDistance < minDistance) {
                minDistance = dataDistance;
                nearestDoorID = i;
            }
        }

        DoorComponent.lockedDoorComponents[nearestDoorID].UnlockingDoor();
    }
}
