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
    private int hiScore = 0;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text hiScoreText;

    // Start is called before the first frame update
    private void Start()
    {
        hiScore = PlayerPrefs.GetInt("hiScore");

    }


    private void OnEnable()
    {
        scoreString = "00000000000";
        hiScoreString = "HISCORE: 00000000000";
        score = ScoreCounter.score;
        scoreText.text = scoreString.Substring(0, maxZeroes - score.ToString().Length) + score;
        hiScoreText.text = "HISCORE: "+hiScoreString.Substring(9, maxZeroes - hiScore.ToString().Length) + hiScore;
        scoreText.transform.position = new Vector2(scoreText.transform.position.x, Screen.height * 0.9f);
        hiScoreText.transform.position = new Vector2(hiScoreText.transform.position.x, Screen.height * 0.85f);
        if (score > hiScore)
        {
            PlayerPrefs.SetInt("hiScore", score);
            hiScore = score;
        }


    }


}
