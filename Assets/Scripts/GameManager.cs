using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool blockBeingHit = false;

    public Player.PlayerState playerState;
    public int coinCounter = 0;


    private static GameManager _instance;

    #region Singlton Pattern
    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

}
