using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelsManager : MonoBehaviour
{
    public static LevelsManager singltone;
    [SerializeField] private BobMenuWay[] bobMenuWayScript;

    [SerializeField] private Button[] levels;
    [HideInInspector] public LevelComponent[] levelComponents;
    [SerializeField] private int[] levelPoints_ID;
    private static int levelNow = 0;
    private static string levelNow_SaveKey = "LEVEL_NOW";
    //public static int currentLevel_id = 0;
    private int lastLevel_id = -1;

    private int countClicks = 0;

    private void Awake()
    {
        singltone = this;
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey(levelNow_SaveKey))
        {
            levelNow = PlayerPrefs.GetInt(levelNow_SaveKey);
        }
        levelNow = 28;
        Time.timeScale = 1f;
        levelComponents = new LevelComponent[levels.Length];
        for (int i = levelNow + 1; i < levels.Length; i++)
        {
            levels[i].interactable = false;
        }
        for (int i = 0; i < levelComponents.Length; i++)
        {
            levelComponents[i] = levels[i].GetComponent<LevelComponent>();
        }
    }
    public void LevelUp() {
        levelNow++;
/*        for (int i = levelNow + 1; i < levels.Length; i++)
        {
            levels[i].interactable = false;
        }
        for (int i = 0; i < levelNow; i++)
        {
            levels[i].interactable = true;
        }*/

        PlayerPrefs.SetInt(levelNow_SaveKey, levelNow);
    }
    public void Click(int buttonID) {
        if (lastLevel_id == buttonID)
        {
            countClicks++;
        }
        else {
            countClicks = 1;
            if (buttonID < 15)//for next chapters
            {
                bobMenuWayScript[0].GoToPoint(levelPoints_ID[buttonID]);
            }
            else {
                bobMenuWayScript[1].GoToPoint(levelPoints_ID[buttonID]);
            }

            if (lastLevel_id != -1) levelComponents[lastLevel_id].HideLevelStars();

            lastLevel_id = buttonID;

            levelComponents[lastLevel_id].ShowLevelStars();
        }

        if (countClicks >= 2) {
            LevelConditions.level_id = buttonID;
            OpenScene("GameScene");
        }
    }
    public void OpenScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public int GetLastOpenedLevel() { return levelNow; }
}
