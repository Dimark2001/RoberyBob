using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;

    private void Start()
    {
        Instantiate(levels[LevelConditions.singltone.GetLevelID()]);
    }
}
