using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("List Reference")]
    [SerializeField] List<QuestionSO> questions = new();
    [SerializeField] List<Button> optionsList = new();

    [Header("Sprite and Image Reference")]
    [SerializeField] Image questionSprite;
    [SerializeField] Sprite correctButtonSprite;
    [SerializeField] Sprite wrongButtonSprite;
    [SerializeField] Sprite defaultButtonSprite;

    [Header("Text Reference")]
    [SerializeField] TextMeshProUGUI currentQuestionNumText;

    [Header("Button Reference")]
    [SerializeField] Button fiftyFiftyBtn;
    [SerializeField] Button addThirtySecondsBtn;
    [SerializeField] Button nextButton;

    List<int> incorrectOptionsIndex = new();
    QuestionSO currentQuestion;
    int currentQuestionIndex = 0;
    bool isFiftyFiftyUsed = false;
    bool isAddThirtyUsed = false;
    public static Action s_startTimer;
    public static Action s_pauseTimer;
    public static Action s_addTime;
    public static Action s_displayScore;
    private void OnEnable()
    {
        Timer.s_showCorrectAnswer += ShowCorrectAnswerOnTimerComplete;
    }

    private void OnDisable()
    {
        Timer.s_showCorrectAnswer -= ShowCorrectAnswerOnTimerComplete;
    }

    void Start()
    {
        isFiftyFiftyUsed = false;
        isAddThirtyUsed = false;
        DisplayQuestion();
    }


    void DisplayQuestion()
    {
        nextButton.interactable = false;
        currentQuestionNumText.text = (currentQuestionIndex + 1).ToString() + "/" + questions.Count;
        ToggleHalfAndHalfLifelineButtonsState(true);
        ToggleAddThirtyLifelineButtonsState(true);
        ToggleOptionsButton(true);
        SetOptionsToDefaultState();
        RestoreHalfOptions();
        currentQuestion = questions[currentQuestionIndex];
        questionSprite.sprite = currentQuestion.GetFlagSprite();
        s_startTimer?.Invoke();
        for (int i = 0; i < optionsList.Count; i++)
        {
            TextMeshProUGUI optionText = optionsList[i].GetComponentInChildren<TextMeshProUGUI>();
            optionText.text = currentQuestion.GetOption(i);
        }
    }

    public void GetAnswerFromUser(int answerIndex)
    {
        ToggleOptionsButton(false);
        s_pauseTimer?.Invoke();
        ToggleHalfAndHalfLifelineButtonsState(false);
        ToggleAddThirtyLifelineButtonsState(false);
        if (currentQuestion.GetRightAnswerIndex() == answerIndex)
        {
            ScoreKeeper.instance.IncrementRightQuestions();
        }
        else
        {
            Image selectedButton = optionsList[answerIndex].GetComponent<Image>();
            selectedButton.sprite = wrongButtonSprite;
        }
        ShowCorrectAnswerOnTimerComplete();
        nextButton.interactable = true;
    }


    public void ShowCorrectAnswerOnTimerComplete()
    {
        int correctAnswerIndex = currentQuestion.GetRightAnswerIndex();
        ToggleOptionsButton(false);
        ToggleHalfAndHalfLifelineButtonsState(false);
        ToggleAddThirtyLifelineButtonsState(false);
        Image correctButton = optionsList[correctAnswerIndex].GetComponent<Image>();
        correctButton.sprite = correctButtonSprite;
        nextButton.interactable = true;
    }

    private void ToggleOptionsButton(bool state)
    {
        foreach (Button button in optionsList)
        {
            button.interactable = state;
        }
    }

    public void GoToNextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex < questions.Count)
        {
            DisplayQuestion();
        }
        else
        {
            SceneManage.Instance.ChangeScene();
        }
    }

    private void SetOptionsToDefaultState()
    {
        foreach (Button button in optionsList)
        {
            Image image = button.GetComponent<Image>();
            image.sprite = defaultButtonSprite;
        }
    }

    public void RemoveHalfOptions()
    {
        isFiftyFiftyUsed = true;
        fiftyFiftyBtn.interactable = false;
        for (int i = 0; i < optionsList.Count; i++)
        {
            if (i == currentQuestion.GetRightAnswerIndex()) continue;
            incorrectOptionsIndex.Add(i);
        }
        for (int i = 0; i < 2; i++)
        {
            int randomOptionIndex = UnityEngine.Random.Range(0, incorrectOptionsIndex.Count);
            int incorrectIndexToRemove = incorrectOptionsIndex[randomOptionIndex];
            optionsList[incorrectIndexToRemove].gameObject.SetActive(false);
            incorrectOptionsIndex.RemoveAt(randomOptionIndex);
        }
    }

    private void RestoreHalfOptions()
    {
        if (!isFiftyFiftyUsed) return;
        incorrectOptionsIndex.Clear();
        foreach (Button button in optionsList)
        {
            if (!button.gameObject.activeInHierarchy)
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    public void AddThirtySeconds()
    {
        isAddThirtyUsed = true;
        s_addTime?.Invoke();
        addThirtySecondsBtn.interactable = false;
    }

    private void ToggleHalfAndHalfLifelineButtonsState(bool state)
    {
        if (isFiftyFiftyUsed) return;
        fiftyFiftyBtn.interactable = state;
    } 
    private void ToggleAddThirtyLifelineButtonsState(bool state)
    {
        if (isAddThirtyUsed) return;
        addThirtySecondsBtn.interactable = state;
    }
}
