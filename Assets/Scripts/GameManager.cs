using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject baseA;
    public GameObject baseB;
    [Space]
    public GameObject unitA;
    public GameObject unitB;
    [Space]
    public TextMeshPro gameTimeText;
    [Space]
    public AudioSource startAudSrc;
    public AudioClip startClip;
    public AudioClip winClip;
    public AudioClip loseClip;
    
    private float timer = 0.0f;
    private float gameTime = 0.0f;

    private AudioSource audSrc;

    static public GameManager instance = null;

    public void IncreaseUnitsSpeed()
    {
        for(int i = 0; i < unitA.transform.childCount; i++)
            unitA.transform.GetChild(i).GetComponent<Unit>().speedMultiplier *= 1.5f; 
    }

    public void DecreaseUnitsSpeed()
    {
        for(int i = 0; i < unitA.transform.childCount; i++)
            unitA.transform.GetChild(i).GetComponent<Unit>().speedMultiplier /= 1.5f; 
    }

    public void ResetUnitsHealth()
    {
        for(int i = 0; i < unitA.transform.childCount; i++)
            unitA.transform.GetChild(i).GetComponent<Health>().Reset();
    }

    public void HalfUnitsHealth()
    {
        for(int i = 0; i < unitA.transform.childCount; i++)
            unitA.transform.GetChild(i).GetComponent<Health>().Half();
    }

    public void DestroyRandomUnit(bool isA = true)
    {
        if((isA && unitA.transform.childCount <= 0) || (!isA && unitB.transform.childCount <= 0)) return;
        int randomIndex = Random.Range(0, isA ? unitA.transform.childCount : unitB.transform.childCount);
        if(isA) Destroy(unitA.transform.GetChild(randomIndex).gameObject);
        else Destroy(unitB.transform.GetChild(randomIndex).gameObject);
    }

    public void DestroyAllUnits(bool isA = true)
    {
        List<GameObject> objectsToDestroy = new List<GameObject>();
        if(isA)
            for(int i = 0; i < unitA.transform.childCount; i++)
                objectsToDestroy.Add(unitA.transform.GetChild(i).gameObject);
        else
            for(int i = 0; i < unitB.transform.childCount; i++)
                objectsToDestroy.Add(unitB.transform.GetChild(i).gameObject);
        foreach(var o in objectsToDestroy)
            Destroy(o);
        objectsToDestroy.Clear();
    }

    public void ResetBaseHealth()
    {
        baseA.GetComponent<Health>().Reset();
    }

    public void HalfBaseHealth()
    {
        baseA.GetComponent<Health>().Half();
    }

    void Awake()
    {
        instance = this;
        startAudSrc.PlayOneShot(startClip);
        audSrc = GetComponent<AudioSource>();
        audSrc.volume = 0.0f;
        gameTime = 0.0f;
    }

    void Update()
    {
        if(timer <= 0.0f)
        {
            if(baseB == null)
            {
                Start.clip = winClip;
                Start.firstText = "You win!\nTime: " + gameTimeText.text;
                Start.secondText = "Click to play again";
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if(baseA == null)
            {
                Start.clip = loseClip;
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

        gameTime += Time.unscaledDeltaTime;
        gameTimeText.text = (Mathf.FloorToInt(gameTime) / 60).ToString() + ":" + (Mathf.FloorToInt(gameTime) % 60).ToString();

        audSrc.volume = Mathf.Lerp(audSrc.volume, 1.0f, Time.deltaTime * 0.25f);
    }

    Vector3 GetMovementIntention(Transform unitTr, bool isA = true)
    {
        Vector3 intention = Vector3.zero;

        GameObject target = unitTr.GetComponent<Unit>().GetTarget();
        if(target != null)
        {
            Vector3 direction = (target.transform.position - unitTr.position).normalized;
            float distance = Vector3.Distance(unitTr.position, target.transform.position);

            float targetDistance = 0.5f;
            float springStrength = distance - targetDistance;
            intention += direction * springStrength;
        }

        for(int i = 0; i < unitA.transform.childCount; i++)
        {
            if(unitTr == unitA.transform.GetChild(i)) continue;

            Vector3 direction = (unitA.transform.GetChild(i).position - unitTr.position).normalized;
            float distance = Vector3.Distance(unitTr.position, unitA.transform.GetChild(i).position);

            float targetDistance = (isA ? 2.0f : 1.0f);
            float springStrength = distance - targetDistance;
            intention += direction * springStrength * (isA ? -1.0f : 1.0f);
        }

        for(int i = 0; i < unitB.transform.childCount; i++)
        {
            if(unitTr == unitB.transform.GetChild(i)) continue;

            Vector3 direction = (unitB.transform.GetChild(i).position - unitTr.position).normalized;
            float distance = Vector3.Distance(unitTr.position, unitB.transform.GetChild(i).position);

            float targetDistance = (isA ? 0.5f : 1.0f);
            float springStrength = distance - targetDistance;
            intention += direction * springStrength * (isA ? 1.0f : -1.0f);
        }

        if(intention.magnitude < 0.5f)
            return Vector3.zero;

        return intention.normalized;
    }
}
