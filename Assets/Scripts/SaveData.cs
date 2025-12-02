//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData{
    //handles the player's save data

    //all of the upgrades that need to be saved and loaded, as well as the player's current skin and coin amounts
    public int coins;
    public bool includeNotes1;
    public bool includeNotes2;
    public bool changeResidentColorOnCorrectRows;
    public bool includeUndoRedo;
    public int highestUnlockedSize;
    public bool showMed; //deprecated
    public bool showLarge; //deprecated
    public bool showHuge; //deprecated
    public bool includeRemoveAllOfNumber;
    public bool includeClearPuzzle;
    public bool includeHighlightBuildings;
    public bool includeBuildingQuantityStatus;

    public bool unlockedMed;
    public bool unlockedLarge;
    public bool unlockedHuge;
    public bool unlockedNotes1;
    public bool unlockedNotes2;
    public bool unlockedResidentsChangeColor;
    public bool unlockedUndoRedo;
    public bool unlockedRemoveAllOfNumber;
    public bool unlockedClearPuzzle;
    public bool unlockedHighlightBuildings;
    public bool unlockedBuildingQuantityStatus;

    public string skinName;
    public string unlockedSkinNames;
    public bool hidePurchasedUpgrades;

    public bool hasSavedPuzzleState;
    public string previousPuzzleStates;
    public string currentPuzzleState;
    public string nextPuzzleStates;
    public string puzzleSolution;
    public int savedPuzzleSize;
    public int savedBuildNumber;
    public string savedBuildType;

    public bool hasBeatenTutorial;
    public bool unlockedMassive;
    public bool showMassive; //deprecated
    public float gameVersionNumber;
    public bool unlockedRemoveButtonFillsNotes;
    public bool includeRemoveButtonFillsNotes;

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO SAVE PLAYER DATA
    // ---------------------------------------------------
    
    public SaveData() {
        //takes all of the necessary variables from StaticVariables and stores them into the SaveData object variables
        coins = StaticVariables.coins;
        includeNotes1 = StaticVariables.includeNotes1Button;
        includeNotes2 = StaticVariables.includeNotes2Button;
        changeResidentColorOnCorrectRows = StaticVariables.changeResidentColorOnCorrectRows;
        highestUnlockedSize = StaticVariables.highestUnlockedSize;
        includeUndoRedo = StaticVariables.includeUndoRedo;
        includeRemoveAllOfNumber = StaticVariables.includeRemoveAllOfNumber;
        includeClearPuzzle = StaticVariables.includeClearPuzzle;
        includeHighlightBuildings = StaticVariables.includeHighlightBuildings;
        includeBuildingQuantityStatus = StaticVariables.includeBuildingQuantityStatus;
        includeRemoveButtonFillsNotes = StaticVariables.includeRemoveButtonFillsNotes;

        showMed = true;
        showLarge = true;
        showHuge = true;
        showMassive = true;

        unlockedMed = StaticVariables.unlockedMedium;
        unlockedLarge = StaticVariables.unlockedLarge;
        unlockedHuge = StaticVariables.unlockedHuge;
        unlockedMassive = StaticVariables.unlockedMassive;
        unlockedNotes1 = StaticVariables.unlockedNotes1;
        unlockedNotes2 = StaticVariables.unlockedNotes2;
        unlockedResidentsChangeColor = StaticVariables.unlockedResidentsChangeColor;
        unlockedUndoRedo = StaticVariables.unlockedUndoRedo;
        unlockedRemoveAllOfNumber = StaticVariables.unlockedRemoveAllOfNumber;
        unlockedClearPuzzle = StaticVariables.unlockedClearPuzzle;
        unlockedHighlightBuildings = StaticVariables.unlockedHighlightBuildings;
        unlockedBuildingQuantityStatus = StaticVariables.unlockedBuildingQuantityStatus;
        unlockedRemoveButtonFillsNotes = StaticVariables.unlockedRemoveButtonFillsNotes;

        if (StaticVariables.skin == null)
            skinName = "Rural";
        else
            skinName = StaticVariables.skin.skinName;
        unlockedSkinNames = GetUnlockedSkinNames();
        hidePurchasedUpgrades = StaticVariables.hidePurchasedUpgrades;

        hasBeatenTutorial = StaticVariables.hasBeatenTutorial;

        //stores the player's puzzle states
        hasSavedPuzzleState = StaticVariables.hasSavedPuzzleState;
        if (hasSavedPuzzleState) {
            previousPuzzleStates = GetPuzzleStateStringsFromList(StaticVariables.previousPuzzleStates);
            currentPuzzleState = StaticVariables.currentPuzzleState.ReturnStateAsString();
            nextPuzzleStates = GetPuzzleStateStringsFromList(StaticVariables.nextPuzzleStates);
            puzzleSolution = StaticVariables.puzzleSolution;
            savedPuzzleSize = StaticVariables.savedPuzzleSize;
            savedBuildNumber = StaticVariables.savedBuildNumber;
            savedBuildType = StaticVariables.savedBuildType switch {
                (GameManager.ClickTileActions.Build) => "Apply Selected",
                (GameManager.ClickTileActions.ToggleNote1) => "Toggle Note 1",
                (GameManager.ClickTileActions.ToggleNote2) => "Toggle Note 2",
                (GameManager.ClickTileActions.Erase) => "Clear Tile",
                _ => "Apply Selected",
            };
        }
        gameVersionNumber = StaticVariables.gameVersionNumber;
    }

    private string GetPuzzleStateStringsFromList(List<PuzzleState> list) {
        //takes the player's list of puzzle states and stores it as a string, specifically for saving
        string result = "";
        foreach (PuzzleState ps in list) {
            result += ps.ReturnStateAsString();
            result += ",";
        }
        if (result.Length > 1) 
            result = result.Substring(0, result.Length - 1);
        return result;
    }

    private string GetUnlockedSkinNames() {
        //takes the player's unlocked skins and stores them as a string, specifically for saving
        string result = "";
        foreach (Skin skin in StaticVariables.unlockedSkins) 
            result += skin.skinName + " ";
        if (result.Length > 1)
            result = result.Substring(0, result.Length - 1);
        return result;
    }


    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO LOAD PLAYER DATA
    // ---------------------------------------------------

    public void LoadData() {
        //takes all of the variables stored in the SaveData object and stores them into StaticVariables
        StaticVariables.coins = coins;
        StaticVariables.includeNotes1Button = includeNotes1;
        StaticVariables.includeNotes2Button = includeNotes2;
        StaticVariables.changeResidentColorOnCorrectRows = changeResidentColorOnCorrectRows;
        StaticVariables.highestUnlockedSize = highestUnlockedSize;
        StaticVariables.includeUndoRedo = includeUndoRedo;
        StaticVariables.includeRemoveAllOfNumber = includeRemoveAllOfNumber;
        StaticVariables.includeClearPuzzle = includeClearPuzzle;
        StaticVariables.includeHighlightBuildings = includeHighlightBuildings;
        StaticVariables.includeBuildingQuantityStatus = includeBuildingQuantityStatus;
        StaticVariables.includeRemoveButtonFillsNotes = includeRemoveButtonFillsNotes;

        StaticVariables.unlockedMedium = unlockedMed;
        StaticVariables.unlockedLarge = unlockedLarge;
        StaticVariables.unlockedHuge = unlockedHuge;
        StaticVariables.unlockedMassive = unlockedMassive;
        StaticVariables.unlockedNotes1 = unlockedNotes1;
        StaticVariables.unlockedNotes2 = unlockedNotes2;
        StaticVariables.unlockedResidentsChangeColor = unlockedResidentsChangeColor;
        StaticVariables.unlockedUndoRedo = unlockedUndoRedo;
        StaticVariables.unlockedRemoveAllOfNumber = unlockedRemoveAllOfNumber;
        StaticVariables.unlockedClearPuzzle = unlockedClearPuzzle;
        StaticVariables.unlockedHighlightBuildings = unlockedHighlightBuildings;
        StaticVariables.unlockedBuildingQuantityStatus = unlockedBuildingQuantityStatus;
        StaticVariables.unlockedRemoveButtonFillsNotes = unlockedRemoveButtonFillsNotes;
        
        StaticVariables.skin = InterfaceFunctions.GetSkinFromName(skinName);
        StaticVariables.unlockedSkins = GetUnlockedSkins();
        StaticVariables.hidePurchasedUpgrades = hidePurchasedUpgrades;

        StaticVariables.hasBeatenTutorial = hasBeatenTutorial;

        //loads the player's puzzle states
        StaticVariables.hasSavedPuzzleState = hasSavedPuzzleState;
        if (hasSavedPuzzleState) {
            StaticVariables.previousPuzzleStates = GetPuzzleStateListFromString(previousPuzzleStates, savedPuzzleSize);
            StaticVariables.currentPuzzleState = new PuzzleState(currentPuzzleState, savedPuzzleSize);
            StaticVariables.nextPuzzleStates = GetPuzzleStateListFromString(nextPuzzleStates, savedPuzzleSize);
            StaticVariables.puzzleSolution = puzzleSolution;
            StaticVariables.savedPuzzleSize = savedPuzzleSize;
            StaticVariables.savedBuildNumber = savedBuildNumber;
            StaticVariables.savedBuildType = savedBuildType switch {
                ("Apply Selected") => GameManager.ClickTileActions.Build,
                ("Toggle Note 1") => GameManager.ClickTileActions.ToggleNote1,
                ("Toggle Note 2") => GameManager.ClickTileActions.ToggleNote2,
                ("Clear Tile") => GameManager.ClickTileActions.Erase,
                _ => GameManager.ClickTileActions.Build,
            };
        }
        StaticVariables.gameVersionNumber = gameVersionNumber; //if there is no saved version number, it defaults to 0
    }
    
    private List<PuzzleState> GetPuzzleStateListFromString(string s, int size) {
        //parses the string that stores the player's current puzzle's states, specifically for loading
        List<PuzzleState> list = new List<PuzzleState>();
        if (s != "") {
            string[] strings = s.Split(',');
            foreach (string str in strings) {
                PuzzleState p = new PuzzleState(str, size);
                list.Add(p);
            }
        }
        return list;
    }
    
    private List<Skin> GetUnlockedSkins() {
        //parses the string containing the player's unlocked skins, specifically for loading
        List<Skin> skins = new List<Skin>();
        if (unlockedSkinNames != "") {
            string[] strings = unlockedSkinNames.Split(' ');
            foreach (string str in strings)
                skins.Add(InterfaceFunctions.GetSkinFromName(str));
        }
        if (!skins.Contains(InterfaceFunctions.GetSkinFromName("Rural")))
            skins.Add(InterfaceFunctions.GetSkinFromName("Rural"));
        return skins;
    }


}
