using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SettingsCanvasController : MonoBehaviour {
    
    //public GameObject changeCorrectResidentColorButton;
    //public GameObject undoRedoButton;
    //public GameObject removeAllOfNumber;
    //public GameObject clearPuzzle;

    //public GameObject creditsButton;
    //public GameObject creditsText;

    public GameObject scrollView;


    public GameObject blackForeground; //used to transition to/from the puzzle menu
    private SpriteRenderer blackSprite;
    public float fadeOutTime;
    public float fadeInTime;
    private float fadeTimer;

    public GameObject medCityButton;
    public GameObject largeCityButton;
    public GameObject hugeCityButton;
    public GameObject notes1Button;
    public GameObject notes2Button;
    public GameObject residentColorButton;
    public GameObject undoRedoButton;
    public GameObject removeNumbersButton;
    public GameObject clearPuzzleButton;

    public GameObject background;

    public GameObject menuButton;
    


    private void Start() {
        showAndHideButtons();
        setCurrentToggleTexts();

        //background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.shopBackground;
        blackSprite = blackForeground.GetComponent<SpriteRenderer>();
        //InterfaceFunctions.colorMenuButton(menuButton);
        loadSkin();

        if (StaticVariables.isFading && StaticVariables.fadingTo == "settings") {
            fadeTimer = fadeInTime;
        }
        /*
        creditsButton.transform.Find("Button").Find("Show").gameObject.SetActive(true);
        creditsButton.transform.Find("Button").Find("Hide").gameObject.SetActive(false);
        creditsText.SetActive(false);
        */

        setScrollViewHeight();
    }
    
    private void Update() {
        if (StaticVariables.isFading && StaticVariables.fadingFrom == "settings") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                if (StaticVariables.fadingTo == "menu") { SceneManager.LoadScene("MainMenu"); }
                if (StaticVariables.fadingTo == "credits") { SceneManager.LoadScene("Credits"); }
            }
        }
        if (StaticVariables.isFading && StaticVariables.fadingTo == "settings") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeTimer) / fadeInTime;
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
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "menu";
            startFadeOut();
        }
    }

    public void startFadeOut() {
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "settings";
    }

    public void showAndHideButtons() {
        medCityButton.SetActive(StaticVariables.unlockedMedium);
        largeCityButton.SetActive(StaticVariables.unlockedLarge);
        hugeCityButton.SetActive(StaticVariables.unlockedHuge);
        notes1Button.SetActive(StaticVariables.unlockedNotes1);
        notes2Button.SetActive(StaticVariables.unlockedNotes2);
        residentColorButton.SetActive(StaticVariables.unlockedResidentsChangeColor);
        undoRedoButton.SetActive(StaticVariables.unlockedUndoRedo);
        removeNumbersButton.SetActive(StaticVariables.unlockedRemoveAllOfNumber);
        clearPuzzleButton.SetActive(StaticVariables.unlockedClearPuzzle);

    }

    public void setCurrentToggleTexts() {
        toggleText(medCityButton, StaticVariables.showMed);
        toggleText(largeCityButton, StaticVariables.showLarge);
        toggleText(hugeCityButton, StaticVariables.showHuge);
        toggleText(notes1Button, StaticVariables.includeNotes1Button);
        toggleText(notes2Button, StaticVariables.includeNotes2Button);
        toggleText(residentColorButton, StaticVariables.changeResidentColorOnCorrectRows);
        toggleText(undoRedoButton, StaticVariables.includeUndoRedo);
        toggleText(removeNumbersButton, StaticVariables.includeRemoveAllOfNumber);
        toggleText(clearPuzzleButton, StaticVariables.includeClearPuzzle);
    }

    public void toggleText(GameObject button, bool cond) {
        button.transform.Find("Button").Find("On").gameObject.SetActive(cond);
        button.transform.Find("Button").Find("Off").gameObject.SetActive(!cond);
    }

    public void pushMedButton() {
        StaticVariables.showMed = !StaticVariables.showMed;
        if (!StaticVariables.showMed) {
            StaticVariables.showLarge = false;
            StaticVariables.showHuge = false;
        }
        setCurrentToggleTexts();
    }

    public void pushLargeButton() {
        StaticVariables.showLarge = !StaticVariables.showLarge;
        if (StaticVariables.showLarge) {
            StaticVariables.showMed = true;
        }
        else {
            StaticVariables.showHuge = false;
        }
        setCurrentToggleTexts();
    }

    public void pushHugeButton() {
        StaticVariables.showHuge = !StaticVariables.showHuge;
        if (StaticVariables.showHuge) {
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
        }
        setCurrentToggleTexts();
    }

    public void pushNotes1Button() {
        StaticVariables.includeNotes1Button = !StaticVariables.includeNotes1Button;
        setCurrentToggleTexts();
    }

    public void pushNotes2Button() {
        StaticVariables.includeNotes2Button = !StaticVariables.includeNotes2Button;
        setCurrentToggleTexts();
    }
    
    public void pushResidentColorButton() {
        StaticVariables.changeResidentColorOnCorrectRows = !StaticVariables.changeResidentColorOnCorrectRows;
        setCurrentToggleTexts();
    }

    public void pushUndoRedoButton() {
        StaticVariables.includeUndoRedo = !StaticVariables.includeUndoRedo;
        if (!StaticVariables.includeUndoRedo) {
            StaticVariables.includeRemoveAllOfNumber = false;
            StaticVariables.includeClearPuzzle = false;
        }
        setCurrentToggleTexts();
    }

    public void pushRemoveNumbersButton() {
        StaticVariables.includeRemoveAllOfNumber = !StaticVariables.includeRemoveAllOfNumber;
        if (StaticVariables.includeRemoveAllOfNumber) {
            StaticVariables.includeUndoRedo = true;
        }
        setCurrentToggleTexts();
    }

    public void pushClearPuzzleButton() {
        StaticVariables.includeClearPuzzle = !StaticVariables.includeClearPuzzle;
        if (StaticVariables.includeClearPuzzle) {
            StaticVariables.includeUndoRedo = true;
        }
        setCurrentToggleTexts();
    }

    public void pushCreditsButton() {
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "credits";
            startFadeOut();
        }
        /*
        if (creditsText.activeSelf) {
            creditsButton.transform.Find("Button").Find("Show").gameObject.SetActive(true);
            creditsButton.transform.Find("Button").Find("Hide").gameObject.SetActive(false);
            creditsText.SetActive(false);
        }
        else {
            creditsButton.transform.Find("Button").Find("Show").gameObject.SetActive(false);
            creditsButton.transform.Find("Button").Find("Hide").gameObject.SetActive(true);
            creditsText.SetActive(true);
        }
        setScrollViewHeight();
        */
    }
    /*
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
    */
    public void setScrollViewHeight() {
        //sets the scroll viewer (vertical layout group) height to match its contents. minimum is the height of its parent scrollable container
        //to be called whenever an item is shown or hidden in the settings window

        //define the current top height - for use at the end of the function
        float topHeight = (float)Math.Round(((1 - scrollView.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition) * (scrollView.GetComponent<RectTransform>().sizeDelta.y - scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y)), 2);

        resizeToFitChildren(scrollView);

        //set the scroll view to be at the same position as previously
        scrollView.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1 - (topHeight / (scrollView.GetComponent<RectTransform>().sizeDelta.y - scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y));

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




    public void chooseSkin(Skin s) {

        StaticVariables.skin = s;
        loadSkin();
    }
    

    private void loadSkin() {
        background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.shopBackground;
        InterfaceFunctions.colorMenuButton(menuButton);
    }

}