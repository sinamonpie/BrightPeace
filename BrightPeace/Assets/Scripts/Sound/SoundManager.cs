using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;

    #region singleton
    void Awake()    
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public AudioSource[] audioSoundEffects;
    public AudioSource audioSourceBGM;

    public Sound[] soundEffects;
    public Sound[] bgmSounds;

    public void PlaySoundEffect(string _name)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (_name == soundEffects[i].name)
            {
                for (int j = 0; j < audioSoundEffects.Length; j++)
                {
                    if (!audioSoundEffects[j].isPlaying)
                    {
                        audioSoundEffects[j].clip = soundEffects[i].clip;
                        audioSoundEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 AudioSource 재생 중");
                return;
            }
        }
        Debug.Log(_name + "사운드 효과를 찾을 수 없음");
    }

    public void PlayBGM(string _name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (_name == bgmSounds[i].name)
            {
                audioSourceBGM.clip = bgmSounds[i].clip;
                audioSourceBGM.Play();
                return;
            }
        }
        Debug.Log(_name + "BGM을 찾을 수 없음");
    }

    public void StopBGM()
    {
        audioSourceBGM.Stop();
    }

    public void StopEverySoundEffects()
    {
        for (int i = 0; i < audioSoundEffects.Length; i++)
        {
            audioSoundEffects[i].Stop();
        }
    }

    public void StopAllSounds()
    {
        StopBGM();
        StopEverySoundEffects();
    }
}
