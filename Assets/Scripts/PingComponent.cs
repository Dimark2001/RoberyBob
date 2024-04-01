using UnityEngine;

public class PingComponent : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private RectTransform compasRU, compasLD;
    RectTransform ping;

    private Camera mainCamera;

    static bool start = true;
    private void Awake()
    {
        start = true;
    }
    private void Start()
    {
        ping = GetComponent<RectTransform>();
        if (tag == "ExitPing" && start) { LevelConditions.singltone.exitPing = gameObject.transform.parent.gameObject; gameObject.transform.parent.gameObject.SetActive(false); start = false; }

        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (target == null) { 
            gameObject.SetActive(false);
        }
        Vector2 newPingPos = mainCamera.WorldToScreenPoint(target.position);
        if (TargetOnCamera())
        {
            ping.position = newPingPos;
        }
        else
        {
            Vector2 targetOnCamera = mainCamera.WorldToViewportPoint(target.position);

            if (targetOnCamera.x > 0.97f) {
                newPingPos.x = compasRU.position.x;
            }
            else if (targetOnCamera.x < 0.03f) {
                newPingPos.x = compasLD.position.x;
            }
            if (targetOnCamera.y > 0.97f)
            {
                newPingPos.y = compasRU.position.y;
            }
            else if (targetOnCamera.y < 0.03f)
            {
                newPingPos.y = compasLD.position.y;
            }

            ping.position = newPingPos;
        }
    }
    private bool TargetOnCamera() {
        Vector2 targetOnCamera = mainCamera.WorldToViewportPoint(target.position);
        if (targetOnCamera.x > 1f || targetOnCamera.y > 1f) return false;
        else if (targetOnCamera.x < 0f || targetOnCamera.y < 0f) return false;
        else return true;
    }
}
