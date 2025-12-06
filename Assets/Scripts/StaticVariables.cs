//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StaticVariables{
    //contains all of the variables that need to be tracked between scenes
    //the SaveData functionality reads and writes variables to and from this class
    
    //current puzzle information
    static public int size = 0;
    static public bool hasSavedPuzzleState = false;
    static public List<PuzzleState> previousPuzzleStates;
    static public PuzzleState currentPuzzleState;
    static public List<PuzzleState> nextPuzzleStates;
    static public string puzzleSolution;
    static public int savedPuzzleSize;
    static public int savedBuildNumber;
    static public GameManager.ClickTileActions savedBuildType;
    static public bool unlockedRemoveButtonFillsNotes = false;

    //if the player has purchased various upgrades, not including skins
    static public bool unlockedMedium = false;
    static public bool unlockedLarge = false;
    static public bool unlockedHuge = false;
    static public bool unlockedMassive = false;
    static public bool unlockedNotes1 = false;
    static public bool unlockedNotes2 = false;
    static public bool unlockedResidentsChangeColor = false;
    static public bool unlockedUndoRedo = false;
    static public bool unlockedRemoveAllOfNumber = false;
    static public bool unlockedClearPuzzle = false;
    static public bool unlockedHighlightBuildings = false;
    static public int highestUnlockedSize = 3;
    static public bool unlockedBuildingQuantityStatus = false;
    static public bool includeRemoveButtonFillsNotes = false;
    
    //if the player has any of the various upgrades toggled active, which is changed in settings
    static public bool includeNotes1Button = false;
    static public bool includeNotes2Button = false;
    static public bool changeResidentColorOnCorrectRows = false;
    static public bool includeUndoRedo = false;
    static public bool includeRemoveAllOfNumber = false;
    static public bool includeClearPuzzle = false;
    static public bool includeHighlightBuildings = false;
    static public bool hidePurchasedUpgrades = true;
    static public bool includeBuildingQuantityStatus = false;

    //variables that deal with fading in and out between scenes
    static public bool fadingIntoPuzzleSameSize;

    //variables that remember data about skins - equipped skins, unlocked skins, etc
    static public List<Skin> unlockedSkins = new List<Skin>();
    static public Skin skin;
    static public Skin[] allSkins;

    //about the tutorial
    static public bool isTutorial = false;
    static public string whiteHex = "#ffffff";
    static public bool hasBeatenTutorial = false;

    //keybinds
    static public KeyCode keybindBuilding1 = KeyCode.Alpha1;
    static public KeyCode keybindBuilding2 = KeyCode.Alpha2;
    static public KeyCode keybindBuilding3 = KeyCode.Alpha3;
    static public KeyCode keybindBuilding4 = KeyCode.Alpha4;
    static public KeyCode keybindBuilding5 = KeyCode.Alpha5;
    static public KeyCode keybindBuilding6 = KeyCode.Alpha6;
    static public KeyCode keybindBuilding7 = KeyCode.Alpha7;
    static public KeyCode keybindBuild = KeyCode.Q;
    static public KeyCode keybindNote1 = KeyCode.W;
    static public KeyCode keybindNote2 = KeyCode.E;
    static public KeyCode keybindErase = KeyCode.R;
    static public KeyCode keybindUndo = KeyCode.A;
    static public KeyCode keybindRedo = KeyCode.S;
    static public KeyCode keybindRemoveAll = KeyCode.D;
    static public KeyCode keybindClearPuzzle = KeyCode.F;

    //default keybinds
    
    static public KeyCode keybindBuilding1Default = KeyCode.Alpha1;
    static public KeyCode keybindBuilding2Default = KeyCode.Alpha2;
    static public KeyCode keybindBuilding3Default = KeyCode.Alpha3;
    static public KeyCode keybindBuilding4Default = KeyCode.Alpha4;
    static public KeyCode keybindBuilding5Default = KeyCode.Alpha5;
    static public KeyCode keybindBuilding6Default = KeyCode.Alpha6;
    static public KeyCode keybindBuilding7Default = KeyCode.Alpha7;
    static public KeyCode keybindBuildDefault = KeyCode.Q;
    static public KeyCode keybindNote1Default = KeyCode.W;
    static public KeyCode keybindNote2Default = KeyCode.E;
    static public KeyCode keybindEraseDefault = KeyCode.R;
    static public KeyCode keybindUndoDefault = KeyCode.A;
    static public KeyCode keybindRedoDefault = KeyCode.S;
    static public KeyCode keybindRemoveAllDefault = KeyCode.D;
    static public KeyCode keybindClearPuzzleDefault = KeyCode.F;

    //misc
    static public System.Random rand = new System.Random();
    static public int coins = 0;
    static public bool isApplicationLaunchingFirstTime = true;
    static private string sceneName = "";
    static public float sceneFadeDuration = 0.3f;
    static public Transform tweenDummy;
    static public Image fadeImage;
    static public float gameVersionNumber = 2.5f;
    public enum OSTypes {Mobile, PC};
    static public OSTypes osType;
    

    static public void WaitTimeThenCallFunction(float delay, TweenCallback function) {
        tweenDummy.DOLocalMove(tweenDummy.transform.localPosition, delay, false).OnComplete(function);
    }    
    static public void WaitTimeThenCallFunction(float delay, TweenCallback<string> function, string param) {
        tweenDummy.DOLocalMove(tweenDummy.transform.localPosition, delay, false).OnComplete(()=>function(param));
    }


    static public void FadeOutThenLoadScene(string name){
        sceneName = name;
        StartFadeDarken(sceneFadeDuration);
        WaitTimeThenCallFunction(sceneFadeDuration, LoadScene);
    }

    static public void FadeIntoScene(){
        StartFadeLighten(sceneFadeDuration);
    }

    static public void StartFadeDarken(float duration){
        Color currentColor = Color.black;
        currentColor.a = 0;
        fadeImage.color = currentColor;
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(Color.black, duration);
    }

    static public void StartFadeLighten(float duration){
        Color nextColor = Color.black;
        nextColor.a = 0;
        fadeImage.color = Color.black;
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(nextColor, duration).OnComplete(HideFadeObject);
    }

    static private void HideFadeObject(){
        fadeImage.gameObject.SetActive(false);
    }

    static private void LoadScene(){
        SceneManager.LoadScene(sceneName);
    }

    static public void StopFade(){
        DOTween.Kill(fadeImage);
    }

    static public void AddCoins(int amount) {
        coins += amount;
        //cap coins at 9,999,999
        if (coins > 9999999)
            coins = 9999999;
    }

}
