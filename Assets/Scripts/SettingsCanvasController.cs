//for Cityscapes, copyright Cole Hilscher 2020

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCanvasController : MonoBehaviour {
    
    public GameObject blackForeground; //used to transition to/from the puzzle menu
    private Image blackSprite; //derived from the blackForeground gameObject
    public float fadeOutTime; //total time for fade-out, from complete light to complete darkness
    public float fadeInTime; //total time for fade-in, from complete darkness to complete light
    private float fadeTimer; //the timer on which the fadeout and fadein mechanics operate
    public GameObject scrollView; //the gameobject that is used to hide all buttons and text outside of the scrollable shop window

    //the toggle buttons for the different upgrades
    public GameObject medCityButton;
    public GameObject largeCityButton;
    public GameObject hugeCityButton;
    public GameObject massiveCityButton;
    public GameObject notes1Button;
    public GameObject notes2Button;
    public GameObject residentColorButton;
    public GameObject undoRedoButton;
    public GameObject removeNumbersButton;
    public GameObject clearPuzzleButton;
    public GameObject highlightBuildingsButton;

    //the following deal with switching between different skins
    private bool expandedSkinButton = false;
    public GameObject expandSkinButton;
    public GameObject currentSkinText;

    //the buttons that are not related to any upgrades
    public GameObject hidePurchasedUpgradesButton;

    //the following are properties taken from the skin
    public GameObject background;
    public GameObject menuButton;
    
    //headers for the different types of buttons you can interact with in the setting scene
    public GameObject cosmeticsTitle;
    public GameObject visualTitle;
    public GameObject buttonsTitle;
    public GameObject citiesTitle;
    public GameObject othersTitle;

    private void Start() {
        //show the buttons and headers based on what upgrades you have purchased
        //update the visuals for the buttons based on what is toggled on or off at the current time
        showAndHideButtons();
        showAndHideText();
        setCurrentToggleTexts();
        updateCurrentSkinText();
        showChooseSkinButton();
        blackSprite = blackForeground.GetComponent<Image>();
        //also apply the current skin
        loadSkin();

        //starts the fade-in process, which is carried out in the Update function
        if (StaticVariables.isFading && StaticVariables.fadingTo == "settings") {
            fadeTimer = fadeInTime;
        }
    }
    
    private void Update() {
        //if the player is fading from the settings scene to another scene, this block handles that fading process
        if (StaticVariables.isFading && StaticVariables.fadingFrom == "settings") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) { //after the fade timer is done, and the screen is dark, transition to another scene.
                if (StaticVariables.fadingTo == "menu") { SceneManager.LoadScene("MainMenu"); }
                if (StaticVariables.fadingTo == "credits") { SceneManager.LoadScene("Credits"); }
            }
        }
        //if the player is fading into the shop scene, this block handles that fading process
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
        //if the player presses their phone's back button, call the goToMainMenu function
        //identical to if the player just pushed the on-screen menu button
        if (Input.GetKeyDown(KeyCode.Escape)) {
            goToMainMenu();
        }
    }
    
    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO LEAVE THE SETTINGS SCENE
    // ---------------------------------------------------
    
    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void goToMainMenu() {
        //pushing the on-screen menu button calls this function, which returns the player to the main menu
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "menu";
            startFadeOut();
        }
        //if the player pushes the menu button while the scene is still fading in, then implement the button press after the fade-in is done
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "menu";
        }
    }

    public void startFadeOut() {
        //begin the fade-out process. This function is called by goToMainMenu
        //the fade-out mechanics of darkening the screen are implemented in the Update function
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "settings";
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO CHANGE WHAT THE BUTTONS, TEXT, AND COLORS OF THE SETTINGS MENU LOOK LIKE, EXCEPT FOR THE ONES THAT DEAL WITH SKIN SELECTION
    // ---------------------------------------------------
    
    public void showAndHideButtons() {
        //shows all of the upgrades that the player has purchased, except for skins
        medCityButton.SetActive(StaticVariables.unlockedMedium);
        largeCityButton.SetActive(StaticVariables.unlockedLarge);
        hugeCityButton.SetActive(StaticVariables.unlockedHuge);
        massiveCityButton.SetActive(StaticVariables.unlockedMassive);
        notes1Button.SetActive(StaticVariables.unlockedNotes1);
        notes2Button.SetActive(StaticVariables.unlockedNotes2);
        residentColorButton.SetActive(StaticVariables.unlockedResidentsChangeColor);
        undoRedoButton.SetActive(StaticVariables.unlockedUndoRedo);
        removeNumbersButton.SetActive(StaticVariables.unlockedRemoveAllOfNumber);
        clearPuzzleButton.SetActive(StaticVariables.unlockedClearPuzzle);
        highlightBuildingsButton.SetActive(StaticVariables.unlockedHighlightBuildings);

        hidePurchasedUpgradesButton.SetActive(true);
        
    }

    public void showAndHideText() {
        //shows any header text if the player has any of the relevant unlocks purchased
        bool anyCosmetics = StaticVariables.unlockedSkins.Count > 0;
        bool anyVisuals = residentColorButton.activeSelf || highlightBuildingsButton.activeSelf;
        bool anyButtons = notes1Button.activeSelf || notes2Button.activeSelf || undoRedoButton.activeSelf || removeNumbersButton.activeSelf || clearPuzzleButton.activeSelf;
        bool anyCities = medCityButton.activeSelf || largeCityButton.activeSelf || hugeCityButton.activeSelf || massiveCityButton.activeSelf;
        bool anyUpgrades = anyCosmetics || anyVisuals || anyButtons || anyCities;

        cosmeticsTitle.SetActive(anyCosmetics);
        visualTitle.SetActive(anyVisuals);
        buttonsTitle.SetActive(anyButtons);
        citiesTitle.SetActive(anyCities);
        othersTitle.SetActive(anyUpgrades);
}

    public void setCurrentToggleTexts() {
        //changes the text and text-color of the buttons that toggle settings
        //ex: the MedCityButton will either have CURRENTLY: ON in green, or CURRENTLY: OFF in red displayed
        toggleText(medCityButton, StaticVariables.showMed);
        toggleText(largeCityButton, StaticVariables.showLarge);
        toggleText(hugeCityButton, StaticVariables.showHuge);
        toggleText(massiveCityButton, StaticVariables.showMassive);
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
        //changes the toggle text on an invidual button to one of two pre-defined layouts
        button.transform.Find("Button").Find("On").gameObject.SetActive(cond);
        button.transform.Find("Button").Find("Off").gameObject.SetActive(!cond);
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT GET CALLED WHEN A SETTING-TOGGLE BUTTON GETS PUSHED
    // ---------------------------------------------------

    public void pushMedButton() {
        StaticVariables.showMed = !StaticVariables.showMed;
        if (!StaticVariables.showMed) {
            StaticVariables.showLarge = false;
            StaticVariables.showHuge = false;
            StaticVariables.showMassive = false;
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
            StaticVariables.showMassive = false;
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
        else 
            StaticVariables.showMassive = false;
        setCurrentToggleTexts();
        SaveSystem.SaveGame();
    }
    public void pushMassiveButton() {
        StaticVariables.showMassive = !StaticVariables.showMassive;
        if (StaticVariables.showMassive) {
            StaticVariables.showHuge = true;
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

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT LET THE PLAYER CHOOSE A SKIN TO EQUIP
    // ---------------------------------------------------
    
    public void chooseSkin(Skin s) {
        //changes the selected skin
        StaticVariables.skin = s;
        loadSkin();

        SaveSystem.SaveGame();
    }
    
    private void loadSkin() {
        //updates the visuals of the settings scene based on what skin is used
        background.GetComponent<Image>().sprite = StaticVariables.skin.shopBackground;
        InterfaceFunctions.colorMenuButton(menuButton);
    }

    public void clickedExpandSkinButton() {
        //expands or contracts the selected skin button's dropdown
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
        //expands the skin details button beneath the "Choose Skin" button
        GameObject parentBox = expandSkinButton.transform.parent.gameObject;
        parentBox.transform.Find("Basic").gameObject.SetActive(true);
        for (int i = 2; i < parentBox.transform.childCount; i++) {
            bool switchTo = StaticVariables.unlockedSkins.Contains(InterfaceFunctions.getSkinFromName(parentBox.transform.GetChild(i).name));
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
            parentBox.transform.GetChild(i).Find("Button").Find("Text").GetComponent<Text>().text = parentBox.transform.GetChild(i).name.ToUpper() + " SKIN";
        }
    }

    private void contractSkinButtons() {
        //contracts the dropdowns beneath the "Choose Skin" button
        GameObject parentBox = expandSkinButton.transform.parent.gameObject;
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            bool switchTo = false;
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
        }
    }

    public void clickedSkinButton(GameObject button) {
        //equips a particular skin
        chooseSkin(InterfaceFunctions.getSkinFromName(button.transform.parent.gameObject.name));
        updateCurrentSkinText();
    }

    private void updateCurrentSkinText() {
        //updates the text on the "Current Skin" button
        currentSkinText.GetComponent<Text>().text = "CURRENT SKIN:\n" + StaticVariables.skin.skinName.ToUpper();
        GameObject parentBox = expandSkinButton.transform.parent.gameObject;
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            bool isActive = (InterfaceFunctions.getSkinFromName(parentBox.transform.GetChild(i).name) == StaticVariables.skin);
            parentBox.transform.GetChild(i).Find("Button").Find("On").gameObject.SetActive(isActive);
            parentBox.transform.GetChild(i).Find("Button").Find("Off").gameObject.SetActive(!isActive);
        }
    }

    private void showChooseSkinButton() {
        //only shows the "current skin" button if the player has purchased at least one skin.
        expandSkinButton.transform.parent.gameObject.SetActive(StaticVariables.unlockedSkins.Count >= 1);
    }
}