using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    AudioSource[] m_audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();

    AudioMixer audioMixer; 

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);
            audioMixer = Managers.Resource.Load<AudioMixer>($"Sounds/{Path.SoundMixer}");

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound)); // "Bgm", "Effect"
            for (int i = 1; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                m_audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
            m_audioSources[(int)Define.Sound.Bgm].loop = true; // bgm ������ ���� �ݺ� ���
        }
        DontDestroyOnLoad(m_audioSources[(int)Define.Sound.Bgm]);
    }

    public void Clear()
    {
        // ����� ���� ��� ��ž, ���� ����
        foreach (AudioSource audioSource in m_audioSources)
        {
            if (audioSource == null) continue;
            audioSource.clip = null;
            audioSource.Stop();
        }
        // ȿ���� Dictionary ����
        m_audioClips.Clear();
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm) // BGM ������� ���
        {
            AudioSource audioSource = m_audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[(int)Define.Sound.Bgm];
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // Effect ȿ���� ���
        {
            AudioSource audioSource = m_audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}"; // Sound ���� �ȿ� ����� �� �ֵ���

        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm) // BGM ������� Ŭ�� ���̱�
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else // Effect ȿ���� Ŭ�� ���̱�
        {
            if (m_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                m_audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }
}