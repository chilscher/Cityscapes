using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVariables{

    static public int size = 0;
    static public System.Random rand = new System.Random();
    static public bool isTutorial = false;
    static public bool includeNotes1Button = true;
    static public bool includeNotes2Button = false;
    static public bool changeResidentColorOnCorrectRows = false;
    static public bool includeUndoRedo = false;
    
    static public bool showMed = true;
    static public bool showLarge = true;
    static public bool showHuge = true;
    
    static public int highestUnlockedSize = 6;
    static public int coins = 100;

    static public bool isApplicationLaunchingFirstTime = true;

    static public string whiteHex = "#ffffff";
    static public string mintHex = "#98ff98";
    static public string greenHex = "#228b22";
    static public string redHex = "#ff0000";

    static public bool isFading;
    static public string fadingFrom;
    static public string fadingTo;
}
