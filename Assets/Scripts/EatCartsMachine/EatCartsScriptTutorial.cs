using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EatCartsScriptTutorial : MonoBehaviour
{
    [SerializeField] private CoinShooter coinShooter;
    [SerializeField] private static List<int> curMission;
    [SerializeField] private float timeBtwCarts = 0.2f;
    private int _missionNum;
    private float _curTimeBtwCarts;
    private bool cartEnteredInTime;
    private snakeMovmentScript _player;
    private int _numOfCartsToAccept;
    
    private Dictionary<string, int> _colorDictionary;

    private void Start()
    {
        curMission = new List<int>();
        curMission.Add(2);
        curMission.Add(1);
        _player = snakeMovmentScript.instance;
        _curTimeBtwCarts = timeBtwCarts;
        _colorDictionary = GameManager.Shared.colorDictionary;
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
            return;
        };  
        if(_player.bodyParts[0] != other.transform) return;
        _curTimeBtwCarts = timeBtwCarts;
        _numOfCartsToAccept++;
        coinShooter.ShootSmall();
        SoundManager.shared.PlayVding();
        curMission.RemoveAt(curMission.Count - 1);
        snakeMovmentScript.instance.RemoveBodyPart();
        cartEnteredInTime = true;
    }
    

    private void AcceptCarts()
    {
        cartEnteredInTime = false;

        switch (_numOfCartsToAccept)
        {
            case 3 or 4:
                break;
            
            case >= 5:
                break;
        }
        _numOfCartsToAccept = 0;
        if (curMission.Count != 0) return; 

        coinShooter.Shoot();
        ImageSwitcher.instance.PlayThirdSet();
        Invoke("PlayStartShift", 2f);
        Invoke("LoadScene", 4f);
        
    }
    private void PlayStartShift()
    {
        ImageSwitcher.instance.PlayFourthSet();
    }
    
    private void LoadScene()
    {
        // Load the specified scene by name
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
