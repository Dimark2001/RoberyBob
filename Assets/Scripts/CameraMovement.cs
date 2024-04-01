using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float damping = 1.5f;
    [SerializeField] private Vector2 offset = new Vector2(2f, 1f);
    [SerializeField] private Transform target;

    [SerializeField] private bool lockX = false;
    [SerializeField] private bool for2Vector = false;

    private Vector2 leftBottomBorder;
    private Vector2 rightTopBorder;

    void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
    }
    void Update()
    {
        if (target)
        {
            if (!for2Vector)
            { 
                transform.position = GetCurentPosition();
            }
            else {
                Vector2 targetPos;

                targetPos = new Vector2(target.position.x + offset.x, target.position.y + offset.y);
                if (lockX)
                {
                    targetPos.x = 0f;
                }

                Vector2 currentPosition = Vector2.Lerp(transform.position, targetPos, damping * Time.deltaTime);
                transform.position = currentPosition;
            }
        }
    }
    private Vector3 GetCurentPosition() {
        Vector3 targetPos;

        targetPos = new Vector3(target.position.x + offset.x, transform.position.y, target.position.z + offset.y);
        if (leftBottomBorder != Vector2.zero && rightTopBorder != Vector2.zero)
        {
            if (target.position.x < leftBottomBorder.x) { targetPos.x = leftBottomBorder.x; }
            else if (target.position.x > rightTopBorder.x) { targetPos.x = rightTopBorder.x; }

            if (target.position.z < leftBottomBorder.y) { targetPos.z = leftBottomBorder.y; }
            else if (target.position.z > rightTopBorder.y) { targetPos.z = rightTopBorder.y; }
        }
        if (lockX)
        {
            targetPos.x = 0f;
        }
        Vector3 currentPosition = Vector3.Lerp(transform.position, targetPos, damping * Time.deltaTime);

        return currentPosition;
    }
    public void SetBorders(Vector2 leftBottomBorder, Vector2 rightTopBorder) 
    { 
        this.leftBottomBorder = leftBottomBorder;
        this.rightTopBorder = rightTopBorder;
    }
    public void SetTarget(Transform newTarget) { target = newTarget; }
}
