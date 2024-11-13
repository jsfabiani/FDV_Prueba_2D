using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Platform : MonoBehaviour
{

    public GameObject[] torchesObjects;
    public GameObject[] blocksObjects;

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize arrays for torches and hazards
        torchesObjects = GameObject.FindGameObjectsWithTag("PowerUp");
        blocksObjects = GameObject.FindGameObjectsWithTag("Hazard");

        // Deactivate torches
        foreach (GameObject torch in torchesObjects)
        {
            torch.SetActive(false);
        }
    }

    // Update is called once per frame
    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
        // On collision with a player, reenable the torches.
        foreach (GameObject torch in torchesObjects)
            {
                torch.SetActive(true);
            }

        // And disable the hazard blocks.
        foreach (GameObject block in blocksObjects)
            {
                block.SetActive(false);
            }
        }
    }
}
