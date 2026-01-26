//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioSetup : MonoBehaviour {

    public AudioSource audioSource;
    public List<SoundEffect> soundEffects;

    public void Setup(){
        print("setting up");
        DontDestroyOnLoad(gameObject);
        AudioManager.audioSource = audioSource;
        AudioManager.allSoundEffects = soundEffects;

        foreach (SoundEffect soundEffect in soundEffects){
            bool doesIDListAlreadyExist = false;
            AudioManager.IDs id = soundEffect.ID;
            foreach(AllSoundsWithID list in AudioManager.soundsSortedByID){
                if (list.ID == id){
                    list.soundEffects.Add(soundEffect);
                    doesIDListAlreadyExist = true;
                }
            }
            if (doesIDListAlreadyExist == false){
                AllSoundsWithID newList = new() {ID = id};
                newList.soundEffects.Add(soundEffect);
                AudioManager.soundsSortedByID.Add(newList);
            }
        }
    }
}