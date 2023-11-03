//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skin: MonoBehaviour{
    //used to store all of the information related to a single skin. These variables are only changed in the inspector

    public string skinName;
    public int skinTier; //price tier, for use in the shop
    
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

    //the background art
    public Sprite puzzleBackground;
    public Sprite mainMenuBackground;
    public Sprite shopBackground;

    //the interior and exterior colors of main menu buttons
    public string mainMenuButtonInterior;
    public string mainMenuButtonExterior;
    public string resumePuzzleInterior;
    public string resumePuzzleExterior;

    //city art, used on the main menu and during the win pop-up after you beat a puzzle
    //public Sprite smallCityArt;
    //public Sprite medCityArt;
    //public Sprite largeCityArt;
    //public Sprite hugeCityArt;
    //public Sprite massiveCityArt;


    /* TO ADD A NEW SKIN, FOLLOW THESE INSTRUCTIONS!
    
    I will update these instructions next time I add a new skin :/


    */

}
