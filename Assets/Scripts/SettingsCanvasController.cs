using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsCanvasController : MonoBehaviour {
    
    public GameObject note1Button;
    public GameObject note2Button;
    public GameObject changeCorrectResidentColorButton;
    public GameObject undoRedoButton;

    public GameObject showMedButton;
    public GameObject showLargeButton;
    public GameObject showHugeButton;


    public GameObject blackForeground; //used to transition to/from the puzzle menu
    private SpriteRenderer blackSprite;
    public float fadeOutTime;
    public float fadeInTime;
    private float fadeTimer;


    private void Start() {
        updateButtons();


        blackSprite = blackForeground.GetComponent<SpriteRenderer>();

        if (StaticVariables.isFading && StaticVariables.fadingTo == "settings") {
            fadeTimer = fadeInTime;
        }

    }


    private void Update() {
        if (StaticVariables.isFading && StaticVariables.fadingFrom == "settings") {
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
        if (StaticVariables.isFading && StaticVariables.fadingTo == "settings") {
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
        //SceneManager.LoadScene("MainMenu");
        StaticVariables.fadingTo = "menu";
        startFadeOut();
    }

    public void startFadeOut() {
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "settings";
        //fadingOut = true;
    }



    public void clickedNote1Button() {
        StaticVariables.includeNotes1Button = !StaticVariables.includeNotes1Button;
        updateButtons();
    }
    public void clickedNote2Button() {
        StaticVariables.includeNotes2Button = !StaticVariables.includeNotes2Button;
        updateButtons();
    }
    public void clickedCorrectResidentButton() {
        StaticVariables.changeResidentColorOnCorrectRows = !StaticVariables.changeResidentColorOnCorrectRows;
        updateButtons();
    }

    private void updateButtons() {
        note1Button.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeNotes1Button);
        note2Button.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeNotes2Button);
        changeCorrectResidentColorButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.changeResidentColorOnCorrectRows);
        undoRedoButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeUndoRedo);


        showMedButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showMed);
        showLargeButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showLarge);
        showHugeButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showHuge);

        showMedButton.SetActive(StaticVariables.unlockedMedium);
        showLargeButton.SetActive(StaticVariables.unlockedLarge);
        showHugeButton.SetActive(StaticVariables.unlockedHuge);
        showMedButton.gameObject.transform.parent.gameObject.SetActive((StaticVariables.unlockedMedium || StaticVariables.unlockedLarge || StaticVariables.unlockedHuge));
        note1Button.SetActive(StaticVariables.unlockedNotes1);
        note2Button.SetActive(StaticVariables.unlockedNotes2);
        changeCorrectResidentColorButton.SetActive(StaticVariables.unlockedResidentsChangeColor);
        undoRedoButton.SetActive(StaticVariables.unlockedUndoRedo);

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
        StaticVariables.includeUndoRedo= !StaticVariables.includeUndoRedo;
        updateButtons();
    }
}