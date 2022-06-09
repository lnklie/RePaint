using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : SoundManager.cs
==============================
*/
public enum Sound
{
    WORLDMAP,
    BACKGROUND,
    EFFECTGROUND,
    EFFECT
}
public class SoundManager : MonoBehaviour
{
    AudioSource[] audioSources = new AudioSource[4];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public void GenerateSound()
    {
        // ���� ����
        GameObject root = GameObject.Find("Sound");
        if(root == null)
        {
            root = new GameObject { name = "Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Sound));

            for(int i = 0; i <soundNames.Length; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            audioSources[0].clip = Resources.Load<AudioClip>("Sounds\\BACKGROUND\\WorldMapBGM");
            audioSources[(int)Sound.WORLDMAP].loop = true;
            audioSources[(int)Sound.BACKGROUND].loop = true;
            audioSources[(int)Sound.EFFECTGROUND].loop = true;
        }
    }

    public void Clear()
    {
        // ���� �ʱ�ȭ
        for(int i = 1; i < audioSources.Length; i++)
        {
            audioSources[i].Stop();
            audioSources[i].clip = null;
        }

        audioClips.Clear();
    }

    public void SoundStop(Sound _soundType)
    {
        // ���� ����
        audioSources[(int)_soundType].Stop();
    }

    public void SetSoundOption(Sound _soundType,float _value)
    {
        // ���� �ɼ� ����
        audioSources[(int)_soundType].volume = _value;
    }

    public void SoundPlay(AudioClip _audioClip, Sound _soundType =Sound.EFFECT, float _pitch = 1.0f )
    {
        // Ŭ������ �ҷ��� ���� ����
        if (_audioClip == null)
            return;
        if(_soundType == Sound.BACKGROUND)
        {
            AudioSource audioSource = audioSources[(int)Sound.BACKGROUND];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = _pitch;
            audioSource.clip = _audioClip;
            audioSource.Play();
        }
        else if(_soundType == Sound.EFFECTGROUND)
        {
            AudioSource audioSource = audioSources[(int)Sound.EFFECTGROUND];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = _pitch;
            audioSource.clip = _audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = audioSources[(int)Sound.EFFECT];
            audioSource.pitch = _pitch;
            audioSource.PlayOneShot(_audioClip);
        }
    }
    public void SoundPlay(string _path,Sound _soundType = Sound.EFFECT, float _pitch = 1.0f)
    {
        // ��η� ���� ����
        AudioClip audioClip = GetOrAddAudioClip(_path, _soundType);
        SoundPlay(audioClip, _soundType, _pitch);
    }

    public void SoundPlay(Sound _soundType = Sound.WORLDMAP,float _pitch = 1.0f)
    {
        audioSources[0].Play();
    }

    private AudioClip GetOrAddAudioClip(string _path, Sound _soundType = Sound.EFFECT)
    {
        // ���� �߰� �Ǵ� ��������
        if(_path.Contains($"Sounds/ + {_soundType}") == false)
        {
            _path = $"Sounds/{_soundType}/{_path}";
        }

        AudioClip audioClip = null;

        if(_soundType == Sound.BACKGROUND)
        {
            audioClip = Resources.Load<AudioClip>(_path);
        }
        else if(_soundType == Sound.EFFECTGROUND)
        {
            audioClip = Resources.Load<AudioClip>(_path);
        }
        else
        {
            if(audioClips.TryGetValue(_path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(_path);
                audioClips.Add(_path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {_path}");

        return audioClip;
    }
}
