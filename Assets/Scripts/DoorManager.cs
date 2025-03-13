using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class DoorManager : MonoBehaviour
{
    // Lista estática de puertas que se usa en el Button
    public static List<DoorController> m_DoorControllers = new List<DoorController>();

    void Start()
    {
        var l_Doors = FindObjectsOfType<DoorController>(); // Encuentra todas las puertas en la escena
        Debug.Log("Puertas encontradas: " + l_Doors.Length);

        m_DoorControllers.Clear();  // Asegúrate de que la lista esté vacía antes de agregar las puertas

        // Agrega las puertas en el orden correcto
        foreach (var _Door in l_Doors)
        {
            m_DoorControllers.Insert(0, _Door);  // Inserta cada puerta al principio de la lista
        }

        // Debug para ver el orden de las puertas
        foreach (var _Door in m_DoorControllers)
        {
           // Debug.Log("Puerta en la lista: " + _Door.name);
        }
    }
}