using Unity.VisualScripting;
using UnityEngine;

public class PoliceAssault : MonoBehaviour
{
    [SerializeField] private GameObject policemansObject;
    [SerializeField] private LootComponent lootComponent;
    private void Start()
    {
        lootComponent.whenLootAction += StartAssault;
    }
    public void StartAssault() { policemansObject.SetActive(true); }
}
