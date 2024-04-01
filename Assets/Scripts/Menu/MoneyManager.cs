using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager singltone;

    private static string money_saveKey = "MONEY";

    public static int money { get; private set; }

    [SerializeField] private TextMeshProUGUI counter;

    private void Awake()
    {
        singltone = this;
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey(money_saveKey)) money = PlayerPrefs.GetInt(money_saveKey);
        else money = 100;
        UpdateMoneyCounters();
    }
    public void PlusMoney(int count) {
        money += count;
        UpdateMoneyCounters();
    }
    public void MinusMoney(int count)
    {
        money -= count;
        UpdateMoneyCounters();
    }
    private void UpdateMoneyCounters() {
        counter.text = money.ToString();

        PlayerPrefs.SetInt(money_saveKey, money);
    }
}
