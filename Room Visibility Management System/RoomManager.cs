using System.Collections.Generic;
using UnityEngine;
using static GameStateEnum;


// ==========================================================================================
// RoomManager.cs
//
// Manages player location within the mansion and updates room visibility states 
// (FULL_VISIBLE, PARTIAL_VISIBLE, INVISIBLE). Notifies AmbientController to adjust 
// scene ambient lighting and character emission color.
// ==========================================================================================


public class RoomManager : MonoBehaviour
{
    [Header("Starting Room")]
    [SerializeField] private Room startingRoom;

    [Header("All Rooms in Scene")]
    [SerializeField] private List<Room> allRooms = new();

    [Header("References")]
    [SerializeField] private AmbientController ambientController;

    private Room currentRoom;
    private Room previousRoom;

    // Neutral emission color if no room lights are active
    private readonly Color neutralEmissionColor = new Color(0, 0, 0, 255);


    // ====================================================
    // Public Accessors
    // ====================================================

    public string CharacterLocation => currentRoom.RoomName;
    public int CharacterLocationID => currentRoom.RoomID;


    // ====================================================
    // Unity Callbacks
    // ====================================================

    private void Start()
    {
#if UNITY_EDITOR
        if (!startingRoom)
        {
            Debug.LogWarning("Starting room is not assigned!", this);
        }
        if (!ambientController)
        {
            Debug.LogWarning("AmbientController reference missing!", this);
        }
#endif

        // Begining the game, it Sets all rooms initially invisible
        foreach (Room room in allRooms)
        {
            room.SetVisibility(VisibilityLevel.INVISIBLE);
        }

        // once all rooms are invisible, it applies the attributes of the room from which the player starts setting the stage
        EnterRoom(startingRoom);
    }


    // ====================================================
    // Room Entry Logic
    // ====================================================
    /// <summary>
    /// Updates visibility states and adjust characters emission color and ambient light.
    /// </summary>
    /// <param name="newRoom">The room the player enters.</param>
    public void EnterRoom(Room newRoom)
    {
        if (newRoom == currentRoom) return;	// Skip if already in this room


        previousRoom = currentRoom;
        currentRoom = newRoom;

        // Update visibility of linked rooms
        foreach (RoomVisibilityInfo info in newRoom.VisibleFromHere)
        {
            if (previousRoom != null)
            {
                previousRoom.SetVisibility(VisibilityLevel.FULL_VISIBLE);
            }
        }

        // Adjust ambient lighting if room overrides it
        if (newRoom.OverrideAmbientLight)
        {
            RenderSettings.ambientLight = newRoom.AmbientLightColor;
            ambientController.SetAmbientIntensitySmooth(
                newRoom.AmbientIntensity, newRoom.AmbientIntensity
            );
        }

        // Adjust character emission
        if (newRoom.LightSource)
        {
            if (newRoom.LightSource.activeSelf)
            {
                ambientController.SetEmissionColorSmooth(newRoom.CharacterEmissionColor, false);
            }
            else
            {
                ambientController.SetEmissionColorSmooth(neutralEmissionColor, false);
            }
        }
    }

}
