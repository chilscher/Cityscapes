//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;

public class AudioManager{

    static public AudioSource audioSource;
    public enum IDs {Select, PlaceNote, PlaceBuilding, Erase, Undo, Redo, ClearPuzzle, VictoryCheer, GotCoins, ClickedTutorial, SpendCoins, MenuButton, ExpandButton, ContractButton, SceneChangeIn, SceneChangeOut}
    static public List<SoundEffect> allSoundEffects = new();
    static public List<AllSoundsWithID> soundsSortedByID = new();
    static public SoundEffect lastSoundPlayed;

    static public void PlaySound(IDs ID){
        SoundEffect se = allSoundEffects[0];
        foreach (AllSoundsWithID list in soundsSortedByID){
            if (list.ID == ID)
                se = list.GetRandomSoundFromList();
        }
        lastSoundPlayed = se;
        audioSource.clip = se.audioClip;
        audioSource.volume = (float)(se.volumePercentage / 100.0) * (float)(StaticVariables.globalVolume / 100.0);
        
        //Debug.Log("playing ID " + ID.ToString() + " (" + audioSource.clip.name + ")");

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
        SoundEffect se = soundEffects[StaticVariables.rand.Next(0, soundEffects.Count)];
        if (se == AudioManager.lastSoundPlayed)
            if (soundEffects.Count > 1)
                return GetRandomSoundFromList();
        return se;
    }
}
