//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class WinPopup : MonoBehaviour {
    public GameManager gameManager;
    [Header("Coin Amounts")]
    public int coinsFor3Win = 1;
    public int coinsFor4Win = 3;
    public int coinsFor5Win = 10;
    public int coinsFor6Win = 30;
    public int coinsFor7Win = 60;
    [Header("Coin Display")]
    public GameObject coinsBox1s;
    public GameObject coinsBox10s;
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
    public Text anotherPuzzleText;
    public Image transparentBackground;
    public Transform mainPopup;
    private Color transparentBackgroundColor;

    public void Start(){
        ShowCityArt();
        IncreaseCoins();
        StaticVariables.WaitTimeThenCallFunction(0.5f, PlayVictoryCheer);
        StaticVariables.WaitTimeThenCallFunction(1.5f, ShowWinPopup);
        transparentBackgroundColor = transparentBackground.color;
        Color tempColor = transparentBackgroundColor;
        tempColor.a = 0;
        transparentBackground.color = tempColor;
        mainPopup.localScale = Vector3.zero;
    }
    
    private void IncreaseCoins(){
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

        coinsBox1s.GetComponent<Image>().sprite = gameManager.numberSprites[onesDigit];
        coinsBox10s.GetComponent<Image>().sprite = gameManager.numberSprites[tensDigit];
        StaticVariables.AddCoins(amt);
        DisplayTotalCoinsAmount();
    }


    private void ShowCityArt() {
        //shows the "city art" on the win popup, depending on which skin and which city size the player is using
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

    private void DisplayTotalCoinsAmount() {
        //show the player's total coins on the win popup screen
        int value1 = StaticVariables.coins % 10;
        int value10 = (StaticVariables.coins / 10) % 10;
        int value100 = (StaticVariables.coins / 100) % 10;
        int value1k = (StaticVariables.coins / 1000) % 10;
        int value10k = (StaticVariables.coins / 10000) % 10;
        int value100k = (StaticVariables.coins / 100000) % 10;
        int value1m = (StaticVariables.coins / 100000) % 10;
        
        totalCoin1.sprite = gameManager.numberSprites[value1];
        totalCoin10.sprite = gameManager.numberSprites[value10];
        totalCoin100.sprite = gameManager.numberSprites[value100];
        totalCoin1k.sprite = gameManager.numberSprites[value1k];
        totalCoin10k.sprite = gameManager.numberSprites[value10k];
        totalCoin100k.sprite = gameManager.numberSprites[value100k];
        totalCoin1m.sprite = gameManager.numberSprites[value1m];

        totalCoin1.gameObject.SetActive(true);
        totalCoin10.gameObject.SetActive(StaticVariables.coins > 9);
        totalCoin100.gameObject.SetActive(StaticVariables.coins > 99);
        totalCoin1k.gameObject.SetActive(StaticVariables.coins > 999);
        totalCoin10k.gameObject.SetActive(StaticVariables.coins > 9999);
        totalCoin100k.gameObject.SetActive(StaticVariables.coins > 99999);
        totalCoin1m.gameObject.SetActive(StaticVariables.coins > 999999);
    }

    private void PlayVictoryCheer(){
        AudioManager.PlaySound(AudioManager.IDs.VictoryCheer);
    }

    private void ShowWinPopup(){
        transparentBackground.DOColor(transparentBackgroundColor, 0.4f);
        mainPopup.DOScale(Vector3.one * 1.1f, 0.4f).OnComplete(ScaleWinPopupToNormalSize);
    }

    private void ScaleWinPopupToNormalSize(){
        mainPopup.DOScale(Vector3.one, 0.1f);
    }

    public void PushAnotherPuzzleButton() {
        StaticVariables.fadingIntoPuzzleSameSize = false;
        StaticVariables.FadeOutThenLoadScene("InPuzzle");
    }
}