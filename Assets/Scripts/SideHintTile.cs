//for Cityscapes, copyright Fancy Bus Games 2026

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SideHintTile : Tile {
    //contains information for the Side Hints, also known as Residents, which are the clues on the side of the puzzle
    //the SideHintTile script is attached to a SideHintTilePrefab object. The PuzzleGenerator makes an instance of the SideHintTilePrefab and provides it with data

    private int hintValue;
    [HideInInspector]
    public PuzzleTile[] row; //all of the Puzzle Tiles that this SideHintTile's hint references (this SideHintTile is the number at the end of a row of buildings)

    //some visual elements of the SideHintTile Prefab
    public Image background;
    public Image arrow;
    public Image number;
    public Image redBorder;

    //the number sprites
    public Sprite[] whiteSprites;

    //the colors that the numbers in the SideHintTiles can be
    private Color correctColor;
    private Color incorrectColor;

    private bool isSatisfied = false;
    private float danceMaximum = 0;
    private float danceMinimum = 0;
    private float danceOrigin;
    private bool dance = false;

    public void Initialize(int hintValue) {
        //creates the sideHintTile. Here goes all of the code that defines the private variables used later
        this.hintValue = hintValue;

        SetNumberColors();
        AddNumberToTile(hintValue);

        Skin tempSkin = StaticVariables.skin;
        if (StaticVariables.isTutorial) { tempSkin = StaticVariables.allSkins[0]; }
        background.GetComponent<Image>().color = tempSkin.street;
    }

    public int NumBuildingsCurrentlyVisible() {
        //iterates through the PuzzleTiles that this SideHintTile looks upon, and determines how many buildings are currently visible
        //note: does NOT reference the solution, but instead whatever buildings are actually there
        int count = 0;
        int highest = 0;
        foreach (PuzzleTile t in row) {
            if (t.shownNumber != 0) {
                if (t.shownNumber > highest) {
                    highest = t.shownNumber;
                    count++;
                }
            }
        }
        return count;
    }

    public void SetNumberColors() {
        //sets the colors that the SideHintTile number can be, based off of the current skin.
        //the tutorial uses its own colors, which are the ones used in the basic skin
        if (StaticVariables.isTutorial) {
            incorrectColor = InterfaceFunctions.GetDefaultSkin().normalCitizen;
            correctColor = InterfaceFunctions.GetDefaultSkin().satisfiedCitizen;
        }
        else {
            incorrectColor = StaticVariables.skin.normalCitizen;
            correctColor = StaticVariables.skin.satisfiedCitizen;
        }
    }

    public bool IsRowValid() {
        //checks if this SideHintTile's building requirement is satisfied. Part of the PuzzleGenerator function to check if the player has won
        List<int> usedValues = new List<int>();
        foreach (PuzzleTile t in row) {
            if (t.shownNumber == 0) {
                return false;
            }
            if (usedValues.Contains(t.shownNumber)) {
                return false;
            }
            usedValues.Add(t.shownNumber);
        }
        return (NumBuildingsCurrentlyVisible() == hintValue);
    }

    public void AddHint() {
        //called at the start of a puzzle
        AddNumberToTile(hintValue);
        //number.color = incorrectColor;
        //arrow.color = incorrectColor;
    }
    

    private void AddNumberToTile(int num) {
        number.sprite = whiteSprites[num - 1];
        SetAppropriateColor();
    }

    public void SetAppropriateColor() {
        if (!StaticVariables.changeResidentColorOnCorrectRows)
            return;
        if (StaticVariables.isTutorial)
            return;

        bool wasSatisfied = isSatisfied;
        isSatisfied = ((NumBuildingsCurrentlyVisible() == hintValue) && (row[0].shownNumber != 0));
        if (isSatisfied != wasSatisfied){
            Color c = incorrectColor;
            if (isSatisfied)
                c = correctColor;
            number.DOColor(c, 0.25f);
            arrow.DOColor(c, 0.25f);
        }
    }

    public void SetAppropriateColorImmediate(){
        number.color = incorrectColor;
        arrow.color = incorrectColor;
        SetAppropriateColor();
    }

    public void RotateHint(int amt, float tileSize) {
        //rotate the arrow and puzzle border to face the interior of the puzzle
        background.transform.Rotate(new Vector3(0, 0, amt));
        arrow.transform.Rotate(new Vector3(0, 0, amt));
        float numberOffset_Vert = tileSize / 6;
        float numberOffset_Horiz = tileSize / 15;
        float arrowOffset_Vert = -1.25f;
        float arrowOffset_Horiz = -1.25f;
        Vector3 numberPos = number.transform.position;
        Vector3 arrowPos = Vector3.zero;
        if (amt == 0){
            numberPos.y += numberOffset_Vert;
            arrowPos.y += arrowOffset_Vert;
        }
        else if(amt == 180){
            numberPos.y -= numberOffset_Vert;
            arrowPos.y -= arrowOffset_Vert;
        }
        else if (amt == 90){
            numberPos.x -= numberOffset_Horiz;
            arrowPos.x -= arrowOffset_Horiz;
        }
        else if (amt == 270){
            numberPos.x += numberOffset_Horiz;
            arrowPos.x += arrowOffset_Horiz;
        }
        number.transform.position = numberPos;
        redBorder.transform.position = numberPos;
        arrow.transform.localPosition = arrowPos;
    }

    public void AddRedBorder() {
        //add a border around the Side Hint Tile number. Used in the tutorial
        redBorder.gameObject.SetActive(true);
    }

    public void RemoveRedBorder() {
        //removes the border around the SideHintTile number. Used in the tutorial
        redBorder.gameObject.SetActive(false);
    }

    public void MakeResidentDance(){
        dance = true;
        danceOrigin = number.transform.localPosition.y;
        danceMaximum = danceOrigin + 0.05f;
        danceMinimum = danceOrigin - 0.05f;
        float startPos = StaticVariables.rand.Next(-4, 5) * 0.2f * 0.01f;
        float time = Mathf.Abs(startPos * 2f);
        number.transform.DOLocalMoveY(danceOrigin + startPos, time).OnComplete(OscillateNumber);
    }

    private void OscillateNumber(){
        if (dance == false) 
            return;
        float time = StaticVariables.rand.Next(80, 100) * 0.01f * 0.2f;
        float newPos = danceMinimum;
        if (number.transform.localPosition.y < danceOrigin)
            newPos = danceMaximum;
        number.transform.DOLocalMoveY(newPos, time).OnComplete(OscillateNumber);
    }

    public void StopDancing(){
        dance = false;
        float dist = MathF.Abs(number.transform.localPosition.y - danceOrigin);
        number.transform.DOKill();
        number.transform.DOLocalMoveY(danceOrigin, dist * 2f);
    }
}
