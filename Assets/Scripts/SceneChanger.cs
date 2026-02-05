//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneChanger{

    public enum Direction {North, South, East, West, None}
    static public Direction iconMoveInDirection = Direction.None;
    static public Direction iconMoveOutDirection = Direction.None;
    public enum Icon {Menu, Shop, Settings, Credits, Tutorial, SmallCity, MedCity, LargeCity, HugeCity, MassiveCity, None}
    static public Icon icon = Icon.None;

    static public SceneChangerVisuals visuals;
    static public string nextSceneName;
    static public float panelMoveTime = 0.5f;
    static public float waitTime = 0.2f;

    static private void PickIconMoveDirections(){
        int inRand = StaticVariables.rand.Next(1, 5);
        iconMoveInDirection = inRand switch {
            1 => Direction.North,
            2 => Direction.East,
            3 => Direction.South,
            _ => Direction.West,
        };
        int outRand = StaticVariables.rand.Next(1,3);
        if (iconMoveInDirection == Direction.North || iconMoveInDirection == Direction.South){
            iconMoveOutDirection = outRand switch {
            1 => Direction.East,
            _ => Direction.West,
            };
        }
        else{
            iconMoveOutDirection = outRand switch {
            1 => Direction.North,
            _ => Direction.South,
            };
        }
    }

    static public void GoMenu(){
        nextSceneName = "MainMenu";
        icon = Icon.Menu;
        PickIconMoveDirections();
        visuals.MovePanelsIn();
        StaticVariables.WaitTimeThenCallFunction(panelMoveTime + waitTime, LoadScene);
    }

    static public void GoSettings(){
        nextSceneName = "Settings";
        icon = Icon.Settings;
        PickIconMoveDirections();
        visuals.MovePanelsIn();
        StaticVariables.WaitTimeThenCallFunction(panelMoveTime + waitTime, LoadScene);
    }

    static public void GoTutorial(){
        nextSceneName = "InPuzzle";
        icon = Icon.Tutorial;
        PickIconMoveDirections();
        visuals.MovePanelsIn();
        StaticVariables.WaitTimeThenCallFunction(panelMoveTime + waitTime, LoadScene);
    }
    
    static public void GoPuzzle(int size){
        nextSceneName = "InPuzzle";
        icon = size switch {
            4 => Icon.MedCity,
            5 => Icon.LargeCity,
            6 => Icon.HugeCity,
            7 => Icon.MassiveCity,
            _ => Icon.SmallCity,
        };
        PickIconMoveDirections();
        visuals.MovePanelsIn();
        StaticVariables.WaitTimeThenCallFunction(panelMoveTime + waitTime, LoadScene);
    }

    static public void GoShop(){
        nextSceneName = "Shop";
        icon = Icon.Shop;
        PickIconMoveDirections();
        visuals.MovePanelsIn();
        StaticVariables.WaitTimeThenCallFunction(panelMoveTime + waitTime, LoadScene);
    }
    
    static public void GoCredits(){
        nextSceneName = "Credits";
        icon = Icon.Credits;
        PickIconMoveDirections();
        visuals.MovePanelsIn();
        StaticVariables.WaitTimeThenCallFunction(panelMoveTime + waitTime, LoadScene);
    }

    static private void LoadScene(){
        SceneManager.LoadScene(nextSceneName);
    }

}
