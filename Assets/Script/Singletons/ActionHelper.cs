using Assets.Script.Actions;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Singleton
{
    public static class ActionHelper
    {
        /// <summary>
        /// Creates instances
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static IAction CreateInstance(string className, string paramter)
        {
            if (!String.IsNullOrEmpty(className))
            {
                var type = Type.GetType(className);

                var parameters = paramter.Split(';');

                var createdInstance = !string.IsNullOrEmpty(paramter)
                    ? Activator.CreateInstance(type, parameters)
                    : Activator.CreateInstance(type);

                return createdInstance as IAction;
            }

            return null;
        }

        /// <summary>
        /// Creates a Method Calls
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static MethodInfo CreateMethodCall(Type type, string methodName)
        {
            if (!String.IsNullOrEmpty(methodName))
            {
                var methodCall = type.GetMethod(methodName);
                return methodCall;
            }

            return null;
        }

        /// <summary>
        /// Prepares the instances from the GUI
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="actionTexts"></param>
        /// <param name="actionButtons"></param>
        /// <param name="actionButtonsText"></param>
        /// <param name="actionCount"></param>
        public static void PrepareInstances(GameObject panel, ref List<Text> actionTexts, ref List<Button> actionButtons, ref List<Text> actionButtonsText, int actionCount)
        {
            PrepareInstances(panel, ref actionButtons, ref actionButtonsText, actionCount);

            actionTexts = new List<Text>();
            var texts = panel.GetComponentsInChildren<Text>();
            actionTexts.Add(texts.First(btn => btn.name == "Action1Text"));
            actionTexts.Add(texts.First(btn => btn.name == "Action2Text"));
            actionTexts.Add(texts.First(btn => btn.name == "Action3Text"));

            if (actionCount > 3)
            {
                actionTexts.Add(panel.GetComponentsInChildren<Text>().First(btn => btn.name == "Action4Text"));
            }

            SetActiveStatus(actionTexts, actionButtons, actionButtonsText, false);
        }

        /// <summary>
        /// Prepares the instances from the GUI
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="actionTexts"></param>
        /// <param name="actionButtons"></param>
        /// <param name="actionButtonsText"></param>
        /// <param name="actionCount"></param>
        public static void PrepareInstances(GameObject panel, ref List<Button> actionButtons, ref List<Text> actionButtonsText, int actionCount)
        {
            actionButtons = new List<Button>();
            actionButtons.Add(panel.GetComponentsInChildren<Button>().First(btn => btn.name == "Action1Button"));
            actionButtons.Add(panel.GetComponentsInChildren<Button>().First(btn => btn.name == "Action2Button"));
            actionButtons.Add(panel.GetComponentsInChildren<Button>().First(btn => btn.name == "Action3Button"));

            actionButtonsText = new List<Text>();
            actionButtonsText.Add(panel.GetComponentsInChildren<Text>().First(btn => btn.name == "Action1ButtonText"));
            actionButtonsText.Add(panel.GetComponentsInChildren<Text>().First(btn => btn.name == "Action2ButtonText"));
            actionButtonsText.Add(panel.GetComponentsInChildren<Text>().First(btn => btn.name == "Action3ButtonText"));

            if (actionCount > 3)
            {
                actionButtons.Add(panel.GetComponentsInChildren<Button>().First(btn => btn.name == "Action4Button"));
                actionButtonsText.Add(panel.GetComponentsInChildren<Text>().First(btn => btn.name == "Action4ButtonText"));
            }
        }

        /// <summary>
        /// Prepares the actions for the GUI
        /// </summary>
        /// <param name="container"></param>
        /// <param name="actions"></param>
        /// <param name="actionTexts"></param>
        /// <param name="actionButtons"></param>
        /// <param name="actionButtonsText"></param>
        public static void PrepareActions(ActionContainer container, List<Text> actionTexts, List<Button> actionButtons, List<Text> actionButtonsText)
        {
            for (int i = 0; i < container.Actions.Length; i++)
            {
                string buttonName = String.Concat("Action", (i + 1).ToString(), "Button");
                string textName = String.Concat("Action", (i + 1).ToString(), "Text");
                string buttonTextName = String.Concat("Action", (i + 1).ToString(), "ButtonText");
                string action = container.Actions.Length > i ? container.Actions[i] : String.Empty;
                string parameters = container.Parameters.Length > i ? container.Parameters[i] : String.Empty;

                if (String.IsNullOrEmpty(action))
                {
                    continue;
                }

                actionTexts.First(txt => txt.name == textName).gameObject.SetActive(true);
                actionButtons.First(btn => btn.name == buttonName).gameObject.SetActive(true);
                actionButtonsText.First(btn => btn.name == buttonTextName).gameObject.SetActive(true);

                container.CreatedActions[i] = container.CreatedActions[i] ?? ActionHelper.CreateInstance(action, parameters);
                actionTexts.First(txt => txt.name == textName).text = ResourceSingleton.Instance.CreateActionText(container.TextRessourcePrefix, i);
                actionButtonsText.First(btn => btn.name == buttonTextName).text = container.CreatedActions[i].ButtonText;
            }
        }

        /// <summary>
        /// Prepares the actions for the GUI
        /// </summary>
        /// <param name="container"></param>
        /// <param name="actions"></param>
        /// <param name="actionTexts"></param>
        /// <param name="actionButtons"></param>
        /// <param name="actionButtonsText"></param>
        public static void PrepareActions(ActionContainerMethod container, MethodInfo[] methodInfos, List<Button> actionButtons, List<Text> actionButtonsText)
        {
            for (int i = 0; i < methodInfos.Length; i++)
            {
                string buttonName = String.Concat("Action", (i + 1).ToString(), "Button");
                string buttonTextName = String.Concat("Action", (i + 1).ToString(), "ButtonText");
                string method = container.MethodCalls.Length > i ? container.MethodCalls[i] : String.Empty;
                string methodText = container.MethodTexts.Length > i ? container.MethodTexts[i] : String.Empty;
                string parameters = container.Parameters.Length > i ? container.Parameters[i] : String.Empty;

                if (String.IsNullOrEmpty(method))
                {
                    continue;
                }

                actionButtons.First(btn => btn.name == buttonName).gameObject.SetActive(true);
                actionButtonsText.First(btn => btn.name == buttonTextName).gameObject.SetActive(true);

                methodInfos[i] = ActionHelper.CreateMethodCall(container.Instance.GetType(), method);
                actionButtonsText.First(btn => btn.name == buttonTextName).text = methodText;
            }
        }

        /// <summary>
        /// Sets the new active status
        /// </summary>
        /// <param name="actionTexts"></param>
        /// <param name="actionButtons"></param>
        /// <param name="actionButtonsText"></param>
        /// <param name="newStatus"></param>
        public static void SetActiveStatus(List<Text> actionTexts, List<Button> actionButtons, List<Text> actionButtonsText, bool newStatus)
        {
            if (actionTexts != null)
            {
                actionTexts.ForEach(txt => txt.gameObject.SetActive(newStatus));
            }

            actionButtons.ForEach(btn => btn.gameObject.SetActive(newStatus));
            actionButtonsText.ForEach(btn => btn.gameObject.SetActive(newStatus));
        }

        /// <summary>
        /// Execute a button action
        /// </summary>
        /// <param name="actionButton"></param>
        public static void ExecuteAction(string actionButton, IAction[] actions)
        {
            switch (actionButton)
            {
                case "Action1Button":
                    actions[0].ExecuteAction();
                    break;
                case "Action2Button":
                    actions[1].ExecuteAction();
                    break;
                case "Action3Button":
                    actions[2].ExecuteAction();
                    break;
                case "Action4Button":
                    actions[3].ExecuteAction();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Execute a button action
        /// </summary>
        /// <param name="actionButton"></param>
        public static void ExecuteMethod(string actionButton, IAction instance, MethodInfo[] methodCall)
        {
            switch (actionButton)
            {
                case "Action1Button":
                    methodCall[0].Invoke(instance, null);
                    break;
                case "Action2Button":
                    methodCall[1].Invoke(instance, null);
                    break;
                case "Action3Button":
                    methodCall[2].Invoke(instance, null);
                    break;
                case "Action4Button":
                    methodCall[3].Invoke(instance, null);
                    break;
                default:
                    break;
            }
        }
    }
}
