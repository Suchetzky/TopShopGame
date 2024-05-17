using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager shared { get; private set; }
    [SerializeField] private AudioSource horn;
    [SerializeField] private AudioSource recieptPrint;
    [SerializeField] private AudioSource recieptTear;
    [SerializeField] private AudioSource Vding;
    [SerializeField] private AudioSource carEngine;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource alarmSound;

    private void Awake()
    {
        shared = this;
    }

    public void PlayHorn()
    {
        horn.Play();
    }

    public void PlayRecipetPrint()
    {
        recieptPrint.Play();
    }

    public void PlayRecieptTear()
    {
        recieptTear.Play();
    }

    public void PlayVding()
    {
        Vding.Play();
    }
    
    public void PlayCarEngine()
    {
        if(carEngine.isPlaying) return;
        carEngine.Play();
    }

    public void PlayHitSound()
    {
        hitSound.Play();
    }

    public void PlayAlarmSound()
    {
        alarmSound.Play();
    }
}
