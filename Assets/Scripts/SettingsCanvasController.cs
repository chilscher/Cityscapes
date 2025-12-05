//for Cityscapes, copyright Cole Hilscher 2024

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCanvasController : MonoBehaviour {
    public GameObject scrollView; //the gameobject that is used to hide all buttons and text outside of the scrollable shop window
    [Header("Toggle Unlocked Features")]
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
    public GameObject expandSkinButton;
    public GameObject currentSkinText;
    public List<SkinApplicator> skinApplicators;

    [Header("Keybind Buttons")]
    public GameObject expandKeybindsButton;
    public KeybindAdjuster keybindBuild1Button;
    public KeybindAdjuster keybindBuild2Button;
    public KeybindAdjuster keybindBuild3Button;
    public KeybindAdjuster keybindBuild4Button;
    public KeybindAdjuster keybindBuild5Button;
    public KeybindAdjuster keybindBuild6Button;
    public KeybindAdjuster keybindBuild7Button;
    public KeybindAdjuster keybindBuildButton;
    public KeybindAdjuster keybindNote1Button;
    public KeybindAdjuster keybindNote2Button;
    public KeybindAdjuster keybindEraseButton;
    public KeybindAdjuster keybindUndoButton;
    public KeybindAdjuster keybindRedoButton;
    public KeybindAdjuster keybindRemoveAllButton;
    public KeybindAdjuster keybindClearPuzzleButton;

    [Header("Misc Settings")]
    public GameObject hidePurchasedUpgradesButton;
    public GameObject inviteButton;
    public string inviteLink = "https://discord.gg/KtARNGgaC8";
    public GameObject keybindsButton;

    [Header("UI Elements")]
    public GameObject background;
    private bool expandedTogglesButton = false;
    private bool expandedKeybindsButton = false;
    private bool expandedSkinButton = false;

    private void Start() {
        SetCurrentToggleTexts();
        ContractToggleButtons();
        UpdateCurrentSkinText();
        ShowChooseSkinButton();
        ShowToggleUnlocksButton();
        ShowKeybindButton();
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
        Application.OpenURL(inviteLink);
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
        StaticVariables.skin = s;
        LoadSkin();
        SaveSystem.SaveGame();
    }
    
    private void LoadSkin() {
        background.GetComponent<Image>().sprite = StaticVariables.skin.mainMenuBackground;
        foreach (SkinApplicator sa in skinApplicators)
            sa.ApplySkin(StaticVariables.skin);
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

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT LET THE PLAYER CHANGE KEYBINDS
    // ---------------------------------------------------
    
    private void ShowKeybindButton(){
        keybindsButton.SetActive(StaticVariables.osType == StaticVariables.OSTypes.PC);
    }

    public void PushExpandKeybindsButton() {
        if (!expandedKeybindsButton)
            ExpandKeybindButtons();
        else 
            ContractKeybindButtons();
        expandedKeybindsButton = !expandedKeybindsButton;
    }

    private void ExpandKeybindButtons() {
        GameObject parentBox = expandKeybindsButton.transform.parent.gameObject;
        for (int i = 1; i < parentBox.transform.childCount; i++)
            parentBox.transform.GetChild(i).gameObject.SetActive(true);
        ShowKeybinds();
    }

    private void ContractKeybindButtons() {
        GameObject parentBox = expandKeybindsButton.transform.parent.gameObject;
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            parentBox.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ShowKeybinds(){
        keybindBuild1Button.DisplayKeybind(StaticVariables.keybindBuilding1);
        keybindBuild2Button.DisplayKeybind(StaticVariables.keybindBuilding2);
        keybindBuild3Button.DisplayKeybind(StaticVariables.keybindBuilding3);
        keybindBuild4Button.DisplayKeybind(StaticVariables.keybindBuilding4);
        keybindBuild5Button.DisplayKeybind(StaticVariables.keybindBuilding5);
        keybindBuild6Button.DisplayKeybind(StaticVariables.keybindBuilding6);
        keybindBuild7Button.DisplayKeybind(StaticVariables.keybindBuilding7);
        keybindBuildButton.DisplayKeybind(StaticVariables.keybindBuild);
        keybindNote1Button.DisplayKeybind(StaticVariables.keybindNote1);
        keybindNote2Button.DisplayKeybind(StaticVariables.keybindNote2);
        keybindEraseButton.DisplayKeybind(StaticVariables.keybindErase);
        keybindUndoButton.DisplayKeybind(StaticVariables.keybindUndo);
        keybindRedoButton.DisplayKeybind(StaticVariables.keybindRedo);
        keybindRemoveAllButton.DisplayKeybind(StaticVariables.keybindRemoveAll);
        keybindClearPuzzleButton.DisplayKeybind(StaticVariables.keybindClearPuzzle);
    }

    public void PushBuilding1KeybindButton(){
        StaticVariables.keybindBuilding1 = KeyCode.None;
        ShowKeybinds();
    }
    public void PushBuilding2KeybindButton(){
        StaticVariables.keybindBuilding2 = KeyCode.None;
        ShowKeybinds();
    }
    public void PushBuilding3KeybindButton(){
        StaticVariables.keybindBuilding3 = KeyCode.None;
        ShowKeybinds();
    }
    public void PushBuilding4KeybindButton(){
        StaticVariables.keybindBuilding4 = KeyCode.None;
        ShowKeybinds();
    }
    public void PushBuilding5KeybindButton(){
        StaticVariables.keybindBuilding5 = KeyCode.None;
        ShowKeybinds();
    }
    public void PushBuilding6KeybindButton(){
        StaticVariables.keybindBuilding6 = KeyCode.None;
        ShowKeybinds();
    }
    public void PushBuilding7KeybindButton(){
        StaticVariables.keybindBuilding7 = KeyCode.None;
        ShowKeybinds();
    }
    public void PushBuildKeybindButton(){
        StaticVariables.keybindBuild = KeyCode.None;
        ShowKeybinds();
    }
    public void PushNote1KeybindButton(){
        StaticVariables.keybindNote1 = KeyCode.None;
        ShowKeybinds();
    }

    public void PushNote2KeybindButton(){
        StaticVariables.keybindNote2 = KeyCode.None;
        ShowKeybinds();
    }

    public void PushEraseKeybindButton(){
        StaticVariables.keybindErase = KeyCode.None;
        ShowKeybinds();
    }

    public void PushUndoKeybindButton(){
        StaticVariables.keybindUndo = KeyCode.None;
        ShowKeybinds();
    }

    public void PushRedoKeybindButton(){
        StaticVariables.keybindRedo = KeyCode.None;
        ShowKeybinds();
    }

    public void PushRemoveAllKeybindButton(){
        StaticVariables.keybindRemoveAll = KeyCode.None;
        ShowKeybinds();
    }

    public void PushClearPuzzleKeybindButton(){
        StaticVariables.keybindClearPuzzle = KeyCode.None;
        ShowKeybinds();
    }

    public void PushResetKeybindsButton(){
        StaticVariables.keybindBuilding1 = StaticVariables.keybindBuilding1Default;
        StaticVariables.keybindBuilding2 = StaticVariables.keybindBuilding2Default;
        StaticVariables.keybindBuilding3 = StaticVariables.keybindBuilding3Default;
        StaticVariables.keybindBuilding4 = StaticVariables.keybindBuilding4Default;
        StaticVariables.keybindBuilding5 = StaticVariables.keybindBuilding5Default;
        StaticVariables.keybindBuilding6 = StaticVariables.keybindBuilding6Default;
        StaticVariables.keybindBuilding7 = StaticVariables.keybindBuilding7Default;
        StaticVariables.keybindBuild = StaticVariables.keybindBuildDefault;
        StaticVariables.keybindNote1 = StaticVariables.keybindNote1Default;
        StaticVariables.keybindNote2 = StaticVariables.keybindNote2Default;
        StaticVariables.keybindErase = StaticVariables.keybindEraseDefault;
        StaticVariables.keybindUndo = StaticVariables.keybindUndoDefault;
        StaticVariables.keybindRedo = StaticVariables.keybindRedoDefault;
        StaticVariables.keybindRemoveAll = StaticVariables.keybindRemoveAllDefault;
        StaticVariables.keybindClearPuzzle = StaticVariables.keybindClearPuzzleDefault;
        ShowKeybinds();
    }

}