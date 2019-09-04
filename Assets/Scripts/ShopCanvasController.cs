﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopCanvasController : MonoBehaviour {
    
    public GameObject coinsBox1s;
    public GameObject coinsBox10s;
    public GameObject coinsBox100s;
    public GameObject coinsBox1000s;
    public GameObject coinsBox10000s;

    private SpriteRenderer sprite1s;
    private SpriteRenderer sprite10s;
    private SpriteRenderer sprite100s;
    private SpriteRenderer sprite1000s;
    private SpriteRenderer sprite10000s;

    public Sprite[] numbers = new Sprite[10]; 
    
    public GameObject redNoteButton;
    public GameObject greenNoteButton;
    public GameObject changeCorrectResidentColorButton;
    public GameObject undoRedoButton;

    public GameObject showMedButton;
    public GameObject showLargeButton;
    public GameObject showHugeButton;

    private void Start() {
        sprite1s = coinsBox1s.GetComponent<SpriteRenderer>();
        sprite10s = coinsBox10s.GetComponent<SpriteRenderer>();
        sprite100s = coinsBox100s.GetComponent<SpriteRenderer>();
        sprite1000s = coinsBox1000s.GetComponent<SpriteRenderer>();
        sprite10000s = coinsBox10000s.GetComponent<SpriteRenderer>();

        displayCoinsAmount();
        updateButtons();

    }
    

    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void goToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }


    public void displayCoinsAmount() {
        int value1s = StaticVariables.coins % 10;
        int value10s = (StaticVariables.coins / 10) % 10;
        int value100s = (StaticVariables.coins / 100) % 10;
        int value1000s = (StaticVariables.coins / 1000) % 10;
        int value10000s = (StaticVariables.coins / 10000) % 10;
        sprite1s.sprite = numbers[value1s];
        sprite10s.sprite = numbers[value10s];
        sprite100s.sprite = numbers[value100s];
        sprite1000s.sprite = numbers[value1000s];
        sprite10000s.sprite = numbers[value10000s];

        if (value10000s == 0) {
            coinsBox10000s.SetActive(false);
            if (value1000s == 0) {
                coinsBox1000s.SetActive(false);
                if (value100s == 0) {
                    coinsBox100s.SetActive(false);
                    if (value10s == 0) {
                        coinsBox10s.SetActive(false);
                    }
                }
            }
        }
    }


    public void clickedHugePuzzle() {
        if (StaticVariables.highestUnlockedSize == 6) {
            StaticVariables.highestUnlockedSize = 5;
        }
        else {
            StaticVariables.highestUnlockedSize = 6;
        }
        updateButtons();
    }

    public void clickedNotes1Button() {
        StaticVariables.includeNotes1Button = !StaticVariables.includeNotes1Button;
        updateButtons();
    }
    public void clickedNotes2Button() {
        StaticVariables.includeNotes2Button = !StaticVariables.includeNotes2Button;
        updateButtons();
    }
    public void clickedCorrectResidentButton() {
        StaticVariables.changeResidentColorOnCorrectRows = !StaticVariables.changeResidentColorOnCorrectRows;
        updateButtons();
    }

    private void updateButtons() {
        //hugePuzzleButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.highestUnlockedSize == 6);
        redNoteButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeNotes1Button);
        greenNoteButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeNotes2Button);
        changeCorrectResidentColorButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.changeResidentColorOnCorrectRows);
        undoRedoButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeUndoRedo);


        showMedButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showMed);
        showLargeButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showLarge);
        showHugeButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showHuge);

    }

    public void clicked4Button() {
        if (!StaticVariables.showLarge && !StaticVariables.showHuge) {
            StaticVariables.showMed = !StaticVariables.showMed;
        }
        updateButtons();
    }
    public void clicked5Button() {
        if (!StaticVariables.showHuge && StaticVariables.showMed) {
            StaticVariables.showLarge = !StaticVariables.showLarge;
        }
        updateButtons();
    }
    public void clicked6Button() {
        if (StaticVariables.showLarge && StaticVariables.showMed) {
            StaticVariables.showHuge = !StaticVariables.showHuge;
        }
        updateButtons();
    }

    public void clickedUndoRedoButton() {
        StaticVariables.includeUndoRedo = !StaticVariables.includeUndoRedo;
        updateButtons();
    }
}