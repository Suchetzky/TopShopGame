using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [SerializeField] private float dstTravelled;
    [SerializeField] private float rayLength;
    [SerializeField] private PathCreator inTrack;
    [SerializeField] private PathCreator outTrack;
    [SerializeField] private EndOfPathInstruction end = EndOfPathInstruction.Stop;
    [SerializeField] private Transform carTransform;
    [SerializeField] private Transform frontRay1, frontRay2;
    [SerializeField] private Transform backRay1, backRay2;
    [SerializeField] private LayerMask checkHitLayers;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private float timeToStopHitPlayer = 1;
    
    private BoxCollider _col;
    private TracksManager _tracksManager;
    private MakeObjectShake _shakeScript;
    private snakeMovmentScript _player;
    private GameObject _avatar;
    private Vector3 _lastPos;
    private Vector3 _hitDir;
    private int _freeSpaceNum;
    private float _speed;
    private float _timeBtwCars;
    private bool _endedTrack = true;
    private bool _driveIn;
    private bool _driveOut;
    private bool _isWaiting;
    private bool _enterTrack;
    private bool _exitTrack;
    private bool _stopCar;
    private bool _hitPlayer;
    private Dictionary<string, int> _colorDictionary;
    private Animator _animator;
    private void Start()
    {
        _colorDictionary = GameManager.Shared.colorDictionary;
        dstTravelled = 0;
        _shakeScript = GetComponent<MakeObjectShake>();
        carTransform = transform;
        _tracksManager = TracksManager.Shared;
        _col = GetComponent<BoxCollider>();
        _player = FindObjectOfType<snakeMovmentScript>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_hitPlayer)
        {
            HitPlayer();
        }
        if (_driveIn)
        {
            IsNearObject(true);
            TrackIn();
        }
        if (!_driveOut) return;
        IsNearObject(false);
        TrackOut();
        
    }

    private void LateUpdate()
    {
        _lastPos = transform.position;
    }

    public void SetTrackIn(PathCreator pathIn, PathCreator pathOut, float timeBtwCars, int freeSpace)
    {
        inTrack = pathIn;
        outTrack = pathOut;
        _timeBtwCars = timeBtwCars;
        _freeSpaceNum = freeSpace;
    }
    

    private void TrackIn()
    {
        if(_stopCar || _isWaiting) return;
        dstTravelled += _speed * Time.deltaTime;
        transform.position = inTrack.path.GetPointAtDistance(dstTravelled, end);
        if (transform.position == _lastPos) return;
        var transform1 = transform;
        if (transform1.position - _lastPos != Vector3.zero) transform1.forward = -(transform1.position - _lastPos);
        
        if (inTrack.path.GetClosestTimeOnPath(transform.position) != 1) return;
        transform1.Rotate(0,180,0);
        StartCoroutine(WaitAndStartOut(_timeBtwCars));
    }

    private void TrackOut()
    {
        if(_stopCar) return;
        dstTravelled -= _speed * Time.deltaTime;
        var transform1 = transform;
        transform1.position = outTrack.path.GetPointAtDistance(dstTravelled, end);
        if (transform1.position - _lastPos != Vector3.zero) transform1.forward = -(transform1.position - _lastPos);

        if (inTrack.path.GetClosestTimeOnPath(transform.position) != 0) return;
        _endedTrack = true;
        _col.enabled = false;
        transform.position = _tracksManager.carSpawnPoint.position;
    }

    public void StartIn()
    {
        dstTravelled = 0;
        _driveIn = true;
        _driveOut = false;
        _endedTrack = false;
        _col.enabled = true;
        _shakeScript.ShakeObject();
        GetBig();
    }

    private void StartOut()
    {
        dstTravelled = outTrack.path.length;
        _isWaiting = false;
        _stopCar = false;
        _driveIn = false;
        _driveOut = true;
        _col.isTrigger = true;
    }

    private IEnumerator WaitAndStartOut(float timeBtwCars)
    {
        _isWaiting = true;
        _col.isTrigger = false;
        _shakeScript.ShakeObject();
        yield return new WaitForSeconds(timeBtwCars);
        StartCoroutine(SmokeAndStartOut());
    }

    private IEnumerator SmokeAndStartOut()
    {
        _shakeScript.ShakeObject();
        smoke.Play();
        SoundManager.shared.PlayCarEngine();
        yield return new WaitForSeconds(2);
        StartOut();
        _tracksManager.FreeSpace(_freeSpaceNum);
    }

    public bool EndedTrack()
    {
        return _endedTrack;
    }
    
    public void SetSpeed(float speed) {_speed = speed;}

    private void IsNearObject(bool back)
    {
        _stopCar = false;
        if (!back)
        {
            if (Physics.Raycast(frontRay1.position, carTransform.forward * rayLength,2, checkHitLayers) ||
                Physics.Raycast(frontRay2.position, carTransform.forward * rayLength,2, checkHitLayers))
            {
                _stopCar = true;
                SoundManager.shared.PlayHorn();
            }
        }
        else
        {
            if (Physics.Raycast(backRay1.position, -carTransform.forward * rayLength,2, checkHitLayers) ||
                Physics.Raycast(backRay2.position, -carTransform.forward * rayLength,2, checkHitLayers))
            {
                _stopCar = true;
                SoundManager.shared.PlayHorn();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var forward = carTransform.forward;
        Gizmos.DrawRay(backRay1.position, -forward * rayLength);
        Gizmos.DrawRay(backRay2.position, -forward * rayLength);
        Gizmos.DrawRay(frontRay1.position, forward * rayLength);
        Gizmos.DrawRay(frontRay2.position, forward * rayLength);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !_isWaiting)
        {
            if (ThirdPersonMovement.instance.gotHit) return;
            SoundManager.shared.PlayHitSound();
            SoundManager.shared.PlayHorn();
            _hitPlayer = true;
            Gamepad.current?.SetMotorSpeeds(1,1);
            StartCoroutine(StopHitPlayer(Gamepad.current));
            _hitDir = new Vector3((transform.position - snakeMovmentScript.instance.bodyParts[^1].transform.position).x,
            0, (transform.position - snakeMovmentScript.instance.bodyParts[^1].transform.position).z);
            _player.transform.GetChild(0).GetComponent<Rigidbody>()?.AddTorque(new Vector3(0,1,0) * 13, ForceMode.Impulse);
            ThirdPersonMovement.instance.gotHit = true;
            
        }
        
        if (!_colorDictionary.ContainsKey(collision.gameObject.tag) || 
            collision.GetComponent<TriggerEncunterAddsLink>() || _isWaiting) return;
        SoundManager.shared.PlayHorn();
        snakeMovmentScript.instance.Explode(collision.transform);
    }

    private void HitPlayer()
    {
        _player.transform.Translate(-_hitDir * (Time.deltaTime * _speed / 4),Space.World );
    }

    IEnumerator StopHitPlayer(Gamepad pad)
    {
        yield return new WaitForSeconds(timeToStopHitPlayer);
        pad?.SetMotorSpeeds(0,0);
        _hitPlayer = false;
        ThirdPersonMovement.instance.gotHit = false;
    }

    public bool IsOut()
    {
        return _driveOut;
    }

    public void GetSmall()
    {
        _animator.SetBool("Small",true);
        _animator.SetBool("Big",false);
    }
    
    public void GetBig()
    {
        _animator.SetBool("Big",true);
        _animator.SetBool("Small",false);
    }
}
