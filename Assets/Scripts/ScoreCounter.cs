using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        scoreText.transform.position = new Vector2(scoreText.transform.position.x, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)).y);
    }

    private void Update()
    {
        score = (int)transform.position.y;
        if (score > 0 && score > previousScore)
        {
            scoreText.text = scoreString.Substring(0, maxZeroes - score.ToString().Length) + score;
            previousScore = score;

        }
    }



}
