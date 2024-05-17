using UnityEngine;

public class OpenCloseGate : MonoBehaviour
{
    
    public float openAngle = -90f; // The angle at which the gate should be fully open
    public float moveSpeed = 1f; // The speed at which the gate should move

    private bool _isOpening;
    private bool _isClosing;
    private Quaternion _targetRotation;

    public void OpenGate()
    {
        _isOpening = true;
        _targetRotation = Quaternion.Euler(0f, 0, openAngle);
    }

    public void CloseGate()
    {
        _isClosing = true;
        _targetRotation = Quaternion.Euler(0f, 0, 0f);
    }
    private void Update()
    {
        if (!_isOpening && !_isClosing) return;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _targetRotation, moveSpeed * Time.deltaTime);

        if (!(Quaternion.Angle(transform.localRotation, _targetRotation) < 0.1f)) return;
        
        // The gate has reached the target rotation
        if (_isOpening)
        {
            _isOpening = false;
            // Debug.Log("Gate opened!");
        }
        else if (_isClosing)
        {
            _isClosing = false;
            // Debug.Log("Gate closed!");
        }
    }

}
