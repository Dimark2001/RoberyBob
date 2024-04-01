using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelConditions : MonoBehaviour
{
    public static LevelConditions singltone;

    public static int level_id = 0;
    [Space(15)]
    [SerializeField] private float timeToGetStar = 60f;

    [HideInInspector] public int seeStar = 1;
    [HideInInspector] public int timeStar = 1;
    [HideInInspector] public int lootStar = 0;

    [HideInInspector] public GameObject exitLevel;

    private bool levelComplite;
    [Space(15)]
    [Header("Win Panel")]
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject perfectObject;
    [Space(5)]
    [SerializeField] private Image seeStarImage;
    [SerializeField] private Image timeStarImage;
    [SerializeField] private Image lootStarImage;

    [Space(15)]
    public GameObject exitPing;
    [Space(15)]
    [Header("Defeat Panel")]
    [SerializeField] private GameObject defeatCanvas;
    [SerializeField] private GameObject defeatEffect;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private GameObject pingsCanvas;

    [Space(15)]
    [Header("BORDERS")]
    [SerializeField] private Vector2 leftBottomBorder;
    [SerializeField] private Vector2 rightTopBorder;

    private void Awake()
    {
        singltone = this;
    }
    private void Start()
    {
        seeStar = 1;
        timeStar = 1;
        lootStar = 0;

        StartCoroutine(LevelTimerCoroutine());
    }
    
    public void OpenExit()
    {
        pingsCanvas = exitPing.transform.parent.gameObject;
        exitLevel.SetActive(true);
        exitPing.SetActive(true);
    }
    public void Win() {
        exitPing.SetActive(false);

        string key = LevelsManager.singltone.levelComponents[level_id].saveKey;
        if (level_id == LevelsManager.singltone.GetLastOpenedLevel()) {
            LevelsManager.singltone.LevelUp();
        }

        string save = PlayerPrefs.GetString(key);
        if (save.Length != 4) save = "0000";
        string newSave = "";

        if (save[0] == '0') newSave += seeStar.ToString();
        else newSave += "1";
        if (save[1] == '0') newSave += timeStar.ToString();
        else newSave += "1";
        if (save[2] == '0') newSave += lootStar.ToString();
        else newSave += "1";

        newSave += "1";
        PlayerPrefs.SetString(LevelsManager.singltone.levelComponents[level_id].saveKey, newSave);

        if (seeStar == 1) seeStarImage.color = new Color(1f, 1f, 1f, 1f);
        if (timeStar == 1) timeStarImage.color = new Color(1f, 1f, 1f, 1f);
        if (lootStar == 1) lootStarImage.color = new Color(1f, 1f, 1f, 1f);

        if(seeStar + timeStar + lootStar == 3) perfectObject.SetActive(true);
        winCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
    

    IEnumerator LevelTimerCoroutine() {
        float timer = 0f;
        while (timer <= timeToGetStar || !levelComplite)
        {
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }
        if (timer > timeToGetStar) timeStar = 0;
    }

    public void OpenScene(string name) { 
        SceneManager.LoadScene(name);
    }
    public void OpenNextLevel(string name)
    {
        level_id++;
        SceneManager.LoadScene(name);
    }
    public void Defeat() {
        defeatCanvas.SetActive(true);
        defeatEffect.SetActive(true);
        playerSpriteRenderer.enabled = false;
        pingsCanvas = exitPing.transform.parent.gameObject;
        pingsCanvas.SetActive(false);
        Time.timeScale = 0f;
    }
    public void SetOpenShopArgumentForMenu() { ArgumentsForOpenMenu.openShop = true; }

    public int GetLevelID() { return level_id; }
}
