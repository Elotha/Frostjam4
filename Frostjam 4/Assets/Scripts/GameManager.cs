using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text leftText;
    public Text timeText;
    public int problemLeft;
    public int gameLastsFor;
    void Start()
    {
        InvokeRepeating("CountDowner", 0f, 1f);
        problemLeft = FindObjectsOfType<Problem>().Length;
        UpdateProblemLeft();
        UpdateTime();
    }

    private void CountDowner()
    {
        if (gameLastsFor > 0)
        {
            gameLastsFor--;
            UpdateTime();
        }
        else
        {
            GameOver();
        }
    }

    private void UpdateTime()
    {
        timeText.text = "Time left: " + gameLastsFor;
    }

    public void UpdateProblemLeft()
    {
        leftText.text = "Problems left: " + problemLeft;
    }

    private void GameOver()
    {
        if (problemLeft > 0)
        {
            Debug.Log("Lose");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("Win");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
