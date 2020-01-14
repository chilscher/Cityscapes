using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData{

    public int coins;
    public bool includeNotes1;
    public bool includeNotes2;
    public bool changeResidentColorOnCorrectRows;
    public bool includeUndoRedo;
    public int highestUnlockedSize;
    public bool showMed;
    public bool showLarge;
    public bool showHuge;
    public bool includeRemoveAllOfNumber;
    public bool includeClearPuzzle;
    public bool includeHighlightBuildings;

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


    public SaveData() {
        coins = StaticVariables.coins;
        includeNotes1 = StaticVariables.includeNotes1Button;
        includeNotes2 = StaticVariables.includeNotes2Button;
        changeResidentColorOnCorrectRows = StaticVariables.changeResidentColorOnCorrectRows;
        highestUnlockedSize = StaticVariables.highestUnlockedSize;
        includeUndoRedo = StaticVariables.includeUndoRedo;
        includeRemoveAllOfNumber = StaticVariables.includeRemoveAllOfNumber;
        includeClearPuzzle = StaticVariables.includeClearPuzzle;
        includeHighlightBuildings = StaticVariables.includeHighlightBuildings;

        showMed = StaticVariables.showMed;
        showLarge = StaticVariables.showLarge;
        showHuge = StaticVariables.showHuge;

        unlockedMed = StaticVariables.unlockedMedium;
        unlockedLarge = StaticVariables.unlockedLarge;
        unlockedHuge = StaticVariables.unlockedHuge;
        unlockedNotes1 = StaticVariables.unlockedNotes1;
        unlockedNotes2 = StaticVariables.unlockedNotes2;
        unlockedResidentsChangeColor = StaticVariables.unlockedResidentsChangeColor;
        unlockedUndoRedo = StaticVariables.unlockedUndoRedo;
        unlockedRemoveAllOfNumber = StaticVariables.unlockedRemoveAllOfNumber;
        unlockedClearPuzzle = StaticVariables.unlockedClearPuzzle;
        unlockedHighlightBuildings = StaticVariables.unlockedHighlightBuildings;

        skinName = StaticVariables.skin.skinName;
        unlockedSkinNames = getUnlockedSkinNames();
        hidePurchasedUpgrades = StaticVariables.hidePurchasedUpgrades;

        hasSavedPuzzleState = StaticVariables.hasSavedPuzzleState;
        if (hasSavedPuzzleState) {
            previousPuzzleStates = getPuzzleStateStringsFromList(StaticVariables.previousPuzzleStates);
            currentPuzzleState = StaticVariables.currentPuzzleState.returnStateAsString();
            nextPuzzleStates = getPuzzleStateStringsFromList(StaticVariables.nextPuzzleStates);
            puzzleSolution = StaticVariables.puzzleSolution;
            savedPuzzleSize = StaticVariables.savedPuzzleSize;
            savedBuildNumber = StaticVariables.savedBuildNumber;
            savedBuildType = StaticVariables.savedBuildType;
        }

    }

    public void LoadData() {

        StaticVariables.coins = coins;
        StaticVariables.includeNotes1Button = includeNotes1;
        StaticVariables.includeNotes2Button = includeNotes2;
        StaticVariables.changeResidentColorOnCorrectRows = changeResidentColorOnCorrectRows;
        StaticVariables.highestUnlockedSize = highestUnlockedSize;
        StaticVariables.includeUndoRedo = includeUndoRedo;
        StaticVariables.includeRemoveAllOfNumber = includeRemoveAllOfNumber;
        StaticVariables.includeClearPuzzle = includeClearPuzzle;
        StaticVariables.includeHighlightBuildings = includeHighlightBuildings;

        StaticVariables.showMed = showMed;
        StaticVariables.showLarge = showLarge;
        StaticVariables.showHuge = showHuge;

        StaticVariables.unlockedMedium = unlockedMed;
        StaticVariables.unlockedLarge = unlockedLarge;
        StaticVariables.unlockedHuge = unlockedHuge;
        StaticVariables.unlockedNotes1 = unlockedNotes1;
        StaticVariables.unlockedNotes2 = unlockedNotes2;
        StaticVariables.unlockedResidentsChangeColor = unlockedResidentsChangeColor;
        StaticVariables.unlockedUndoRedo = unlockedUndoRedo;
        StaticVariables.unlockedRemoveAllOfNumber = unlockedRemoveAllOfNumber;
        StaticVariables.unlockedClearPuzzle = unlockedClearPuzzle;
        StaticVariables.unlockedHighlightBuildings = unlockedHighlightBuildings;
        
        StaticVariables.skin = InterfaceFunctions.getSkinFromName(skinName);
        StaticVariables.unlockedSkins = getUnlockedSkins();
        StaticVariables.hidePurchasedUpgrades = hidePurchasedUpgrades;

        StaticVariables.hasSavedPuzzleState = hasSavedPuzzleState;
        if (hasSavedPuzzleState) {
            StaticVariables.previousPuzzleStates = getPuzzleStateListFromString(previousPuzzleStates, savedPuzzleSize);
            StaticVariables.currentPuzzleState = new PuzzleState(currentPuzzleState, savedPuzzleSize);
            StaticVariables.nextPuzzleStates = getPuzzleStateListFromString(nextPuzzleStates, savedPuzzleSize);
            StaticVariables.puzzleSolution = puzzleSolution;
            StaticVariables.savedPuzzleSize = savedPuzzleSize;
            StaticVariables.savedBuildNumber = savedBuildNumber;
            StaticVariables.savedBuildType = savedBuildType;
        }
    }

    private string getPuzzleStateStringsFromList(List<PuzzleState> list) {
        string result = "";
        foreach (PuzzleState ps in list) {
            result += ps.returnStateAsString();
            result += ",";
        }
        if (result.Length > 1) {
            result = result.Substring(0, result.Length - 1);
        }
        

        return result;
    }

    private List<PuzzleState> getPuzzleStateListFromString(string s, int size) {
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
    
    private string getUnlockedSkinNames() {
        string result = "";
        foreach (Skin skin in StaticVariables.unlockedSkins) {
            result += skin.skinName + " ";
        }
        if (result.Length > 1) {
            result = result.Substring(0, result.Length - 1);
        }
        return result;
    }

    private List<Skin> getUnlockedSkins() {
        List<Skin> skins = new List<Skin>();
        if (unlockedSkinNames != "") {
            string[] strings = unlockedSkinNames.Split(' ');
            foreach (string str in strings) {
                skins.Add(InterfaceFunctions.getSkinFromName(str));
            }
        }
        return skins;
    }


}
