using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;
    [SerializeField]
    public int score { get; private set; } = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementRightQuestions()
    {
        score++;
    }

    public void SetScoreToDefault()
    {
        score = 0;
    }
}
