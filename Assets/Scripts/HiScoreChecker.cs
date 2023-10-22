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
    [SerializeField] private RectTransform scoreTransform;

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


        float aspectRatio = ((float)Screen.height / (float)Screen.width);



        float transformHeight = -1f * (scoreTransform.transform.localScale.y * scoreTransform.rect.height);

        if (aspectRatio >= 1.8f)
        {
            float sizeDelta = (Screen.width ) / scoreTransform.rect.width;
            scoreTransform.transform.localScale = new Vector2(sizeDelta, sizeDelta);
            scoreTransform.position = new Vector3(scoreTransform.transform.position.x, Screen.height + (transformHeight ), scoreTransform.transform.position.z);

        }

        else if (aspectRatio >= 1.5f)
        {
            float sizeDelta = (Screen.width * 0.8f) / scoreTransform.rect.width;
            scoreTransform.transform.localScale = new Vector2(sizeDelta, sizeDelta);
            scoreTransform.position = new Vector3(scoreTransform.transform.position.x, Screen.height + (transformHeight), scoreTransform.transform.position.z);

        }
        else
        {
            float sizeDelta = (Screen.width * 0.7f) / scoreTransform.rect.width;
            scoreTransform.transform.localScale = new Vector2(sizeDelta, sizeDelta);
            scoreTransform.position = new Vector3(scoreTransform.transform.position.x, Screen.height + (transformHeight / 2), scoreTransform.transform.position.z);


        }
        Debug.Log("TEEEEEEEST: " + scoreTransform.transform.localScale.y * scoreTransform.rect.height + " or " + scoreTransform.rect.height + " at scale: " + scoreTransform.transform.localScale.y);
        if (score > hiScore)
        {
            PlayerPrefs.SetInt("hiScore", score);
            hiScore = score;
        }
        hiScoreText.text = "HISCORE: " + hiScoreString.Substring(9, maxZeroes - hiScore.ToString().Length) + hiScore;


    }


}
