//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skin: MonoBehaviour{
    //used to store all of the information related to a single skin. These variables are only changed in the inspector

    [Header("Metadata")]
    public string skinName;
    public int skinTier; //price tier, for use in the shop

    [Header("Sprites")]
    public Sprite puzzleBackground;
    public Sprite mainMenuBackground;
    public Sprite shopBackground;
    public Sprite buildingSprite;
    
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


    /* TO ADD A NEW SKIN, FOLLOW THESE INSTRUCTIONS!
    
    I will update these instructions next time I add a new skin :/

    duplicate one of the pre-existing skin prefabs
    change the images and colors in the skin script attached to the prefab
    duplicate one of the purchase buttons in the shop scene, and update the button text and dropdown text
    duplicate one of the skin selection buttons in the settings scene, and update the button text

    */

}
