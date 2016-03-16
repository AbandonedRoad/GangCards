using Assets.Script.Actions;
using UnityEngine;

public class ActionContainer : MonoBehaviour
{
    public IAction[] CreatedActions { get; set; }

    public string TextRessourcePrefix;
    public string[] Actions;
    public string[] Parameters;
}
