//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvasController : MonoBehaviour {
    //controls the main menu canvas. Only one is used, and only on the main menu scene.

    //buttons, and children of buttons, that the player interacts with from the main menu
    [Header("UI Elements to color")]
    public GameObject shopButton;
    public GameObject tutorialButton;
    public GameObject largeCenterTutorialButton;
    public GameObject settingsButton;
    public GameObject smallCityButton;
    public GameObject mediumCityButton;
    public GameObject largeCityButton;
    public GameObject hugeCityButton;
    public GameObject massiveCityButton;
    public GameObject returnToPuzzleButton;
    public GameObject abandonPuzzleButton;
    public Image progressPopupBorder;
    public Image progressPopupInside;

    [Header("Everything else")]
    public GameObject smallShadow;
    public GameObject mediumShadow;
    public GameObject largeShadow;
    public GameObject hugeShadow;
    public GameObject massiveShadow;
    public GameObject smallCityArt;
    public GameObject mediumCityArt;
    public GameObject largeCityArt;
    public GameObject hugeCityArt;
    public GameObject massiveCityArt;
    public GameObject puzzleButtons;
    public GameObject returnOrAbandonButtons;
    public GameObject background;
    public GameObject smallText;
    public GameObject mediumText;
    public GameObject largeText;
    public GameObject hugeText;
    public GameObject massiveText;
    public GameObject leftSmallText;
    public GameObject leftMediumText;
    public GameObject leftLargeText;
    public GameObject leftHugeText;
    public GameObject leftMassiveText;
    public GameObject returnSmall;
    public GameObject returnMedium;
    public GameObject returnLarge;
    public GameObject returnHuge;
    public GameObject returnMassive;
    public GameObject abandonSmall;
    public GameObject abandonMedium;
    public GameObject abandonLarge;
    public GameObject abandonHuge;
    public GameObject abandonMassive;

    public GameObject updatePopup;
    public Text updatePopupText;
    public GameObject confirmUpdateButton;
    public Image updatePopupBorder;
    public Image updatePopupInside;

    public Skin defaultSkin; //the default skin used when the player boots up the game for the first time

    public Skin[] skins; //a list of all skins. To work effectively, any new skin must be added to this list in the inspector


    private void Start() {
        //check to see if the game is being opened, or if the game is transitioning from another menu
        if (StaticVariables.isApplicationLaunchingFirstTime) {
            StaticVariables.allSkins = skins;
            StaticVariables.unlockedSkins = new List<Skin> {skins[0]}; //set up the current unlocked skin list to only include the basic skin (now called "rural")
            SaveSystem.LoadGame();
            CheckForVersionUpdate();
            StaticVariables.isApplicationLaunchingFirstTime = false;
        }

        //skip this temporarily for development
        
        if (!StaticVariables.hasBeatenTutorial){
            StaticVariables.StopFade();

            StaticVariables.size = 3;
            StaticVariables.isTutorial = true;
            SceneManager.LoadScene("InPuzzle");
            return;
        }
        

        //apply the current skin
        if (StaticVariables.skin)
            background.GetComponent<Image>().sprite = StaticVariables.skin.mainMenuBackground;
        ColorButtons();

        if (!updatePopup.activeSelf){
            ShowReturnAbandon();
            //update the puzzle buttons based on what puzzle sizes the player has unlocked
            ShowCityButtons();
        }
    }
    
    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO UPDATE THE VISUALS FOR THE MAIN MENU SCENE
    // ---------------------------------------------------

    private void ColorButtons() {
        //color all of the menu buttons to fit the current skin
        InterfaceFunctions.ColorMenuButton(smallCityButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(mediumCityButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(largeCityButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(hugeCityButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(massiveCityButton, StaticVariables.skin);

        InterfaceFunctions.ColorMenuButton(returnToPuzzleButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(abandonPuzzleButton, StaticVariables.skin);

        progressPopupBorder.color = StaticVariables.skin.popupBorder;
        progressPopupInside.color = StaticVariables.skin.popupInside;

        InterfaceFunctions.ColorMenuButton(confirmUpdateButton, StaticVariables.skin);
        updatePopupBorder.color = StaticVariables.skin.popupBorder;
        updatePopupInside.color = StaticVariables.skin.popupInside;

        InterfaceFunctions.ColorMenuButton(shopButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(tutorialButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(settingsButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(largeCenterTutorialButton, StaticVariables.skin);
    }

    private void ShowReturnAbandon() {
        if (StaticVariables.hasSavedPuzzleState) {
            leftSmallText.SetActive(StaticVariables.savedPuzzleSize == 3);
            returnSmall.SetActive(StaticVariables.savedPuzzleSize == 3);
            abandonSmall.SetActive(StaticVariables.savedPuzzleSize == 3);
            returnMedium.SetActive(StaticVariables.savedPuzzleSize == 4);
            abandonMedium.SetActive(StaticVariables.savedPuzzleSize == 4);
            leftMediumText.SetActive(StaticVariables.savedPuzzleSize == 4);
            returnLarge.SetActive(StaticVariables.savedPuzzleSize == 5);
            abandonLarge.SetActive(StaticVariables.savedPuzzleSize == 5);
            leftLargeText.SetActive(StaticVariables.savedPuzzleSize == 5);
            returnHuge.SetActive(StaticVariables.savedPuzzleSize == 6);
            abandonHuge.SetActive(StaticVariables.savedPuzzleSize == 6);
            leftHugeText.SetActive(StaticVariables.savedPuzzleSize == 6);
            returnMassive.SetActive(StaticVariables.savedPuzzleSize == 7);
            abandonMassive.SetActive(StaticVariables.savedPuzzleSize == 7);
            leftMassiveText.SetActive(StaticVariables.savedPuzzleSize == 7);
        }
    }

    private void ShowCityButtons() {
        //shows or hides buttons on the menu depending on if the player has completed the tutorial, has a puzzle currently in progress, or has not unlocked all of the puzzle size upgrades yet
        largeCenterTutorialButton.SetActive(false);
        returnOrAbandonButtons.SetActive(false);
        shopButton.SetActive(true);
        tutorialButton.SetActive(true);
        settingsButton.SetActive(true);
        smallShadow.SetActive(false);
        mediumShadow.SetActive(false);
        largeShadow.SetActive(false);
        hugeShadow.SetActive(false);
        massiveShadow.SetActive(false);
        smallText.SetActive(true);
        mediumText.SetActive(true);
        largeText.SetActive(true);
        hugeText.SetActive(true);
        massiveText.SetActive(true);

        int highestUnlockedSize = GetHighestUnlockedSize();
        if (StaticVariables.hasSavedPuzzleState) {
            puzzleButtons.SetActive(false);
            returnOrAbandonButtons.SetActive(true);
        }
        else {
            puzzleButtons.SetActive(true);
            smallShadow.SetActive(!(highestUnlockedSize >= 3));
            smallCityArt.SetActive(highestUnlockedSize >= 3);
            mediumShadow.SetActive(!(highestUnlockedSize >= 4));
            mediumCityArt.SetActive(highestUnlockedSize >= 4);
            largeShadow.SetActive(!(highestUnlockedSize >= 5));
            largeCityArt.SetActive(highestUnlockedSize >= 5);
            hugeShadow.SetActive(!(highestUnlockedSize >= 6));
            hugeCityArt.SetActive(highestUnlockedSize >= 6);
            massiveShadow.SetActive(!(highestUnlockedSize >= 7));
            massiveCityArt.SetActive(highestUnlockedSize >= 7);

            smallText.SetActive(highestUnlockedSize >= 3);
            mediumText.SetActive(highestUnlockedSize >= 4);
            largeText.SetActive(highestUnlockedSize >= 5);
            hugeText.SetActive(highestUnlockedSize >= 6);
            massiveText.SetActive(highestUnlockedSize >= 7);
        }

        //skip this temporarily for development
        
        if (!StaticVariables.hasBeatenTutorial) {
            returnOrAbandonButtons.SetActive(false);
            shopButton.SetActive(false);
            tutorialButton.SetActive(false);
            settingsButton.SetActive(false);

            puzzleButtons.SetActive(false);
            largeCenterTutorialButton.SetActive(true);            
        }
    }

    public void AutoStartTutorial(){
        if (StaticVariables.hasBeatenTutorial)
            return;
        else
            PushStartTutorialButton();
    }

    public int GetHighestUnlockedSize() {
        //gets the highest size puzzle that the player has unlocked as an int
        int highestUnlockedSize = 3;
        if (StaticVariables.unlockedMedium) {
            highestUnlockedSize = 4;
        }
        if (StaticVariables.unlockedLarge) {
            highestUnlockedSize = 5;
        }
        if (StaticVariables.unlockedHuge) {
            highestUnlockedSize = 6;
        }
        if (StaticVariables.unlockedMassive) {
            highestUnlockedSize = 7;
        }
        return highestUnlockedSize;
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO LEAVE THE MAIN MENU SCENE
    // ---------------------------------------------------
    
    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void PushStartPuzzleButton(int size) {
        //start fading out, and after the fade-out process is completed, go to the puzzle scene
        if (size > GetHighestUnlockedSize())
            return;
        StaticVariables.size = size;
        StaticVariables.isTutorial = false;
        StaticVariables.FadeOutThenLoadScene("InPuzzle");
    }
    
    public void PushStartTutorialButton() {
        //start fading out, and after the fade-out process is completed, go to the puzzle scene, in tutorial mode
        StaticVariables.size = 3;
        StaticVariables.isTutorial = true;
        StaticVariables.FadeOutThenLoadScene("InPuzzle");
    }

    public void PushShopButton() {
        StaticVariables.FadeOutThenLoadScene("Shop");
    }

    public void PushSettingsButton() {
        StaticVariables.FadeOutThenLoadScene("Settings");
    }

    public void PushReturnToPuzzleButton() {
        StaticVariables.size = StaticVariables.savedPuzzleSize;
        StaticVariables.isTutorial = false;
        StaticVariables.FadeOutThenLoadScene("InPuzzle");
    }

    public void PushAbandonPuzzleButton() {
        //start fading out, and after the fade-out process is completed, reload the main menu scene, but without the "return/abandon puzzle" options
        StaticVariables.hasSavedPuzzleState = false;
        SaveSystem.SaveGame();
        StaticVariables.FadeOutThenLoadScene("MainMenu");
    }

    
    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO UPDATE GAME VERSION
    // ---------------------------------------------------
    
    private void CheckForVersionUpdate(){
        updatePopup.SetActive(false);
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
            ShowUpdatePopup("CITYSCAPES HAS\nBEEN UPDATED!\n\nTHE COST OF SKINS IN THE SHOP HAS CHANGED, AND YOU HAVE BEEN REFUNDED " + coinRefund + " COINS FOR THE SKINS YOU PREVIOUSLY PURCHASED.");
        }
        SaveSystem.SaveGame();
    }

    private void ChangeVersionNumber(float newVersionNum){
        print("updating version number! Old version " + StaticVariables.gameVersionNumber + ", new version " + newVersionNum);
        StaticVariables.gameVersionNumber = newVersionNum;
    }

    private void ShowUpdatePopup(string text){
        updatePopupText.text = text.ToUpper();
        updatePopup.SetActive(true);
        
        puzzleButtons.SetActive(false);
        returnOrAbandonButtons.SetActive(false);
        shopButton.SetActive(false);
        tutorialButton.SetActive(false);
        settingsButton.SetActive(false);
    }

    public void PushedConfirmUpdateButton(){
        StaticVariables.FadeOutThenLoadScene("MainMenu");
    }
}