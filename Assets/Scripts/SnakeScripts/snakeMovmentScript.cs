using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class snakeMovmentScript : MonoBehaviour
{
    public List<Transform> bodyParts = new List<Transform>();

    public float minimumDistance = 0.27f;
    public int initialSize;
    public float movementSpeed = 5;
    public float rotationSpeed = 50;

    public GameObject[] bodyPrefab;
    public GameObject[] pickAbleCart;

    private float dis;
    private Transform curBodyPart;
    private Transform PrevBodyPart;
    private int[] leftOrRight = { 1, -1 };

    public static snakeMovmentScript instance;
    [SerializeField] private float explodeForce = 30;
    [SerializeField] private float littleBoostAddition = 0.05f;
    private Dictionary<string, int> _colorDictionary;

    [SerializeField] private GameObject movingAvatar;
    [SerializeField] private GameObject idleAvatar;
    [SerializeField] private GameObject movingAvatarNoCart;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitSnake();
        _colorDictionary = GameManager.Shared.colorDictionary;
    }

    private void InitSnake()
    {
        for (int i = 0; i < initialSize - 1; i++)
        {
            AddBodyPart("Basic");
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float curspeed = movementSpeed;
        if (ThirdPersonMovement.instance.IsMove())
        {
            // update all carts
            if (bodyParts.Count > 1)
            {
                movingAvatar.SetActive(true);
                idleAvatar.SetActive(false);
                movingAvatarNoCart.SetActive(false);
            }
            else
            {
                movingAvatarNoCart.SetActive(true);
                idleAvatar.SetActive(false);
                movingAvatar.SetActive(false);
            }

            for (int i = 1; i < bodyParts.Count; i++)
            {
                curBodyPart = bodyParts[i];
                PrevBodyPart = bodyParts[i - 1];

                dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);

                Vector3 newpos = PrevBodyPart.position;

                newpos.y = bodyParts[0].position.y;

                float T = Time.deltaTime * dis / minimumDistance * curspeed;

                if (T > 0.5f)
                    T = 0.5f;
                curBodyPart.position = Vector3.Slerp(curBodyPart.position, newpos, T);
                curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, PrevBodyPart.rotation, T);
            }
        }
        else
        {
            movingAvatar.SetActive(false);
            movingAvatarNoCart.SetActive(false);
            idleAvatar.SetActive(true);
        }
    }


    // Add and remove from start
    public void AddBodyPart(string color)
    {
        int index = 0;
        index = _colorDictionary[color];

        Transform newPart =
            (Instantiate(bodyPrefab[index], bodyParts[0].position + bodyParts[0].forward,
                bodyParts[0].rotation) as GameObject).transform;


        newPart.SetParent(transform);

        bodyParts.Insert(0, newPart);
        ThirdPersonMovement.instance.ChangeGameObjectToMove(bodyParts[0].gameObject);
        if (movementSpeed >= 5)
        {
            movementSpeed -= .5f;
            ThirdPersonMovement.instance.SetSpeed(movementSpeed);
        }


        ThirdPersonMovement.instance.timeForBoost += littleBoostAddition;
    }

    public void RemoveBodyPart()
    {
        if (bodyParts.Count <= 1)
            return;

        Transform firstBodyPart = bodyParts[0];
        bodyParts.RemoveAt(0);
        ThirdPersonMovement.instance.ChangeGameObjectToMove(bodyParts[0].gameObject);
        Destroy(firstBodyPart.gameObject);
        if (movementSpeed <= 8) movementSpeed += .5f;
        ThirdPersonMovement.instance.SetSpeed(movementSpeed);
        ThirdPersonMovement.instance.timeForBoost -= littleBoostAddition;
    }

    public void Explode(Transform cart)
    {
        if (bodyParts.Count < 1 || !cart) return;
        var color = bodyParts[0].tag;
        GameObject newCart = null;
        try
        {
            var index = _colorDictionary[color];
            newCart = Instantiate(pickAbleCart[index], cart.position, quaternion.identity);
        }
        catch (Exception e)
        {
            return;
        }

        if (!newCart) return;
        Gamepad.current?.SetMotorSpeeds(1, 1);
        StartCoroutine(StopShake(Gamepad.current));
        var rb = newCart.GetComponent<Rigidbody>();
        int dir = leftOrRight[Random.Range(0, 2)];
        rb.AddForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * explodeForce, ForceMode.Impulse);
        rb.AddTorque(new Vector3(0, 1 * dir, 0) * Random.Range(2, 6), ForceMode.Impulse);
        var cols = newCart.GetComponents<BoxCollider>();
        foreach (var VARIABLE in cols)
        {
            VARIABLE.enabled = false;
        }

        StartCoroutine(SetToPickable(cols));
        RemoveBodyPart();
    }

    private IEnumerator SetToPickable(BoxCollider[] carts)
    {
        yield return new WaitForSeconds(1);
        if (carts.Length <= 0) yield break;
        foreach (var VARIABLE in carts)
        {
            if (VARIABLE.isTrigger) VARIABLE.enabled = true;
        }
    }

    private IEnumerator StopShake(Gamepad pad)
    {
        yield return new WaitForSeconds(.1f);
        pad?.SetMotorSpeeds(0, 0);
        
    }

    public void ExplodeAll()
    {
        while (bodyParts.Count > 1)
        {
            Explode(bodyParts[0]);
        }
    }
}