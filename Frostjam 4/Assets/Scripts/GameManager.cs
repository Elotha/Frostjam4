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
    public Slider aiSentienceSlider;
    public Image sliderFillImage;
    public Gradient colorGradient;
    public int problemLeft => GridManager.Instance.problemsList.Count;
    public int gameLastsFor;
    private float aiHighestSentience = 0f;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            // Destroy(this.gameObject);
        }
    }

    void Start()
    {
        InvokeRepeating("CountDowner", 0f, 1f);
        UpdateProblemLeft();
        UpdateTime();
    }

    private void Update()
    {
        GameOverCheck();
        WinCheck();

        UpdateAISentience();
    }

    private void CountDowner()
    {
        gameLastsFor--;
        UpdateTime();
    }
    private void UpdateTime()
    {
        timeText.text = "Time left: " + gameLastsFor;
    }

    public void UpdateProblemLeft()
    {
        leftText.text = "Problems left: " + problemLeft;
    }

    public void UpdateAISentience()
    {
        // update AI sentience
        var highestSentience = 0f;
        foreach (var variable in GridManager.Instance.robotList)
            if (highestSentience < variable.Key.sentience)
                highestSentience = variable.Key.sentience;

        aiHighestSentience = highestSentience;
        aiSentienceText.text = "Maximum AI Sentience: " + (int) highestSentience + "%";
        aiSentienceSlider.value = highestSentience;
        sliderFillImage.color = colorGradient.Evaluate(highestSentience / 100f);
    }    

    private void GameOverCheck()
    {
        if ((gameLastsFor <= 0 && problemLeft > 0) || aiHighestSentience >= 100f)
        {
            Debug.Log("Lose");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void WinCheck()
    {
        if(problemLeft <= 0 && gameLastsFor < 0)
        {
            Debug.Log("Win");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
