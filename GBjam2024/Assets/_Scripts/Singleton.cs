using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{

    public static Singleton Instance { get; private set; }

    private GameObject player;
    private MovementController movementController;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        movementController = GetComponent<MovementController>();
    }

    public Vector2 GetPlayerPosition()
    {
        return new Vector2(player.transform.position.x, player.transform.position.y);
    }

}

