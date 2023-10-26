using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 50f;



    public void TakeDamage (float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {

        transform.DOScale(new Vector3(0.1f,0.1f,0.1f), 0.5f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
