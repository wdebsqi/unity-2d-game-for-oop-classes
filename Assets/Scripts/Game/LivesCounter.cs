﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesCounter : MonoBehaviour
{
    public PlayerHealthManager playerHM;
    Text lives;

    // Start is called before the first frame update
    void Start()
    {
        lives = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // if/else statements - Game - score counting mechanism
        #region Lives mechanics
        lives.text = "" + playerHM.health;
        #endregion
    }
}