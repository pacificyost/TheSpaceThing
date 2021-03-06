﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListener : MonoBehaviour
{
    public List<PlayerInputManager> inputManagers = new List<PlayerInputManager>();

    JoinedPlayers joinedPlayers;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInputManager[] playerSlots = GetComponentsInChildren<PlayerInputManager>(true);

        joinedPlayers = FindObjectOfType<JoinedPlayers>();

        const int joystickMax = 4;

        int joystickCount = Mathf.Clamp(Input.GetJoystickNames().Length, 0, joystickMax);

        if (playerSlots.Length < joystickMax + 1)
        {
            Debug.LogError("Not enough player slots");
            return;
        }        

        for (int i = 0; i < playerSlots.Length; i++)
        {
            inputManagers.Add(playerSlots[i]);
            inputManagers[i].gameObject.GetComponentInChildren<Animator>(true).gameObject.SetActive(false);
            if (i > 0)
            {
                inputManagers[i].SetJoystickController(i);
            }
            else
            {
                inputManagers[i].SetKeyboardControlled();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInput();
        CheckForStart();
    }

    void CheckForInput()
    {
        foreach (PlayerInputManager player in inputManagers)
        {
            if (Mathf.Abs(player.GetX()) > 0 || Mathf.Abs(player.GetY()) > 0 || player.InteractPressed())
            {
                player.gameObject.GetComponentInChildren<Animator>(true).gameObject.SetActive(true);
                if (joinedPlayers.players.Contains(player) == false)
                {
                    joinedPlayers.players.Add(player);
                    player.transform.parent = joinedPlayers.transform;
                }                
            }
        }
    }

    void CheckForStart()
    {
        if (Input.GetButtonDown("Submit") && joinedPlayers.players.Count > 0)
        {
            joinedPlayers.StartGame();
        }
    }
}
