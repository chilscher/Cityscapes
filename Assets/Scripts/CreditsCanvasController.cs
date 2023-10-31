//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CreditsCanvasController : MonoBehaviour {
    //controls the credits canvas. Only one is used, and only on the credits scene.
 
    public GameObject blackForeground; //used to transition to/from the settings menu
    private Image blackSprite; //derived from the blackForeground gameObject
    public float fadeOutTime; //total time for fade-out, from complete light to complete darkness
    public float fadeInTime; //total time for fade-in, from complete darkness to complete light
    private float fadeTimer; //the timer on which the fadeout and fadein mechanics operate

    //the following are properties taken from the skin
    public GameObject background;
    private Color buttonColorExterior;
    private Color buttonColorInterior;
    public GameObject menuButton;
    


    private void Start() {
        //apply the cosmetics from the current skin
        background.GetComponent<Image>().sprite = StaticVariables.skin.shopBackground;
        blackSprite = blackForeground.GetComponent<Image>();
        InterfaceFunctions.colorMenuButton(menuButton);

        //starts the fade-in process, which is carried out in the Update function
        if (StaticVariables.isFading && StaticVariables.fadingTo == "credits") {
            fadeTimer = fadeInTime;
        }
    }
    
    private void Update() {
        //if the player is fading from the credits scene back to the settings scene, this block handles that fading process
        if (StaticVariables.isFading && StaticVariables.fadingFrom == "credits") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) { //after the fade timer is done, and the screen is dark, transition to another scene.
                if (StaticVariables.fadingTo == "settings") { SceneManager.LoadScene("Settings"); }
            }
        }
        //if the player is fading into the settings scene, this block handles that fading process
        if (StaticVariables.isFading && StaticVariables.fadingTo == "credits") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeTimer) / fadeInTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                StaticVariables.isFading = false;
                if (StaticVariables.waitingOnButtonClickAfterFadeIn) {
                    StaticVariables.waitingOnButtonClickAfterFadeIn = false;
                    if (StaticVariables.buttonClickInWaiting.Contains("settings")) {
                        back();
                    }
                }
            }
        }
        //if the player presses their phone's back button, call the "back" function
        //identical to if the player just pushed the on-screen back button
        if (Input.GetKeyDown(KeyCode.Escape)) {
            back();
        }
    }

    public void back() {
        //pushing the on-screen back button calls this function, which returns the player to the settings scene
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "settings";
            startFadeOut();
        }
        //if the player pushes the back button while the scene is still fading in, then implement the button press after the fade-in is done
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "settings";
        }
    }

    public void startFadeOut() {
        //begin the fade-out process. This function is called by back
        //the fade-out mechanics of darkening the screen are implemented in the Update function
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "credits";
    }

    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }



}