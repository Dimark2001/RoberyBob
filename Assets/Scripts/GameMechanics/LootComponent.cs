using System;
using System.Collections;
using UnityEngine;

public class LootComponent : MonoBehaviour
{
    [SerializeField] private bool needToWin = false;
    [Space(15)]
    [SerializeField] private bool isKey = false;
    [SerializeField] private DoorComponent lockedDoor;
    [SerializeField] private AutoDoorComponent lockedAutoDoor;

    [SerializeField] private LayerMask detectLayers;

    private bool alreadyPlussed = false;

    public bool looting = false;

    [HideInInspector] public Action whenLootAction;

    [HideInInspector] public Coroutine reservCoroutine;
    void Start()
    {
        if (!alreadyPlussed) { alreadyPlussed = true; LootingCounter.singleton.countItems++; }
        GetComponent<SphereCollider>().radius *= Bufs.grabAbilityBufCoef;
    }
    private void OnDestroy()
    {
        if (whenLootAction != null) whenLootAction.Invoke();
        if (needToWin) {
            LevelConditions levelConditions = FindObjectOfType<LevelConditions>();
            if(levelConditions != null) levelConditions.OpenExit();
        }
        if (isKey) {
            if (lockedDoor != null) lockedDoor.UnlockingDoor();
            else lockedAutoDoor.UnlockingDoor();
        }
    }
    public IEnumerator FindPlayer() { 
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (Physics.Raycast(transform.position, PlayerMovement.singltone.transform.position - transform.position, out RaycastHit hit, 1000f, detectLayers))
            {
                if (hit.collider.gameObject.name == "Player")
                {
                    looting = true;
                    break;
                }
            }
        }
    }
}
