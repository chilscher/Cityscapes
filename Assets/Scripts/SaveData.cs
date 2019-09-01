using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData{

    public int coins;
    public bool includeRedNotes;
    public bool includeGreenNotes;
    public bool changeResidentColorOnCorrectRows;
    public bool includeUndoRedo;
    public int highestUnlockedSize;
    public bool showMed;
    public bool showLarge;
    public bool showHuge;


    public SaveData() {
        coins = StaticVariables.coins;
        includeRedNotes = StaticVariables.includeRedNoteButton;
        includeGreenNotes = StaticVariables.includeGreenNoteButton;
        changeResidentColorOnCorrectRows = StaticVariables.changeResidentColorOnCorrectRows;
        highestUnlockedSize = StaticVariables.highestUnlockedSize;
        includeUndoRedo = StaticVariables.includeUndoRedo;

        showMed = StaticVariables.showMed;
        showLarge = StaticVariables.showLarge;
        showHuge = StaticVariables.showHuge;

    }

    public void LoadData() {

        StaticVariables.coins = coins;
        StaticVariables.includeGreenNoteButton = includeGreenNotes;
        StaticVariables.includeRedNoteButton = includeRedNotes;
        StaticVariables.changeResidentColorOnCorrectRows = changeResidentColorOnCorrectRows;
        StaticVariables.highestUnlockedSize = highestUnlockedSize;
        StaticVariables.includeUndoRedo = includeUndoRedo;

        StaticVariables.showMed = showMed;
        StaticVariables.showLarge = showLarge;
        StaticVariables.showHuge = showHuge;
    }



}
