using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FovMesh : MonoBehaviour
{
    [SerializeField] private LayerMask detectLayers;
    [SerializeField] private Vector3 origin;
    [SerializeField] private int rayCount = 150;
    [SerializeField] private float viewDistance = 2f;

    private float startingAngle;
    [SerializeField] private float fov = 90f;

    private Mesh mesh;
    void Start()
    {
        transform.position = new Vector3(0f, 0.02f, 0f);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    private void LateUpdate()
    {
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            Vector3 vertex;
            Physics.Raycast(origin, new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad)), out RaycastHit hit, viewDistance, detectLayers);
            Debug.DrawRay(origin, new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad)) * viewDistance);
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                vertex = hit.point;

                if (hit.collider.gameObject.tag == "Player" && !PlayerMovement.singltone.playerOnInvise && !PlayerMovement.singltone.playerOnMask)
                {
                    AI_Controller.NoiseAlert(hit.collider.gameObject.transform, false);
                }
            }
            else
            {
                vertex = origin + new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad)) * viewDistance;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;

            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
    public void SetOrigin(Vector3 origin) {
        this.origin = origin;
    }
    public void SetDirection(Vector3 direction) { 
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360f;

        startingAngle = n - fov / 2f -90f;
    }
}
