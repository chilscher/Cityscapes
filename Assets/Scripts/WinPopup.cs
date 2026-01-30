//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class WinPopup : MonoBehaviour {
    [Header("Coin Amounts")]
    public int coinsFor3Win = 1;
    public int coinsFor4Win = 3;
    public int coinsFor5Win = 10;
    public int coinsFor6Win = 30;
    public int coinsFor7Win = 60;
    [Header("Coin Display")]
    public RectTransform addedCoinParent;
    public Image addedCoin1;
    public Image addedCoin10;
    public Image addedCoinPlus1;
    public Image addedCoinPlus10;
    public Image totalCoin1;
    public Image totalCoin10;
    public Image totalCoin100;
    public Image totalCoin1k;
    public Image totalCoin10k;
    public Image totalCoin100k;
    public Image totalCoin1m;
    [Header("City Art")]
    public GameObject smallCityArt;
    public GameObject mediumCityArt;
    public GameObject largeCityArt;
    public GameObject hugeCityArt;
    public GameObject massiveCityArt;
    [Header("Misc")]
    public GameManager gameManager;
    public Text anotherPuzzleText;
    public Image transparentBackground;
    public Transform mainPopup;
    private Color transparentBackgroundColor;
    private int previousCoins;

    public void Start(){
        ShowCityArt();
        IncreaseCoins();
        StaticVariables.WaitTimeThenCallFunction(0.5f, PlayVictoryCheer);
        StaticVariables.WaitTimeThenCallFunction(0.5f, MakeResidentsDance);
        StaticVariables.WaitTimeThenCallFunction(1.5f, ShowWinPopup);
        StaticVariables.WaitTimeThenCallFunction(3f, ShowCoinIncrease);
        transparentBackgroundColor = transparentBackground.color;
        Color tempColor = transparentBackgroundColor;
        tempColor.a = 0;
        transparentBackground.color = tempColor;
        mainPopup.localScale = Vector3.zero;
    }
    
    private void IncreaseCoins(){
        previousCoins = StaticVariables.coins;
        int amt = gameManager.size switch {
            3 => coinsFor3Win,
            4 => coinsFor4Win,
            5 => coinsFor5Win,
            6 => coinsFor6Win,
            7 => coinsFor7Win,
            _ => coinsFor3Win,
        };
        
        int onesDigit = amt % 10;
        int tensDigit = (amt / 10) % 10;
        addedCoin10.gameObject.SetActive(tensDigit != 0);
        addedCoinPlus1.gameObject.SetActive(tensDigit == 0);
        addedCoinPlus10.gameObject.SetActive(tensDigit != 0);

        addedCoin1.sprite = gameManager.numberSprites[onesDigit];
        addedCoin10.sprite = gameManager.numberSprites[tensDigit];
        Color c = Color.white;
        c.a = 0;
        addedCoin1.color = c;
        addedCoin10.color = c;
        addedCoinPlus1.color = c;
        addedCoinPlus10.color = c;

        StaticVariables.AddCoins(amt);
        DisplayTotalCoins(previousCoins, StaticVariables.coins);
    }

    private void ShowCoinIncrease(){
        StopResidentsDancing();
        DisplayTotalCoins(StaticVariables.coins);
        AudioManager.PlaySound(AudioManager.IDs.GotCoins);

        addedCoin1.DOColor(Color.white, 0.5f);
        addedCoin10.DOColor(Color.white, 0.5f);
        addedCoinPlus1.DOColor(Color.white, 0.5f);
        addedCoinPlus10.DOColor(Color.white, 0.5f);

        float endPos = addedCoinParent.localPosition.y;
        addedCoinParent.position = totalCoin1.transform.position;
        addedCoinParent.DOLocalMoveY(endPos, 1.75f);
        StaticVariables.WaitTimeThenCallFunction(0.75f, FadeOutCoinIncrease);
    }

    private void FadeOutCoinIncrease(){
        Color c = Color.white;
        c.a = 0;
        addedCoin1.DOColor(c, 1f);
        addedCoin10.DOColor(c, 1f);
        addedCoinPlus1.DOColor(c, 1f);
        addedCoinPlus10.DOColor(c, 1f);
    }

    private void ShowCityArt() {
        smallCityArt.SetActive(gameManager.size == 3);
        mediumCityArt.SetActive(gameManager.size == 4);
        largeCityArt.SetActive(gameManager.size == 5);
        hugeCityArt.SetActive(gameManager.size == 6);
        massiveCityArt.SetActive(gameManager.size == 7);

        anotherPuzzleText.text = gameManager.size switch {
            3 => "ANOTHER SMALL CITY",
            4 => "ANOTHER MEDIUM CITY",
            5 => "ANOTHER LARGE CITY",
            6 => "ANOTHER HUGE CITY",
            7 => "ANOTHER MASSIVE CITY",
            _ => "ANOTHER SMALL CITY",
        };
    }

    private void DisplayTotalCoins(int amt, int amtToShowDigits = -1) {
        if (amtToShowDigits == -1)
            amtToShowDigits = amt;

        int value1 = amt % 10;
        int value10 = (amt / 10) % 10;
        int value100 = (amt / 100) % 10;
        int value1k = (amt / 1000) % 10;
        int value10k = (amt / 10000) % 10;
        int value100k = (amt / 100000) % 10;
        int value1m = (amt / 100000) % 10;

        totalCoin1.sprite = gameManager.numberSprites[value1];
        totalCoin10.sprite = gameManager.numberSprites[value10];
        totalCoin100.sprite = gameManager.numberSprites[value100];
        totalCoin1k.sprite = gameManager.numberSprites[value1k];
        totalCoin10k.sprite = gameManager.numberSprites[value10k];
        totalCoin100k.sprite = gameManager.numberSprites[value100k];
        totalCoin1m.sprite = gameManager.numberSprites[value1m];

        totalCoin1.gameObject.SetActive(true);
        totalCoin10.gameObject.SetActive(amtToShowDigits > 9);
        totalCoin100.gameObject.SetActive(amtToShowDigits > 99);
        totalCoin1k.gameObject.SetActive(amtToShowDigits > 999);
        totalCoin10k.gameObject.SetActive(amtToShowDigits > 9999);
        totalCoin100k.gameObject.SetActive(amtToShowDigits > 99999);
        totalCoin1m.gameObject.SetActive(amtToShowDigits > 999999);
    }

    private void PlayVictoryCheer(){
        AudioManager.PlaySound(AudioManager.IDs.VictoryCheer);
    }

    private void MakeResidentsDance(){
        foreach (SideHintTile sht in gameManager.puzzleGenerator.allHints)
            sht.MakeResidentDance();
    }

    private void StopResidentsDancing(){
        foreach (SideHintTile sht in gameManager.puzzleGenerator.allHints)
            sht.StopDancing();
    }

    private void ShowWinPopup(){
        //transparentBackground.DOColor(transparentBackgroundColor, 2f);
        transparentBackground.DOColor(transparentBackgroundColor, 0.4f);
        mainPopup.DOScale(Vector3.one * 1.05f, 0.4f).OnComplete(ScaleWinPopupToNormalSize);
        //mainPopup.DOScale(Vector3.one, 2f);
    }

    private void ScaleWinPopupToNormalSize(){
        mainPopup.DOScale(Vector3.one, 0.1f);
    }

    public void PushAnotherPuzzleButton() {
        StaticVariables.fadingIntoPuzzleSameSize = false;
        StaticVariables.FadeOutThenLoadScene("InPuzzle");
    }
}