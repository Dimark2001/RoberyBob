using UnityEngine;

public class TimeController : MonoBehaviour
{
    public void SetTimeSpeed(float speed) { 
        Time.timeScale = speed;
    }
}
