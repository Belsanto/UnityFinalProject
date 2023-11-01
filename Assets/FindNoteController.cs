using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public enum NotaPosicion
{
    Nota1 = 0,
    Nota2 = 1,
    Nota3 = 2,
    Nota4 = 3,
    Nota5 = 4,
    Nota6 = 5
}

public class NoteActivator : MonoBehaviour
{
    public NotaPosicion notaPosicion;

    private void OnEnable()
    {
        GameManager.Instance.ActivateNote((int)notaPosicion);
    }
}
