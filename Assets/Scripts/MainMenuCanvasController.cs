using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvasController : MonoBehaviour {
    /*
    [Header("Canvases")]
    public GameObject homeCanvas;
    public GameObject creditsCanvas;
    public GameObject playCanvas;

    [Header("Options")]
    public bool canReviveEnemy = true;

    [Header("Back Key")]
    public KeyCode backKey = KeyCode.Escape;


    private GameObject homeRevPlayerBtn;
    private GameObject homeRevEnemyBtn;
    private GameObject playRevPlayerBtn;
    private GameObject playRevEnemyBtn;
    private GameObject creditsRevPlayerBtn;
    private GameObject creditsRevEnemyBtn;

    private Player player;
    private Enemy enemy;
    private AudioManager audioManager;

    private void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        audioManager = FindObjectOfType<AudioManager>();
        homeRevPlayerBtn = homeCanvas.transform.Find("Revive Player Button").gameObject;
        homeRevEnemyBtn = homeCanvas.transform.Find("Revive Enemy Button").gameObject;
        playRevPlayerBtn = playCanvas.transform.Find("Revive Player Button").gameObject;
        playRevEnemyBtn = playCanvas.transform.Find("Revive Enemy Button").gameObject;
        creditsRevPlayerBtn = creditsCanvas.transform.Find("Revive Player Button").gameObject;
        creditsRevEnemyBtn = creditsCanvas.transform.Find("Revive Enemy Button").gameObject;
        homeCanvas.SetActive(true);
        playCanvas.SetActive(false);
        creditsCanvas.SetActive(false);

        audioManager.fadeIn("Main Menu");
    }

    private void Update() {
        hitBackKey();
        if (canReviveEnemy) {
            homeRevEnemyBtn.SetActive(enemy.getIsDead());
            playRevEnemyBtn.SetActive(enemy.getIsDead());
            creditsRevEnemyBtn.SetActive(enemy.getIsDead());
        }
        homeRevPlayerBtn.SetActive(player.getIsDead());
        playRevPlayerBtn.SetActive(player.getIsDead());
        creditsRevPlayerBtn.SetActive(player.getIsDead());
    }

    public void _btnMainMenu() {
        homeCanvas.SetActive(true);
        playCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    public void _btnLevelSelect() {
        playCanvas.SetActive(true);
        homeCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    public void _btnCredits() {
        creditsCanvas.SetActive(true);
        homeCanvas.SetActive(false);
        playCanvas.SetActive(false);
    }

    public void _btnQuit() {
        Application.Quit();
    }

    public void _btnRevivePlayer() {
        player.startReviving();
    }

    public void _btnReviveEnemy() {
        enemy.startReviving();
    }

    public void _btnLoadLevel(int x) {
        audioManager.fadeOutAll();
        SceneManager.LoadScene("Level " + x.ToString());
    }

    private void hitBackKey() {
        if (Input.GetKeyDown(backKey)) {
            if (homeCanvas.activeSelf == false) {
                homeCanvas.SetActive(true);
                playCanvas.SetActive(false);
                creditsCanvas.SetActive(false);
            }
        }
    }
    */
    public void startPuzzle(int size) {
        StaticVariables.size = size;
        StaticVariables.isTutorial = false;
        StaticVariables.includeRedHintButton = true;
        StaticVariables.includeGreenHintButton = false;
        SceneManager.LoadScene("InPuzzle");
    }

    public void startTutorial() {
        StaticVariables.size = 3;
        StaticVariables.isTutorial = true;
        StaticVariables.includeRedHintButton = false;
        StaticVariables.includeGreenHintButton = false;
        SceneManager.LoadScene("InPuzzle");
    }
}