using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehavior1 : MonoBehaviour
{
    public float speed = 3.0f; // Velocidad del zombie
    public float stoppingDistance = 2.0f; // Distancia a la que el zombie se detiene al ver al jugador
    private Transform player; 

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Encuentra al jugador por su tag  "Player"
    }

    void Update()
    {
        // Calcula la distancia entre el zombie y el jugador
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < stoppingDistance)
        {
            // El jugador está cerca, persigue al jugador
            Vector3 direction = player.position - transform.position;
            transform.LookAt(player);
            //transform.Translate(direction.normalized * speed * Time.deltaTime);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else
        {
            //Activar Idle
            
        }
    }
}