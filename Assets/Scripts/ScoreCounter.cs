using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ScoreCounter : MonoBehaviour
{

    private int score = 0;
    private int previousScore;
    private int maxZeroes = 11;
    private string scoreString;

    [SerializeField] private Text scoreText;

    private void Start()
    {
        scoreString = "00000000000";
        scoreText.transform.position = new Vector2(scoreText.transform.position.x, Camera.main.WorldToScreenPoint(Camera.main.transform.position).y*1.8f );


    }

    private void Update()
    {
        scoreText.transform.position = new Vector2(scoreText.transform.position.x, Camera.main.WorldToScreenPoint(Camera.main.transform.position).y * 1.8f);

        score = (int)transform.position.y;
        if (score > 0 && score > previousScore)
        {
            scoreText.text = scoreString.Substring(0, maxZeroes - score.ToString().Length) + score;
            previousScore = score;

        }
    }



}
