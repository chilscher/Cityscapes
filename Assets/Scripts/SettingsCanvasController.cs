using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SettingsCanvasController : MonoBehaviour {
    
    public GameObject note1Button;
    public GameObject note2Button;
    public GameObject changeCorrectResidentColorButton;
    public GameObject undoRedoButton;
    //public GameObject removeColoredNumberNotes;
    public GameObject removeAllOfNumber;
    public GameObject clearPuzzle;

    public GameObject showMedButton;
    public GameObject showLargeButton;
    public GameObject showHugeButton;

    public GameObject creditsButton;
    public GameObject creditsText;

    public GameObject scrollView;


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

        creditsButton.transform.GetChild(1).GetComponent<Text>().text = "SHOW CREDITS";
        creditsText.SetActive(false);


        setScrollViewHeight();
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
        //setScrollViewHeight();
    }


    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void goToMainMenu() {
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "menu";
            startFadeOut();
        }
        //SceneManager.LoadScene("MainMenu");
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
        //removeColoredNumberNotes.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeRemoveColoredNotesOfChosenNumber);
        removeAllOfNumber.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeRemoveAllOfNumber);
        clearPuzzle.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeClearPuzzle);


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
        //removeColoredNumberNotes.SetActive(StaticVariables.unlockedRemoveColoredNotesOfChosenNumber);
        removeAllOfNumber.SetActive(StaticVariables.unlockedRemoveAllOfNumber);
        clearPuzzle.SetActive(StaticVariables.unlockedClearPuzzle);

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
    /*
    public void clickedRemoveColoredNumberNotesButton() {
        StaticVariables.includeRemoveColoredNotesOfChosenNumber = !StaticVariables.includeRemoveColoredNotesOfChosenNumber;
        updateButtons();
    }
    */

    public void clickedRemoveAllOfNumberButton() {
        StaticVariables.includeRemoveAllOfNumber = !StaticVariables.includeRemoveAllOfNumber;
        updateButtons();
    }

    public void clickedClearPuzzleButton() {
        StaticVariables.includeClearPuzzle = !StaticVariables.includeClearPuzzle;
        updateButtons();
    }

    public void clickedCreditsButton() {
        if (creditsText.activeSelf) {
            creditsButton.transform.GetChild(1).GetComponent<Text>().text = "SHOW CREDITS";
            creditsText.SetActive(false);
        }
        else {
            creditsButton.transform.GetChild(1).GetComponent<Text>().text = "HIDE CREDITS";
            creditsText.SetActive(true);
        }
        setScrollViewHeight();
    }
    
    public void setScrollViewHeight() {
        //sets the scroll viewer (vertical layout group) height to match its contents. minimum is the height of its parent scrollable container
        //to be called whenever an item is shown or hidden in the settings window

        //define the current top height - for use at the end of the function
        float topHeight = (float)Math.Round(((1 - scrollView.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition) * (scrollView.GetComponent<RectTransform>().sizeDelta.y - scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y)), 2);
        
        //get height of contents, including spaces between objects and top and bottom padding
        float newHeight = 0f;
        int activeCount = 0;
        for (int i = 0; i < scrollView.transform.childCount; i++){
            float h = scrollView.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
            if (scrollView.transform.GetChild(i).gameObject.activeSelf) {
                newHeight += h;
                activeCount++;
            }
        }
        newHeight += ((activeCount - 1) * scrollView.GetComponent<VerticalLayoutGroup>().spacing);
        newHeight += scrollView.GetComponent<VerticalLayoutGroup>().padding.top + scrollView.GetComponent<VerticalLayoutGroup>().padding.bottom;

        //if lower than size of parent scrollable container, set it to that value
        if (newHeight < scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y) {
            newHeight = scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        }

        //apply the new height
        Vector2 newSize = new Vector2(scrollView.GetComponent<RectTransform>().sizeDelta.x, newHeight);
        scrollView.GetComponent<RectTransform>().sizeDelta = newSize;

        //set the scroll view to be at the same position as previously
        scrollView.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1 - (topHeight / (scrollView.GetComponent<RectTransform>().sizeDelta.y - scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y));


    }
    
}