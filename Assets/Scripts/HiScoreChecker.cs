using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiScoreChecker : MonoBehaviour
{

    private int score;
    private int maxZeroes = 11;
    private string scoreString;
    private string hiScoreString;
    private int hiScore;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text hiScoreText;

    // Start is called before the first frame update
    private void Start()
    {
        hiScore = PlayerPrefs.GetInt("hiScore",0);

    }


    private void OnEnable()
    {
        scoreString = "00000000000";
        hiScoreString = "HISCORE: 00000000000";
        hiScore = PlayerPrefs.GetInt("hiScore");
        score = ScoreCounter.score;
        scoreText.text = scoreString.Substring(0, maxZeroes - score.ToString().Length) + score;
        scoreText.transform.position = new Vector2(scoreText.transform.position.x, Screen.height * 0.9f);
        if (score > hiScore)
        {
            PlayerPrefs.SetInt("hiScore", score);
            hiScore = score;
        }
        hiScoreText.text = "HISCORE: " + hiScoreString.Substring(9, maxZeroes - hiScore.ToString().Length) + hiScore;


    }


}
