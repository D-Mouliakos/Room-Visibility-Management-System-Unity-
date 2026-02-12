using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ==========================================================================================
// RoomTrigger.cs
// 
// Attach this script to a Collider that represents a Room's trigger volume.
// When the player enters or exits the trigger, it notifies Room_Manager to update 
// room visibility states: FULL_VISIBLE, PARTIAL_VISIBLE, or INVISIBLE.
// ==========================================================================================


[RequireComponent(typeof(Collider))]
public class RoomTrigger : MonoBehaviour
{

    [Header("Linked Room")]
    [SerializeField] private Room linkedRoom;      // Assign the Room this trigger corresponds to

    [Header("Trigger Behavior")]
    [SerializeField] private bool onExit = false;  // Fire EnterRoom also on exit if true (for gaps or neutral zones)

    private Room_Manager roomManager;          // Cached reference to Room_Manager to avoid repeated FindObjectOfType calls


    // ==========================================================================================
    // ON-Awake it finds and assigns the FindObjectOfType to roomManager
    // ==========================================================================================
    private void Awake()
    {
        roomManager = FindObjectOfType<Room_Manager>();

        // DEBUGING : it Checks if the needed refferances are set giving a worning message if not.
#if UNITY_EDITOR
        if (!linkedRoom)
            Debug.LogWarning($"{name}: LinkedRoom reference is missing!", this);

        if (!roomManager)
            Debug.LogWarning($"{name}: Room_Manager not found in the scene!", this);
#endif
    }


    // ==========================================================================================
    // When a Player tagged object enters the collider,
    // It calls the EnterRoom procedure of Room_Manager Class which rearange the Rooms' Visibility State.
    // ==========================================================================================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && roomManager != null)
        {
            roomManager.EnterRoom(linkedRoom);
        }
    }


    // ==========================================================================================
    // If onExit function is "requested" When a Player tagged object enters the collider,
    // It calls the EnterRoom procedure of Room_Manager Class which rearange the Rooms' Visibility State.
    // ==========================================================================================
    private void OnTriggerExit(Collider other)
    {
        if (onExit && other.CompareTag("Player") && roomManager != null)
        {
            roomManager.EnterRoom(linkedRoom);
        }
    }
}