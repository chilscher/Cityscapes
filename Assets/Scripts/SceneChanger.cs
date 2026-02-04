//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneChanger{
    public enum SwipeDirection {North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest, None}


    static public SceneChangerVisuals visuals;
    static public SwipeDirection swipeDirection = SwipeDirection.None;
    static public string nextSceneName;

    static private void PickRandomSwipeDirection(){
        int r = StaticVariables.rand.Next(1, 9);
        swipeDirection = r switch        {
            1 => SwipeDirection.North,
            2 => SwipeDirection.NorthEast,
            3 => SwipeDirection.East,
            4 => SwipeDirection.SouthEast,
            5 => SwipeDirection.South,
            6 => SwipeDirection.SouthWest,
            7 => SwipeDirection.West,
            8 => SwipeDirection.NorthWest,
            _ => SwipeDirection.North,
        };
        Debug.Log(swipeDirection.ToString());
    }

    static public void GoMenu(){
        nextSceneName = "MainMenu";
        PickRandomSwipeDirection();
        visuals.StartSwipeIn();
        StaticVariables.WaitTimeThenCallFunction(0.5f, LoadScene);
    }

    static public void GoSettings(){
        nextSceneName = "Settings";
        PickRandomSwipeDirection();
        visuals.StartSwipeIn();
        StaticVariables.WaitTimeThenCallFunction(0.5f, LoadScene);
    }
    
    static public void GoPuzzle(){
        nextSceneName = "InPuzzle";
        PickRandomSwipeDirection();
        visuals.StartSwipeIn();
        StaticVariables.WaitTimeThenCallFunction(0.5f, LoadScene);
    }

    static public void GoShop(){
        nextSceneName = "Shop";
        PickRandomSwipeDirection();
        visuals.StartSwipeIn();
        StaticVariables.WaitTimeThenCallFunction(0.5f, LoadScene);
    }
    
    static public void GoCredits(){
        nextSceneName = "Credits";
        PickRandomSwipeDirection();
        visuals.StartSwipeIn();
        StaticVariables.WaitTimeThenCallFunction(0.5f, LoadScene);
    }

    static private void LoadScene(){
        SceneManager.LoadScene(nextSceneName);
    }

}
