using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BobMenuWay : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float bobSpeed = 5f;

    private Animator anim;
    private int pointNow = 0;
    private int nextPointAbs = 0;
    private statesOfAnim stateAnim = statesOfAnim.Stop;

    enum statesOfAnim
    {
        F, B, R, L, RF, RB, LF, LB, Stop
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void GoToPoint(int numberPoint) {
        StopAllCoroutines(); 
        StartCoroutine(GoToPointCoroutine(numberPoint)); 
    }
    IEnumerator GoToPointCoroutine(int numberPoint) {
        if (numberPoint > pointNow) { nextPointAbs = 1; }
        else { nextPointAbs = -1; }

        while (pointNow != numberPoint)
        {
            yield return null;
            float distance = Vector2.Distance(transform.position, points[pointNow + nextPointAbs].position);
            if (distance < 0.1f)
            {
                pointNow += nextPointAbs;
            }
            else {
                transform.position = Vector2.MoveTowards(transform.position, points[pointNow + nextPointAbs].position, bobSpeed * Time.deltaTime);

                Vector2 vectorWalking = points[pointNow + nextPointAbs].position - transform.position;
                if (Mathf.Abs(vectorWalking.x) / distance < 0.25f) //Движение только по Y
                {
                    if (vectorWalking.y > 0 && stateAnim != statesOfAnim.F) { anim.SetTrigger("goF"); stateAnim = statesOfAnim.F; }
                    else if(vectorWalking.y < 0 && stateAnim != statesOfAnim.B) { anim.SetTrigger("goB"); stateAnim = statesOfAnim.B; }
                }
                else if (Mathf.Abs(vectorWalking.y) / distance < 0.25f) //Движение только по X
                {
                    if (vectorWalking.x > 0 && stateAnim != statesOfAnim.R) { anim.SetTrigger("goR"); stateAnim = statesOfAnim.R; }
                    else if(vectorWalking.x < 0 && stateAnim != statesOfAnim.L) { anim.SetTrigger("goL"); stateAnim = statesOfAnim.L; }
                }
                else {
                    if (vectorWalking.x > 0 && vectorWalking.y > 0 && stateAnim != statesOfAnim.RF) { anim.SetTrigger("goRF"); stateAnim = statesOfAnim.RF; }
                    else if (vectorWalking.x > 0 && vectorWalking.y < 0 && stateAnim != statesOfAnim.RB) { anim.SetTrigger("goRB"); stateAnim = statesOfAnim.RB; }
                    else if (vectorWalking.x < 0 && vectorWalking.y > 0 && stateAnim != statesOfAnim.LF) { anim.SetTrigger("goLF"); stateAnim = statesOfAnim.LF; }
                    else if (vectorWalking.x < 0 && vectorWalking.y < 0 && stateAnim != statesOfAnim.LB) { anim.SetTrigger("goLB"); stateAnim = statesOfAnim.LB; }
                }
            }

        }
        stateAnim = statesOfAnim.Stop;
        anim.SetTrigger("Stop");
        transform.position = points[numberPoint].position;
    }
}
