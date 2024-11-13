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
        torchesObjects = GameObject.FindGameObjectsWithTag("PowerUp");
        blocksObjects = GameObject.FindGameObjectsWithTag("Hazard");

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
        foreach (GameObject torch in torchesObjects)
            {
                torch.SetActive(true);
            }

        foreach (GameObject block in blocksObjects)
            {
                block.SetActive(false);
            }
        }
    }
}
