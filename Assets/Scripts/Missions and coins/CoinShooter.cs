using Unity.Mathematics;
using UnityEngine;

public class CoinShooter : MonoBehaviour
{
    [SerializeField] private ParticleSystem effects;
    [SerializeField] private ParticleSystem singleCoin;
    public void Shoot()
    {
        effects.Play();
    }

    public void ShootSmall()
    {
        singleCoin.Play();
    }
}
