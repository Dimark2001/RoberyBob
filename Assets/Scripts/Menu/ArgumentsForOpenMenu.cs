using UnityEngine;
using UnityEngine.Events;

public class ArgumentsForOpenMenu : MonoBehaviour
{
    public static bool openShop = false;
    [SerializeField] private UnityEvent openShopEvent;

    private void Start()
    {
        if (openShop) { openShopEvent.Invoke(); openShop = false; }
    }
}
