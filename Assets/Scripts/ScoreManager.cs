using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    Text scoreText;
    private int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = "Score:"+score.ToString();
        }
    }

    private void Awake()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
    }
}
