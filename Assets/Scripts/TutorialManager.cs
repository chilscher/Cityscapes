using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager{

    public string puzzle = "231312123";
    public GameManager gameManager;
    private int tutorialStage = 0;
    //private int tutorialStage = 26;
    public string advanceRequirement;
    public Text tutorialText;
    public Text continueClue;
    private string text;
    private string continueText;

    public void startTutorial() {
        gameManager.puzzleGenerator.usePredeterminedSolution = true;
        gameManager.puzzleGenerator.predeterminedSolution = puzzle;
        gameManager.puzzleGenerator.createPuzzle(3);
        gameManager.drawFullPuzzle();
        gameManager.hideHints();
        gameManager.hidePositioningObjects();
        gameManager.setSelectionModeButtons();
        gameManager.hitBuildButton();
        gameManager.screenTappedMonitor.SetActive(true);
        gameManager.tutorialTextBox.SetActive(true);
        tutorialText = gameManager.tutorialTextBox.GetComponent<Transform>().GetChild(0).GetComponent<Text>();
        continueClue = gameManager.tutorialTextBox.GetComponent<Transform>().GetChild(1).GetComponent<Text>();
        gameManager.undoRedoButtons.SetActive(false);
        gameManager.setRemoveAllAndClearButtons();
        //gameManager.removeColoredHintsOfChosenNumberButton.SetActive(false);
        //gameManager.removeAllOfNumberButton.SetActive(false);
        //gameManager.clearPuzzleButton.SetActive(false);

        advanceStage();
    }

    private void advanceStage() {
        tutorialStage++;

        switch (tutorialStage) {
            case 1:
                text = "Welcome to Cityscapes! This is a number- placement puzzle game. In Cityscapes, you are a city designer, and your job is to build a city to fit its new residents' wishes.";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 2:
                text = "This 3x3 grid is your city, and each space on the grid can hold one building.";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 3:
                text = "The buildings you place will either be one story...";
                continueText = "Tap to continue...";
                fillInSpace(6, 1);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 4:
                text = "The buildings you place will either be one story,\ntwo stories...";
                continueText = "Tap to continue...";
                fillInSpace(7, 2);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 5:
                text = "The buildings you place will either be one story,\ntwo stories,\nor three stories tall.";
                continueText = "Tap to continue...";
                fillInSpace(8, 3);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 6:
                text = "To place a building, tap the building size you would like to place...";
                continueText = "Choose the right building size...";
                gameManager.drawNumberButtons();
                addRedBoxAroundNumButton(2);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap number button 2";
                break;
            case 7:
                text = "To place a building, tap the building size you would like to place,\nthen tap the space you would like to build on.";
                continueText = "Place the building...";
                removeRedBoxesAroundNums();
                addRedBoxAroundTile(0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 2 to tile 0";
                break;
            case 8:
                text = "Congratulations! You have built your first building!";
                continueText = "Tap to continue...";
                removeRedBoxesAroundTiles();
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
                break;
            case 11:
                text = "Firstly, every street has to contain exactly one building of each height. These three buildings form a street and already satisfy this rule.";
                continueText = "Tap to continue...";
                addRedBoxAroundStreet("horizontal", 2);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 12:
                text = "However, this street does not yet have one of every building!";
                continueText = "Tap to continue...";
                removeRedBoxesAroundStreets();
                addRedBoxAroundStreet("vertical", 0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 13:
                text = "However, this street does not yet have one of every building! It still needs a three-story building in the middle.";
                continueText = "Place the correct building...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 3 to tile 3";
                break;
            case 14:
                text = "Great! The second building requirement involves the residents of the city...";
                continueText = "Tap to continue...";
                removeRedBoxesAroundStreets();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 15:
                text = "Great! The second building requirement involves the residents of the city, who are now standing at the ends of every street!";
                continueText = "Tap to continue...";
                gameManager.showHints();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 16:
                text = "This resident...";
                continueText = "Tap to continue...";
                addRedBoxAroundResident("top", 1);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 17:
                text = "This resident is looking down this street...";
                continueText = "Tap to continue...";
                addRedBoxAroundResident("top", 1);
                addRedBoxAroundStreet("vertical", 1);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 18:
                text = "This resident is looking down this street, and only wants to be able to see one building.";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 19:
                text = "This resident is looking down this street, and only wants to be able to see one building. So we know that the building closest to them has to be the tallest one on the whole street!";
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
                break;
            case 21:
                text = "Awesome!";
                continueText = "Tap to continue...";
                removeRedBoxesAroundStreets();
                removeRedBoxesAroundResidents();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 22:
                text = "Awesome! Now, you can fill in the last missing building on the topmost street.";
                continueText = "Place the correct building...";
                addRedBoxAroundStreet("horizontal", 0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 1 to tile 2";
                break;
            case 23:
                text = "And now you can fill in the missing building on the middle street.";
                continueText = "Place the correct building...";
                removeRedBoxesAroundStreets();
                addRedBoxAroundStreet("vertical", 1);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 1 to tile 4";
                break;
            case 24:
                text = "And finally, you can fill in the last remaining building of the city.";
                continueText = "Place the correct building...";
                removeRedBoxesAroundStreets();
                addRedBoxAroundTile(5);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 2 to tile 5";
                break;
            case 25:
                text = "Congratulations! You have completed your first city in Cityscapes!";
                continueText = "Tap to continue...";
                removeRedBoxesAroundTiles();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 26:
                text = "Congratulations! You have completed your first city in Cityscapes! Now let's start a new city from scratch!";
                continueText = "Tap to continue...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 27:
                text = "Welcome to another city. From now on, you can see all residents from the start!";
                continueText = "Tap to continue...";
                puzzle = "213132321";
                deleteOldCity();
                gameManager.puzzleGenerator.predeterminedSolution = puzzle;
                gameManager.puzzleGenerator.createPuzzle(3);
                gameManager.drawFullPuzzle();
                //gameManager.drawNumberButtons();
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
                addRedBoxAroundResident("top", 2);
                addRedBoxAroundResident("left", 2);
                addRedBoxAroundResident("right", 0);
                addRedBoxAroundResident("bottom", 0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 30:
                text = "These four residents only want to see one building, so you know the spaces next to them have to have three-story buildings.";
                continueText = "Place the correct buildings...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add buildings of height 3 to tiles 2 and 6";
                break;
            case 31:
                text = "Well done!";
                continueText = "Tap to continue...";
                removeRedBoxesAroundResidents();
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 32:
                text = "Well done! Now, you know enough to figure out where the final three-story building has to go! Remember, only one of each building size can go in each street!";
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
                addRedBoxAroundResident("left", 0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 35:
                text = "This resident only wants to see two buildings down their street. Therefore, we know that this building...";
                continueText = "Tap to continue...";
                addRedBoxAroundTile(0);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 36:
                text = "This resident only wants to see two buildings down their street. Therefore, we know that this building has to be the second-tallest building in the street!";
                continueText = "Place the correct building...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 2 to tile 0";
                break;
            case 37:
                text = "And now you know enough to completely fill the top street!";
                continueText = "Place the correct building...";
                removeRedBoxesAroundResidents();
                removeRedBoxesAroundTiles();
                addRedBoxAroundTile(1);
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "add building of height 1 to tile 1";
                break;
            case 38:
                text = "Try to finish building the rest of the city!";
                continueText = "Complete the city!";
                removeRedBoxesAroundTiles();
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
                break;
            case 40:
                text = "Congratulations! You have completed the tutorial for Cityscapes. Return to the main menu and try a puzzle on your own! You can redo this tutorial at any time.";
                continueText = "Main Menu...";
                tutorialText.text = text;
                continueClue.text = continueText;
                advanceRequirement = "tap screen";
                break;
            case 41:
                if (StaticVariables.highestUnlockedSize < 3) {
                    StaticVariables.highestUnlockedSize = 3;
                }
                SceneManager.LoadScene("MainMenu");
                break;
        }

    }




    public void tappedScreen() {
        if (advanceRequirement == "tap screen") {
            advanceStage();
        }
    }

    public void tappedNumberButton(int num) {
        if (advanceRequirement == "tap number button " + num) {
            advanceStage();
        }
    }

    public void clickedTile(PuzzleTile t) {
        int chosenNumber = gameManager.selectedNumber;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                int tileNum = (i * 3) + j;
                if (gameManager.puzzleGenerator.tilesArray[i, j] == t) {
                    if (advanceRequirement == "add building of height " + chosenNumber + " to tile " + tileNum) {
                        advanceStage();
                    }
                }
            }
        }

        if (advanceRequirement == "add buildings of height 3 to tiles 2 and 6") {
            PuzzleTile[,] ts = gameManager.puzzleGenerator.tilesArray;
            if (ts[0, 2].shownNumber == 3 && ts[2,0].shownNumber == 3) {
                advanceStage();
            }
        }
        if (advanceRequirement == "complete the city") {
            if (gameManager.puzzleGenerator.checkPuzzle()) {
                advanceStage();
            }
        }
    }

    private void fillInSpace(int spaceNum, int value) {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (((i * 3) + j) == spaceNum){
                    PuzzleTile t = gameManager.puzzleGenerator.tilesArray[i, j];
                    t.toggleNumber(value);
                }
            }
        }
    }

    private void addRedBoxAroundNumButton(int num) {
        NumberButton n = gameManager.numbersPositioning.GetComponent<Transform>().GetChild(num).GetComponent<NumberButton>();
        n.GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
    }

    private void removeRedBoxesAroundNums() {
        for (int i = 1; i<4; i++) {
            NumberButton n = gameManager.numbersPositioning.GetComponent<Transform>().GetChild(i).GetComponent<NumberButton>();
            n.GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);

        }
    }

    private void addRedBoxAroundTile(int spaceNum) {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (((i * 3) + j) == spaceNum) {
                    PuzzleTile t = gameManager.puzzleGenerator.tilesArray[i, j];
                    t.addRedBorder();
                }
            }
        }
    }

    private void removeRedBoxesAroundTiles() {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                PuzzleTile t = gameManager.puzzleGenerator.tilesArray[i, j];
                t.removeRedBorder();
            }
        }
    }

    public bool canPlayerClickTile(PuzzleTile t) {
        int chosenNumber = gameManager.selectedNumber;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                int tileNum = (i * 3) + j;
                if (gameManager.puzzleGenerator.tilesArray[i, j] == t) {
                    if (advanceRequirement == "add building of height " + chosenNumber + " to tile " + tileNum) {
                        return true;
                    }
                }
            }
        }
        if (advanceRequirement == "add buildings of height 3 to tiles 2 and 6") {
            PuzzleTile[,] ts = gameManager.puzzleGenerator.tilesArray;
            if (gameManager.selectedNumber == 3) {
                if (ts[0, 2] == t || ts[2, 0] == t) {
                    return true;
                }
            }
        }
        if (advanceRequirement == "complete the city") {
            if (t.shownNumber == 0) {
                if  (t.solution == gameManager.selectedNumber) {
                    return true;
                }
            }
        }
        return false;
    }

    private void addRedBoxAroundStreet(string vertOrHoriz, int streetNum) {
        int rotation = 0;
        if (vertOrHoriz == "vertical"){
            rotation = 90;
        }

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
        gameManager.addRedStreetBorderForTutorial(centerPoint, rotation);
    }

    private void removeRedBoxesAroundStreets() {
        gameManager.removeRedStreetBordersForTutorial();
    }

    private void addRedBoxAroundResident(string side, int num) {
        SideHintTile[] row;
        if (side == "top"){
            row = gameManager.puzzleGenerator.topHints;
        }
        else if (side == "bottom") {
            row = gameManager.puzzleGenerator.bottomHints;
        }
        else if (side == "left") {
            row = gameManager.puzzleGenerator.leftHints;
        }
        else{
            row = gameManager.puzzleGenerator.rightHints;
        }
        SideHintTile s = row[num];
        s.addRedBorder();
    }

    public void removeRedBoxesAroundResidents() {
        foreach (SideHintTile s in gameManager.puzzleGenerator.allHints) {
            s.removeRedBorder();
        }
    }

    private void deleteOldCity() {
        gameManager.deleteCityForTutorial();
    }
}