using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVariables{

    static public int size = 0;
    static public System.Random rand = new System.Random();
    static public bool isTutorial = false;
    static public bool includeNotes1Button = false;
    static public bool includeNotes2Button = false;
    static public bool changeResidentColorOnCorrectRows = false;
    static public bool includeUndoRedo = false;
    //static public bool includeRemoveColoredNotesOfChosenNumber = false;
    static public bool includeRemoveAllOfNumber = false;
    static public bool includeClearPuzzle = false;
    
    static public bool showMed = false;
    static public bool showLarge = false;
    static public bool showHuge = false;
    
    static public int highestUnlockedSize = 3;
    static public int coins = 1000;

    static public bool isApplicationLaunchingFirstTime = true;

    static public string whiteHex = "#ffffff";
    static public string mintHex = "#98ff98";
    static public string greenHex = "#228b22";
    static public string redHex = "#ff0000";
    static public string blueHex = "#3552ca";
    static public string lightRedHex = "#f85f65";
    static public string lightBlueHex = "#99d9ea";
    static public string darkMintHex = "";

    static public bool isFading;
    static public string fadingFrom;
    static public string fadingTo;
    static public bool fadingIntoPuzzleSameSize;

    static public bool unlockedMedium = false;
    static public bool unlockedLarge = false;
    static public bool unlockedHuge = false;
    static public bool unlockedNotes1 = false;
    static public bool unlockedNotes2 = false;
    static public bool unlockedResidentsChangeColor = false;
    static public bool unlockedUndoRedo = false;
    //static public bool unlockedRemoveColoredNotesOfChosenNumber = false;
    static public bool unlockedRemoveAllOfNumber = false;
    static public bool unlockedClearPuzzle = false;
}
