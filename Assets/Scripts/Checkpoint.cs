using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            ActivateCheckpoint();
            GameManager.GetGameManager().GetPlayer().SetCheckpoint(transform.position, transform.rotation);
        }
    }

    public void ActivateCheckpoint()
    {
        isActivated = true;
        Debug.Log("Checkpoint activated at position: " + transform.position);
    }
}
