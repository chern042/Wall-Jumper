using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicVolumeController : MonoBehaviour
{
    [SerializeField] private AudioSource bGMusic;
    // Start is called before the first frame update
    void Start()
    {
        bGMusic.volume = PlayerPrefs.GetFloat("Music Slider", 0.4f);
    }

}
