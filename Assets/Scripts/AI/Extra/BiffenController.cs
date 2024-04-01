using UnityEngine;

public class BiffenController : MonoBehaviour
{
    private bool spawned = false;
    [SerializeField] private AI_Controller biffenObject;
    public void Catch(Transform target) {
        if (!spawned) {
            spawned = true;
            biffenObject.transform.parent.parent.gameObject.SetActive(true);
            biffenObject.GoToNoiseWithoutDistance(target);
        }
    }
}
