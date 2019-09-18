using System.Collections;
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

    public GameObject notes1Button;
    public GameObject notes2Button;
    public GameObject changeCorrectResidentColorButton;
    public GameObject undoRedoButton;

    //public GameObject showMedButton;
    //public GameObject showLargeButton;
    public GameObject unlockMedButton;
    public GameObject unlockLargeButton;
    //public GameObject showHugeButton;
    public GameObject unlockHugeButton;


    public GameObject blackForeground; //used to transition to/from the puzzle menu
    private SpriteRenderer blackSprite;
    public float fadeOutTime;
    public float fadeInTime;
    private float fadeTimer;

    private void Start() {
        sprite1s = coinsBox1s.GetComponent<SpriteRenderer>();
        sprite10s = coinsBox10s.GetComponent<SpriteRenderer>();
        sprite100s = coinsBox100s.GetComponent<SpriteRenderer>();
        sprite1000s = coinsBox1000s.GetComponent<SpriteRenderer>();
        sprite10000s = coinsBox10000s.GetComponent<SpriteRenderer>();

        displayCoinsAmount();
        updateButtons();


        blackSprite = blackForeground.GetComponent<SpriteRenderer>();

        if (StaticVariables.isFading && StaticVariables.fadingTo == "shop") {
            fadeTimer = fadeInTime;
        }

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

    /*
    public void clickedHugePuzzle() {
        if (StaticVariables.highestUnlockedSize == 6) {
            StaticVariables.highestUnlockedSize = 5;
        }
        else {
            StaticVariables.highestUnlockedSize = 6;
        }
        updateButtons();
    }
    */
    /*
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
    */

    private void updateButtons() {
        /*
        //hugePuzzleButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.highestUnlockedSize == 6);
        redNoteButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeNotes1Button);
        greenNoteButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeNotes2Button);
        changeCorrectResidentColorButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.changeResidentColorOnCorrectRows);
        undoRedoButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeUndoRedo);


        //showMedButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showMed);
        showLargeButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showLarge);
        showHugeButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showHuge);

        */


        if (StaticVariables.unlockedMedium) { unlockMedButton.transform.GetChild(0).GetComponent<Text>().color = Color.grey; }
        else { unlockMedButton.transform.GetChild(0).GetComponent<Text>().color = Color.black; }
        if (StaticVariables.unlockedLarge) { unlockLargeButton.transform.GetChild(0).GetComponent<Text>().color = Color.grey; }
        else { unlockLargeButton.transform.GetChild(0).GetComponent<Text>().color = Color.black; }
        if (StaticVariables.unlockedHuge) { unlockHugeButton.transform.GetChild(0).GetComponent<Text>().color = Color.grey; }
        else { unlockHugeButton.transform.GetChild(0).GetComponent<Text>().color = Color.black; }
        if (StaticVariables.unlockedNotes1) { notes1Button.transform.GetChild(0).GetComponent<Text>().color = Color.grey; }
        else { notes1Button.transform.GetChild(0).GetComponent<Text>().color = Color.black; }
        if (StaticVariables.unlockedNotes2) { notes2Button.transform.GetChild(0).GetComponent<Text>().color = Color.grey; }
        else { notes2Button.transform.GetChild(0).GetComponent<Text>().color = Color.black; }
        if (StaticVariables.unlockedResidentsChangeColor) { changeCorrectResidentColorButton.transform.GetChild(0).GetComponent<Text>().color = Color.grey; }
        else { changeCorrectResidentColorButton.transform.GetChild(0).GetComponent<Text>().color = Color.black; }
        if (StaticVariables.unlockedUndoRedo) { undoRedoButton.transform.GetChild(0).GetComponent<Text>().color = Color.grey; }
        else { undoRedoButton.transform.GetChild(0).GetComponent<Text>().color = Color.black; }

    }
    /*
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
    */

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
        StaticVariables.includeNotes1Button = false;
        StaticVariables.includeNotes2Button = false;
        StaticVariables.changeResidentColorOnCorrectRows = false;
        StaticVariables.includeUndoRedo = false;

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


}