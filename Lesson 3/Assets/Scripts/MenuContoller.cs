using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuContoller : MonoBehaviour {

    public InputField playerCompName;
    public InputField playerOneName;
    public InputField playerTwoName;
    public InputField countWins;

    public void Computer()
    {
        GameController._instance._isComputer = true;
    }

    public void OneByOne()
    {
        GameController._instance._isComputer = false;
    }

    public void StartGame()
    {
        if (GameController._instance._isComputer)
        {
            if (playerCompName.text != "")
                GameController._instance.PlayerOneName = playerCompName.text;
            GameController._instance.PlayerTwoName = "Computer BOT";
        }
        else
        {
            if (playerOneName.text != "")
                GameController._instance.PlayerOneName = playerOneName.text;
            if (playerTwoName.text != "")
                GameController._instance.PlayerTwoName = playerTwoName.text;
        }
        if (countWins.text != "")
            GameController._instance.countOfWins = Convert.ToInt32(countWins.text);
        SceneManager.LoadScene(1);
    }
    public void Exit ()
    {
        Application.Quit();
    }
}
