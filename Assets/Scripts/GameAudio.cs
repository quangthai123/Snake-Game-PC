using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public static GameAudio instance;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private AudioSource[] sfx;
    void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    public void PlayBGM(int i)
    {
        bgm[i].Play();
    }
    public void StopBGM(int i)
    {
        bgm[i].Stop();
    }
    public void PlaySFX(int i) 
    { 
        sfx[i].Play(); 
    }
    public void StopSFX(int i)
    {
        sfx[i].Stop();
    }
}
