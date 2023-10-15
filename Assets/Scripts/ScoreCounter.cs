using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ScoreCounter : MonoBehaviour
{

    public static int score = 0;
    private int previousY;
    private int maxZeroes = 11;
    private string scoreString;

    [SerializeField] private TextMesh scoreTextMesh;
    [SerializeField] private int scoreModulo = 20;

    private void Start()
    {
        scoreString = "00000000000";
        scoreTextMesh.gameObject.SetActive(false);
        previousY = (int)transform.position.y;
    }

    private void Update()
    {
        if(JumpController.gameStart == true)
        {
            scoreTextMesh.gameObject.SetActive(true);

            scoreTextMesh.transform.position = new Vector3(scoreTextMesh.transform.position.x, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - 1, scoreTextMesh.transform.position.z);

            if (previousY != (int)transform.position.y && previousY < (int)transform.position.y)
            {
                if ((int)transform.position.y % scoreModulo == 0)
                {
                    score += 1;
                }

                scoreTextMesh.text = scoreString.Substring(0, maxZeroes - score.ToString().Length) + score;
                previousY = (int)transform.position.y;
            }
        }

       
    }



}
