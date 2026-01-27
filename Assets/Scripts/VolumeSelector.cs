//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSelector: MonoBehaviour {
    
    public int volume;
    public GameObject numberDisplay;
    public GameObject selectedDisplay;
    public GameObject unselectedDisplay;
    public SettingsCanvasController settingsCanvasController;

    public void ShowSelected(){
        bool selected = (StaticVariables.globalVolume == volume);
        numberDisplay.SetActive(selected);
        selectedDisplay.SetActive(selected);
        unselectedDisplay.SetActive(!selected);
    }

    public void ClickedButton(){
        settingsCanvasController.SetVolume(volume);
    }

}
