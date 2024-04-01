using System.Collections;
using UnityEngine;

public class SpyCoordinator : MonoBehaviour
{
    private bool needRevil = true;

    [SerializeField] private AI_Controller spyController;
    private void Start()
    {
        spyController.spyAction += UpdateRevil;
        StartCoroutine(SendPlayerPosToSpyCoroutine());
    }
    IEnumerator SendPlayerPosToSpyCoroutine() { 
        while (true)
        {
            yield return new WaitForSeconds(15f);
            if (needRevil && !PlayerMovement.singltone.isHide && !PlayerMovement.singltone.playerOnInvise && !PlayerMovement.singltone.playerOnMask) {
                needRevil = false;
                spyController.SetOneWay(PlayerMovement.singltone.transform);
                Debug.Log("Start");
            }
        }
    }
    private void UpdateRevil() {
        needRevil = true;
        Debug.Log("Reloaded");
    }
}
