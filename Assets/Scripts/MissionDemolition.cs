using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.NetworkInformation;
using UnityEngine.SceneManagement;

public enum GameMode {
    idle,
    playing,
    levelEnd,
    gameOver
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Inscribed")]
    public TMP_Text uitLevel;
    public TMP_Text uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    public GameObject winScreen;   // The panel that will show the "You Win" screen
    public TMP_Text winMessage;    // Text object to display "You Win"
    public Button restartButton;   // Button to restart the game

    // Add UI elements for the game over screen
    public GameObject gameOverScreen;   // The panel that will show the "Game Over" screen
    public TMP_Text gameOverMessage;    // Text object to display "Game Over"
    public Button gameOverRestartButton;   // Button to restart the game

    public int maxShots = 7;


    void Start()
    {
        S = this;

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();

        // Make sure the win screen is initially hidden
        winScreen.SetActive(false);
        gameOverScreen.SetActive(false);

        // Assign restart button listener
        restartButton.onClick.AddListener(RestartGame);
        gameOverRestartButton.onClick.AddListener(RestartGame);
    }

    void StartLevel() {
        if (castle != null) {
            Destroy(castle);
        }

        BallLifetime.DESTROY_PROJECTILES();

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        Goal.goalMet = false;

        UpdateGUI();

        shotsTaken = 0;
        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
    }

    void UpdateGUI() {
        uitLevel.text = "Level: " +(level+1)+" of "+levelMax;
        uitShots.text = "Shots Taken: "+shotsTaken;
    }

    void Update() {
        UpdateGUI();

        if (shotsTaken > maxShots && !Goal.goalMet) {
            // If shots exceed maxShots and goal isn't met, show game over screen
            ShowGameOverScreen();
            return;
        }

        if ((mode == GameMode.playing) && Goal.goalMet) {
            mode = GameMode.levelEnd;

            FollowCam.SWITCH_VIEW(FollowCam.eView.castle);

            // If we are at the last level, show the "You Win" screen
            if (level == levelMax - 1) {
                ShowWinScreen();
            } else {
                Invoke("NextLevel", 2f);
            }


        }
    }

    void NextLevel() {
        level++;
        if (level == levelMax) {
            level = 0;
            shotsTaken = 0;
        }
        StartLevel();
    }

    static public void SHOT_FIRED() {
        S.shotsTaken++;
    }

    static public GameObject GET_CASTLE() {
        return S.castle;
    }

    // New method to show the "You Win" screen
    private void ShowWinScreen() {
        winScreen.SetActive(true);   // Show the win screen
        winMessage.text = "You Win!"; // Update the message
    }

    // New method to show the "Game Over" screen
    private void ShowGameOverScreen() {
        gameOverScreen.SetActive(true);  // Show the game over screen
        gameOverMessage.text = "Game Over!"; // Update the message
    }

    // New method to restart the game
    private void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads the current scene
    }

}
