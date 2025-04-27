using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToCompleteQuestion;
    [SerializeField] float timerValue;
    [SerializeField] Image m_fillImage;
    [SerializeField] TextMeshProUGUI timerText;
    float timeToComplete;
    bool isTimerPause;
    float timeAdded = 30f;
    public static Action s_showCorrectAnswer;

    private void OnEnable()
    {
        QuizManager.s_startTimer += StartingTimer;
        QuizManager.s_pauseTimer += PauseTimer;
        QuizManager.s_addTime += AddTime;
    }

    private void OnDisable()
    {
        QuizManager.s_startTimer -= StartingTimer;
        QuizManager.s_pauseTimer -= PauseTimer;
        QuizManager.s_addTime -= AddTime;
    }

    public void StartingTimer()
    {
        isTimerPause = false;
        timeToComplete = timeToCompleteQuestion;
        timerValue = timeToComplete;
        StartCoroutine(nameof(StartTimer));
    }

    IEnumerator StartTimer()
    {
        while(timerValue > 0 && !isTimerPause)
        {
            timerValue -= Time.deltaTime;
            timerValue = timerValue < 0 ? 0 : timerValue;
            m_fillImage.fillAmount = timerValue / timeToComplete;
            timerText.text = timerValue.ToString("0");
            yield return null;
        }
        TimerFinished();
    }

    private void TimerFinished()
    {
        if (timerValue != 0) return;
        s_showCorrectAnswer?.Invoke();
    }

    public void PauseTimer()
    {
        isTimerPause = true;
    }

    public void AddTime()
    {
        timerValue += timeAdded;
        timeToComplete += timeAdded;
    }
}
