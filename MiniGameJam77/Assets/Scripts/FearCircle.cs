using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearCircle : MonoBehaviour
{
    [SerializeField] private float fearAmt;

    private bool playerInside;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerInside = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInside && playerController != null)
        {
            playerController.ChangeCourage(-fearAmt * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInside = true;
            playerController = other.GetComponent<PlayerController>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInside = false;
        }
    }
}
