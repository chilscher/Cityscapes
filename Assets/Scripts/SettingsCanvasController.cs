using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject highlightBuildingsButton;

    public GameObject background;

    public GameObject menuButton;

    public GameObject hidePurchasedUpgradesButton;

    private bool expandedSkinButton = false;
    public GameObject expandSkinButton;

    public GameObject currentSkinText;

    private void Start() {
        showAndHideButtons();
        setCurrentToggleTexts();
        updateCurrentSkinText();
        showChooseSkinButton();
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
                if (StaticVariables.waitingOnButtonClickAfterFadeIn) {
                    StaticVariables.waitingOnButtonClickAfterFadeIn = false;
                    if (StaticVariables.buttonClickInWaiting.Contains("menu")) {
                        goToMainMenu();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            goToMainMenu();
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
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "menu";
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
        highlightBuildingsButton.SetActive(StaticVariables.unlockedHighlightBuildings);

        hidePurchasedUpgradesButton.SetActive(true);
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
        toggleText(highlightBuildingsButton, StaticVariables.includeHighlightBuildings);

        toggleText(hidePurchasedUpgradesButton, StaticVariables.hidePurchasedUpgrades);
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
        SaveSystem.SaveGame();
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
        SaveSystem.SaveGame();
    }

    public void pushHugeButton() {
        StaticVariables.showHuge = !StaticVariables.showHuge;
        if (StaticVariables.showHuge) {
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
        }
        setCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void pushNotes1Button() {
        StaticVariables.includeNotes1Button = !StaticVariables.includeNotes1Button;
        setCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void pushNotes2Button() {
        StaticVariables.includeNotes2Button = !StaticVariables.includeNotes2Button;
        setCurrentToggleTexts();
        SaveSystem.SaveGame();
    }
    
    public void pushResidentColorButton() {
        StaticVariables.changeResidentColorOnCorrectRows = !StaticVariables.changeResidentColorOnCorrectRows;
        setCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void pushUndoRedoButton() {
        StaticVariables.includeUndoRedo = !StaticVariables.includeUndoRedo;
        if (!StaticVariables.includeUndoRedo) {
            StaticVariables.includeRemoveAllOfNumber = false;
            StaticVariables.includeClearPuzzle = false;
        }
        setCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void pushRemoveNumbersButton() {
        StaticVariables.includeRemoveAllOfNumber = !StaticVariables.includeRemoveAllOfNumber;
        if (StaticVariables.includeRemoveAllOfNumber) {
            StaticVariables.includeUndoRedo = true;
        }
        setCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void pushClearPuzzleButton() {
        StaticVariables.includeClearPuzzle = !StaticVariables.includeClearPuzzle;
        if (StaticVariables.includeClearPuzzle) {
            StaticVariables.includeUndoRedo = true;
        }
        setCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void pushHighlightBuildingsButton() {
        StaticVariables.includeHighlightBuildings = !StaticVariables.includeHighlightBuildings;
        if (StaticVariables.includeHighlightBuildings) {
            StaticVariables.changeResidentColorOnCorrectRows = true;
        }
        setCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void pushCreditsButton() {
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "credits";
            startFadeOut();
        }
    }

    public void pushHidePurchasedUpgradesButton() {
        StaticVariables.hidePurchasedUpgrades = !StaticVariables.hidePurchasedUpgrades;
        setCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void setScrollViewHeight() {
        //sets the scroll viewer (vertical layout group) height to match its contents. minimum is the height of its parent scrollable container
        //to be called whenever an item is shown or hidden in the settings window

        //define the current top height - for use at the end of the function
        float topHeight = (float)Math.Round(((1 - scrollView.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition) * (scrollView.GetComponent<RectTransform>().sizeDelta.y - scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y)), 2);

        resizeToFitChildren(scrollView, true);

        //set the scroll view to be at the same position as previously
        scrollView.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1 - (topHeight / (scrollView.GetComponent<RectTransform>().sizeDelta.y - scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y));

    }

    public void resizeToFitChildren(GameObject parent, bool minSize) {
        //get height of contents, including spaces between objects and top and bottom padding
        //if minSize is true, the container height has to be at least the height of its parent
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

        //set the container height to be at least the height of the parent
        if (minSize && newHeight < parent.transform.parent.GetComponent<RectTransform>().sizeDelta.y) { newHeight = parent.transform.parent.GetComponent<RectTransform>().sizeDelta.y; }

        Vector2 newSize = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, newHeight);
        parent.GetComponent<RectTransform>().sizeDelta = newSize;

    }




    public void chooseSkin(Skin s) {
        StaticVariables.skin = s;
        loadSkin();

        SaveSystem.SaveGame();
    }
    

    private void loadSkin() {
        background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.shopBackground;
        InterfaceFunctions.colorMenuButton(menuButton);
    }

    public void clickedExpandSkinButton() {
        if (!expandedSkinButton) {
            expandSkinButtons();
            expandedSkinButton = true;
        }
        else {
            contractSkinButtons();
            expandedSkinButton = false;
        }

        
    }

    private void expandSkinButtons() {
        GameObject parentBox = expandSkinButton.transform.parent.gameObject;
        parentBox.transform.Find("Basic").gameObject.SetActive(true);
        for (int i = 2; i < parentBox.transform.childCount; i++) {
            bool switchTo = StaticVariables.unlockedSkins.Contains(InterfaceFunctions.getSkinFromName(parentBox.transform.GetChild(i).name));
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
            parentBox.transform.GetChild(i).Find("Button").Find("Text").GetComponent<Text>().text = parentBox.transform.GetChild(i).name.ToUpper() + " SKIN";
        }
        resizeToFitChildren(parentBox, false);
        setScrollViewHeight();
    }

    private void contractSkinButtons() {
        GameObject parentBox = expandSkinButton.transform.parent.gameObject;
        //parentBox.transform.Find("Basic").gameObject.SetActive(true);
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            bool switchTo = false;
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
        }
        resizeToFitChildren(parentBox, false);
        setScrollViewHeight();
    }

    public void clickedSkinButton(GameObject button) {
        chooseSkin(InterfaceFunctions.getSkinFromName(button.transform.parent.gameObject.name));
        updateCurrentSkinText();
    }

    private void updateCurrentSkinText() {
        currentSkinText.GetComponent<Text>().text = "CURRENT SKIN:\n" + StaticVariables.skin.skinName.ToUpper();


        GameObject parentBox = expandSkinButton.transform.parent.gameObject;
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            bool isActive = (InterfaceFunctions.getSkinFromName(parentBox.transform.GetChild(i).name) == StaticVariables.skin);
            parentBox.transform.GetChild(i).Find("Button").Find("On").gameObject.SetActive(isActive);
            parentBox.transform.GetChild(i).Find("Button").Find("Off").gameObject.SetActive(!isActive);
        }
    }

    private void showChooseSkinButton() {
        expandSkinButton.transform.parent.gameObject.SetActive(StaticVariables.unlockedSkins.Count >= 1);
    }


}