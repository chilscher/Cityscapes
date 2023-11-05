﻿//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public List<GameObject> redBorders;

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
        tutorialText = gameManager.tutorialTextBox.transform.Find("Text").GetComponent<Text>();
        continueClue = gameManager.tutorialTextBox.transform.Find("Continue clue").GetComponent<Text>();
        redBorders = new List<GameObject>();
        foreach (Transform t in gameManager.tutorialParent.transform.Find("Tile Borders").transform)
            redBorders.Add(t.gameObject);
        gameManager.tutorialParent.transform.Find("Numbers").gameObject.SetActive(false);
        //proceed to stage 1
        AdvanceStage();
    }

    // ---------------------------------------------------
    //ADVANCE STAGE IS THE CORE OF THE TUTORIAL, WHERE ALL OF THE TEXT, CONTINUE REQUIREMENTS, VISUAL UPDATES, AND SPECIAL TUTORIAL MECHANICS ARE IMPLEMENTED
    // ---------------------------------------------------

    private void AdvanceStage() {
        //each stage has its own text, required step to proceed, and stage you can jump to if you complete a specific requirement ahead of time
        tutorialStage++;

        switch (tutorialStage) {
            case 1:
                text = "Welcome to Cityscapes! This is a number-placement puzzle game. In Cityscapes, you are a city designer, and your job is to build a city to fit its new residents' wishes.";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 2:
                text = "The above 3x3 grid is your city, and each space on the grid can hold one building.";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 3:
                text = "The buildings you place will either be one story...";
                continueText = "Tap to continue...";
                FillInSpace(6, 1);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 4:
                text = "The buildings you place will either be one story...\ntwo stories...";
                continueText = "Tap to continue...";
                FillInSpace(7, 2);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 5:
                text = "The buildings you place will either be one story...\ntwo stories...\nor three stories tall.";
                continueText = "Tap to continue...";
                FillInSpace(8, 3);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 6:
                text = "To place a building, tap the building size you would like to place...";
                continueText = "Choose the right building size...";
                gameManager.tutorialParent.transform.Find("Numbers").gameObject.SetActive(true);
                AddRedBoxAroundNumButton(2);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap number button 2";
                break;
            case 7:
                text = "To place a building, tap the building size you would like to place...\n\nthen tap the space you would like to build on.";
                continueText = "Place the building...";
                RemoveRedBoxesAroundNums();
                AddRedBoxAroundTile(0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 2 to tile 0";
                break;
            case 8:
                text = "Congratulations! You have built your first building!";
                continueText = "Tap to continue...";
                RemoveRedBoxesAroundTiles();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 9:
                text = "The residents have very particular requirements for their perfect city.";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 10:
                text = "Firstly, every street has to contain exactly one building of each height.";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                skipRequirement = "add building of height 3 to tile 3";
                skipToStage = 14;
                break;
            case 11:
                text = "Firstly, every street has to contain exactly one building of each height.\n\nThese three buildings form a street and already satisfy this rule.";
                continueText = "Tap to continue...";
                AddRedBoxAroundStreet("horizontal", 2);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 12:
                text = "However, this street does not yet have one of every building!";
                continueText = "Tap to continue...";
                RemoveRedBoxesAroundStreets();
                AddRedBoxAroundStreet("vertical", 0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 13:
                text = "However, this street does not yet have one of every building!\n\nIt still needs a three-story building in the middle.";
                continueText = "Place the correct building...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 3 to tile 3";
                skipRequirement = "";
                break;
            case 14:
                text = "Great! The second building requirement involves the residents of the city...";
                continueText = "Tap to continue...";
                RemoveRedBoxesAroundStreets();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 15:
                text = "Great! The second building requirement involves the residents of the city...\n\nwho are now standing at the ends of every street!";
                continueText = "Tap to continue...";
                gameManager.ShowHints();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 16:
                text = "This resident...";
                continueText = "Tap to continue...";
                AddRedBoxAroundResident("top", 1);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 17:
                text = "This resident...\nis looking down this street...";
                continueText = "Tap to continue...";
                AddRedBoxAroundResident("top", 1);
                AddRedBoxAroundStreet("vertical", 1);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 18:
                text = "This resident...\nis looking down this street...\nand only wants to be able to see one building.";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                skipRequirement = "add building of height 3 to tile 1";
                skipToStage = 21;
                break;
            case 19:
                text = "This resident...\nis looking down this street...\nand only wants to be able to see one building.\nSo we know that the building closest to them has to be the tallest one on the whole street!";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 20:
                text = "Place the tallest building on the street closest to the resident.";
                continueText = "Place the correct building...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 3 to tile 1";
                skipRequirement = "";
                break;
            case 21:
                text = "Awesome!";
                continueText = "Tap to continue...";
                RemoveRedBoxesAroundStreets();
                RemoveRedBoxesAroundResidents();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 22:
                text = "Awesome!\n\nNow, you can fill in the last missing building on the topmost street.";
                continueText = "Place the correct building...";
                AddRedBoxAroundStreet("horizontal", 0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 1 to tile 2";
                break;
            case 23:
                text = "And now you can fill in the missing building on the middle street.";
                continueText = "Place the correct building...";
                RemoveRedBoxesAroundStreets();
                AddRedBoxAroundStreet("vertical", 1);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 1 to tile 4";
                break;
            case 24:
                text = "And finally, you can fill in the last remaining building of the city.";
                continueText = "Place the correct building...";
                RemoveRedBoxesAroundStreets();
                AddRedBoxAroundTile(5);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 2 to tile 5";
                break;
            case 25:
                text = "Congratulations! You have completed your first city in Cityscapes!";
                continueText = "Tap to continue...";
                RemoveRedBoxesAroundTiles();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 26:
                text = "Congratulations! You have completed your first city in Cityscapes!\n\nNow let's start a new city from scratch!";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 27:
                text = "Welcome to another city. From now on, you can see all residents from the start!";
                continueText = "Tap to continue...";
                puzzle = "213132321";
                DeleteOldCity();
                gameManager.puzzleGenerator.predeterminedSolution = puzzle;
                gameManager.puzzleGenerator.CreatePuzzle(3);
                gameManager.DrawFullPuzzle();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 28:
                text = "The best place to start when building a city is to fill in all the tallest buildings first.";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 29:
                text = "These four residents only want to see one building...";
                continueText = "Tap to continue...";
                AddRedBoxAroundResident("top", 2);
                AddRedBoxAroundResident("left", 2);
                AddRedBoxAroundResident("right", 0);
                AddRedBoxAroundResident("bottom", 0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                skipRequirement = "add buildings of height 3 to tiles 2 and 6";
                skipToStage = 31;
                break;
            case 30:
                text = "These four residents only want to see one building...\n\nso you know the spaces next to them have to have three-story buildings.";
                continueText = "Place the correct buildings...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add buildings of height 3 to tiles 2 and 6";
                skipRequirement = "";
                break;
            case 31:
                text = "Well done!";
                continueText = "Tap to continue...";
                RemoveRedBoxesAroundResidents();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 32:
                text = "Well done!\nNow, you know enough to figure out where the final three-story building has to go! Remember, only one of each building size can go in each street!";
                continueText = "Place the correct building...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 3 to tile 4";
                break;
            case 33:
                text = "Now, you know enough to place a two-story building!";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 34:
                text = "This resident only wants to see two buildings down their street...";
                continueText = "Tap to continue...";
                AddRedBoxAroundResident("left", 0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                skipRequirement = "add building of height 2 to tile 0";
                skipToStage = 37;
                break;
            case 35:
                text = "This resident only wants to see two buildings down their street...\nTherefore, we know that this building...";
                continueText = "Tap to continue...";
                AddRedBoxAroundTile(0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 36:
                text = "This resident only wants to see two buildings down their street...\nTherefore, we know that this building...\nhas to be the second-tallest building in the street!";
                continueText = "Place the correct building...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 2 to tile 0";
                skipRequirement = "";
                break;
            case 37:
                text = "And now you know enough to completely fill the top street!";
                continueText = "Place the correct building...";
                RemoveRedBoxesAroundResidents();
                RemoveRedBoxesAroundTiles();
                AddRedBoxAroundTile(1);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 1 to tile 1";
                break;
            case 38:
                text = "Try to finish building the rest of the city!";
                continueText = "Complete the city!";
                RemoveRedBoxesAroundTiles();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "complete the city";
                break;
            case 39:
                text = "Congratulations! You have completed the tutorial for Cityscapes.";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                StaticVariables.hasBeatenTutorial = true;
                SaveSystem.SaveGame();
                break;
            case 40:
                text = "Congratulations! You have completed the tutorial for Cityscapes.\n\nReturn to the main menu and try a puzzle on your own! You can redo this tutorial at any time.";
                continueText = "Main Menu...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 41:
                if (StaticVariables.highestUnlockedSize < 3) {
                    StaticVariables.highestUnlockedSize = 3;
                }
                gameManager.PushMainMenuButton();
                break;
        }

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
        if (advanceRequirement == "complete the city") {
            if (t.shownNumber == 0) {
                if (t.solution == gameManager.selectedNumber)
                    return true;
            }
        }
        return false;
    }

    // ---------------------------------------------------
    //HERE ARE THE FUNCTIONS THAT SPECIFICALLY ADD RED BORDERS AROUND OBJECTS THE PLAYER IS SUPPOSED TO INTERACT WITH
    // ---------------------------------------------------

    private void AddRedBoxAroundNumButton(int num) {
        //adds a border around a number button, to draw attention to that button
        gameManager.numberButtons[num - 1].transform.Find("Red Border").gameObject.SetActive(true);
    }

    private void RemoveRedBoxesAroundNums() {
        //removes all red boxes around all number buttons
        for (int i = 1; i<gameManager.size + 1; i++)
            gameManager.numberButtons[i - 1].transform.Find("Red Border").gameObject.SetActive(false);
    }

    private void AddRedBoxAroundTile(int spaceNum) {
        //adds a red border around a PuzzleTile, to draw attention to that tile
        redBorders[spaceNum].SetActive(true);
    }

    private void RemoveRedBoxesAroundTiles() {
        //removes all red borders around all tiles
        foreach (GameObject go in redBorders)
            go.SetActive(false);
    }

    private void AddRedBoxAroundStreet(string vertOrHoriz, int streetNum) {
        //adds a red border around an entire street, to draw attention to it
        int rotation = 0;
        if (vertOrHoriz == "vertical")
            rotation = 90;

        int centerX = 0;
        int centerY = 0;
        if (vertOrHoriz == "horizontal") {
            centerX = 1;
            centerY = streetNum;
        }
        else {
            centerX = streetNum;
            centerY = 1;
        }

        Vector3 centerPoint = gameManager.puzzleGenerator.tilesArray[centerY, centerX].transform.position;
        gameManager.AddRedStreetBorderForTutorial(centerPoint, rotation);
    }

    private void RemoveRedBoxesAroundStreets() {
        //removes all red borders around all streets
        gameManager.RemoveRedStreetBordersForTutorial();
    }

    private void AddRedBoxAroundResident(string side, int num) {
        //adds a red border around a resident, to draw attention to that resident
        SideHintTile[] row;
        if (side == "top")
            row = gameManager.puzzleGenerator.topHints;
        else if (side == "bottom") 
            row = gameManager.puzzleGenerator.bottomHints;
        else if (side == "left") 
            row = gameManager.puzzleGenerator.leftHints;
        else
            row = gameManager.puzzleGenerator.rightHints;
        SideHintTile s = row[num];
        s.AddRedBorder();
    }

    public void RemoveRedBoxesAroundResidents() {
        //removes a red border around all residents
        foreach (SideHintTile s in gameManager.puzzleGenerator.allHints)
            s.RemoveRedBorder();
    }
}