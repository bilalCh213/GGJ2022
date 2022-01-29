using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject baseA;
    public GameObject baseB;
    [Space]
    public GameObject unitA;
    public GameObject unitB;
    
    private float timer = 0.0f;

    void Update()
    {
        if(timer <= 0.0f)
        {
            if(baseB == null)
            {
                Start.firstText = "You win!";
                Start.secondText = "Click to play again";
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if(baseA == null)
            {
                Start.firstText = "Game Over";
                Start.secondText = "Click to play again";
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            for(int i = 0; i < unitA.transform.childCount; i++)
                unitA.transform.GetChild(i).GetComponent<Unit>().intention = GetMovementIntention(unitA.transform.GetChild(i));
            for(int i = 0; i < unitB.transform.childCount; i++)
                unitB.transform.GetChild(i).GetComponent<Unit>().intention = GetMovementIntention(unitB.transform.GetChild(i), false);

            timer = 0.2f;
        }
        else
            timer -= Time.deltaTime;
    }

    Vector3 GetMovementIntention(Transform unitTr, bool isA = true)
    {
        Vector3 intention = Vector3.zero;

        GameObject target = unitTr.GetComponent<Unit>().GetTarget();
        if(target != null)
        {
            Vector3 direction = (target.transform.position - unitTr.position).normalized;
            float distance = Vector3.Distance(unitTr.position, target.transform.position);

            float targetDistance = 1.0f;
            float springStrength = distance - targetDistance;
            intention += direction * springStrength;
        }

        for(int i = 0; i < unitA.transform.childCount; i++)
        {
            if(unitTr == unitA.transform.GetChild(i)) continue;

            Vector3 direction = (unitA.transform.GetChild(i).position - unitTr.position).normalized;
            float distance = Vector3.Distance(unitTr.position, unitA.transform.GetChild(i).position);

            float targetDistance = 1.0f;
            float springStrength = distance - targetDistance;
            intention += direction * springStrength * (isA ? -0.75f : 1.0f);
        }

        for(int i = 0; i < unitB.transform.childCount; i++)
        {
            if(unitTr == unitB.transform.GetChild(i)) continue;

            Vector3 direction = (unitB.transform.GetChild(i).position - unitTr.position).normalized;
            float distance = Vector3.Distance(unitTr.position, unitB.transform.GetChild(i).position);

            float targetDistance = 1.0f;
            float springStrength = distance - targetDistance;
            intention += direction * springStrength * (isA ? 1.0f : -0.75f);
        }

        if(intention.magnitude < 0.5f)
            return Vector3.zero;

        return intention.normalized;
    }
}
