//for Cityscapes, copyright Cole Hilscher 2020

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCanvasController : MonoBehaviour {
    
    //public GameObject blackForeground; //used to transition to/from the puzzle menu
    //private Image blackSprite; //derived from the blackForeground gameObject
    //public float fadeOutTime; //total time for fade-out, from complete light to complete darkness
    //public float fadeInTime; //total time for fade-in, from complete darkness to complete light
    //private float fadeTimer; //the timer on which the fadeout and fadein mechanics operate
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
    public GameObject buildingQuantityStatusButton;

    //the following deal with switching between different skins
    private bool expandedSkinButton = false;
    public GameObject expandSkinButton;
    public GameObject currentSkinText;

    //the buttons that are not related to any upgrades
    public GameObject hidePurchasedUpgradesButton;
    public GameObject creditsButton;

    //the following are properties taken from the skin
    public GameObject background;
    public GameObject menuButton;
    public GameObject shopButton;
    public Image popupBorder;
    public Image popupInside;
    
    //headers for the different types of buttons you can interact with in the setting scene
    public GameObject cosmeticsTitle;
    public GameObject visualTitle;
    public GameObject buttonsTitle;
    public GameObject citiesTitle;
    public GameObject othersTitle;

    private void Start() {
        //show the buttons and headers based on what upgrades you have purchased
        //update the visuals for the buttons based on what is toggled on or off at the current time
        ShowAndHideButtons();
        ShowAndHideText();
        SetCurrentToggleTexts();
        UpdateCurrentSkinText();
        ShowChooseSkinButton();
        LoadSkin();
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            StaticVariables.FadeOutThenLoadScene("MainMenu");
    }
    
    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO LEAVE THE SETTINGS SCENE
    // ---------------------------------------------------
    
    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void PushMainMenuButton() {
        StaticVariables.FadeOutThenLoadScene("MainMenu");
    }
    public void PushCreditsButton(){
        StaticVariables.FadeOutThenLoadScene("Credits");
    }
    public void PushShopButton(){
        StaticVariables.FadeOutThenLoadScene("Shop");
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO CHANGE WHAT THE BUTTONS, TEXT, AND COLORS OF THE SETTINGS MENU LOOK LIKE, EXCEPT FOR THE ONES THAT DEAL WITH SKIN SELECTION
    // ---------------------------------------------------
    
    public void ShowAndHideButtons() {
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
        buildingQuantityStatusButton.SetActive(StaticVariables.unlockedBuildingQuantityStatus);

        hidePurchasedUpgradesButton.SetActive(true);
        
    }

    public void ShowAndHideText() {
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

    public void SetCurrentToggleTexts() {
        //changes the text and text-color of the buttons that toggle settings
        //ex: the MedCityButton will either have CURRENTLY: ON in green, or CURRENTLY: OFF in red displayed
        ToggleText(medCityButton, StaticVariables.showMed);
        ToggleText(largeCityButton, StaticVariables.showLarge);
        ToggleText(hugeCityButton, StaticVariables.showHuge);
        ToggleText(massiveCityButton, StaticVariables.showMassive);
        ToggleText(notes1Button, StaticVariables.includeNotes1Button);
        ToggleText(notes2Button, StaticVariables.includeNotes2Button);
        ToggleText(residentColorButton, StaticVariables.changeResidentColorOnCorrectRows);
        ToggleText(undoRedoButton, StaticVariables.includeUndoRedo);
        ToggleText(removeNumbersButton, StaticVariables.includeRemoveAllOfNumber);
        ToggleText(clearPuzzleButton, StaticVariables.includeClearPuzzle);
        ToggleText(highlightBuildingsButton, StaticVariables.includeHighlightBuildings);
        ToggleText(buildingQuantityStatusButton, StaticVariables.includeBuildingQuantityStatus);

        ToggleText(hidePurchasedUpgradesButton, StaticVariables.hidePurchasedUpgrades);
    }

    public void ToggleText(GameObject button, bool cond) {
        //changes the toggle text on an invidual button to one of two pre-defined layouts
        button.transform.Find("Button").Find("On").gameObject.SetActive(cond);
        button.transform.Find("Button").Find("Off").gameObject.SetActive(!cond);
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT GET CALLED WHEN A SETTING-TOGGLE BUTTON GETS PUSHED
    // ---------------------------------------------------

    public void PushMedButton() {
        StaticVariables.showMed = !StaticVariables.showMed;
        if (!StaticVariables.showMed) {
            StaticVariables.showLarge = false;
            StaticVariables.showHuge = false;
            StaticVariables.showMassive = false;
        }
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushLargeButton() {
        StaticVariables.showLarge = !StaticVariables.showLarge;
        if (StaticVariables.showLarge)
            StaticVariables.showMed = true;
        else {
            StaticVariables.showHuge = false;
            StaticVariables.showMassive = false;
        }
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushHugeButton() {
        StaticVariables.showHuge = !StaticVariables.showHuge;
        if (StaticVariables.showHuge) {
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
        }
        else 
            StaticVariables.showMassive = false;
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }
    public void PushMassiveButton() {
        StaticVariables.showMassive = !StaticVariables.showMassive;
        if (StaticVariables.showMassive) {
            StaticVariables.showHuge = true;
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
        }
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushNotes1Button() {
        StaticVariables.includeNotes1Button = !StaticVariables.includeNotes1Button;
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushNotes2Button() {
        StaticVariables.includeNotes2Button = !StaticVariables.includeNotes2Button;
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }
    
    public void PushResidentColorButton() {
        StaticVariables.changeResidentColorOnCorrectRows = !StaticVariables.changeResidentColorOnCorrectRows;
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushUndoRedoButton() {
        StaticVariables.includeUndoRedo = !StaticVariables.includeUndoRedo;
        if (!StaticVariables.includeUndoRedo) {
            StaticVariables.includeRemoveAllOfNumber = false;
            StaticVariables.includeClearPuzzle = false;
        }
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushRemoveNumbersButton() {
        StaticVariables.includeRemoveAllOfNumber = !StaticVariables.includeRemoveAllOfNumber;
        if (StaticVariables.includeRemoveAllOfNumber)
            StaticVariables.includeUndoRedo = true;
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushClearPuzzleButton() {
        StaticVariables.includeClearPuzzle = !StaticVariables.includeClearPuzzle;
        if (StaticVariables.includeClearPuzzle)
            StaticVariables.includeUndoRedo = true;
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushHighlightBuildingsButton() {
        StaticVariables.includeHighlightBuildings = !StaticVariables.includeHighlightBuildings;
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushHidePurchasedUpgradesButton() {
        StaticVariables.hidePurchasedUpgrades = !StaticVariables.hidePurchasedUpgrades;
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushBuildingQuantityStatusButton() {
        StaticVariables.includeBuildingQuantityStatus = !StaticVariables.includeBuildingQuantityStatus;
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT LET THE PLAYER CHOOSE A SKIN TO EQUIP
    // ---------------------------------------------------
    
    public void ChooseSkin(Skin s) {
        //changes the selected skin
        StaticVariables.skin = s;
        LoadSkin();

        SaveSystem.SaveGame();
    }
    
    private void LoadSkin() {
        //updates the visuals of the settings scene based on what skin is used
        background.GetComponent<Image>().sprite = StaticVariables.skin.mainMenuBackground;
        InterfaceFunctions.ColorMenuButton(menuButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(shopButton, StaticVariables.skin);
        popupBorder.color = StaticVariables.skin.popupBorder;
        popupInside.color = StaticVariables.skin.popupInside;
        ColorSkinButtons();
        ColorSettingsButton(residentColorButton);
        ColorSettingsButton(highlightBuildingsButton);
        ColorSettingsButton(notes1Button);
        ColorSettingsButton(notes2Button);
        ColorSettingsButton(undoRedoButton);
        ColorSettingsButton(removeNumbersButton);
        ColorSettingsButton(clearPuzzleButton);
        ColorSettingsButton(medCityButton);
        ColorSettingsButton(largeCityButton);
        ColorSettingsButton(hugeCityButton);
        ColorSettingsButton(massiveCityButton);
        ColorSettingsButton(hidePurchasedUpgradesButton);
        ColorSettingsButton(buildingQuantityStatusButton);
        ColorSettingsButton(creditsButton, false);
    }

    private void ColorSkinButtons(){
        InterfaceFunctions.ColorMenuButton(expandSkinButton, StaticVariables.skin);
        expandSkinButton.transform.Find("On").GetComponent<Text>().color = StaticVariables.skin.settingsText_On;
        GameObject parentBox = expandSkinButton.transform.parent.gameObject;
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            Transform button = parentBox.transform.GetChild(i).transform.Find("Button");
            button.Find("Dropdown Image").Find("Exterior").GetComponent<Image>().color = StaticVariables.skin.menuButtonBorder;
            button.Find("Dropdown Image").Find("Interior").GetComponent<Image>().color = StaticVariables.skin.menuButtonInside;
            button.Find("On").GetComponent<Text>().color = StaticVariables.skin.settingsText_On;
            button.Find("Off").GetComponent<Text>().color = StaticVariables.skin.settingsText_Off;
        }
    }

    private void ColorSettingsButton(GameObject button, bool changeText=true){
        InterfaceFunctions.ColorMenuButton(button.transform.GetChild(0).gameObject, StaticVariables.skin);
        if (changeText){
            button.transform.GetChild(0).Find("On").GetComponent<Text>().color = StaticVariables.skin.settingsText_On;
            button.transform.GetChild(0).Find("Off").GetComponent<Text>().color = StaticVariables.skin.settingsText_Off;
        }
    }

    public void PushExpandSkinsButton() {
        //expands or contracts the selected skin button's dropdown
        if (!expandedSkinButton) {
            ExpandSkinButtons();
            expandedSkinButton = true;
        }
        else {
            ContractSkinButtons();
            expandedSkinButton = false;
        }
    }

    private void ExpandSkinButtons() {
        //expands the skin details button beneath the "Choose Skin" button
        GameObject parentBox = expandSkinButton.transform.parent.gameObject;
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            bool switchTo = StaticVariables.unlockedSkins.Contains(InterfaceFunctions.GetSkinFromName(parentBox.transform.GetChild(i).name));
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
            parentBox.transform.GetChild(i).Find("Button").Find("Text").GetComponent<Text>().text = parentBox.transform.GetChild(i).name.ToUpper();
        }
    }

    private void ContractSkinButtons() {
        //contracts the dropdowns beneath the "Choose Skin" button
        GameObject parentBox = expandSkinButton.transform.parent.gameObject;
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            bool switchTo = false;
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
        }
    }

    public void PushSkinButton(GameObject button) {
        //equips a particular skin
        ChooseSkin(InterfaceFunctions.GetSkinFromName(button.transform.parent.gameObject.name));
        UpdateCurrentSkinText();
    }

    private void UpdateCurrentSkinText() {
        //updates the text on the "Current Skin" button
        currentSkinText.GetComponent<Text>().text = "CURRENT SKIN:\n" + StaticVariables.skin.skinName.ToUpper();
        GameObject parentBox = expandSkinButton.transform.parent.gameObject;
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            bool isActive = (InterfaceFunctions.GetSkinFromName(parentBox.transform.GetChild(i).name) == StaticVariables.skin);
            parentBox.transform.GetChild(i).Find("Button").Find("On").gameObject.SetActive(isActive);
            parentBox.transform.GetChild(i).Find("Button").Find("Off").gameObject.SetActive(!isActive);
        }
    }

    private void ShowChooseSkinButton() {
        //only shows the "current skin" button if the player has purchased at least one skin.
        expandSkinButton.transform.parent.gameObject.SetActive(StaticVariables.unlockedSkins.Count >= 2);
    }
}