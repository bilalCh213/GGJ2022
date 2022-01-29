using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Start : MonoBehaviour
{
    public GameObject play;
    public AudioLowPassFilter filter;
    public TextMeshPro title;
    public TextMeshPro click;

    public static string firstText = "<size=3>Dino</size>\nFlippers";
    public static string secondText = "Click to Play";

    void Awake()
    {
        title.text = firstText;
        click.text = secondText;
    }

    void OnEnable()
    {
        Awake();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
            play.SetActive(true);
            filter.enabled = false;
        }
    }
}
