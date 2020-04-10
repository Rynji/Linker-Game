using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioSource audioSourceEffects;

    private AudioClip gridRefillClip;

    public AudioClip GridRefillClip { get => gridRefillClip; set => gridRefillClip = value; }

    
    public void PlaySoundEffect(AudioClip audioClip)
    {
        StartCoroutine(DoPlaySoundEffect(audioClip));
    }

    private IEnumerator DoPlaySoundEffect(AudioClip soundEffect)
    {
        audioSourceEffects.PlayOneShot(soundEffect);
        yield return new WaitForSeconds(soundEffect.length);
    }
}
