using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int score = 100;
    [SerializeField] private AudioClip coinPickupSfx = null;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<GameSession>().AddToScore(score);

        AudioSource.PlayClipAtPoint(coinPickupSfx, Camera.main.transform.position);
        Destroy(gameObject);
    }
}
