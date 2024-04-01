using UnityEngine;

public class CameraBordersSettings : MonoBehaviour
{
    [SerializeField] private Vector2 leftBottomBorder;
    [SerializeField] private Vector2 rightTopBorder;

    private void Start()
    {
        Camera.main.GetComponent<CameraMovement>().SetBorders(leftBottomBorder, rightTopBorder);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(leftBottomBorder.x, 0f, leftBottomBorder.y), 0.1f);
        Gizmos.DrawSphere(new Vector3(rightTopBorder.x, 0f, rightTopBorder.y), 0.1f);
    }
}
