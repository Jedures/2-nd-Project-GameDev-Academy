using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController _instance = null;

    public bool _isComputer = true;
    public int countOfWins = 10;
    public string PlayerOneName = "Player One";
    public string PlayerTwoName = "Player Two";
    

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(_instance);
        DontDestroyOnLoad(this);
    }
    

    
}
