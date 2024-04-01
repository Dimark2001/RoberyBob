using UnityEngine;

public class LevelExit : MonoBehaviour
{
    bool levelComplite = false;
    private void Start()
    {
        LevelConditions.singltone.exitLevel = gameObject;
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            if (!levelComplite)
            {
                levelComplite = true;
                LevelConditions.singltone.Win();
            }
        }
    }
}
