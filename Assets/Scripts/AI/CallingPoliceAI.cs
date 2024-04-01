using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CallingPoliceAI : MonoBehaviour
{
    public static CallingPoliceAI singltone;

    [SerializeField] private GameObject policeManPrefab;
    [SerializeField] private Transform policeManSpawnPos;
    private Transform playerPos;

    public Action newCall;

    private GameObject dataPoliceMan;
    private AI_Controller policeManController;
    private void Awake()
    {
        singltone = this;
    }
    private void Start()
    {
        playerPos = PlayerMovement.singltone.gameObject.transform;
    }
    public void CallPolice() {
        if (dataPoliceMan == null)
        {
            dataPoliceMan = Instantiate(policeManPrefab, policeManSpawnPos.position, Quaternion.identity);
            dataPoliceMan.transform.position = new Vector3(dataPoliceMan.transform.position.x, 0f, dataPoliceMan.transform.position.y);
            policeManController = dataPoliceMan.transform.GetChild(0).GetChild(0).GetComponent<AI_Controller>();
            Vector3 playerPosCorrected = playerPos.position;
            playerPosCorrected.y = 0f;
            policeManController.policeWay = new List<Vector3>
            {
                playerPosCorrected,
                dataPoliceMan.transform.position
            };
        }
        else if (policeManController.policeWay.Count == 2)
        {
            policeManController.policeWay.RemoveAt(0);
            Vector3 playerPosCorrected = playerPos.position;
            playerPosCorrected.y = 0f;
            policeManController.policeWay.Insert(0, playerPosCorrected);
            newCall.Invoke();
        }  
    }
}
