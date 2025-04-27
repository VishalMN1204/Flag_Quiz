using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Question", menuName ="Create Question")]
public class QuestionSO : ScriptableObject
{
    [SerializeField] Sprite flagImage;
    [SerializeField] List<string> options = new(4);
    [SerializeField] int correctAnswer;


    public Sprite GetFlagSprite()
    {
        return flagImage;
    }

    public string GetOption(int index)
    {
        return options[index];
    }

    public int GetRightAnswerIndex()
    {
        return correctAnswer;
    }
}
