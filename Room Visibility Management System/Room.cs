using System.Collections.Generic;
using UnityEngine;


// =============================================================================
// A class used in the script bellow (for a list) so you can declare the change of Visibility state of a room other than the one the player enters
// =============================================================================


[System.Serializable]
public class RoomVisibilityInfo
{
    public Room room;
    public VisibilityLevel visibility;
}



// =============================================================================
// This must be attached in Every Room gameObject
// It creates a list in which you can add the rooms which change their Visibility State (when you enter this particular room) & the new State they should be in.
// It's a little of manual work but that way you avoid an automated system that will produce more overhead by affecting unrelated rooms
// and therefor it remains extremely lightweight even if the scene contain hundreds of rooms & thousands of objects.

// Since this is the Rooms Main Script i have added some extra features such as...
// 1) A name & an ID number variables for each room.
// 2) variables one can use to affect the ambient light of the entire scene whenever the player enters a room
// 3) Since light probes doesn't work well with dynamic lighting such as lights turning on/off and with open doors restricting the ability to adjust the scene's ambient light, 
// by adding a slight emission on the character based on the room he stands, it makes it look like the ambient light is changing without affecting the entire scene.
// That way it replicates (to a degree) a raytracing level of detail in an extremely lightweight way.
// =============================================================================


public class Room : MonoBehaviour
{
    // =============================================================================
    // Room Metadata :
    // =============================================================================

    [Header("Room Components")]
    [SerializeField] private string roomName;
    [SerializeField] private int roomID;
    [SerializeField] private bool isCentralHub = false; 		// In Case you Need a room In which you want the doors automaticaly to close when you enter, mark this as TRUE.


    // =============================================================================
    // Visibility Components
    // =============================================================================

    [Header("Room's visibility components")]
    [SerializeField] private GameObject partialVisible;			// In Unity, place the game object that Contains the current room's lights&walls (including floor & ceiling).
    [SerializeField] private GameObject fullyVisible;			// In Unity, place the game object that contains all the items of the current room except the Lights & Walls.
    [SerializeField] private GameObject roomLight;			// In Unity, place the game object that contains the room's Main Lights.

    [Header("Rooms' Visibility List")]
    [SerializeField] private List<RoomVisibilityInfo> visibleFromHere;	// In Unity, Defines room visibility relationships triggered on player entry.


    // =============================================================================
    // Ambient Override (Optional)
    /// =============================================================================

    [Header("Ambient Override (optional)")]
    [SerializeField] private bool overrideAmbientLight = false;		// In Unity, you can declare this value as true if you want the room to affect the entire scenes ambient light.
    [SerializeField] private Color ambientLightColor = Color.white;	// in Unity. you can declare the a new color for ambient light (works only if the above value is TRUE).
    [SerializeField] private float ambientIntensity = 1.0f;		// in Unity. you can affect the strength of the ambient light (works only if the above value is TRUE).


    // =============================================================================
    // Character Emission Override
    // =============================================================================

    [Header("Character's Emission Light")]				// Using those variables you can declare in Unity the color of emission you want for the current room to apply on the character.
    [SerializeField] private float red = 0f;
    [SerializeField] private float green = 0f;
    [SerializeField] private float blue = 0f;
    private Color finalEmissionColor;


    // =============================================================================
    // Public Accessors
    // =============================================================================

    public string RoomName => roomName;
    public int RoomID => roomID;
    public bool IsCentralHub => isCentralHub;

    public GameObject LightSource => roomLight;
    public List<RoomVisibilityInfo> VisibleFromHere => visibleFromHere;

    public bool OverrideAmbientLight => overrideAmbientLight;
    public Color AmbientLightColor => ambientLightColor;
    public float AmbientIntensity => ambientIntensity;

    public Color CharacterEmissionColor => finalEmissionColor;



    // =============================================================================
    // EDITOR VALIDATION : USED TO SET THE PROPER VALUES OF EMISSION ON CHARACTER AT THE START OF THE GAME & WHEN CHANGES AREAPPLIED.
    // =============================================================================

    private void OnValidate()
    {
        // Clamp emission values (for extra safety)
        red = Mathf.Clamp(red, 0, 255);
        green = Mathf.Clamp(green, 0, 255);
        blue = Mathf.Clamp(blue, 0, 255);

        finalEmissionColor = new Color(red / 255f, green / 255f, blue / 255f);

        // Basic reference validation (Editor only)	
#if UNITY_EDITOR
        if (!partialVisible)
        {
            Debug.LogWarning($"{name}: Missing reference to PartialVisible object.", this);
        }

        if (!partialVisible)
        {
            Debug.LogWarning($"{name}: Missing reference to FullyVisible object.", this);
        }
#endif
    }


    // =============================================================================
    // RUNTIME VISIBILITY CONTROL : USED IN Room_Manager TO CHANGE THE VISIBILITY STATE OF A ROOM'S COMPONENTS.
    // =============================================================================
    public void SetVisibility(VisibilityLevel level)
    {
        if (!partialVisible || !fullyVisible)       // Just for the case an error occures
        {
            return;
        }

        switch (level)		// By reading the Enum/State of a room it enables or disables its components accordingly
        {
            case VisibilityLevel.FULL_VISIBLE:
                partialVisible.SetActive(true);
                fullyVisible.SetActive(true);
                break;

            case VisibilityLevel.PARTIAL_VISIBLE:
                partialVisible.SetActive(true);
                fullyVisible.SetActive(false);
                break;

            case VisibilityLevel.INVISIBLE:
                partialVisible.SetActive(false);
                fullyVisible.SetActive(false);
                break;
        }
    }
}

