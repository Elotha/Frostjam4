using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text leftText;
    public Text timeText;
    public Text aiSentienceText;
    public int problemLeft => GridManager.Instance.problemsList.Count;
    public int gameLastsFor;
    public static GameManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        InvokeRepeating("CountDowner", 0f, 1f);
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

    private void Update()
    {
        UpdateAiSentiencePercent();
    }

    private void UpdateTime()
    {
        timeText.text = "Time left: " + gameLastsFor;
    }

    public void UpdateProblemLeft()
    {
        leftText.text = "Problems left: " + problemLeft;
    }

    public void UpdateAiSentiencePercent()
    {
        var highestSentience = 0f;
        foreach (var variable in GridManager.Instance.robotList)
            if (highestSentience < variable.Key.sentience)
                highestSentience = variable.Key.sentience;

        aiSentienceText.text = "Maximum AI Sentience: " + (int) highestSentience + "%";
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
