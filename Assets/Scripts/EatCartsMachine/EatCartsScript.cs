using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EatCartsScript : MonoBehaviour
{
    [SerializeField] private CoinShooter coinShooter;
    [SerializeField] private static List<int> curMission;
    [SerializeField] private float timeBtwCarts = 0.2f;
    [SerializeField] private List<GameObject> staticCarts;
    [SerializeField] private GameObject combo3;
    [SerializeField] private GameObject combo5;
    // [SerializeField] private 
    private int _missionNum;
    private float _curTimeBtwCarts;
    private bool cartEnteredInTime;
    private snakeMovmentScript _player;
    private MissionCanvas _missionCanvas;
    private int _numOfCartsToAccept;
    private Combos _combos;
    private Dictionary<string, int> _colorDictionary;

    private void Start()
    {
        _player = snakeMovmentScript.instance;
        _missionCanvas = FindObjectOfType<MissionCanvas>();
        _curTimeBtwCarts = timeBtwCarts;
        _colorDictionary = GameManager.Shared.colorDictionary;
        _combos = FindObjectOfType<Combos>();
    }

    private void Update()
    {
        if (!cartEnteredInTime) return;
        _curTimeBtwCarts -= Time.deltaTime;
        if (_curTimeBtwCarts <= 0)
        {
            AcceptCarts();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_colorDictionary.ContainsKey(other.gameObject.tag) || 
            other.GetComponent<TriggerEncunterAddsLink>()) return;
        if(curMission.Count == 0 )return;
        if (_colorDictionary[other.tag] != curMission[^1])
        {
            MissionsGeneration.Shared.BopLastCart();
            return;
        };  
        if(_player.bodyParts[0] != other.transform) return;
        _curTimeBtwCarts = timeBtwCarts;
        _numOfCartsToAccept++;
        coinShooter.ShootSmall();
        _missionCanvas.Mark();
        SoundManager.shared.PlayVding();
        curMission.RemoveAt(curMission.Count - 1);
        snakeMovmentScript.instance.RemoveBodyPart();
        cartEnteredInTime = true;
    }
    

    private void AcceptCarts()
    {
        cartEnteredInTime = false;
        for (int i = 0; i < _numOfCartsToAccept; i++)
        {
            ScoreScript.instance.AddScore(5);
        }

        switch (_numOfCartsToAccept)
        {
            case 3 or 4:
                _combos.ShowThreeCombo();
                ScoreScript.instance.AddScore(15);
                break;
            
            case >= 5:
                _combos.ShowFiveCombo();
                ScoreScript.instance.AddScore(25);
                break;
        }
        _numOfCartsToAccept = 0;
        if (curMission.Count != 0) return; 
        MissionsGeneration.Shared.FinishMission();
        if (_missionNum < 3) staticCarts[_missionNum++].SetActive(true);
        coinShooter.Shoot();
    }

    public void SetMission(List<int> mission)
    {
        curMission = mission;
    }
    
    // private IEnumerator SetComboActive (GameObject combo)
    // {
    //     combo.SetActive(true);
    //     yield return new WaitForSeconds(5f);
    //     combo.SetActive(false);
    // }
}
