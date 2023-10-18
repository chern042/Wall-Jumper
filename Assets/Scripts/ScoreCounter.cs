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

    [SerializeField] private Text scoreTextMesh;
    [SerializeField] private int scoreModulo = 20;
    [SerializeField] private AudioSource scoreSound;


    private void Start()
    {
        score = 0;
        scoreString = "00000000000";
        scoreTextMesh.gameObject.SetActive(false);
        previousY = (int)transform.position.y;
        scoreTextMesh.transform.position = new Vector3(scoreTextMesh.transform.position.x, Screen.height *0.9f, scoreTextMesh.transform.position.z);

    }

    private void Update()
    {
        if(JumpController.gameStart == true)
        {
            scoreTextMesh.gameObject.SetActive(true);


            if (previousY != (int)transform.position.y && previousY < (int)transform.position.y)
            {
                if ((int)transform.position.y % scoreModulo == 0)
                {
                    score += 1;
                    scoreSound.Play();
                }

                scoreTextMesh.text = scoreString.Substring(0, maxZeroes - score.ToString().Length) + score;
                previousY = (int)transform.position.y;
            }
        }
        else
        {
            scoreTextMesh.gameObject.SetActive(false);
        }

       
    }



}
