using UnityEngine;

public class MakeObjectShake : MonoBehaviour
{
    [SerializeField, Tooltip("how fast it shakes")] private float speed = 1.0f;
    [SerializeField, Tooltip("how much it shakes")] private float amount = 1.0f;
    private bool _shake;
    [SerializeField] private Transform objectToShake;
    [SerializeField] private Transform objectToShakeInitialPos;
    
    public void ShakeObject()
    {
        _shake = !_shake;
    }

    private void Update()
    {
        if (!_shake) return;
        var position = objectToShakeInitialPos.transform.position;
        objectToShake.position = new Vector3(position.x + Mathf.Sin(Time.time * speed) * amount
            ,position.y + Mathf.Sin(Time.time * speed) * amount,position.z);
    }
}
