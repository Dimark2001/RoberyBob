using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bufs : MonoBehaviour
{
    //SAVES
    private static string stealthProgress_SaveKey = "STEALTH_PROGRESS";
    private static string staminaProgress_SaveKey = "STAMINA_PROGRESS";
    private static string grabAbilityProgress_SaveKey = "GRAB_ABILITY_PROGRESS";
    private static string speedProgress_SaveKey = "SPEED_PROGRESS";

    public static string rottenDonutCount_SaveKey = "ROTTEN_DONUT_COUNT";
    public static string windUpToyCount_SaveKey = "WINDUP_TOY_COUNT";
    public static string invisibilityPotionCount_SaveKey = "INVISIBILITY_POTION_COUNT";
    public static string teleportMineCount_SaveKey = "TELEPORT_MINE_COUNT";
    public static string lockPickCount_SaveKey = "LOCK_PICK_COUNT";

    //PROGRESSES
    private static int stealthProgress = 0;
    private static int staminaProgress = 0;
    private static int grabAbilityProgress = 0;
    private static int speedProgress = 0;

    public static int rottenDonutCount = 0;
    public static int windUpToyCount = 0;
    public static int invisibilityPotionCount = 0;
    public static int teleportMineCount = 0;
    public static int lockPickCount = 0;

    //COAST
    private static int stealthCoast = 20;
    private static int staminaCoast = 20;
    private static int grabAbilityCoast = 20;
    private static int speedCoast = 20;

    private static int rottenDonutCoast = 30;
    private static int windUpToyCoast = 30;
    private static int invisibilityPotionCoast = 50;
    private static int teleportMineCoast = 100;
    private static int lockPickCoast = 150;

    //BUF COEF
    public static float stealthBufCoef { get; private set; } = 1f;
    public static float staminaBufCoef { get; private set; } = 1f;
    public static float grabAbilityBufCoef { get; private set; } = 1f;
    public static float speedBufCoef { get; private set; } = 1f;

    //PLUS FOR LEVEL
    private static float stealthPlusForLevel = -0.1f;
    private static float staminaPlusForLevel = 0.15f;
    private static float grabAbilityPlusForLevel = 0.15f;
    private static float speedPlusForLevel = 0.15f;

    //Upgrade Progress Images
    [SerializeField] private Image stealthImage;
    [SerializeField] private Image staminaImage;
    [SerializeField] private Image grabImage;
    [SerializeField] private Image speedImage;
    [Space(15)]
    [SerializeField] private TextMeshProUGUI stealthCoastText;
    [SerializeField] private TextMeshProUGUI staminaCoastText;
    [SerializeField] private TextMeshProUGUI grabCoastText;
    [SerializeField] private TextMeshProUGUI speedCoastText;


    [Space(15)]
    [Header("Items")]
    [SerializeField] private TextMeshProUGUI rottenDonutCountText;
    [SerializeField] private TextMeshProUGUI windUpToyCountText;
    [SerializeField] private TextMeshProUGUI invisibilityPotionCountText;
    [SerializeField] private TextMeshProUGUI teleportMineCountText;
    [SerializeField] private TextMeshProUGUI lockPickCountText;

    private void Awake()
    {
        stealthBufCoef = 1f;
        staminaBufCoef = 1f;
        grabAbilityBufCoef = 1f;
        speedBufCoef = 1f;

        #region loadProgress
        if (PlayerPrefs.HasKey(stealthProgress_SaveKey))
        {
            stealthProgress = PlayerPrefs.GetInt(stealthProgress_SaveKey);
            stealthImage.fillAmount = 1f / 4f * stealthProgress;
            stealthBufCoef += stealthPlusForLevel * stealthProgress;

            if (stealthProgress == 1) stealthCoast = 50;
            else if (stealthProgress == 2) stealthCoast = 150;
            else if (stealthProgress == 3) stealthCoast = 450;
            else if (stealthProgress == 4) stealthCoast = 0;
            stealthCoastText.text = stealthCoast.ToString();
        }
        if (PlayerPrefs.HasKey(staminaProgress_SaveKey))
        {
            staminaProgress = PlayerPrefs.GetInt(staminaProgress_SaveKey);
            staminaImage.fillAmount = 1f / 4f * staminaProgress;
            staminaBufCoef += staminaPlusForLevel * staminaProgress;

            if (staminaProgress == 1) staminaCoast = 50;
            else if (staminaProgress == 2) staminaCoast = 150;
            else if (staminaProgress == 3) staminaCoast = 450;
            else if (staminaProgress == 4) staminaCoast = 0;
            staminaCoastText.text = staminaCoast.ToString();
        }
        if (PlayerPrefs.HasKey(grabAbilityProgress_SaveKey))
        {
            grabAbilityProgress = PlayerPrefs.GetInt(grabAbilityProgress_SaveKey);
            grabImage.fillAmount = 1f / 4f * grabAbilityProgress;
            grabAbilityBufCoef += grabAbilityPlusForLevel * grabAbilityProgress;

            if (grabAbilityProgress == 1) grabAbilityCoast = 50;
            else if (grabAbilityProgress == 2) grabAbilityCoast = 150;
            else if (grabAbilityProgress == 3) grabAbilityCoast = 450;
            else if (grabAbilityProgress == 4) grabAbilityCoast = 0;
            grabCoastText.text = grabAbilityCoast.ToString();
        }
        if (PlayerPrefs.HasKey(speedProgress_SaveKey))
        {
            speedProgress = PlayerPrefs.GetInt(speedProgress_SaveKey);
            speedImage.fillAmount = 1f / 4f * speedProgress;
            speedBufCoef += speedPlusForLevel * speedProgress;

            if (speedProgress == 1) speedCoast = 50;
            else if (speedProgress == 2) speedCoast = 150;
            else if (speedProgress == 3) speedCoast = 450;
            else if (speedProgress == 4) speedCoast = 0;
            speedCoastText.text = speedCoast.ToString();
        }
        if (PlayerPrefs.HasKey(rottenDonutCount_SaveKey))
        {
            rottenDonutCount = PlayerPrefs.GetInt(rottenDonutCount_SaveKey);
            rottenDonutCountText.text = rottenDonutCount.ToString();
        }
        if (PlayerPrefs.HasKey(windUpToyCount_SaveKey))
        {
            windUpToyCount = PlayerPrefs.GetInt(windUpToyCount_SaveKey);
            windUpToyCountText.text = windUpToyCount.ToString();
        }
        if (PlayerPrefs.HasKey(invisibilityPotionCount_SaveKey))
        {
            invisibilityPotionCount = PlayerPrefs.GetInt(invisibilityPotionCount_SaveKey);
            invisibilityPotionCountText.text = invisibilityPotionCount.ToString();
        }
        if (PlayerPrefs.HasKey(teleportMineCount_SaveKey))
        {
            teleportMineCount = PlayerPrefs.GetInt(teleportMineCount_SaveKey);
            teleportMineCountText.text = teleportMineCount.ToString();
        }
        if (PlayerPrefs.HasKey(lockPickCount_SaveKey))
        {
            lockPickCount = PlayerPrefs.GetInt(lockPickCount_SaveKey);
            lockPickCountText.text = lockPickCount.ToString();
        }
        #endregion
    }
    public void UpgradeAbility(string name)
    {
        switch (name)
        {
            case "stealth":
                if (MoneyManager.money < stealthCoast) {
                    break; //donate page in the future
                }
                if (stealthProgress < 4) {
                    stealthProgress++;
                    stealthImage.fillAmount = 1f / 4f * stealthProgress;
                    stealthBufCoef += stealthPlusForLevel;

                    MoneyManager.singltone.MinusMoney(stealthCoast);

                    if (stealthProgress == 1) stealthCoast = 50;
                    else if (stealthProgress == 2) stealthCoast = 150;
                    else if (stealthProgress == 3) stealthCoast = 450;
                    else if (stealthProgress == 4) stealthCoast = 0;

                    stealthCoastText.text = stealthCoast.ToString();

                    PlayerPrefs.SetInt(stealthProgress_SaveKey, stealthProgress);
                }
                break;
            case "stamina":
                if (MoneyManager.money < staminaCoast)
                {
                    break; //donate page in the future
                }
                if (staminaProgress < 4)
                {
                    staminaProgress++;
                    staminaImage.fillAmount = 1f / 4f * staminaProgress;
                    staminaBufCoef += staminaPlusForLevel;

                    MoneyManager.singltone.MinusMoney(staminaCoast);

                    if (staminaProgress == 1) staminaCoast = 50;
                    else if (staminaProgress == 2) staminaCoast = 150;
                    else if (staminaProgress == 3) staminaCoast = 450;
                    else if (staminaProgress == 4) staminaCoast = 0;

                    staminaCoastText.text = staminaCoast.ToString();

                    PlayerPrefs.SetInt(staminaProgress_SaveKey, staminaProgress);
                }
                break;
            case "grab":
                if (MoneyManager.money < grabAbilityCoast)
                {
                    break; //donate page in the future
                }
                if (grabAbilityProgress < 4)
                {
                    grabAbilityProgress++;
                    grabImage.fillAmount = 1f / 4f * grabAbilityProgress;
                    grabAbilityBufCoef += grabAbilityPlusForLevel;

                    MoneyManager.singltone.MinusMoney(grabAbilityCoast);

                    if (grabAbilityProgress == 1) grabAbilityCoast = 50;
                    else if (grabAbilityProgress == 2) grabAbilityCoast = 150;
                    else if (grabAbilityProgress == 3) grabAbilityCoast = 450;
                    else if (grabAbilityProgress == 4) grabAbilityCoast = 0;

                    grabCoastText.text = grabAbilityCoast.ToString();

                    PlayerPrefs.SetInt(grabAbilityProgress_SaveKey, grabAbilityProgress);
                }
                break;
            case "speed":
                if (MoneyManager.money < speedCoast)
                {
                    break; //donate page in the future
                }
                if (speedProgress < 4)
                {
                    speedProgress++;
                    speedImage.fillAmount = 1f / 4f * speedProgress;
                    speedBufCoef += speedPlusForLevel;
                    
                    MoneyManager.singltone.MinusMoney(speedCoast);

                    if (speedProgress == 1) speedCoast = 50;
                    else if (speedProgress == 2) speedCoast = 150;
                    else if (speedProgress == 3) speedCoast = 450;
                    else if (speedProgress == 4) speedCoast = 0;

                    speedCoastText.text = speedCoast.ToString();

                    PlayerPrefs.SetInt(speedProgress_SaveKey, speedProgress);
                }
                break;
        }
    }
    public void BuyItem(string name) {
        switch (name)
        {
            case "rottenDonut":
                if (MoneyManager.money < rottenDonutCoast) {
                    break; //donate page in the future
                }
                MoneyManager.singltone.MinusMoney(rottenDonutCoast);
                rottenDonutCount++;
                rottenDonutCountText.text = rottenDonutCount.ToString();

                PlayerPrefs.SetInt(rottenDonutCount_SaveKey, rottenDonutCount);
                break;
            case "windUpToy":
                if (MoneyManager.money < windUpToyCoast)
                {
                    break; //donate page in the future
                }
                MoneyManager.singltone.MinusMoney(windUpToyCoast);
                windUpToyCount++;
                windUpToyCountText.text = windUpToyCount.ToString();

                PlayerPrefs.SetInt(windUpToyCount_SaveKey, windUpToyCount);
                break;
            case "invisibilityPotion":
                if (MoneyManager.money < invisibilityPotionCoast)
                {
                    break; //donate page in the future
                }
                MoneyManager.singltone.MinusMoney(invisibilityPotionCoast);
                invisibilityPotionCount++;
                invisibilityPotionCountText.text = invisibilityPotionCount.ToString();

                PlayerPrefs.SetInt(invisibilityPotionCount_SaveKey, invisibilityPotionCount);
                break;
            case "teleportMine":
                if (MoneyManager.money < teleportMineCoast)
                {
                    break; //donate page in the future
                }
                MoneyManager.singltone.MinusMoney(teleportMineCoast);
                teleportMineCount++;
                teleportMineCountText.text = teleportMineCount.ToString();

                PlayerPrefs.SetInt(teleportMineCount_SaveKey, teleportMineCount);
                break;
            case "lockPick":
                if (MoneyManager.money < lockPickCoast)
                {
                    break; //donate page in the future
                }
                MoneyManager.singltone.MinusMoney(lockPickCoast);
                lockPickCount++;
                lockPickCountText.text = lockPickCount.ToString();

                PlayerPrefs.SetInt(lockPickCount_SaveKey, lockPickCount);
                break;
        }
    }
}
