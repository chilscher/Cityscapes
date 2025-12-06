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
    public enum Keybinds {None, Size1, Size2, Size3, Size4, Size5, Size6, Size7, Build, Note1, Note2, Erase, Undo, Redo, RemoveAll, ClearPuzzle}
    public Keybinds currentEditableKeybind = Keybinds.None;
    private List<KeyCode> allowedKeycodes = new() { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6, KeyCode.F7,
        KeyCode.F8, KeyCode.F9, KeyCode.F10, KeyCode.F11, KeyCode.F12, KeyCode.F13, KeyCode.F14, KeyCode.F15, KeyCode.F16, KeyCode.F17, KeyCode.F18, 
        KeyCode.F19, KeyCode.F20, KeyCode.F21, KeyCode.F22, KeyCode.F23, KeyCode.F24, KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, 
        KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, 
        KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Space};

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
        else if ((StaticVariables.osType == StaticVariables.OSTypes.PC ) && (currentEditableKeybind != Keybinds.None))
            CheckForKeybindInput();
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
        keybindBuild1Button.DisplayKeybind(StaticVariables.keybindSize1, currentEditableKeybind == Keybinds.Size1);
        keybindBuild2Button.DisplayKeybind(StaticVariables.keybindSize2, currentEditableKeybind == Keybinds.Size2);
        keybindBuild3Button.DisplayKeybind(StaticVariables.keybindSize3, currentEditableKeybind == Keybinds.Size3);
        keybindBuild4Button.DisplayKeybind(StaticVariables.keybindSize4, currentEditableKeybind == Keybinds.Size4);
        keybindBuild5Button.DisplayKeybind(StaticVariables.keybindSize5, currentEditableKeybind == Keybinds.Size5);
        keybindBuild6Button.DisplayKeybind(StaticVariables.keybindSize6, currentEditableKeybind == Keybinds.Size6);
        keybindBuild7Button.DisplayKeybind(StaticVariables.keybindSize7, currentEditableKeybind == Keybinds.Size7);
        keybindBuildButton.DisplayKeybind(StaticVariables.keybindBuild, currentEditableKeybind == Keybinds.Build);
        keybindNote1Button.DisplayKeybind(StaticVariables.keybindNote1, currentEditableKeybind == Keybinds.Note1);
        keybindNote2Button.DisplayKeybind(StaticVariables.keybindNote2, currentEditableKeybind == Keybinds.Note2);
        keybindEraseButton.DisplayKeybind(StaticVariables.keybindErase, currentEditableKeybind == Keybinds.Erase);
        keybindUndoButton.DisplayKeybind(StaticVariables.keybindUndo, currentEditableKeybind == Keybinds.Undo);
        keybindRedoButton.DisplayKeybind(StaticVariables.keybindRedo, currentEditableKeybind == Keybinds.Redo);
        keybindRemoveAllButton.DisplayKeybind(StaticVariables.keybindRemoveAll, currentEditableKeybind == Keybinds.RemoveAll);
        keybindClearPuzzleButton.DisplayKeybind(StaticVariables.keybindClearPuzzle, currentEditableKeybind == Keybinds.ClearPuzzle);
    }

    public void PushBuilding1KeybindButton(){
        StaticVariables.keybindSize1 = KeyCode.None;
        currentEditableKeybind = Keybinds.Size1;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }
    public void PushBuilding2KeybindButton(){
        StaticVariables.keybindSize2 = KeyCode.None;
        currentEditableKeybind = Keybinds.Size2;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }
    public void PushBuilding3KeybindButton(){
        StaticVariables.keybindSize3 = KeyCode.None;
        currentEditableKeybind = Keybinds.Size3;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }
    public void PushBuilding4KeybindButton(){
        StaticVariables.keybindSize4 = KeyCode.None;
        currentEditableKeybind = Keybinds.Size4;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }
    public void PushBuilding5KeybindButton(){
        StaticVariables.keybindSize5 = KeyCode.None;
        currentEditableKeybind = Keybinds.Size5;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }
    public void PushBuilding6KeybindButton(){
        StaticVariables.keybindSize6 = KeyCode.None;
        currentEditableKeybind = Keybinds.Size6;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }
    public void PushBuilding7KeybindButton(){
        StaticVariables.keybindSize7 = KeyCode.None;
        currentEditableKeybind = Keybinds.Size7;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }
    public void PushBuildKeybindButton(){
        StaticVariables.keybindBuild = KeyCode.None;
        currentEditableKeybind = Keybinds.Build;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }
    public void PushNote1KeybindButton(){
        StaticVariables.keybindNote1 = KeyCode.None;
        currentEditableKeybind = Keybinds.Note1;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }

    public void PushNote2KeybindButton(){
        StaticVariables.keybindNote2 = KeyCode.None;
        currentEditableKeybind = Keybinds.Note2;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }

    public void PushEraseKeybindButton(){
        StaticVariables.keybindErase = KeyCode.None;
        currentEditableKeybind = Keybinds.Erase;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }

    public void PushUndoKeybindButton(){
        StaticVariables.keybindUndo = KeyCode.None;
        currentEditableKeybind = Keybinds.Undo;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }

    public void PushRedoKeybindButton(){
        StaticVariables.keybindRedo = KeyCode.None;
        currentEditableKeybind = Keybinds.Redo;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }

    public void PushRemoveAllKeybindButton(){
        StaticVariables.keybindRemoveAll = KeyCode.None;
        currentEditableKeybind = Keybinds.RemoveAll;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }

    public void PushClearPuzzleKeybindButton(){
        StaticVariables.keybindClearPuzzle = KeyCode.None;
        currentEditableKeybind = Keybinds.ClearPuzzle;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }

    public void PushResetKeybindsButton(){
        StaticVariables.ApplyDefaultKeybinds();
        currentEditableKeybind = Keybinds.None;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }

    
    private void CheckForKeybindInput(){
        foreach (KeyCode keycode in allowedKeycodes){
            if (Input.GetKeyDown(keycode)){
                NewKeybindSelected(keycode);
                return;
            }
        }
    }

    private void NewKeybindSelected(KeyCode keycode){
        if (currentEditableKeybind == Keybinds.None)
            return;
        if (StaticVariables.keybindSize1 == keycode)
            StaticVariables.keybindSize1 = KeyCode.None;
        if (StaticVariables.keybindSize2 == keycode)
            StaticVariables.keybindSize2 = KeyCode.None;
        if (StaticVariables.keybindSize3 == keycode)
            StaticVariables.keybindSize3 = KeyCode.None;
        if (StaticVariables.keybindSize4 == keycode)
            StaticVariables.keybindSize4 = KeyCode.None;
        if (StaticVariables.keybindSize5 == keycode)
            StaticVariables.keybindSize5 = KeyCode.None;
        if (StaticVariables.keybindSize6 == keycode)
            StaticVariables.keybindSize6 = KeyCode.None;
        if (StaticVariables.keybindSize7 == keycode)
            StaticVariables.keybindSize7 = KeyCode.None;
        if (StaticVariables.keybindBuild == keycode)
            StaticVariables.keybindBuild = KeyCode.None;
        if (StaticVariables.keybindNote1 == keycode)
            StaticVariables.keybindNote1 = KeyCode.None;
        if (StaticVariables.keybindNote2 == keycode)
            StaticVariables.keybindNote2 = KeyCode.None;
        if (StaticVariables.keybindErase == keycode)
            StaticVariables.keybindErase = KeyCode.None;
        if (StaticVariables.keybindUndo == keycode)
            StaticVariables.keybindUndo = KeyCode.None;
        if (StaticVariables.keybindRedo == keycode)
            StaticVariables.keybindRedo = KeyCode.None;
        if (StaticVariables.keybindRemoveAll == keycode)
            StaticVariables.keybindRemoveAll = KeyCode.None;
        if (StaticVariables.keybindClearPuzzle == keycode)
            StaticVariables.keybindClearPuzzle = KeyCode.None;
        switch (currentEditableKeybind){
            case Keybinds.Size1:
                StaticVariables.keybindSize1 = keycode;
                break;
            case Keybinds.Size2:
                StaticVariables.keybindSize2 = keycode;
                break;
            case Keybinds.Size3:
                StaticVariables.keybindSize3 = keycode;
                break;
            case Keybinds.Size4:
                StaticVariables.keybindSize4 = keycode;
                break;
            case Keybinds.Size5:
                StaticVariables.keybindSize5 = keycode;
                break;
            case Keybinds.Size6:
                StaticVariables.keybindSize6 = keycode;
                break;
            case Keybinds.Size7:
                StaticVariables.keybindSize7 = keycode;
                break;
            case Keybinds.Build:
                StaticVariables.keybindBuild = keycode;
                break;
            case Keybinds.Note1:
                StaticVariables.keybindNote1 = keycode;
                break;
            case Keybinds.Note2:
                StaticVariables.keybindNote2 = keycode;
                break;
            case Keybinds.Erase:
                StaticVariables.keybindErase = keycode;
                break;
            case Keybinds.Undo:
                StaticVariables.keybindUndo = keycode;
                break;
            case Keybinds.Redo:
                StaticVariables.keybindRedo = keycode;
                break;
            case Keybinds.RemoveAll:
                StaticVariables.keybindRemoveAll = keycode;
                break;
            case Keybinds.ClearPuzzle:
                StaticVariables.keybindClearPuzzle = keycode;
                break;
        }
        currentEditableKeybind = Keybinds.None;
        ShowKeybinds();
        SaveSystem.SaveGame();
    }
}