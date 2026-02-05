//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneChangerVisuals : MonoBehaviour {

    public RectTransform canvas;

    public RectTransform northPanel;
    public RectTransform eastPanel;
    public RectTransform southPanel;
    public RectTransform westPanel;
    public GameObject clickBlocker;   

    public GameObject northIcons;
    public GameObject southIcons;
    public GameObject eastIcons;
    public GameObject westIcons;

    public List<GameObject> menuIcons;
    public List<GameObject> settingsIcons;
    public List<GameObject> shopIcons;
    public List<GameObject> creditsIcons;
    public List<GameObject> tutorialIcons;
    public List<GameObject> smallCityIcons;
    public List<GameObject> medCityIcons;
    public List<GameObject> largeCityIcons;
    public List<GameObject> hugeCityIcons;
    public List<GameObject> massiveCityIcons;

    public void Start(){
        SceneChanger.visuals = this;
        SetPanelSizes();
        if (SceneChanger.iconMoveOutDirection == SceneChanger.Direction.None){
            northIcons.SetActive(false);
            southIcons.SetActive(false);
            eastIcons.SetActive(false);
            westIcons.SetActive(false);
            clickBlocker.SetActive(false);
        }
        else
            MovePanelsOut();
    }

    public void ShowIcons(){
        foreach (GameObject icon in menuIcons)
            icon.SetActive(SceneChanger.icon == SceneChanger.Icon.Menu);
        foreach (GameObject icon in settingsIcons)
            icon.SetActive(SceneChanger.icon == SceneChanger.Icon.Settings);
        foreach (GameObject icon in shopIcons)
            icon.SetActive(SceneChanger.icon == SceneChanger.Icon.Shop);
        foreach (GameObject icon in creditsIcons)
            icon.SetActive(SceneChanger.icon == SceneChanger.Icon.Credits);
        foreach (GameObject icon in tutorialIcons)
            icon.SetActive(SceneChanger.icon == SceneChanger.Icon.Tutorial);
        foreach (GameObject icon in smallCityIcons)
            icon.SetActive(SceneChanger.icon == SceneChanger.Icon.SmallCity);
        foreach (GameObject icon in medCityIcons)
            icon.SetActive(SceneChanger.icon == SceneChanger.Icon.MedCity);
        foreach (GameObject icon in largeCityIcons)
            icon.SetActive(SceneChanger.icon == SceneChanger.Icon.LargeCity);
        foreach (GameObject icon in hugeCityIcons)
            icon.SetActive(SceneChanger.icon == SceneChanger.Icon.HugeCity);
        foreach (GameObject icon in massiveCityIcons)
            icon.SetActive(SceneChanger.icon == SceneChanger.Icon.MassiveCity);
    }

    public void MovePanelsIn(){
        AudioManager.PlaySound(AudioManager.IDs.SceneChangeIn);
        ShowIcons();
        northIcons.SetActive(SceneChanger.iconMoveInDirection == SceneChanger.Direction.North);
        southIcons.SetActive(SceneChanger.iconMoveInDirection == SceneChanger.Direction.South);
        eastIcons.SetActive(SceneChanger.iconMoveInDirection == SceneChanger.Direction.East);
        westIcons.SetActive(SceneChanger.iconMoveInDirection == SceneChanger.Direction.West);

        bool eastWest = false;
        bool northSouth = false;
        switch (SceneChanger.iconMoveInDirection){
            case (SceneChanger.Direction.North):
                northSouth = true;
                northPanel.SetSiblingIndex(3);
                break;
            case (SceneChanger.Direction.South):
                southPanel.SetSiblingIndex(3);
                northSouth = true;
                break;
            case (SceneChanger.Direction.East):
                eastPanel.SetSiblingIndex(3);
                eastWest = true;
                break;
            case (SceneChanger.Direction.West):
                westPanel.SetSiblingIndex(3);
                eastWest = true;
                break;
        }

        northPanel.gameObject.SetActive(northSouth);
        southPanel.gameObject.SetActive(northSouth);
        eastPanel.gameObject.SetActive(eastWest);
        westPanel.gameObject.SetActive(eastWest);
        clickBlocker.SetActive(true);

        northPanel.DOLocalMove(Vector2.zero, SceneChanger.panelMoveTime).SetEase(Ease.OutSine);
        southPanel.DOLocalMove(Vector2.zero, SceneChanger.panelMoveTime).SetEase(Ease.OutSine);
        eastPanel.DOLocalMove(Vector2.zero, SceneChanger.panelMoveTime).SetEase(Ease.OutSine);
        westPanel.DOLocalMove(Vector2.zero, SceneChanger.panelMoveTime).SetEase(Ease.OutSine);
    }

    public void MovePanelsOut(){
        AudioManager.PlaySound(AudioManager.IDs.SceneChangeOut);
        ShowIcons();
        northIcons.SetActive(SceneChanger.iconMoveOutDirection == SceneChanger.Direction.North);
        southIcons.SetActive(SceneChanger.iconMoveOutDirection == SceneChanger.Direction.South);
        eastIcons.SetActive(SceneChanger.iconMoveOutDirection == SceneChanger.Direction.East);
        westIcons.SetActive(SceneChanger.iconMoveOutDirection == SceneChanger.Direction.West);

        bool eastWest = false;
        bool northSouth = false;
        switch (SceneChanger.iconMoveOutDirection){
            case (SceneChanger.Direction.North):
                northSouth = true;
                northPanel.SetSiblingIndex(3);
                break;
            case (SceneChanger.Direction.South):
                northSouth = true;
                southPanel.SetSiblingIndex(3);
                break;
            case (SceneChanger.Direction.East):
                eastWest = true;
                eastPanel.SetSiblingIndex(3);
                break;
            case (SceneChanger.Direction.West):
                eastWest = true;
                westPanel.SetSiblingIndex(3);
                break;
        }

        northPanel.gameObject.SetActive(northSouth);
        southPanel.gameObject.SetActive(northSouth);
        eastPanel.gameObject.SetActive(eastWest);
        westPanel.gameObject.SetActive(eastWest);
        clickBlocker.SetActive(true);
        
        Vector2 pos = northPanel.localPosition;
        northPanel.localPosition = Vector2.zero;
        northPanel.DOLocalMove(pos, SceneChanger.panelMoveTime).SetEase(Ease.InSine);
        pos = southPanel.localPosition;
        southPanel.localPosition = Vector2.zero;
        southPanel.DOLocalMove(pos, SceneChanger.panelMoveTime).SetEase(Ease.InSine);
        pos = eastPanel.localPosition;
        eastPanel.localPosition = Vector2.zero;
        eastPanel.DOLocalMove(pos, SceneChanger.panelMoveTime).SetEase(Ease.InSine);
        pos = westPanel.localPosition;
        westPanel.localPosition = Vector2.zero;
        westPanel.DOLocalMove(pos, SceneChanger.panelMoveTime).SetEase(Ease.InSine);

        StaticVariables.WaitTimeThenCallFunction(SceneChanger.panelMoveTime, FinishedMoveOut);
    }

    private void FinishedMoveOut(){
        northIcons.SetActive(false);
        southIcons.SetActive(false);
        eastIcons.SetActive(false);
        westIcons.SetActive(false);
        clickBlocker.SetActive(false);
    }

    private void SetPanelSizes(){
        float iconSpaceHeight = 220;
        float iconSpaceWidth = 500;

        float fullHeight = canvas.rect.height;
        float halfHeight = fullHeight / 2;
        float fullWidth = canvas.rect.width;
        float halfWidth = fullWidth / 2;
        Vector2 vertSize = new(fullWidth, halfHeight);
        Vector2 horizSize = new(halfWidth, fullHeight);
        northPanel.sizeDelta = vertSize;
        southPanel.sizeDelta = vertSize;
        eastPanel.sizeDelta = horizSize;
        westPanel.sizeDelta = horizSize;
        northPanel.localPosition = new Vector2(0, halfHeight + iconSpaceHeight);
        southPanel.localPosition = new Vector2(0, -halfHeight - iconSpaceHeight);
        eastPanel.localPosition = new Vector2(halfWidth + iconSpaceWidth, 0);
        westPanel.localPosition = new Vector2(-halfWidth - iconSpaceWidth, 0);
    }
}