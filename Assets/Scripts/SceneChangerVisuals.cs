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

        /*
        Vector2 start = southPanel.transform.localPosition;
        southPanel.transform.localPosition = Vector2.zero;
        southPanel.transform.DOLocalMove(start, 0.5f).SetEase(Ease.Linear);

        start = southWestPanel.transform.localPosition;
        southWestPanel.transform.localPosition = Vector2.zero;
        southWestPanel.transform.DOLocalMove(start, 0.5f).SetEase(Ease.Linear);

        start = westPanel.transform.localPosition;
        westPanel.transform.localPosition = Vector2.zero;
        westPanel.transform.DOLocalMove(start, 0.5f).SetEase(Ease.Linear);

        start = northWestPanel.transform.localPosition;
        northWestPanel.transform.localPosition = Vector2.zero;
        northWestPanel.transform.DOLocalMove(start, 0.5f).SetEase(Ease.Linear);

        start = northPanel.transform.localPosition;
        northPanel.transform.localPosition = Vector2.zero;
        northPanel.transform.DOLocalMove(start, 0.5f).SetEase(Ease.Linear);

        start = northEastPanel.transform.localPosition;
        northEastPanel.transform.localPosition = Vector2.zero;
        northEastPanel.transform.DOLocalMove(start, 0.5f).SetEase(Ease.Linear);

        start = eastPanel.transform.localPosition;
        eastPanel.transform.localPosition = Vector2.zero;
        eastPanel.transform.DOLocalMove(start, 0.5f).SetEase(Ease.Linear);

        start = southEastPanel.transform.localPosition;
        southEastPanel.transform.localPosition = Vector2.zero;
        southEastPanel.transform.DOLocalMove(start, 0.5f).SetEase(Ease.Linear);
        */
    }

    private void MovePanelOut(GameObject panel){
        Vector2 start = panel.transform.localPosition;
        panel.transform.localPosition = Vector2.zero;
        panel.transform.DOLocalMove(start, 0.5f).SetEase(Ease.Linear);
    }

    //public void Setup(){
    //    DontDestroyOnLoad(gameObject);
    //}
}