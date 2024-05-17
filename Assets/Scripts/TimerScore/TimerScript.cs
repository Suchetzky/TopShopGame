using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private float timeLeft;
    private float originalTime;
    [SerializeField] private bool timerOn = false;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image fill;
    [SerializeField] private Image backFill;
    [SerializeField] private GameObject endShiftStiker;
    [SerializeField] private Color green;
    [SerializeField] private Color orange;
    [SerializeField] private Color red;

    private bool flag_1;
    private bool flag_2;
    private Color _backGreen;
    private Color _backOrange;
    private Color _backRed;
    
    [SerializeField] private Animator _animator;
    private void Start()
    {
        timerOn = true;
        originalTime = timeLeft;
        _backGreen = green;
        _backGreen.a = 0.5f;
        _backOrange = orange;
        _backOrange.a = 0.5f;
        _backRed = red;
        _backRed.a = 0.5f;
    }

    private void Update()
    {
        if (!timerOn) return;
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer(timeLeft);
        }
        else
        {
            timeLeft = 0;
            timerOn = false;
            ScoreScript.instance.SetNewScores();
            StartCoroutine(LoadEndScene());
        }

    }
    
    private IEnumerator LoadEndScene()
    {
        endShiftStiker.SetActive(true);
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("End");
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        fill.fillAmount = currentTime / originalTime;
        backFill.fillAmount = currentTime / originalTime;
        if (currentTime > 100)
        {
            fill.color = green;
            backFill.color = _backGreen;
        }
        
        if (currentTime is <= 60 and > 15)
        {
            fill.color = orange;
            backFill.color = _backOrange;
            if (!flag_1)
            {
                flag_1 = true;
                _animator.SetTrigger("Shake");
                SoundManager.shared.PlayAlarmSound();
            }
        }
        
        if (currentTime <= 15)
        {
            fill.color = red;
            backFill.color = _backRed;
            if (!flag_2)
            {
                flag_2 = true;
                _animator.SetTrigger("Shake");
                SoundManager.shared.PlayAlarmSound();
            }
        }

    }

    public void AddTime(float extraTime)
    {
        timeLeft += extraTime;
    }

    public void SubtractTime(float minusTime)
    {
        timeLeft = Mathf.Min(timeLeft - minusTime, 0);
    }

    public void ResetTimer()
    {
        timeLeft = originalTime;
    }
}
