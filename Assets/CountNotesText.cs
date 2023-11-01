using UnityEngine;
using TMPro;

public class CountNotesText : MonoBehaviour
{
    private TMP_Text textMesh;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (textMesh != null && gameManager != null)
        {
            int totalNotes = gameManager.TotalNotes();
            textMesh.text = "Notes: " + totalNotes.ToString()+"/6";
        }
    }
}