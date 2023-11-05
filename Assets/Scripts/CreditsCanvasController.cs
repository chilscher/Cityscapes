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
    private Color buttonColorExterior;
    private Color buttonColorInterior;
    public GameObject menuButton;
    public GameObject settingsButton;
    


    private void Start() {
        //apply the cosmetics from the current skin
        background.GetComponent<Image>().sprite = StaticVariables.skin.shopBackground;
        InterfaceFunctions.ColorMenuButton(menuButton);
        InterfaceFunctions.ColorMenuButton(settingsButton);
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