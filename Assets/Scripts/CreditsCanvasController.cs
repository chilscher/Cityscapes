//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CreditsCanvasController : MonoBehaviour {
    //controls the credits canvas. Only one is used, and only on the credits scene.

    //the following are properties taken from the skin
    public GameObject background;
    public GameObject menuButton;
    public GameObject settingsButton;
    public Image popupBorder;
    public Image popupInside;
    
    


    private void Start() {
        //apply the cosmetics from the current skin
        background.GetComponent<Image>().sprite = StaticVariables.skin.shopBackground;
        InterfaceFunctions.ColorMenuButton(menuButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(settingsButton, StaticVariables.skin);
        popupBorder.color = StaticVariables.skin.popupBorder;
        popupInside.color = StaticVariables.skin.popupInside;
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            StaticVariables.FadeOutThenLoadScene("MainMenu");
    }

    public void PushMainMenuButton() {
        StaticVariables.FadeOutThenLoadScene("MainMenu");
    }

    public void PushSettingsButton() {
        StaticVariables.FadeOutThenLoadScene("Settings");
    }

    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }
}