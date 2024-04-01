using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private static List<Conveyor> conveyorList = new List<Conveyor>();
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) {
            conveyorList.Add(this);

            PlayerMovement.singltone.playerOnConveyor = true;
            PlayerMovement.singltone.conveyorVector = conveyorList[0].transform.up;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            conveyorList.Remove(this);

            if (conveyorList.Count == 0)
            {
                PlayerMovement.singltone.playerOnConveyor = false;
                PlayerMovement.singltone.conveyorVector = Vector3.zero;
            }
            else {
                PlayerMovement.singltone.conveyorVector = conveyorList[0].transform.up;
            }
        }
    }
}
