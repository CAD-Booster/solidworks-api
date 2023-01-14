namespace CADBooster.SolidDna
{
    /// <summary>
    /// Whether to show or hide your own Undo/Redo step in the SolidWorks user interface.
    /// Hidden undo steps are discarded by SolidWorks and therefore cannot be undone, so only use these to hide your actions.
    /// More info: https://help.solidworks.com/2023/english/api/sldworksapi/SolidWorks.Interop.sldworks~SolidWorks.Interop.sldworks.IModelDocExtension~FinishRecordingUndoObject2.html
    /// </summary>
    public enum ModelUndoStepVisibility
    {
        /// <summary>
        /// Make this step visible for the user so they can undo a block of your API calls with one Undo.
        /// </summary>
        Visible,

        /// <summary>
        /// Hide this step from the user
        /// </summary>
        Hidden
    }
}
