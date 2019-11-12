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
    //public bool includeRemoveColoredNumberNotes;
    public bool includeRemoveAllOfNumber;
    public bool includeClearPuzzle;

    public bool unlockedMed;
    public bool unlockedLarge;
    public bool unlockedHuge;
    public bool unlockedNotes1;
    public bool unlockedNotes2;
    public bool unlockedResidentsChangeColor;
    public bool unlockedUndoRedo;
    //public bool unlockedRemoveColoredNumberNotes;
    public bool unlockedRemoveAllOfNumber;
    public bool unlockedClearPuzzle;

    public string skinName;


    public SaveData() {
        coins = StaticVariables.coins;
        includeNotes1 = StaticVariables.includeNotes1Button;
        includeNotes2 = StaticVariables.includeNotes2Button;
        changeResidentColorOnCorrectRows = StaticVariables.changeResidentColorOnCorrectRows;
        highestUnlockedSize = StaticVariables.highestUnlockedSize;
        includeUndoRedo = StaticVariables.includeUndoRedo;
        //includeRemoveColoredNumberNotes = StaticVariables.includeRemoveColoredNotesOfChosenNumber;
        includeRemoveAllOfNumber = StaticVariables.includeRemoveAllOfNumber;
        includeClearPuzzle = StaticVariables.includeClearPuzzle;

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
        //unlockedRemoveColoredNumberNotes = StaticVariables.unlockedRemoveColoredNotesOfChosenNumber;
        unlockedRemoveAllOfNumber = StaticVariables.unlockedRemoveAllOfNumber;
        unlockedClearPuzzle = StaticVariables.unlockedClearPuzzle;
        skinName = StaticVariables.skin.skinName;
    }

    public void LoadData(Skin[] skins) {

        StaticVariables.coins = coins;
        StaticVariables.includeNotes1Button = includeNotes1;
        StaticVariables.includeNotes2Button = includeNotes2;
        StaticVariables.changeResidentColorOnCorrectRows = changeResidentColorOnCorrectRows;
        StaticVariables.highestUnlockedSize = highestUnlockedSize;
        StaticVariables.includeUndoRedo = includeUndoRedo;
        //StaticVariables.includeRemoveColoredNotesOfChosenNumber = includeRemoveColoredNumberNotes;
        StaticVariables.includeRemoveAllOfNumber = includeRemoveAllOfNumber;
        StaticVariables.includeClearPuzzle = includeClearPuzzle;

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
        //StaticVariables.unlockedRemoveColoredNotesOfChosenNumber = unlockedRemoveColoredNumberNotes;
        StaticVariables.unlockedRemoveAllOfNumber = unlockedRemoveAllOfNumber;
        StaticVariables.unlockedClearPuzzle = unlockedClearPuzzle;

        StaticVariables.skin = getSkinFromName(skinName, skins);
    }

    public Skin getSkinFromName(string name, Skin[] skins) {
        foreach (Skin s in skins) {
            if (s.skinName == name) {
                return s;
            }
        }
        return skins[0];
    }



}
