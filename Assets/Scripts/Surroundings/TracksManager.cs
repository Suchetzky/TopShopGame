using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using Random = UnityEngine.Random;

public class TracksManager : MonoBehaviour
{
    public static TracksManager Shared { get; private set; }

    [SerializeField] private List<Car> cars;
    [SerializeField] private List<Transform> tracks;
    [SerializeField] private float timeToOut = 2;
    [SerializeField] private float timeBtwCars = 3;
    [SerializeField] private float speed = .5f;
    [SerializeField] private List<bool> freeSpots;
    private int _curCar;
    private int _lowOrUp;

    public Transform carSpawnPoint;

    private void Awake()
    {
        Shared = this;
    }

    private void Start()
    {
        freeSpots = new List<bool>(tracks.Count);
        for (int i = 0; i < tracks.Count; i++) {freeSpots.Add(true);}
        SetCarsSpeed();
        StartCoroutine(CarsInterval());
    }

    private void EnterCar(Car _car)
    {
        var rnd = Random.Range(0, tracks.Count);
        bool foundEmpty = false;
        if (freeSpots[rnd] == false)
        {
            for (int j = 0; j < freeSpots.Count; j++)
            {
                if (!freeSpots[j]) continue;
                rnd = j;
                foundEmpty = true;
            }
            if (!foundEmpty)
            {
                return;
            }
        }
        
        _car.SetTrackIn(tracks[rnd].GetChild(0).GetComponent<PathCreator>(),
            tracks[rnd].GetChild(1).GetComponent<PathCreator>(), timeToOut, rnd);
        freeSpots[rnd] = false;
        _car.StartIn();
    }

    private IEnumerator CarsInterval()
    {
        yield return new WaitForSeconds(timeBtwCars);
        if (cars[_curCar].EndedTrack())
        {
            EnterCar(cars[_curCar]);
            _curCar = ++_curCar % cars.Count;
        }
        else
        {
            foreach (var t in cars)
            {
                if (t.EndedTrack()) EnterCar(t);
            }
        }
        StartCoroutine(CarsInterval());
    }

    public void FreeSpace( int num)
    {
        freeSpots[num] = true;
    }

    private void SetCarsSpeed()
    {
        foreach (var car in cars)
        {
            car.SetSpeed(speed);
        }
    }
}
