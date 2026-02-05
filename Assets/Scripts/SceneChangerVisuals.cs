//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneChangerVisuals : MonoBehaviour {

    public GameObject northPanel;
    public GameObject northEastPanel;
    public GameObject eastPanel;
    public GameObject southEastPanel;
    public GameObject southPanel;
    public GameObject southWestPanel;
    public GameObject westPanel;
    public GameObject northWestPanel;
    public GameObject clickBlocker;

    private Vector2 northPanelPos;
    private Vector2 northEastPanelPos;
    private Vector2 eastPanelPos;
    private Vector2 southEastPanelPos;
    private Vector2 southPanelPos;
    private Vector2 southWestPanelPos;
    private Vector2 westPanelPos;
    private Vector2 northWestPanelPos;
    


    public void Start(){
        SceneChanger.visuals = this;
        SetPanelStartingPositions();
        StartSwipeOut();
    }

    public void StartSwipeIn(){
        AudioManager.PlaySound(AudioManager.IDs.SceneChange);
        ReturnPanelsToStartingPositions();

        northPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.South);
        northEastPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.SouthWest);
        eastPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.West);
        southEastPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.NorthWest);
        southPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.North);
        southWestPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.NorthEast);
        westPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.East);
        northWestPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.SouthEast);
        clickBlocker.SetActive(true);

        MovePanelIn(northPanel);
        MovePanelIn(northEastPanel);
        MovePanelIn(eastPanel);
        MovePanelIn(southEastPanel);
        MovePanelIn(southPanel);
        MovePanelIn(southWestPanel);
        MovePanelIn(westPanel);
        MovePanelIn(northWestPanel);
    }

    public void StartSwipeOut(){
        northPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.South);
        northEastPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.SouthWest);
        eastPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.West);
        southEastPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.NorthWest);
        southPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.North);
        southWestPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.NorthEast);
        westPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.East);
        northWestPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.SouthEast);
        clickBlocker.SetActive(SceneChanger.swipeDirection != SceneChanger.SwipeDirection.None);


        northPanel.transform.localPosition = Vector2.zero;
        northPanel.transform.DOLocalMove(-northPanelPos, SceneChanger.totalDuration / 2).SetEase(Ease.Linear);
        northEastPanel.transform.localPosition = Vector2.zero;
        northEastPanel.transform.DOLocalMove(-northEastPanelPos, SceneChanger.totalDuration / 2).SetEase(Ease.Linear);
        eastPanel.transform.localPosition = Vector2.zero;
        eastPanel.transform.DOLocalMove(-eastPanelPos, SceneChanger.totalDuration / 2).SetEase(Ease.Linear);
        southEastPanel.transform.localPosition = Vector2.zero;
        southEastPanel.transform.DOLocalMove(-southEastPanelPos, SceneChanger.totalDuration / 2).SetEase(Ease.Linear);
        southPanel.transform.localPosition = Vector2.zero;
        southPanel.transform.DOLocalMove(-southPanelPos, SceneChanger.totalDuration / 2).SetEase(Ease.Linear);
        southWestPanel.transform.localPosition = Vector2.zero;
        southWestPanel.transform.DOLocalMove(-southWestPanelPos, SceneChanger.totalDuration / 2).SetEase(Ease.Linear);
        westPanel.transform.localPosition = Vector2.zero;
        westPanel.transform.DOLocalMove(-westPanelPos, SceneChanger.totalDuration / 2).SetEase(Ease.Linear);
        northWestPanel.transform.localPosition = Vector2.zero;
        northWestPanel.transform.DOLocalMove(-northWestPanelPos, SceneChanger.totalDuration / 2).SetEase(Ease.Linear);


        //MovePanelOut(northPanel);
        //MovePanelOut(northEastPanel);
        //MovePanelOut(eastPanel);
        //MovePanelOut(southEastPanel);
        //MovePanelOut(southPanel);
        //MovePanelOut(southWestPanel);
        //MovePanelOut(westPanel);
        //MovePanelOut(northWestPanel);
        
        if (SceneChanger.swipeDirection != SceneChanger.SwipeDirection.None)
            StaticVariables.WaitTimeThenCallFunction(SceneChanger.totalDuration / 2, AllowClicks);
    }

    private void MovePanelIn(GameObject panel){
        panel.transform.DOLocalMove(Vector3.zero, SceneChanger.totalDuration / 2).SetEase(Ease.Linear);
    }

    //private void MovePanelOut(GameObject panel){
    //    Vector2 start = panel.transform.localPosition;
    //    panel.transform.localPosition = Vector2.zero;
    //    panel.transform.DOLocalMove(start, SceneChanger.sceneChangeDuration).SetEase(Ease.Linear);
    //}

    private void AllowClicks(){
        clickBlocker.SetActive(false);
    }

    private void SetPanelStartingPositions(){
        northPanelPos = northPanel.transform.localPosition;
        northEastPanelPos = northEastPanel.transform.localPosition;
        eastPanelPos = eastPanel.transform.localPosition;
        southEastPanelPos = southEastPanel.transform.localPosition;
        southPanelPos = southPanel.transform.localPosition;
        southWestPanelPos = southWestPanel.transform.localPosition;
        westPanelPos = westPanel.transform.localPosition;
        northWestPanelPos = northWestPanel.transform.localPosition;
    }

    private void ReturnPanelsToStartingPositions(){
        northPanel.transform.localPosition = northPanelPos;
        northEastPanel.transform.localPosition = northEastPanelPos;
        eastPanel.transform.localPosition = eastPanelPos;
        southEastPanel.transform.localPosition = southEastPanelPos;
        southPanel.transform.localPosition = southPanelPos;
        southWestPanel.transform.localPosition = southWestPanelPos;
        westPanel.transform.localPosition = westPanelPos;
        northWestPanel.transform.localPosition = northWestPanelPos;
    }

}