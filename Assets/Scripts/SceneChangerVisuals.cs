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
        southPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        southWestPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        westPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        northWestPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        northPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        northEastPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        eastPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        southEastPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
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

        MovePanelOut(northPanel);
        MovePanelOut(northEastPanel);
        MovePanelOut(eastPanel);
        MovePanelOut(southEastPanel);
        MovePanelOut(southPanel);
        MovePanelOut(southWestPanel);
        MovePanelOut(westPanel);
        MovePanelOut(northWestPanel);
    }

    private void MovePanelOut(GameObject panel){
        Vector2 start = panel.transform.localPosition;
        panel.transform.localPosition = Vector2.zero;
        panel.transform.DOLocalMove(start, 0.5f).SetEase(Ease.Linear);
    }
}