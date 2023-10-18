using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndMenu : MonoBehaviour
{

    public static bool gameEnded;
    private bool menuOpen;

    [SerializeField]private Canvas endGameMenu;

    // Start is called before the first frame update
    private void Start()
    {
        gameEnded = false;
        menuOpen = false;
        endGameMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameEnded && !menuOpen)
        {
            StartCoroutine(SetGameEndMenu(true));
        }
    }


    IEnumerator SetGameEndMenu(bool set)
    {
        yield return new WaitForSeconds(1);
        endGameMenu.gameObject.SetActive(set);
        menuOpen = true;

    }

    public void InvokeResetLevel()
    {
        Invoke("ResetLevel", 0.5f);
    }

    private void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
