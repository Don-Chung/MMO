using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();
    public AudioClip[] audioClipArray;
    public AudioSource audioSource;
    public bool isQuiet = false;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        foreach (AudioClip ac in audioClipArray)
        {
            audioDict.Add(ac.name, ac);
        }
    }

    public void Play(string audioName)
    {
        if (isQuiet) return;
        AudioClip ac;
        if(audioDict.TryGetValue(audioName, out ac))
        {
            //AudioSource.PlayClipAtPoint(ac, Vector3.zero);
            audioSource.PlayOneShot(ac);//audioSource本身存在节省性能
        }
    }

    public void Play(string audioName, AudioSource audioSource)
    {
        if (isQuiet) return;
        AudioClip ac;
        if (audioDict.TryGetValue(audioName, out ac))
        {
            //AudioSource.PlayClipAtPoint(ac, Vector3.zero);
            audioSource.PlayOneShot(ac);//audioSource本身存在节省性能
        }
    }
}
