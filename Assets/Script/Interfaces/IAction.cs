namespace Assets.Script.Actions
{
    public interface IAction
    {
        /// <summary>
        /// THe Text for the button.
        /// </summary>
        string ButtonText { get; }

        /// <summary>
        /// Used to fire a certain action
        /// </summary>
        void ExecuteAction();

        /// <summary>
        /// Similiar to dispose. Cancels Listener and stuff
        /// </summary>
        void CleanUp();
    }
}
