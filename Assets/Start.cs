using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Start : MonoBehaviour
{
    public GameObject play;
    public TextMeshPro title;
    public TextMeshPro click;

    public static AudioClip clip = null;
    public static string firstText = "<size=3>Dino</size>\nFlippers";
    public static string secondText = "Click to Play";

    void Awake()
    {
        if(clip != null) { GetComponent<AudioSource>().Stop(); GetComponent<AudioSource>().PlayOneShot(clip); }
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
        }
    }
}
