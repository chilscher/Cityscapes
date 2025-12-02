//for Cityscapes, copyright Cole Hilscher 2024

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCanvasController : MonoBehaviour {
    public GameObject scrollView; //the gameobject that is used to hide all buttons and text outside of the scrollable shop window
    [Header("Toggle Unlocked Features")]
    private bool expandedTogglesButton = false;
    public GameObject expandTogglesButton;
    public GameObject notes1Button;
    public GameObject notes2Button;
    public GameObject residentColorButton;
    public GameObject undoRedoButton;
    public GameObject removeNumbersButton;
    public GameObject clearPuzzleButton;
    public GameObject highlightBuildingsButton;
    public GameObject buildingQuantityStatusButton;
    public GameObject fillNotesButton;

    [Header("Skin Selection")]
    private bool expandedSkinButton = false;
    public GameObject expandSkinButton;
    public GameObject currentSkinText;

    [Header("Misc Settings")]
    public GameObject hidePurchasedUpgradesButton;
    public GameObject creditsButton;
    public GameObject discordButton;
    public GameObject inviteButton;
    public string inviteLink = "https://discord.gg/KtARNGgaC8";

    [Header("UI Elements")]
    public GameObject background;
    public GameObject menuButton;
    public GameObject shopButton;
    public Image popupBorder;
    public Image popupInside;

    private void Start() {
        SetCurrentToggleTexts();
        ContractToggleButtons();
        UpdateCurrentSkinText();
        ShowChooseSkinButton();
        ShowToggleUnlocksButton();
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
    public void PushDiscordButton(){
        inviteButton.SetActive(!inviteButton.activeSelf);
    }
    public void PushInviteButton(){
        System.Diagnostics.Process.Start(inviteLink);
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT RELATE TO TOGGLING AN ALREADY-UNLOCKED UPGRADE
    // ---------------------------------------------------

    public void SetCurrentToggleTexts() {
        //changes the text and text-color of the buttons that toggle settings
        //ex: the Notes1Button will either have CURRENTLY: ON in green, or CURRENTLY: OFF in red displayed
        ToggleText(notes1Button, StaticVariables.includeNotes1Button);
        ToggleText(notes2Button, StaticVariables.includeNotes2Button);
        ToggleText(residentColorButton, StaticVariables.changeResidentColorOnCorrectRows);
        ToggleText(undoRedoButton, StaticVariables.includeUndoRedo);
        ToggleText(removeNumbersButton, StaticVariables.includeRemoveAllOfNumber);
        ToggleText(clearPuzzleButton, StaticVariables.includeClearPuzzle);
        ToggleText(highlightBuildingsButton, StaticVariables.includeHighlightBuildings);
        ToggleText(buildingQuantityStatusButton, StaticVariables.includeBuildingQuantityStatus);
        ToggleText(fillNotesButton, StaticVariables.includeRemoveButtonFillsNotes);
        ToggleText(hidePurchasedUpgradesButton, StaticVariables.hidePurchasedUpgrades);
    }

    public void ToggleText(GameObject button, bool cond) {
        //changes the toggle text on an invidual button to one of two pre-defined layouts
        button.transform.Find("Button").Find("On").gameObject.SetActive(cond);
        button.transform.Find("Button").Find("Off").gameObject.SetActive(!cond);
    }

    public void PushExpandTogglesButton() {
        //expands or contracts the toggle feature button's dropdown
        if (!expandedTogglesButton)
            ExpandToggleButtons();
        else
            ContractToggleButtons();
        expandedTogglesButton = !expandedTogglesButton;
    }


    private void ExpandToggleButtons() {
        //expands the dropdowns beneath the "Toggle Unlocked Features" button    public GameObject notes1Button;
        notes1Button.SetActive(StaticVariables.unlockedNotes1);
        notes2Button.SetActive(StaticVariables.unlockedNotes2);
        residentColorButton.SetActive(StaticVariables.unlockedResidentsChangeColor);
        undoRedoButton.SetActive(StaticVariables.unlockedUndoRedo);
        removeNumbersButton.SetActive(StaticVariables.unlockedRemoveAllOfNumber);
        clearPuzzleButton.SetActive(StaticVariables.unlockedClearPuzzle);
        highlightBuildingsButton.SetActive(StaticVariables.unlockedHighlightBuildings);
        buildingQuantityStatusButton.SetActive(StaticVariables.unlockedBuildingQuantityStatus);
        fillNotesButton.SetActive(StaticVariables.unlockedRemoveButtonFillsNotes);
    }

    private void ContractToggleButtons() {
        //contracts the dropdowns beneath the "Toggle Unlocked Features" button
        notes1Button.SetActive(false);
        notes2Button.SetActive(false);
        residentColorButton.SetActive(false);
        undoRedoButton.SetActive(false);
        removeNumbersButton.SetActive(false);
        clearPuzzleButton.SetActive(false);
        highlightBuildingsButton.SetActive(false);
        buildingQuantityStatusButton.SetActive(false);
        fillNotesButton.SetActive(false);
    }

    private void ShowToggleUnlocksButton() {
        //only shows the "toggle unlocks" button if the player has purchased at least one unlock.
        bool hasUnlockedAny = false;
        if (StaticVariables.unlockedNotes1)
            hasUnlockedAny = true;
        if (StaticVariables.unlockedNotes2)
            hasUnlockedAny = true;
        if (StaticVariables.unlockedResidentsChangeColor)
            hasUnlockedAny = true;
        if (StaticVariables.unlockedUndoRedo)
            hasUnlockedAny = true;
        if (StaticVariables.unlockedRemoveAllOfNumber)
            hasUnlockedAny = true;
        if (StaticVariables.unlockedClearPuzzle)
            hasUnlockedAny = true;
        if (StaticVariables.unlockedHighlightBuildings)
            hasUnlockedAny = true;
        if (StaticVariables.unlockedBuildingQuantityStatus)
            hasUnlockedAny = true;
        if (StaticVariables.includeRemoveButtonFillsNotes)
            hasUnlockedAny = true;
        expandTogglesButton.transform.parent.gameObject.SetActive(hasUnlockedAny);
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
            StaticVariables.includeRemoveButtonFillsNotes = false;
        }
        SetCurrentToggleTexts();
        SaveSystem.SaveGame();
    }

    public void PushRemoveNumbersButton() {
        StaticVariables.includeRemoveAllOfNumber = !StaticVariables.includeRemoveAllOfNumber;
        if (StaticVariables.includeRemoveAllOfNumber)
            StaticVariables.includeUndoRedo = true;
        else
            StaticVariables.includeRemoveButtonFillsNotes = false;
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
    
    public void PushFillNotesButton() {
        StaticVariables.includeRemoveButtonFillsNotes = !StaticVariables.includeRemoveButtonFillsNotes;
        if (StaticVariables.includeRemoveButtonFillsNotes){
            StaticVariables.includeUndoRedo = true;
            StaticVariables.includeRemoveAllOfNumber = true;
        }
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
        ColorToggleFeatureButtons();
        ColorSettingsButton(hidePurchasedUpgradesButton);
        ColorSettingsButton(discordButton, false);   
        ColorInviteButton();
        ColorSettingsButton(creditsButton, false);
    }

    private void ColorInviteButton(){
        inviteButton.transform.Find("Dropdown").Find("Dropdown Image").Find("Exterior").GetComponent<Image>().color = StaticVariables.skin.menuButtonBorder;
        inviteButton.transform.Find("Dropdown").Find("Dropdown Image").Find("Interior").GetComponent<Image>().color = StaticVariables.skin.menuButtonInside;
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

    private void ColorToggleFeatureButtons(){
        InterfaceFunctions.ColorMenuButton(expandTogglesButton, StaticVariables.skin);
        //expandTogglesButton.transform.Find("On").GetComponent<Text>().color = StaticVariables.skin.settingsText_On;
        GameObject parentBox = expandTogglesButton.transform.parent.gameObject;
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            Transform button = parentBox.transform.GetChild(i).transform.Find("Button");
            button.Find("Dropdown Image").Find("Exterior").GetComponent<Image>().color = StaticVariables.skin.menuButtonBorder;
            button.Find("Dropdown Image").Find("Interior").GetComponent<Image>().color = StaticVariables.skin.menuButtonInside;
            button.Find("On").GetComponent<Text>().color = StaticVariables.skin.settingsText_On;
            button.Find("Off").GetComponent<Text>().color = StaticVariables.skin.settingsText_Off;
        }
    }

    public void PushExpandSkinsButton() {
        //expands or contracts the selected skin button's dropdown
        if (!expandedSkinButton)
            ExpandSkinButtons();
        else 
            ContractSkinButtons();
        expandedSkinButton = !expandedSkinButton;
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