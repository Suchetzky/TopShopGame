using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MissionCanvas : MonoBehaviour
{
    [SerializeField] private GameObject mark;
    [SerializeField] private List<GameObject> markslist;
    private List<Vector3> _positions;
    public int curPos;
    private MissionsGeneration _missionsGeneration;
    public static MissionCanvas shared { get; private set; }

    private void Awake()
    {
        shared = this;
    }

    private void Start()
    {
        _missionsGeneration = MissionsGeneration.Shared;
    }

    public void SetPositions(List<Vector3> poses)
    {
        _positions = poses;
        curPos = _positions.Count - 1;
        foreach (var _mark in markslist)
        {
            Destroy(_mark);
        }
        markslist.Clear();
    }
    
    public void Mark()
    {
        var newMark = Instantiate(mark, transform.position, quaternion.identity);
        newMark.transform.SetParent(_missionsGeneration.GetParentOfCarts().transform ,false);
        newMark.GetComponent<RectTransform>().anchoredPosition = _positions[curPos--];
        markslist.Add(newMark);
    }
}
