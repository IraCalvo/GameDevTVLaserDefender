using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileLifeTime;
    [SerializeField] float fireRateBase;

    [Header("AI")]
    [SerializeField] float fireRateVariance;
    [SerializeField] float fireRateMinimum;
    [SerializeField] bool useAI;
    
    [HideInInspector]public bool isFiring;
    Coroutine firingCoroutine;

    AudioPlayer audioPlayer;

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Start()
    {
        if(useAI)
        {
            isFiring = true;
        }
    }


    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if(isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContiously());
        }
        else if(!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContiously()
    {
        while(true)
        {
            GameObject instance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }
            Destroy(instance, projectileLifeTime);

            float timeToNextProjectile = Random.Range(fireRateBase - fireRateVariance, fireRateBase + fireRateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, fireRateMinimum, float.MaxValue);

            audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
