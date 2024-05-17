using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 _initialPlayerPos;
    public static GameManager Shared { get; private set; }

    public Dictionary<string, int> colorDictionary = new()
    {
        { "GreenCart", 0 },
        { "LightBlueCart", 1 },
        { "RedCart", 2 },
        { "WhiteCart", 3 },
        { "YellowCart", 4 }
    };
    private void Awake()
    {
        Shared = this;
        _initialPlayerPos = player.position;
    }

    private void Update()
    {
        if (player.position.y is > 10 or < 0)
        {
            player.position = _initialPlayerPos;
        }
    }
}
