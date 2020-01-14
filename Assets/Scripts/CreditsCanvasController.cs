using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CreditsCanvasController : MonoBehaviour {
 

    public GameObject blackForeground; //used to transition to/from the puzzle menu
    private SpriteRenderer blackSprite;
    public float fadeOutTime;
    public float fadeInTime;
    private float fadeTimer;

    public GameObject background;
    private Color buttonColorExterior;
    private Color buttonColorInterior;
    public GameObject menuButton;
    


    private void Start() {
        background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.creditsBackground;
        blackSprite = blackForeground.GetComponent<SpriteRenderer>();

        InterfaceFunctions.colorMenuButton(menuButton);

        if (StaticVariables.isFading && StaticVariables.fadingTo == "credits") {
            fadeTimer = fadeInTime;
        }
    }
    
    private void Update() {
        if (StaticVariables.isFading && StaticVariables.fadingFrom == "credits") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                if (StaticVariables.fadingTo == "settings") { SceneManager.LoadScene("Settings"); }
            }
        }
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
        if (Input.GetKeyDown(KeyCode.Escape)) {
            back();
        }
    }
    
    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void back() {
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "settings";
            startFadeOut();
        }
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "settings";
        }
    }

    public void startFadeOut() {
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "credits";
    }

    

}