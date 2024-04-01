using UnityEditor.MPE;
using UnityEngine;

public class FixRotationReactions : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, transform.parent.rotation.y);
    }
}
