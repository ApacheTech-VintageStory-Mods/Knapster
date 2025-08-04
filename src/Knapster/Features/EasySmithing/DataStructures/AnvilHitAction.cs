namespace Knapster.Features.EasySmithing.DataStructures;

/// <summary>
///     Represents the possible actions that can occur when hitting an anvil during smithing.
/// </summary>
public enum AnvilHitAction
{
    /// <summary>
    ///     No action was performed.
    /// </summary>
    Nothing,

    /// <summary>
    ///     Slag was removed from the metal.
    /// </summary>
    SlagRemoved,
    
    /// <summary>
    ///     Metal was moved on the anvil.
    /// </summary>
    MetalMoved,

    /// <summary>
    ///     Metal was split into separate pieces.
    /// </summary>
    MetalSplit,
    
    /// <summary>
    ///     The item was completed.
    /// </summary>
    ItemCompleted,
}