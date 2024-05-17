using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    
    public static ScoreScript instance;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Animator animator;
    private SceneManager _sceneManager;
    private int score = 0;
    private int highScore = 0;
    private static readonly int Pop = Animator.StringToHash("Pop");

    /*
    Hadar & Eitam !!
    To Use the ScoreScript you can use: ScoreScript.instance.AddScore(X)
    No need anything else.
    */
    
    private void Awake()
    {
        instance = this;
        PlayerPrefs.SetInt("curScore",0);
    }

    void Start()
    {
        scoreText.text = score.ToString();
        _sceneManager = SceneManager.shared;
    }
    

    public void AddScore(int extraScore)
    {
        score += extraScore;
        scoreText.text = score.ToString();
        animator.SetTrigger(Pop);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highscore",score);
            highScoreText.text = "HIGHSCORE: " + highScore.ToString();
        }
    }

    public void SubtractScore(int minusScore)
    {
        score = Mathf.Min(0, score - minusScore);
        scoreText.text = score.ToString();
    }

    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt("highscore",0);
        highScoreText.text = "HIGHSCORE: " + highScore.ToString();

    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();

    }

    public void SetNewScores()
    {
        _sceneManager.Record(score);

    }
}
