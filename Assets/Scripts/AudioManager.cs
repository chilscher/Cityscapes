//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioManager{

    static public AudioSource audioSource;
    public enum IDs {Generic, Selection, Note, Build, Erase, Undo, Redo, Clear, VictoryCheer}
    static public List<SoundEffect> allSoundEffects = new();
    static public List<AllSoundsWithID> soundsSortedByID = new();

    static public void PlaySound(IDs ID){
        //Debug.Log("playing sound" + ID.ToString());
        SoundEffect se = allSoundEffects[0];
        foreach (AllSoundsWithID list in soundsSortedByID){
            if (list.ID == ID)
                se = list.GetRandomSoundFromList();
        }
        audioSource.clip = se.audioClip;
        audioSource.volume = (float)(se.volumePercentage / 100.0) * (float)(StaticVariables.globalVolume / 100.0);
        //Debug.Log(audioSource.volume);
        //if (ID == IDs.Generic)
        //    audioSource.pitch = (float)(StaticVariables.rand.Next(10, 13) / 10.0);
        audioSource.Play();
    }
}

[System.Serializable]
public class SoundEffect{
    public AudioManager.IDs ID;
    public AudioClip audioClip;
    [Range(0, 200)]
    public int volumePercentage = 100;
}

public class AllSoundsWithID{
    public AudioManager.IDs ID;
    public List<SoundEffect> soundEffects = new();
    public SoundEffect GetRandomSoundFromList(){
        return soundEffects[StaticVariables.rand.Next(0, soundEffects.Count)];
    }
}
