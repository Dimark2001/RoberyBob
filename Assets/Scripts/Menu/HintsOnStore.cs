using UnityEngine;
using TMPro;

public class HintsOnStore : MonoBehaviour
{
    [SerializeField] private string[] hints;

    [SerializeField] private TextMeshProUGUI hintText;

    public void ShowHint(int id) {
        hintText.text = hints[id];
    }
}
