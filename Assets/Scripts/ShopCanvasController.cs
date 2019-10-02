﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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

    public GameObject notes1Button;
    public GameObject notes2Button;
    public GameObject changeCorrectResidentColorButton;
    public GameObject undoRedoButton;

    //public GameObject unlockMedButton;
    public GameObject expandMedButton;
    public GameObject expandLargeButton;
    public GameObject unlockHugeButton;


    public GameObject blackForeground; //used to transition to/from the puzzle menu
    private SpriteRenderer blackSprite;
    public float fadeOutTime;
    public float fadeInTime;
    private float fadeTimer;
    
    public GameObject unlockRemoveAllOfNumberButton;
    public GameObject unlockClearPuzzleButton;

    public GameObject scrollView;

    public int medCityPrice = 10;
    public int largeCityPrice = 40;

    public string affordableCoinColor;
    public string unaffordableCoinColor;
    private Color affordableColor;
    private Color unaffordableColor;

    public Sprite purchasedUpgradeButton;
    public Sprite unpurchasedUpgradeButton;

    private void Start() {
        ColorUtility.TryParseHtmlString(affordableCoinColor, out affordableColor);
        ColorUtility.TryParseHtmlString(unaffordableCoinColor, out unaffordableColor);
        sprite1s = coinsBox1s.GetComponent<SpriteRenderer>();
        sprite10s = coinsBox10s.GetComponent<SpriteRenderer>();
        sprite100s = coinsBox100s.GetComponent<SpriteRenderer>();
        sprite1000s = coinsBox1000s.GetComponent<SpriteRenderer>();
        sprite10000s = coinsBox10000s.GetComponent<SpriteRenderer>();

        displayCoinsAmount();

        displayCoinsOnButton(expandMedButton, medCityPrice);
        displayCoinsOnButton(expandLargeButton, largeCityPrice);

        updateButtons();

        blackSprite = blackForeground.GetComponent<SpriteRenderer>();

        if (StaticVariables.isFading && StaticVariables.fadingTo == "shop") {
            fadeTimer = fadeInTime;
        }

        setScrollViewHeight();

    }

    private void Update() {
        if (StaticVariables.isFading && StaticVariables.fadingFrom == "shop") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                //StaticVariables.isFading = false;
                //if (StaticVariables.isTutorial) { SceneManager.LoadScene("InPuzzle"); }
                if (StaticVariables.fadingTo == "menu") { SceneManager.LoadScene("MainMenu"); }
            }
        }
        if (StaticVariables.isFading && StaticVariables.fadingTo == "shop") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeTimer) / fadeInTime;
            //print(c.a);
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                StaticVariables.isFading = false;
            }
        }
    }


    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void goToMainMenu() {

        StaticVariables.fadingTo = "menu";
        startFadeOut();
        //SceneManager.LoadScene("MainMenu");
    }

    public void startFadeOut() {
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "shop";
        //fadingOut = true;
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
    

    private void updateButtons() {

        Color black = Color.black;
        ColorUtility.TryParseHtmlString("#FFEB42", out black);
        ColorUtility.TryParseHtmlString("#3A3A3A", out black);

        Color grey = Color.grey;
        updateButton(expandMedButton, StaticVariables.unlockedMedium, medCityPrice);
        updateButton(expandLargeButton, StaticVariables.unlockedLarge, largeCityPrice);


        if (StaticVariables.unlockedHuge) { unlockHugeButton.transform.GetChild(0).GetComponent<Text>().color = grey; }
        else { unlockHugeButton.transform.GetChild(0).GetComponent<Text>().color = black; }
        if (StaticVariables.unlockedNotes1) { notes1Button.transform.GetChild(0).GetComponent<Text>().color = grey; }
        else { notes1Button.transform.GetChild(0).GetComponent<Text>().color = black; }
        if (StaticVariables.unlockedNotes2) { notes2Button.transform.GetChild(0).GetComponent<Text>().color = grey; }
        else { notes2Button.transform.GetChild(0).GetComponent<Text>().color = black; }
        if (StaticVariables.unlockedResidentsChangeColor) { changeCorrectResidentColorButton.transform.GetChild(0).GetComponent<Text>().color = grey; }
        else { changeCorrectResidentColorButton.transform.GetChild(0).GetComponent<Text>().color = black; }
        if (StaticVariables.unlockedUndoRedo) { undoRedoButton.transform.GetChild(0).GetComponent<Text>().color = grey; }
        else { undoRedoButton.transform.GetChild(0).GetComponent<Text>().color = black; }

        if (StaticVariables.unlockedRemoveAllOfNumber) { unlockRemoveAllOfNumberButton.transform.GetChild(0).GetComponent<Text>().color = grey; }
        else { unlockRemoveAllOfNumberButton.transform.GetChild(0).GetComponent<Text>().color = black; }
        if (StaticVariables.unlockedClearPuzzle) { unlockClearPuzzleButton.transform.GetChild(0).GetComponent<Text>().color = grey; }
        else { unlockClearPuzzleButton.transform.GetChild(0).GetComponent<Text>().color = black; }

    }

    private void updateButton(GameObject button, bool condition, int cost) {
        //shows if the item has already been purchased or not, and also sets the coin amount to the appropriate color
        if (condition) {
            button.GetComponent<Image>().sprite = purchasedUpgradeButton;
            //hide coins
            button.transform.GetChild(1).gameObject.SetActive(false);
            //show check
            button.transform.GetChild(2).gameObject.SetActive(true);
        }
        else {
            button.GetComponent<Image>().sprite = unpurchasedUpgradeButton;
            //show coins
            button.transform.GetChild(1).gameObject.SetActive(true);
            //hide check
            button.transform.GetChild(2).gameObject.SetActive(false);
        }

        GameObject imageObject1s = button.transform.GetChild(1).GetChild(1).gameObject;
        GameObject imageObject10s = button.transform.GetChild(1).GetChild(2).gameObject;
        GameObject imageObject100s = button.transform.GetChild(1).GetChild(3).gameObject;
        GameObject imageObject1000s = button.transform.GetChild(1).GetChild(4).gameObject;

        if (cost <= StaticVariables.coins) {
            imageObject1s.GetComponent<Image>().color = affordableColor;
            imageObject10s.GetComponent<Image>().color = affordableColor;
            imageObject100s.GetComponent<Image>().color = affordableColor;
            imageObject1000s.GetComponent<Image>().color = affordableColor;
        }
        else {
            imageObject1s.GetComponent<Image>().color = unaffordableColor;
            imageObject10s.GetComponent<Image>().color = unaffordableColor;
            imageObject100s.GetComponent<Image>().color = unaffordableColor;
            imageObject1000s.GetComponent<Image>().color = unaffordableColor;
        }

    }

    public void clearPurchases() {
        //some kind of popup to confirm that the player wants to clear their purchases? 
        //"are you sure you want to reset all of your purchases? you will NOT get your spent coins back!!!"
        //if so, then do the following...

        StaticVariables.unlockedMedium = false;
        StaticVariables.unlockedLarge = false;
        StaticVariables.unlockedHuge = false;
        StaticVariables.highestUnlockedSize = 3;
        StaticVariables.showMed = false;
        StaticVariables.showLarge = false;
        StaticVariables.showHuge = false;

        StaticVariables.unlockedNotes1 = false;
        StaticVariables.unlockedNotes2 = false;
        StaticVariables.unlockedResidentsChangeColor = false;
        StaticVariables.unlockedUndoRedo = false;
        StaticVariables.unlockedRemoveAllOfNumber = false;
        StaticVariables.unlockedClearPuzzle = false;

        StaticVariables.includeNotes1Button = false;
        StaticVariables.includeNotes2Button = false;
        StaticVariables.changeResidentColorOnCorrectRows = false;
        StaticVariables.includeUndoRedo = false;
        StaticVariables.includeRemoveAllOfNumber = false;
        StaticVariables.includeClearPuzzle = false;

        updateButtons();
    }

    public void unlockMedium() {
        if (!StaticVariables.unlockedMedium) {

            StaticVariables.unlockedMedium = true;
            StaticVariables.highestUnlockedSize = 4;
            StaticVariables.showMed = true;

            updateButtons();
        }
    }
    
    public void unlockLarge() {
        if (!StaticVariables.unlockedLarge && StaticVariables.unlockedMedium) {

            StaticVariables.unlockedLarge = true;
            StaticVariables.highestUnlockedSize = 5;
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;

            updateButtons();
        }


    }

    public void pushMedButton() {
        expandSiblings(expandMedButton);
    }
    
    public void pushLargeButton() {
        expandSiblings(expandLargeButton);
    }

    
    

    public void unlockHuge() {
        if (!StaticVariables.unlockedHuge && StaticVariables.unlockedLarge && StaticVariables.unlockedMedium) {

            StaticVariables.unlockedHuge = true;
            StaticVariables.highestUnlockedSize = 6;
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
            StaticVariables.showHuge = true;

            updateButtons();
        }
    }

    public void unlockNotes1() {
        if (!StaticVariables.unlockedNotes1) {

            StaticVariables.unlockedNotes1 = true;
            StaticVariables.includeNotes1Button = true;

            updateButtons();
        }
    }
    public void unlockNotes2() {
        if (!StaticVariables.unlockedNotes2) {

            StaticVariables.unlockedNotes2 = true;
            StaticVariables.includeNotes2Button = true;

            updateButtons();
        }
    }
    public void unlockResidentsChangeColor() {
        if (!StaticVariables.unlockedResidentsChangeColor) {

            StaticVariables.unlockedResidentsChangeColor = true;
            StaticVariables.changeResidentColorOnCorrectRows = true;

            updateButtons();
        }
    }
    public void unlockUndoRedo() {
        if (!StaticVariables.unlockedUndoRedo) {

            StaticVariables.unlockedUndoRedo = true;
            StaticVariables.includeUndoRedo = true;

            updateButtons();
        }
    }
    public void unlockRemoveAllOfNumber() {
        if (!StaticVariables.unlockedRemoveAllOfNumber) {
            StaticVariables.unlockedRemoveAllOfNumber = true;
            StaticVariables.includeRemoveAllOfNumber = true;

            updateButtons();
        }
    }

    public void unlockClearPuzzle() {
        if (!StaticVariables.unlockedClearPuzzle) {
            StaticVariables.unlockedClearPuzzle = true;
            StaticVariables.includeClearPuzzle = true;

            updateButtons();
        }
    }



    public void expandSiblings(GameObject button) {
        //sets all siblings of the chosen button to be active, and resizes all necessary scroll views
        GameObject parentBox = button.transform.parent.gameObject;
        bool switchTo = !parentBox.transform.GetChild(1).gameObject.activeSelf;

        for (int i = 1; i < parentBox.transform.childCount; i++) {
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
        }
        resizeToFitChildren(parentBox);
        setScrollViewHeight();
    }

    public void resizeToFitChildren(GameObject parent) {
        //get height of contents, including spaces between objects and top and bottom padding
        float newHeight = 0f;
        int activeCount = 0;
        for (int i = 0; i < parent.transform.childCount; i++) {
            float h = parent.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
            if (parent.transform.GetChild(i).gameObject.activeSelf) {
                newHeight += h;
                activeCount++;
            }
        }
        newHeight += ((activeCount - 1) * parent.GetComponent<VerticalLayoutGroup>().spacing);
        newHeight += parent.GetComponent<VerticalLayoutGroup>().padding.top + parent.GetComponent<VerticalLayoutGroup>().padding.bottom;

        Vector2 newSize = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, newHeight);
        parent.GetComponent<RectTransform>().sizeDelta = newSize;

    }

    public void setScrollViewHeight() {
        //sets the scroll viewer (vertical layout group) height to match its contents. minimum is the height of its parent scrollable container
        //to be called whenever an item is shown or hidden in the settings window

        //define the current top height - for use at the end of the function
        float topHeight = (float)Math.Round(((1 - scrollView.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition) * (scrollView.GetComponent<RectTransform>().sizeDelta.y - scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y)), 2);

        resizeToFitChildren(scrollView);

        //set the scroll view to be at the same position as previously
        scrollView.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1 - (topHeight / (scrollView.GetComponent<RectTransform>().sizeDelta.y - scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y));

    }

    public void displayCoinsOnButton(GameObject button, int cost) {
        //assumes coins are all image components, with the ones place being child(1), all the way through thousands place being child(4).
        //also colors the coin amount depending on if the player can afford them.

        GameObject imageObject1s = button.transform.GetChild(1).GetChild(1).gameObject;
        GameObject imageObject10s = button.transform.GetChild(1).GetChild(2).gameObject;
        GameObject imageObject100s = button.transform.GetChild(1).GetChild(3).gameObject;
        GameObject imageObject1000s = button.transform.GetChild(1).GetChild(4).gameObject;

        int value1s = cost % 10;
        int value10s = (cost / 10) % 10;
        int value100s = (cost / 100) % 10;
        int value1000s = (cost / 1000) % 10;
        imageObject1s.GetComponent<Image>().sprite = numbers[value1s];
        imageObject10s.GetComponent<Image>().sprite = numbers[value10s];
        imageObject100s.GetComponent<Image>().sprite = numbers[value100s];
        imageObject1000s.GetComponent<Image>().sprite = numbers[value1000s];
        
        if (value1000s == 0) {
            imageObject1000s.SetActive(false);
            if (value100s == 0) {
                imageObject100s.SetActive(false);
                if (value10s == 0) {
                    imageObject10s.SetActive(false);
                }
            }
        }

        if (cost <= StaticVariables.coins) {
            imageObject1s.GetComponent<Image>().color = affordableColor;
            imageObject10s.GetComponent<Image>().color = affordableColor;
            imageObject100s.GetComponent<Image>().color = affordableColor;
            imageObject1000s.GetComponent<Image>().color = affordableColor;
        }
        else {
            imageObject1s.GetComponent<Image>().color = unaffordableColor;
            imageObject10s.GetComponent<Image>().color = unaffordableColor;
            imageObject100s.GetComponent<Image>().color = unaffordableColor;
            imageObject1000s.GetComponent<Image>().color = unaffordableColor;
        }
    }

}