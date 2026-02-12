
// ==========================================================================================
// VisibilityLevel.cs
//
// Defines the different visibility states a Room can have in the Room Visibility Management System.
// Used by Room and Room_Manager scripts to determine which components are active and how lighting 
// propagates between rooms.
// ==========================================================================================


public enum VisibilityLevel
{
    /// <summary>
    /// The room is completely invisible. All components are disabled.
    /// </summary>
    INVISIBLE,

    /// <summary>
    /// The room itself is not visible, but its lights and walls affect visible rooms.
    /// The interior/furniture is disabled, lights & walls remain enabled.
    /// </summary>
    PARTIAL_VISIBLE,

    /// <summary>
    /// The room is fully visible. Both interior and lights/walls are enabled.
    /// </summary>
    FULL_VISIBLE

}
