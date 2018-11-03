using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region Vars

    #region publicVars
    [Header ("UI")]
    public Image playerOne;
    public Image playerTwo;
    public Sprite[] playerSprites;
    public Text manageText;
    public Text playerOneName;
    public Text playerTwoName;

    [Header("Game Statistic")]
    public Text round;
    public Text roundToWin;
    public Text leaderOne;
    public Text leaderTwo;

    [Header("LeaderBoard")]
    public GameObject TabMessage;
    public GameObject TabPanel;
    public Text pl1;
    public Text pl2;
    public Text rnd;

    [Header("Finish Game")]
    public GameObject panel;
    public Text finishRounds;
    public Text playerWins;
    public GameObject FinishPanel;

    [Header("GameObjects")]
    public GameObject _StartGame;
    #endregion
    #region privateVars
    private int gameStage = 0;
    private float timer = 0;
    private float bangTimer = 0;

    private int WinsFirst = 0;
    private int WinsSecond = 0;
    private int roundCount = 0;
    private bool _isOpenLeaderBoard = false;
    #endregion
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (GameController._instance == null) gameObject.AddComponent<GameController>();
    }
    private void Start()
    {
        playerOneName.text = GameController._instance.PlayerOneName;
        playerTwoName.text = GameController._instance.PlayerTwoName;
        roundToWin.text = "Rounds to win: " + GameController._instance.countOfWins;
        leaderOne.text = playerOneName.text + ": " + WinsFirst;
        leaderTwo.text = playerTwoName.text + ": " + WinsSecond;
        round.text = "Round: 1";
        TabMessage.SetActive(true);
    }

    private void Update() {

        GameTimer();

        Shot();

        if (gameStage == 0 && Input.GetKeyDown(KeyCode.Tab)) ShowLeaderBoard();
        else if (Input.GetKeyUp(KeyCode.Tab)) HideLeaderBoard();
    }

    #endregion

    #region Game Controll

    public void GameTimer()
    {
        if (gameStage == 1)
        {
            if (timer >= 1.0f)
            {
                manageText.text = "steady";
                bangTimer = Random.Range(1.0f, 3.0f);
                gameStage = 2;
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else if (gameStage == 2)
        {
            if (timer >= bangTimer)
            {
                manageText.text = "!bang!";
                bangTimer = 0;
                gameStage = 3;
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public void Shot() {

        if (gameStage == 0) return;
        int player;

        #region 1vs1
#if UNITY_EDITOR

        if (gameStage == 3 && Input.GetMouseButton(0) && !GameController._instance._isComputer) {
            player = (Input.mousePosition.x < Screen.width / 2) ? 1 : 2;
            Finish(player);
        }
#endif

#if UNITY_ANDROID
        if (gameStage == 3 && Input.touchCount>0 && !GameController._instance._isComputer)
        {
            player = (Input.touches[0].position.x < Screen.width / 2) ? 1 : 2;
            Finish(player);
        }
#endif
        #endregion

        #region Computer 
        if(gameStage == 3 && GameController._instance._isComputer)
        {
            StartCoroutine(Computer());
#if UNITY_EDITOR
            if (Input.GetMouseButton(0) && Input.mousePosition.x < Screen.width / 2) Finish(1);
#endif
#if UNITY_ANDROID
            if (Input.touchCount > 0 && Input.touches[0].position.x < Screen.width / 2) Finish(1);
#endif
        }
        #endregion

    }

    public void Finish(int player)
    {
        if (gameStage == 3)
        {
            Image activePlayer = (player == 1) ? playerOne : playerTwo;
            Image enemyPlayer = (player == 2) ? playerOne : playerTwo;
            activePlayer.sprite = playerSprites[2];
            enemyPlayer.sprite = playerSprites[3];
            manageText.text = "player " + player + " win!";
            gameStage = 0;
            if (player == 1) WinsFirst++; else WinsSecond++;
            leaderOne.text = playerOneName.text + ": " + WinsFirst;
            leaderTwo.text = playerTwoName.text + ": " + WinsSecond;
            _StartGame.SetActive(true);
            if (WinsFirst == GameController._instance.countOfWins || WinsSecond == GameController._instance.countOfWins) FinishGame();
            TabMessage.SetActive(true);
        }
    }

    public void StartGame()
    {
        manageText.text = "ready";
        playerOne.sprite = playerSprites[1];
        playerTwo.sprite = playerSprites[1];
        gameStage = 1;
        _StartGame.SetActive(false);
        roundCount++;
        round.text = "Round: " + roundCount.ToString();
        TabMessage.SetActive(false);
    }

    #endregion

    #region Finish Game
    public void FinishGame()
    {
        panel.SetActive(false);
        FinishPanel.SetActive(true);
        finishRounds.text = "All rounds: " + roundCount.ToString();
        string winner = (WinsFirst > WinsSecond) ? playerOneName.text : playerTwoName.text;
        playerWins.text = "The winner of the battle is " + winner;
        
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    #region Leader Board

    private void ShowLeaderBoard()
    {
        TabPanel.SetActive(true);
        rnd.text = "Round: " + roundCount;
        pl1.text = playerOneName.text + ": " + WinsFirst;
        pl2.text = playerTwoName.text + ": " + WinsSecond;
        _isOpenLeaderBoard = true;
    }

    private void HideLeaderBoard()
    {
        TabPanel.SetActive(false);
        _isOpenLeaderBoard = false;
    }

    public void LeaderBoard ()
    {
        if (_isOpenLeaderBoard) HideLeaderBoard();
        else ShowLeaderBoard();
    }

    #endregion

    #region Interfaces

    IEnumerator Computer()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        Finish(2);
    }

    #endregion

}
