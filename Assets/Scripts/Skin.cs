//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skin: MonoBehaviour{
    //used to store all of the information related to a single skin. These variables are only changed in the inspector

    [Header("Metadata")]
    public string skinName;
    public int skinTier; //price tier, for use in the shop

    [Header("Background Art")]
    public Sprite puzzleBackground;
    public Sprite mainMenuBackground;
    public Sprite shopBackground;
    
    [Header("UI element Colors")]
    public Color puzzleButtonInside_On; //ex: the build button when build is selected
    public Color puzzleButtonBorder_On;
    public Color puzzleButtonInside_Off;  //ex: the build button when note 1 is selected
    public Color puzzleButtonBorder_Off;
    public Color menuButtonInside; //ex: the buttons on the home screen or in puzzle scene
    public Color menuButtonBorder;
    public Color popupInside; //ex: the win popup or return to puzzle popup
    public Color popupBorder;

    [Header("Puzzle component colors")]
    public Color normalCitizen;
    public Color satisfiedCitizen;
    public Color street;
    public Color highlightBuilding;
    public Color note1;
    public Color note2;
    public Color tileBackground;

    [Header("these are outdated lmao")]
    
    //the interior and exterior colors of buttons when they are toggled on or off.
    public string onButtonColorInterior; 
    public string onButtonColorExterior;
    public string offButtonColorInterior;
    public string offButtonColorExterior;

    //colors and sprites used while the player is doing a puzzle
    public string citizenColor;
    public string satisfiedCitizenColor;
    public Sprite buildingSprite;
    public string streetColor;
    public string highlightBuildingColor;
    public string note1Color;
    public string note2Color;

    //the interior and exterior colors of main menu buttons
    public string mainMenuButtonInterior;
    public string mainMenuButtonExterior;
    public string resumePuzzleInterior;
    public string resumePuzzleExterior;


    /* TO ADD A NEW SKIN, FOLLOW THESE INSTRUCTIONS!
    
    I will update these instructions next time I add a new skin :/


    */

}
