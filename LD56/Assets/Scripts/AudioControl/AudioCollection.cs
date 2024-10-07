using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Collection", menuName = "ScriptableObject/AudioCollection", order = 0)]
public class AudioCollection : ScriptableObject
{
    public List<AudioInfo> infos = new List<AudioInfo>();

    void OnEnable()
    {
        //assign unique id to all aduios
        for (int i = 0; i < infos.Count; i++)
        {
            infos[i].ID = i;
        }
    }
}

public enum AudioType {Sfx, Bgm, UI}

public class AudioInfo
{
    public string audioName;
    public AudioType type = AudioType.Sfx;
    public AudioClip clip;
    public int ID;
}