//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaunchGameSetup : MonoBehaviour {

    public Skin defaultSkin;
    public Skin[] skins; //a list of all skins. any new skin must be added to this list in the inspector
    public AudioSetup audioSetup;
    public Transform tweenDummy;

    private void Start() {
        StaticVariables.tweenDummy = tweenDummy;
        DontDestroyOnLoad(tweenDummy);
        audioSetup.Setup();
        StaticVariables.allSkins = skins;
        StaticVariables.unlockedSkins = new List<Skin> {defaultSkin};
        SaveSystem.LoadGame();
        CheckForVersionUpdate();
        DetermineOSType();

        if (StaticVariables.hasBeatenTutorial)
            SceneManager.LoadScene("MainMenu");
        else{
            StaticVariables.size = 3;
            StaticVariables.isTutorial = true;
            SceneManager.LoadScene("InPuzzle");
        }
    }
    private void DetermineOSType(){
        StaticVariables.osType = Application.platform switch {
            RuntimePlatform.Android => StaticVariables.OSTypes.Mobile,
            RuntimePlatform.IPhonePlayer => StaticVariables.OSTypes.Mobile,
            _ => StaticVariables.OSTypes.PC,
        };
        print("operating system type: " + StaticVariables.osType);
    }
    
    private void ChangeVersionNumber(float newVersionNum){
        print("updating version number! Old version " + StaticVariables.gameVersionNumber + ", new version " + newVersionNum);
        StaticVariables.gameVersionNumber = newVersionNum;
    }

    private void ShowUpdatePopupOnMainMenuLoad(string text){
        StaticVariables.showUpdatePopup = true;
        StaticVariables.updateText = text;
    
    }
    private void CheckForVersionUpdate(){
        StaticVariables.showUpdatePopup = false;
        if (StaticVariables.gameVersionNumber == 0){
            //this happens when the save data has no version number recorded
            //update to 2.1 immediately, then keep checking version numbers after that
            UpdateToVersion2_1();
        }
        if (StaticVariables.gameVersionNumber == 2.1f)
            ChangeVersionNumber(2.2f);
        if (StaticVariables.gameVersionNumber == 2.2f)
            ChangeVersionNumber(2.3f);
        if (StaticVariables.gameVersionNumber == 2.3f)
            ChangeVersionNumber(2.4f);
        if (StaticVariables.gameVersionNumber == 2.4f)
            UpdateToVersion2_5();
        if (StaticVariables.gameVersionNumber == 2.5f)
            UpdateToVersion3_0();
    }

    private void UpdateToVersion2_1(){
        ChangeVersionNumber(2.1f);
        //add up the coins that the player spent on skins minus the new cost of each skin
        //(meaning, count up how many coins the player is owed due to the changed cost of skins in the shop)
        int skinPurchasedCount = StaticVariables.unlockedSkins.Count - 1;
        int coinRefund = 0;
        if (skinPurchasedCount >= 1)
            coinRefund += (200 - 0);
        if (skinPurchasedCount >= 2)
            coinRefund += (200 - 10);
        if (skinPurchasedCount >= 3)
            coinRefund += (200 - 30);
        if (skinPurchasedCount >= 4)
            coinRefund += (200 - 60);
        if (skinPurchasedCount >= 5)
            coinRefund += (200 - 100);
        if (skinPurchasedCount >= 6)
            coinRefund += (200 - 150);
        //below are all the skins that cost more after the version update
        if (skinPurchasedCount >= 7)
            coinRefund += (200 - 210);
        if (skinPurchasedCount >= 8)
            coinRefund += (200 - 280);
        if (skinPurchasedCount >= 9)
            coinRefund += (200 - 360);
        if (skinPurchasedCount >= 10)
            coinRefund += (200 - 450);
        if (skinPurchasedCount >= 11)
            coinRefund += (200 - 550);
        if (skinPurchasedCount >= 12)
            coinRefund += (200 - 660);
        if (coinRefund > 0){
            StaticVariables.AddCoins(coinRefund);
            ShowUpdatePopupOnMainMenuLoad("CITYSCAPES HAS\nBEEN UPDATED!\n\nTHE COST OF SKINS IN THE SHOP HAS CHANGED, AND YOU HAVE BEEN REFUNDED " + coinRefund + " COINS FOR THE SKINS YOU PREVIOUSLY PURCHASED.");
        }
        SaveSystem.SaveGame();
    }

    private void UpdateToVersion2_5(){
        ChangeVersionNumber(2.5f);
        StaticVariables.ApplyDefaultKeybinds();
        SaveSystem.SaveGame();
    }
    
    private void UpdateToVersion3_0(){
        ChangeVersionNumber(3.0f);
        StaticVariables.globalVolume = 50;
        SaveSystem.SaveGame();
    }
}