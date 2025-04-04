using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int score;
    public Vector3 playerPositon;
    public Vector3[] enemies;
    public int health;

    public GameData(int score, Vector3 playerPositon, int health, GameObject[] enemies)
    {
        Debug.Log(enemies.Length);
        this.score = score;
        this.playerPositon = playerPositon;
        this.health = health;

        for (int i = 0; i < enemies.Length; i++)
        {
            this.enemies[i] = enemies[i].transform.position;
        }
    }
}