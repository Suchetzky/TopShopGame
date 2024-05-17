using System;
using Unity.Mathematics;
using UnityEngine;

public class RandomizeCartsMaker : MonoBehaviour
{
    [SerializeField] private GameObject[] nonMoveCart;
    [SerializeField] private float initialTime = 1;
    private float _timer = 3f;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;
    [SerializeField] private int numOfCarts;
    [SerializeField] private int startNumOfCarts = 5;
    [SerializeField] private int MaxNumOfCarts = 25;
    private Camera _mainCamera;
    private float radius = 2f;
    private bool flagStart = true;

    public static RandomizeCartsMaker instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (flagStart)
        {
            flagStart = false;
            for (int i = 0; i < startNumOfCarts; i++)
            {
                RandomizeNonMoveCart();
                numOfCarts++;
            }
        }

        _timer -= Time.deltaTime;
        if (_timer < 0 && numOfCarts < MaxNumOfCarts)
        {
            RandomizeNonMoveCart();
            _timer = initialTime;
        }
    }

    private void RandomizeNonMoveCart()
    {
        int randomIndex = UnityEngine.Random.Range(0, nonMoveCart.Length);
        Instantiate(nonMoveCart[randomIndex], GetRandomPosition(), quaternion.identity);
        numOfCarts++;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new Vector3(
            UnityEngine.Random.Range(minX, maxX),
            1.3f,
            UnityEngine.Random.Range(minZ, maxZ)
        );


        if (IsPositionVisible(randomPosition, _mainCamera))
        {
            randomPosition = new Vector3(
                UnityEngine.Random.Range(minX, maxX),
                1.3f,
                UnityEngine.Random.Range(minZ, maxZ)
            );
        }

        return randomPosition;
    }
    
        private bool IsPositionVisible(Vector3 position, Camera camera)
        {
            Vector3 viewportPoint = camera.WorldToViewportPoint(position);
            return (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1);
        }

        public void ReduceCart()
        {
            numOfCarts--;
        }
    }