using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image fillImage;

    private void Start()
    {
        StartAnimateScore();
    }

    public void StartAnimateScore()
    {
        StartCoroutine(AnimateScoreText());
    }

    IEnumerator AnimateScoreText()
    {
        int num = 0;
        int score = ScoreKeeper.instance.score;
        Debug.Log(score);
        while (score > num)
        {
            num++;
            scoreText.text = num.ToString() + "/20";
            fillImage.fillAmount = num / score;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void ChangeScene()
    {
        ScoreKeeper.instance.SetScoreToDefault();
        SceneManage.Instance.ChangeScene();
    }

    public void ExitGame()
    {
        SceneManage.Instance.QuitGame();
    }
}
