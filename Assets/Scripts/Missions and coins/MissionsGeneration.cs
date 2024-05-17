using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MissionsGeneration : MonoBehaviour
{
	[SerializeField] private GameObject parentOfCarts;
	[SerializeField] private List<GameObject> images;
	[SerializeField] private GameObject visibleCarts;
	[SerializeField] private Sprite[] colorsSprites;
	[SerializeField] private int numberOfColors = 1;
	[SerializeField] private int numberOfCarts = 3;
	[SerializeField] private int missionNumber = 1;
	[SerializeField] private float gapBetweenCarts = 20f;
	[SerializeField] private static EatCartsScript kones;
	[SerializeField] private Transform startPos;
	[SerializeField] private Animator animator;
	private MissionCanvas _canvas;
	private List<Vector3> _positions;

	
	public static MissionsGeneration Shared { get; private set; }

	private void Awake()
	{
		Shared = this;
		_canvas = FindObjectOfType<MissionCanvas>();
		_positions = new List<Vector3>();
		numberOfColors = colorsSprites.Length;
	}
	
	private Vector2 _startPosition;
	private List<int> _colorsList;
	private void Start()
	{
		kones = FindObjectOfType<EatCartsScript>();
		_colorsList = new List<int>();
		_startPosition = startPos.GetComponent<RectTransform>().anchoredPosition;
		GenerateMission();
		kones.SetMission(_colorsList);
		_canvas.SetPositions(_positions);
		SoundManager.shared.PlayRecipetPrint();
	}

	private void GenerateMission()
	{
		foreach (var VARIABLE in images)
		{
			Destroy(VARIABLE);
		}
		images.Clear();
		
		missionNumber++;
		_positions.Clear();
		for (int i = 0; i < numberOfCarts; i++)
		{
			int randomColor = Random.Range(0, numberOfColors);
			_colorsList.Add(randomColor);
			var newCartImage = Instantiate(visibleCarts, parentOfCarts.transform);
			newCartImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(_startPosition.x + i*gapBetweenCarts, _startPosition.y);
			_positions.Add(new Vector2(_startPosition.x + i*gapBetweenCarts, _startPosition.y));
			newCartImage.GetComponent<Image>().sprite = colorsSprites[randomColor];
			images.Add(newCartImage);
		}
		UpdateNumberOfCarts();
		
	}
	
	/**
	 * The function return a list of numbers that represent the colors to collect.
	 */
	public List<int> GetMissionList()
	{
		return _colorsList;
	}
	
	private void UpdateNumberOfCarts()
	{
		if (missionNumber % 2 == 0)
		{
			numberOfCarts+=2;
		}
		else
		{
			numberOfCarts++;
			if (numberOfCarts > 7) numberOfCarts = 7;
		}
		if (numberOfCarts > 8) numberOfCarts = 8;
	}

	public void FinishMission()
	{
		animator.SetTrigger("Done");
		SoundManager.shared.PlayRecieptTear();
		StartCoroutine(WaitAsecond());
		Gamepad.current?.SetMotorSpeeds(.5f,.5f);
		StartCoroutine(StopHitPlayer(Gamepad.current));
	}

	IEnumerator WaitAsecond()
	{
		yield return new WaitForSeconds(1.2f);
		GenerateMission();
		kones.SetMission(_colorsList);
		_canvas.SetPositions(_positions);
	}
	
	IEnumerator StopHitPlayer(Gamepad pad)
	{
		yield return new WaitForSeconds(.1f);
		pad.SetMotorSpeeds(0,0);
		yield return new WaitForSeconds(.2f);
		Gamepad.current?.SetMotorSpeeds(.5f,.5f);
		yield return new WaitForSeconds(.1f);
		pad.SetMotorSpeeds(0,0);
	}

	public void BopLastCart()
	{
		images[MissionCanvas.shared.curPos].GetComponent<Animator>().SetTrigger("Bop");
	}
	
	#region Getters
		public List<Vector3> GetPositions(){ return _positions;}
		public GameObject GetVisibleCarts(){ return visibleCarts;}
		public GameObject GetParentOfCarts(){ return parentOfCarts;}
		
	#endregion
	
	
}
