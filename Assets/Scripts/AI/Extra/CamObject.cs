using System.Collections;
using UnityEngine;

public class CamObject : MonoBehaviour
{
    private Transform mainParent;

    [SerializeField] private FovMesh fovMesh;
    [SerializeField] private Transform fovMeshPos;

    [SerializeField] private bool isRotating = false;
    [Space(5)]
    [SerializeField] private float speedRotate = 1f;
    [SerializeField] private float angleMin;
    [SerializeField] private float angleMax;

    [SerializeField] private float angleNow = 180f;
    private bool toMax = false;

    private bool targetOnTrigger = false;
    private Coroutine reservCheckTargetCoroutine;
    [SerializeField] private LayerMask detectLayers;
    void Start()
    {
        mainParent = transform.parent;
    }
    private void OnEnable()
    {
        if (isRotating) StartCoroutine(RotateCoroutine());
    }
    private void Update()
    {
        fovMesh.SetDirection(transform.parent.up);
        fovMesh.SetOrigin(fovMeshPos.position);
    }
    private IEnumerator RotateCoroutine() {
        while (true)
        {
            yield return null;
            if (toMax)
            {
                mainParent.Rotate(0f, 0f, 2f * speedRotate * Time.deltaTime);
                angleNow += 2f * speedRotate * Time.deltaTime;
                if (angleNow > angleMax) { toMax = !toMax; }
            }
            else
            {
                mainParent.Rotate(0f, 0f, -2f * speedRotate * Time.deltaTime);
                angleNow -= 2f * speedRotate * Time.deltaTime;
                if (angleNow < angleMin) { toMax = !toMax; }
            }
        }
    }
    public void DisactiveCamera() {
        fovMesh.gameObject.SetActive(false);
        this.enabled = false;
    }
}
