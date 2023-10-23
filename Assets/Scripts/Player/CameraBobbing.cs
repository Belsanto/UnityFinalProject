using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [SerializeField] private float walkingBobbingSpeed = 0.18f; // Velocidad de oscilación de la cámara al caminar
    [SerializeField] private float runningBobbingSpeed = 0.24f; // Velocidad de oscilación de la cámara al correr
    [SerializeField] private float bobbingAmount = 0.2f; // Magnitud de oscilación de la cámara

    private float midpoint = 0.1f; // Punto medio del movimiento de la cámara
    private float timer = 0.0f;

    void Update()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float bobbingSpeed = Input.GetKey(KeyCode.LeftShift) ? runningBobbingSpeed : walkingBobbingSpeed;

        Vector3 cSharpConversion = transform.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            cSharpConversion.y = midpoint + translateChange;
        }
        else
        {
            cSharpConversion.y = midpoint;
        }
        transform.localPosition = cSharpConversion;
    }
}