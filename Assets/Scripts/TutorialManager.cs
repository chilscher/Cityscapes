//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class TutorialManager{
    //runs the tutorial, following a specific set of steps in a defined order
    //the tutorial goes through 2 full puzzles

    public string puzzle = "231312123"; //the first puzzle
    public GameManager gameManager; //the tutorial relies heavily on functions from the gameManager
    private int tutorialStage = 0; //each time you complete a tutorial step, increment this int. Its value determines what to do next
    public string advanceRequirement; //the requirement to complete the next tutorial step
    public Text tutorialText; //the text shown on the text box on the bottom of the screen to guide you through the tutorial
    public Text continueClue; //at the bottom of the tutorial text box, there is a small hint saying more clearly what you specifically need to do to proceed
    private string text;
    private string continueText;
    private string skipRequirement = ""; //if you complete this requirement, skip a few steps until you get to "skipToStage"
    private int skipToStage = 0; //skipToStage is changed every few steps
    private GameObject number2Highlight;
    private GameObject tile1Highlight;
    private GameObject street1Highlight;
    private GameObject street2Highlight;
    private GameObject resident1Highlight;
    private GameObject residentAndStreet1Highlight;
    private GameObject street3Highlight;
    private GameObject street4Highlight;
    private GameObject resident2Highlight;
    private GameObject resident3Highlight;
    private GameObject resident4Highlight;
    private GameObject resident5Highlight;
    private GameObject street5Highlight;
    private GameObject residentAndStreet2Highlight;


    public void StartTutorial() {
        //begins the tutorial process. Calls functions here to alter the default gameManager setup
        gameManager.puzzlePositioning = gameManager.tutorialParent.transform.Find("Puzzle Positioning").gameObject;
        gameManager.puzzleGenerator.usePredeterminedSolution = true;
        gameManager.puzzleGenerator.predeterminedSolution = puzzle;
        gameManager.puzzleGenerator.CreatePuzzle(3);
        gameManager.DrawFullPuzzle();
        gameManager.HideHints();
        gameManager.SetSelectionModeButtons();
        gameManager.PushBuildButton();
        tutorialText = gameManager.tutorialTextBox;
        continueClue = gameManager.tutorialContinueClue;
        number2Highlight = gameManager.tutorialHighlightsParent.GetChild(0).gameObject;
        tile1Highlight = gameManager.tutorialHighlightsParent.GetChild(1).gameObject;
        street1Highlight = gameManager.tutorialHighlightsParent.GetChild(2).gameObject;
        street2Highlight = gameManager.tutorialHighlightsParent.GetChild(3).gameObject;
        resident1Highlight = gameManager.tutorialHighlightsParent.GetChild(4).gameObject;
        residentAndStreet1Highlight = gameManager.tutorialHighlightsParent.GetChild(5).gameObject;
        street3Highlight = gameManager.tutorialHighlightsParent.GetChild(6).gameObject;
        street4Highlight = gameManager.tutorialHighlightsParent.GetChild(7).gameObject;
        resident2Highlight = gameManager.tutorialHighlightsParent.GetChild(8).gameObject;
        resident3Highlight = gameManager.tutorialHighlightsParent.GetChild(9).gameObject;
        resident4Highlight = gameManager.tutorialHighlightsParent.GetChild(10).gameObject;
        resident5Highlight = gameManager.tutorialHighlightsParent.GetChild(11).gameObject;
        street5Highlight = gameManager.tutorialHighlightsParent.GetChild(12).gameObject;
        residentAndStreet2Highlight = gameManager.tutorialHighlightsParent.GetChild(13).gameObject;
        foreach (Transform t in gameManager.tutorialHighlightsParent)
            t.gameObject.SetActive(false);

        gameManager.tutorialParent.transform.Find("Numbers").gameObject.SetActive(false);
        AdvanceStage();
    }

    // ---------------------------------------------------
    //ADVANCE STAGE IS THE CORE OF THE TUTORIAL, WHERE ALL OF THE TEXT, CONTINUE REQUIREMENTS, VISUAL UPDATES, AND SPECIAL TUTORIAL MECHANICS ARE IMPLEMENTED
    // ---------------------------------------------------

    private void AdvanceStage() {
        tutorialStage ++;
        //Debug.Log(tutorialStage);
        int i = 0;
        if (++i == tutorialStage){
            text = "Welcome to Cityscapes!\n\nIn Cityscapes, you are an urban planner, and your job is to build a city to fit its new residents' wishes.";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(0);
        }
        else if (++i == tutorialStage){
            text = "The above 3x3 grid is your city, and each space on the grid can hold one building.";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(1);
        }
        else if (++i == tutorialStage){
            text = "The buildings you place will either be one story tall...";
            continueText = "Tap to continue...";
            FillInSpace(6, 1);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(2);
        }
        else if (++i == tutorialStage){
            text = "The buildings you place will either be one story tall...\ntwo stories tall...";
            continueText = "Tap to continue...";
            FillInSpace(7, 2);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
        }
        else if (++i == tutorialStage){
            text = "The buildings you place will either be one story tall...\ntwo stories tall...\nor three stories tall.";
            continueText = "Tap to continue...";
            FillInSpace(8, 3);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
        }
        else if (++i == tutorialStage){
            text = "To place a building, first you must tap a building size to select it...";
            continueText = "Choose the right building size...";
            gameManager.tutorialParent.transform.Find("Numbers").gameObject.SetActive(true);
            number2Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap number button 2";
            ShowTutorailCompletionPercentage(3);
        }
        else if (++i == tutorialStage){
            text = "To place a building, first you must tap a building size to select it...\n\nthen tap the space you would like to build on.";
            continueText = "Place the building...";
            number2Highlight.SetActive(false);
            tile1Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "add building of height 2 to tile 0";
        }
        else if (++i == tutorialStage){
            text = "Congratulations! You have built your first building!";
            continueText = "Tap to continue...";
            tile1Highlight.SetActive(false);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(4);
        }
        else if (++i == tutorialStage){
            text = "The residents have very particular requirements for their perfect city.";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(5);
        }
        else if (++i == tutorialStage){
            text = "Firstly, every street has to contain exactly one building of each height.";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            skipRequirement = "add building of height 3 to tile 3";
            skipToStage = 14;
            ShowTutorailCompletionPercentage(6);
        }
        else if (++i == tutorialStage){
            text = "Firstly, every street has to contain exactly one building of each height.\n\nThese three buildings form a street and already satisfy this rule.";
            continueText = "Tap to continue...";
            street1Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
        }
        else if (++i == tutorialStage){
            text = "However, this street does not yet have one of every building!";
            continueText = "Tap to continue...";
            street1Highlight.SetActive(false);
            street2Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(7);
        }
        else if (++i == tutorialStage){
            text = "However, this street does not yet have one of every building!\n\nIt still needs a three-story building in the middle.";
            continueText = "Place the correct building...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "add building of height 3 to tile 3";
            skipRequirement = "";
        }
        else if (++i == tutorialStage){
            text = "Well done! The second building requirement involves the residents of the city...";
            continueText = "Tap to continue...";
            street2Highlight.SetActive(false);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(8);
        }
        else if (++i == tutorialStage){
            text = "Well done! The second building requirement involves the residents of the city...\n\nwho are now standing at the ends of every street!";
            continueText = "Tap to continue...";
            gameManager.ShowHints();
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
        }
        else if (++i == tutorialStage){
            text = "This resident...";
            continueText = "Tap to continue...";
            resident1Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(9);
        }
        else if (++i == tutorialStage){
            text = "This resident...\nis looking down this street...";
            continueText = "Tap to continue...";
            resident1Highlight.SetActive(false);
            residentAndStreet1Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
        }
        else if (++i == tutorialStage){
            text = "This resident...\nis looking down this street...\nand wants to see only one building.";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            skipRequirement = "add building of height 3 to tile 1";
            skipToStage = 21;
        }
        else if (++i == tutorialStage){
            text = "This resident...\nis looking down this street...\nand wants to see only one building.\nTherefore, the building closest to them has to be the tallest one on the street!";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
        }
        else if (++i == tutorialStage){
            text = "Place the tallest building on the street closest to the resident.";
            continueText = "Place the correct building...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "add building of height 3 to tile 1";
            skipRequirement = "";
            ShowTutorailCompletionPercentage(10);
        }
        else if (++i == tutorialStage){
            text = "Awesome!";
            continueText = "Tap to continue...";
            residentAndStreet1Highlight.SetActive(false);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(11);
        }
        else if (++i == tutorialStage){
            text = "Awesome!\n\nNow, you can fill in the last missing building on the topmost street.";
            continueText = "Place the correct building...";
            street3Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "add building of height 1 to tile 2";
        }
        else if (++i == tutorialStage){
            text = "And now you can fill in the missing building on the middle street.";
            continueText = "Place the correct building...";
            street3Highlight.SetActive(false);
            street4Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "add building of height 1 to tile 4";
            ShowTutorailCompletionPercentage(12);
        }
        else if (++i == tutorialStage){
            text = "And finally, you can fill in the last remaining building of the city.";
            continueText = "Place the correct building...";
            street4Highlight.SetActive(false);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "add building of height 2 to tile 5";
            ShowTutorailCompletionPercentage(13);
        }
        else if (++i == tutorialStage){
            text = "Congratulations! You have completed your first city in Cityscapes!";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(14);
        }
        else if (++i == tutorialStage){
            text = "Congratulations! You have completed your first city in Cityscapes!\n\nNow let's start a new city from scratch!";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
        }
        else if (++i == tutorialStage){
            text = "From now on, every city will have at least one building already in place.";
            continueText = "Tap to continue...";
            puzzle = "213132321";
            DeleteOldCity();
            gameManager.puzzleGenerator.predeterminedSolution = puzzle;
            gameManager.puzzleGenerator.predeterminedPermanentBuilding = 0;
            gameManager.puzzleGenerator.CreatePuzzle(3);
            gameManager.puzzleGenerator.AutofillPermanentBuildings();
            gameManager.DrawFullPuzzle();
            gameManager.HideHints();
            tile1Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(15);
        }
        else if (++i == tutorialStage){
            text = "Additionally, you can now see all residents from the start!";
            continueText = "Tap to continue...";
            gameManager.ShowHints();
            tile1Highlight.SetActive(false);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(16);
        }
        else if (++i == tutorialStage){
            text = "The best place to start when building a city is to fill in all the tallest buildings first.";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(17);
        }
        else if (++i == tutorialStage){
            text = "These four residents only want to see one building...";
            continueText = "Tap to continue...";
            resident2Highlight.SetActive(true);
            resident3Highlight.SetActive(true);
            resident4Highlight.SetActive(true);
            resident5Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            skipRequirement = "add buildings of height 3 to tiles 2 and 6";
            skipToStage = tutorialStage + 2;
            ShowTutorailCompletionPercentage(18);
        }
        else if (++i == tutorialStage){
            text = "These four residents only want to see one building...\n\nso you know the buildings next to them have to be three stories tall.";
            continueText = "Place the correct buildings...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "add buildings of height 3 to tiles 2 and 6";
            skipRequirement = "";
        }
        else if (++i == tutorialStage){
            text = "Well done!";
            continueText = "Tap to continue...";
            resident2Highlight.SetActive(false);
            resident3Highlight.SetActive(false);
            resident4Highlight.SetActive(false);
            resident5Highlight.SetActive(false);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(19);
        }
        else if (++i == tutorialStage){
            text = "Well done!\nNow, you can figure out where the final three-story building has to go! Remember, only one building of each size can go on each street!";
            continueText = "Place the correct building...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "add building of height 3 to tile 4";
        }
        else if (++i == tutorialStage){
            text = "Good work!";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            ShowTutorailCompletionPercentage(20);
        }
        else if (++i == tutorialStage){
            text = "Good work!\nNow, let's take a look at this street.";
            street5Highlight.SetActive(true);
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
        }
        else if (++i == tutorialStage){
            text = "The resident wants to see all three buildings on their street...";
            continueText = "Tap to continue...";
            street5Highlight.SetActive(false);
            residentAndStreet2Highlight.SetActive(true);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            skipRequirement = "finish rightmost street";
            skipToStage = tutorialStage + 2;
            ShowTutorailCompletionPercentage(21);
        }
        else if (++i == tutorialStage) {
            text = "The resident wants to see all three buildings on their street...\nThe only way to satisfy the resident is to line up all three buildings from shortest to tallest!";
            continueText = "Place the correct buildings...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "finish rightmost street";
            skipRequirement = "";
        }
        else if (++i == tutorialStage){
            text = "Great job!\n\nNow you know enough to complete the rest of the city!";
            continueText = "Complete the city!";
            residentAndStreet2Highlight.SetActive(false);
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "complete the city";
            ShowTutorailCompletionPercentage(22);
        }
        else if (++i == tutorialStage){
            text = "Congratulations! You have completed the tutorial for Cityscapes.";
            continueText = "Tap to continue...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
            StaticVariables.hasBeatenTutorial = true;
            SaveSystem.SaveGame();
            ShowTutorailCompletionPercentage(23); //stage 23, should be when 100% completion shows
        }
        else if (++i == tutorialStage){
            text = "Congratulations! You have completed the tutorial for Cityscapes.\n\nGo try a puzzle on your own! You can revisit this tutorial at any time.";
            continueText = "Tap to exit...";
            tutorialText.text = text;
            continueClue.text = continueText;
            advanceRequirement = "tap screen";
        }
        else if (++i == tutorialStage){
            if (StaticVariables.highestUnlockedSize < 3)
                StaticVariables.highestUnlockedSize = 3;
            gameManager.PushMainMenuButton();
        }
    }

    private void ShowTutorailCompletionPercentage(int completionStage){
        //the max tutorial completion happens when completionStage is 23
        //the current convention is to call this function each time a new page of text shows up
        //for example, "one story, two stories, or three stories tall" come out in different tutorial stages, but they all add to the same page of text
        //therefore, advancing between those tutorial stages does not call this function
        float percentage = ((completionStage * 1f) / (23f)) * 100f;
        if (percentage > 100)
            percentage = 100;
        int roundedPercentage = (int)Mathf.Round(percentage);

        //update the tutorial progress text
        string s = "Tutorial Progress - " + roundedPercentage + "% Complete";
        if (roundedPercentage == 100)
            s = "Tutorial Complete!";
        gameManager.tutorialProgressText.text = s;

        //update the bar progress
        int maxWidth = 1400;
        float newWidth = maxWidth * (percentage / 100);
        float roundedWidth = (Mathf.Round(newWidth * 100) / 100);
        Vector2 v = gameManager.tutorialProgressBar.sizeDelta;
        v.x = roundedWidth;
        gameManager.tutorialProgressBar.sizeDelta = v;
    }

    // ---------------------------------------------------
    //HERE ARE THE FUNCTIONS THAT ARE USED TO ADVANCE THE PLAYER THROUGH THE TUTORIAL
    // ---------------------------------------------------

    private void SkipStage() {
        //skips to a stage defined by the current stage in the tutorial
        while(tutorialStage < skipToStage)
            AdvanceStage();
    }
    
    public void TappedScreen() {
        //when the player taps the screen, sometimes that is enough to advance to another dialogue box, or stage
        if (advanceRequirement == "tap screen")
            AdvanceStage();
    }

    public void TappedNumberButton(int num) {
        //tapping a number button is sometimes required to advance the stage
        if (advanceRequirement == "tap number button " + num)
            AdvanceStage();
    }

    public void ClickedTile(PuzzleTile t) {
        //some stages require the player to input a specific number into a specific tile
        //some stages require the player to input several numbers in sequence to advance
        int chosenNumber = gameManager.selectedNumber;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                int tileNum = (i * 3) + j;
                if (gameManager.puzzleGenerator.tilesArray[i, j] == t) {
                    if (advanceRequirement == "add building of height " + chosenNumber + " to tile " + tileNum)
                        AdvanceStage();
                    if (skipRequirement == "add building of height " + chosenNumber + " to tile " + tileNum)
                        SkipStage();
                }
            }
        }

        if (advanceRequirement == "add buildings of height 3 to tiles 2 and 6") {
            PuzzleTile[,] ts = gameManager.puzzleGenerator.tilesArray;
            if (ts[0, 2].shownNumber == 3 && ts[2,0].shownNumber == 3)
                AdvanceStage();
        }
        if (skipRequirement == "add buildings of height 3 to tiles 2 and 6") {
            PuzzleTile[,] ts = gameManager.puzzleGenerator.tilesArray;
            if (ts[0, 2].shownNumber == 3 && ts[2, 0].shownNumber == 3)
                SkipStage();
        }
        if (advanceRequirement == "complete the city") {
            if (gameManager.puzzleGenerator.CheckPuzzle())
                AdvanceStage();
        }
        if (advanceRequirement == "finish rightmost street") {
            PuzzleTile[,] ts = gameManager.puzzleGenerator.tilesArray;
            if (ts[2, 2].shownNumber == 1 && ts[1, 2].shownNumber == 2)
                AdvanceStage();
        }
        if (skipRequirement == "finish rightmost street") {
            PuzzleTile[,] ts = gameManager.puzzleGenerator.tilesArray;
            if (ts[2, 2].shownNumber == 1 && ts[1, 2].shownNumber == 2)
                SkipStage();
        }
    }

    private void FillInSpace(int spaceNum, int value) {
        //the first few stages fill in some basic puzzle numbers for you automatically as you proceed
        //takes a number corresponding to the relevant space, and a value to fill it with, and adds that number to that space
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (((i * 3) + j) == spaceNum){
                    PuzzleTile t = gameManager.puzzleGenerator.tilesArray[i, j];
                    t.ToggleNumber(value);
                }
            }
        }
    }

    private void DeleteOldCity() {
        //clears the entire city to make room for a new one. Called exactly once in the tutorial
        gameManager.DeleteCityForTutorial();
    }

    public bool CanPlayerClickTile(PuzzleTile t) {
        //when the player clicks a tile during the tutorial, this function is called
        //if the tutorial is at the right stage to process that input, it is processed here
        int chosenNumber = gameManager.selectedNumber;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                int tileNum = (i * 3) + j;
                if (gameManager.puzzleGenerator.tilesArray[i, j] == t) {
                    if (advanceRequirement == "add building of height " + chosenNumber + " to tile " + tileNum)
                        return true;
                    if (skipRequirement == "add building of height " + chosenNumber + " to tile " + tileNum)
                        return true;
                }
            }
        }
        if (advanceRequirement == "add buildings of height 3 to tiles 2 and 6") {
            PuzzleTile[,] ts = gameManager.puzzleGenerator.tilesArray;
            if (gameManager.selectedNumber == 3) {
                if (ts[0, 2] == t || ts[2, 0] == t)
                    return true;
            }
        }
        if (skipRequirement == "add buildings of height 3 to tiles 2 and 6") {
            PuzzleTile[,] ts = gameManager.puzzleGenerator.tilesArray;
            if (gameManager.selectedNumber == 3) {
                if (ts[0, 2] == t || ts[2, 0] == t)
                    return true;
            }
        }
        if (advanceRequirement == "finish rightmost street") {
            PuzzleTile[,] ts = gameManager.puzzleGenerator.tilesArray;
            if ((gameManager.selectedNumber == 1) || (gameManager.selectedNumber == 2)) {
                if (ts[2, 2] == t || ts[1, 2] == t)
                    return true;
            }
        }
        if (skipRequirement == "finish rightmost street") {
            PuzzleTile[,] ts = gameManager.puzzleGenerator.tilesArray;
            if ((gameManager.selectedNumber == 1) || (gameManager.selectedNumber == 2)) {
                if (ts[2, 2] == t || ts[1, 2] == t)
                    return true;
            }
        }
        if (advanceRequirement == "complete the city") {
            if (t.shownNumber == 0) {
                if (t.solution == gameManager.selectedNumber)
                    return true;
            }
        }
        return false;
    }
}