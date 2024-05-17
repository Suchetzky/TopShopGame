using System;
using System.Collections;
using System.Collections.Generic;
using Avrahamy.EditorGadgets;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class SceneManager : MonoBehaviour
{
    [SerializeField,ConditionalHide("isEndScene")] private TextMeshProUGUI score1;
    [SerializeField,ConditionalHide("isEndScene")] private TextMeshProUGUI score2;
    [SerializeField,ConditionalHide("isEndScene")] private TextMeshProUGUI score3;
    [SerializeField,ConditionalHide("isEndScene")] private TextMeshProUGUI curScoreTxt;
   

    [SerializeField] private bool isFirstScene;
    [SerializeField,ConditionalHide("isFirstScene")] private SpriteRenderer image;
    [SerializeField,ConditionalHide("isFirstScene")] private SpriteRenderer image2;
    [SerializeField,ConditionalHide("isFirstScene")] private VideoPlayer player1;
    [SerializeField,ConditionalHide("isFirstScene")] private VideoPlayer player2;
    [SerializeField,ConditionalHide("isFirstScene")] private VideoClip clip1;
    [SerializeField,ConditionalHide("isFirstScene")] private VideoClip clip2;

    [SerializeField] private bool isEndScene;
    [SerializeField,ConditionalHide("isEndScene")] private Button again, quit;
        
    public static SceneManager shared { get; private set;}
    private List<VideoPlayer> videoPlayerList;

    private bool show1;
    private bool show2;
    private int videoIndex = 0;
    private void Awake()
    {
        shared = this;
        if (isFirstScene)
        {
            player1.clip = clip1;
            player2.clip = clip2;
            player2.Prepare();
            image2.color = new Color(1, 1, 1, 0);
        }
    }

    private void Start()
    {
        if (isEndScene)
        {
            StartCoroutine(WaitASec());
            score1.text = Entries[0].score.ToString();
            score2.text = Entries[1].score.ToString();
            score3.text = Entries[2].score.ToString();
            curScoreTxt.text = PlayerPrefs.GetInt("CurScore").ToString();
        }
        
        if (isFirstScene) player1.loopPointReached += source =>
        {
            image.color = new Color(1, 1, 1, 0);
            image2.color = Color.white; 
            player2.Play();
            
        };
        
    }

    public struct ScoreEntry {
        public int score;

        public ScoreEntry(int score) {
            this.score = score;
        }
    }
 
    private static List<ScoreEntry> s_Entries;
    [SerializeField] private static int EntryCount = 5;

    private static List<ScoreEntry> Entries {
        get {
            if (s_Entries == null) {
                s_Entries = new List<ScoreEntry>();
                LoadScores();
            }
            return s_Entries;
        }
    }
 
    private const string PlayerPrefsBaseKey = "leaderboard";

    private static void SortScores() {
        s_Entries.Sort((a, b) => b.score.CompareTo(a.score));
    }

    private static void LoadScores() {
        s_Entries.Clear();

        for (int i = 0; i < EntryCount; ++i) {
            ScoreEntry entry;
            entry.score = PlayerPrefs.GetInt(PlayerPrefsBaseKey + "[" + i + "].score", 0);
            s_Entries.Add(entry);
        }

        SortScores();
    }
 
    private static void SaveScores() {
        for (int i = 0; i < EntryCount; ++i) {
            var entry = s_Entries[i];
            PlayerPrefs.SetInt(PlayerPrefsBaseKey + "[" + i + "].score", entry.score);
        }
    }

    public static ScoreEntry GetEntry(int index) {
        return Entries[index];
    }

    public void Record(int score) {
        PlayerPrefs.SetInt("CurScore", score);
        Entries.Add(new ScoreEntry(score));
        SortScores();
        Entries.RemoveAt(Entries.Count - 1);
        SaveScores();
    }

    private IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(1);
        again.interactable = true;
        quit.interactable = true;
    }
}
