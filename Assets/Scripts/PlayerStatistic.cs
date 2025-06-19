using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistic : MonoBehaviour
{
    [SerializeField] public int _score;

    [Header("Fireball setting for dragon")]
    [SerializeField] public int _fireballClipSize;
    [SerializeField] public int _fireballAmmo;

    public void CalcScore(int newIncreaseScore)
    {
        print(newIncreaseScore);
        _score += newIncreaseScore;
    }
}