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


    public void Start(){
        SceneChanger.visuals = this;
        StartSwipeOut();
    }

    public void StartSwipeIn(){
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
        northPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.North);
        northEastPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.NorthEast);
        eastPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.East);
        southEastPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.SouthEast);
        southPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.South);
        southWestPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.SouthWest);
        westPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.West);
        northWestPanel.SetActive(SceneChanger.swipeDirection == SceneChanger.SwipeDirection.NorthWest);
        clickBlocker.SetActive(SceneChanger.swipeDirection != SceneChanger.SwipeDirection.None);

        MovePanelOut(northPanel);
        MovePanelOut(northEastPanel);
        MovePanelOut(eastPanel);
        MovePanelOut(southEastPanel);
        MovePanelOut(southPanel);
        MovePanelOut(southWestPanel);
        MovePanelOut(westPanel);
        MovePanelOut(northWestPanel);
        
        if (SceneChanger.swipeDirection != SceneChanger.SwipeDirection.None)
            StaticVariables.WaitTimeThenCallFunction(SceneChanger.sceneChangeDuration, AllowClicks);
    }

    private void MovePanelIn(GameObject panel){
        panel.transform.DOLocalMove(Vector3.zero, SceneChanger.sceneChangeDuration).SetEase(Ease.Linear);
    }

    private void MovePanelOut(GameObject panel){
        Vector2 start = panel.transform.localPosition;
        panel.transform.localPosition = Vector2.zero;
        panel.transform.DOLocalMove(start, SceneChanger.sceneChangeDuration).SetEase(Ease.Linear);
    }

    private void AllowClicks(){
        clickBlocker.SetActive(false);
    }
}