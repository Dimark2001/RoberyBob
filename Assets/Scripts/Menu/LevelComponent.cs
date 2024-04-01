using UnityEngine;
using UnityEngine.UI;

public class LevelComponent : MonoBehaviour
{
    public string saveKey = "CHAPTER1_LEVEL";
    public string stars { get; private set; } = "0000";
    [Space(15)]
    private bool levelComplite = false;

    [SerializeField] private Image levelImage;
    [SerializeField] private Sprite perfectLevelSprite;

    [Space(15)]
    [Header("Stars Settings")]
    [SerializeField] private GameObject starsPanel;
    [Space(5)]
    [SerializeField] private Image seeStarImage;
    [SerializeField] private Image timeStarImage;
    [SerializeField] private Image lootStarImage;

    private void Start()
    {
        stars = PlayerPrefs.GetString(saveKey);
        if (stars.Length != 4) stars = "0000";
        if (stars[3] == '1') {
            levelComplite = true;
            if (stars == "1111")
            {
                levelImage.sprite = perfectLevelSprite;
            }
        }
    }
    public void ShowLevelStars() {
        if (levelComplite)
        {
            if (stars[0] == '1') seeStarImage.color = new Color(1f, 1f, 1f, 1f);
            if (stars[1] == '1') timeStarImage.color = new Color(1f, 1f, 1f, 1f);
            if (stars[2] == '1') lootStarImage.color = new Color(1f, 1f, 1f, 1f);

            starsPanel.SetActive(true);
        }
    }
    public void HideLevelStars() { starsPanel.SetActive(false); }
    public void SaveStars() { PlayerPrefs.SetString(saveKey, stars); }
}
