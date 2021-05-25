using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    #region Audio

    public AudioMixer mixer;
    public AudioClip[] audioClips;
    public AudioClip currentClips;
    public AudioSource audioSource;
    public void SetVolume(float volume)
    {
        mixer.SetFloat("volume", volume);
    }
    public void ToggleMute(bool isMuted)
    {
        if (isMuted)
        {
            mixer.SetFloat("isMutedVolume", -80);
        }
        else
        {
            mixer.SetFloat("isMutedVolume", 0);
        }
    }
    public void PlayClip()
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
    }
    #endregion
   
}
