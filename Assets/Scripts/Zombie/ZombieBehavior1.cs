using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ZombieBehavior1 : MonoBehaviour
{
    public float speed = 3.0f; // Velocidad del zombie
    public float stoppingDistance = 2.0f; // Distancia a la que el zombie se detiene al ver al jugador
    private Transform player;
    public bool canLookAt = true;
    private Vector3 initialPosition;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Encuentra al jugador por su tag  "Player"
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calcula la distancia entre el zombie y el jugador
        float distance = Vector3.Distance(transform.position, player.position);
      


        if (distance <  stoppingDistance)
        {

            if (canLookAt && player != null)
            {
                // El jugador está cerca, persigue al jugador
                Vector3 direction = player.position - transform.position;
                transform.LookAt(player);
                //transform.Translate(direction.normalized * speed * Time.deltaTime);
                transform.Translate(Vector3.forward * Time.deltaTime * speed);


            }

          

        }
        else
        //if(distance >= stoppingDistance )
        {
            //Activar Idle
            
            //transform.Rotate(Vector3.up, 180.0f); //Rota 180 Grados la posicion del zoombie
            this.transform.LookAt(new Vector3(0f,0f,0f));// Mira la posicion de origen
            ReturnToInitialPosition();
            transform.Translate(Vector3.forward * Time.deltaTime * 0); //Detiene el avance del zombie
            
           // transform.Translate(Vector3.back * Time.deltaTime * speed);
           
       }
    }

    void ReturnToInitialPosition()
    {
        transform.position = initialPosition;
    }
    public void SetLookAt(bool enableLookAt)
    {
        canLookAt = enableLookAt;
    

    }
}