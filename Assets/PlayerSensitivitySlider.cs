using UnityEngine;
using UnityEngine.UI;

public class PlayerSensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private PlayerMovement playerMovementScript;

    private void Start()
    {
        // Asegúrate de que tengas referencias válidas
        if (sensitivitySlider != null && playerMovementScript != null)
        {
            sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        }
    }

    private void UpdateSensitivity(float sensitivityValue)
    {
        // Ajusta la sensibilidad de la vista del jugador
        playerMovementScript.SetLookSensitivity(sensitivityValue);
    }
}