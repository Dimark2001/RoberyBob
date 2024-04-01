using NavMeshPlus.Components;
using System.Collections.Generic;
using UnityEngine;

public class DoorComponent : MonoBehaviour
{
    public static List<DoorComponent> lockedDoorComponents = new List<DoorComponent>();

    [SerializeField] private bool lockedDoor = false;
    [SerializeField] private GameObject lockCanvas;
    [Space(15)]
    [SerializeField] private Transform outsidePoint, insidePoint;
    [SerializeField] private Transform privateOutsidePoint, privateInsidePoint;

    [SerializeField]private int doorState = 0; //-1 = inside /// 0 = close /// 1 = outside
    private float newDoorAngle = 0f;
    private bool needRotation = false;
    private bool halfUpdateNavMesh = false;

    private float doorProgress;

    private Quaternion newRotation;

    private Collider triggerColider;

    // “Œ Œ“ –€À ÀŒ√» ¿
    private string whoOpen = "";

    private NavMeshSurface navSurface;

    private void Start()
    {
        if (lockedDoor) lockedDoorComponents.Add(this);
        triggerColider = GetComponent<Collider>();
        GameObject dataNuvSurfaceObject = GameObject.FindWithTag("NavDogs");
        if (dataNuvSurfaceObject != null) navSurface = dataNuvSurfaceObject.GetComponent<NavMeshSurface>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Enemy"))
        {
            if (!lockedDoor) FindNewDoorAngle(col.transform);
        }
    }
    private void Update()
    {
        if (needRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, doorProgress);
            if (doorProgress >= 0.5f && !halfUpdateNavMesh)
            {
                halfUpdateNavMesh = true;
                if(navSurface != null) navSurface.BuildNavMesh();
            }
            if (doorProgress >= 1f)
            {
                doorProgress = 0f;
                halfUpdateNavMesh = false;
                transform.rotation = newRotation;
                newRotation = new Quaternion();
                needRotation = false;
                triggerColider.enabled = true;
                if (navSurface != null) navSurface.BuildNavMesh();
            }
            else
            {
                doorProgress += Time.deltaTime;
                triggerColider.enabled = false;
            }
        }
    }
    private void FindNewDoorAngle(Transform col) {
        float distanceOutside = Vector3.Distance(outsidePoint.position, col.position);
        float distanceInside = Vector3.Distance(insidePoint.position, col.position);
        if (distanceOutside > distanceInside)
        {
            if (doorState != 1)
            {
                newDoorAngle = -90f;
                doorState += 1;
            }
            else return;
        }
        else
        {
            if (doorState != -1)
            {
                newDoorAngle = 90f;
                doorState -= 1;
            }
            else return;
        }
        if (newRotation != Quaternion.Euler(0, 0, 0))
        {
            newRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + newDoorAngle);
            needRotation = true;
            whoOpen = col.gameObject.tag;
        }
    }
    public int GetState() { return doorState; }
    public Transform GetFarPoint(Transform start) {
        float distanceOutside = Vector3.Distance(privateOutsidePoint.position, start.position);
        float distanceInside = Vector3.Distance(privateInsidePoint.position, start.position);
        if (distanceOutside > distanceInside) return privateOutsidePoint;
        else return privateInsidePoint;
    }
    public void Close() {
        if (doorState == 1)
        {
            newDoorAngle = 90f;
            doorState -= 1;
        }
        else if (doorState == -1)
        {
            newDoorAngle = -90f;
            doorState += 1;
        }
        else {
            return;
        }
        if (newRotation != Quaternion.Euler(0, 0, 0))
        {
            newRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + newDoorAngle);
            needRotation = true;
        }
    }

    public string GetWhoOpenDoor() { return whoOpen; }

    public void UnlockingDoor() {
        lockedDoor = false;
        lockCanvas.SetActive(false);
    }
}
