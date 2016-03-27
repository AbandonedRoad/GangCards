using Actions;
using Assets.Script.Characters;
using Enum;
using Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Actions
{
    public class HireGangMembers : ActionBase, ITextPresenter
    {
        private readonly int _pubLevel;
        private readonly int _amountProspects;
        private readonly int _refillMinutes;

        public ActionContainerMethod ActionContainer { get; private set; }
        private DateTime _refillStarts = DateTime.MinValue;
        private int _membersCreated = 0;
        private GangMember _actualMember;
        private string _textPart1;
        private string _textPart2;

        public string TextPart1 { get { return _textPart1; } }
        public string TextPart2 { get { return _textPart2; } }

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="pubLevel">The Level of the pub - this modifies the level of the prospects</param>
        /// <param name="amountAvailableProspepcts">The amount of available prospects.</param>
        /// <param name="refillDelayInMinutes">Refill delay until all prospects are there again.</param>
        public HireGangMembers(string pubLevel, string amountAvailableProspepcts, string refillDelayInMinutes)
        {
            if (!int.TryParse(pubLevel, out _pubLevel) 
                || !int.TryParse(amountAvailableProspepcts, out _amountProspects)
                || !int.TryParse(refillDelayInMinutes, out _refillMinutes))
            {
                Debug.LogError("Parse failed! Values are not integer!");
            }
        }

        /// <summary>
        /// Executes this action
        /// </summary>
        public override void ExecuteAction()
        {
            base.ExecuteAction();

            ActionContainer = new ActionContainerMethod();
            ActionContainer.TextRessourcePrefix = "HireGang";
            ActionContainer.SetMethods(this, new string[] { "AddProspectToGang", "DeclineProspect", "Leave" }, 
                new string[]                 { ResourceSingleton.Instance.GetSpecialText(SpecialText.Accept),
                    ResourceSingleton.Instance.GetSpecialText(SpecialText.Decline),
                    ResourceSingleton.Instance.GetSpecialText(SpecialText.Close)});
            ActionContainer.Parameters = new string[0];

            EvaluateMemberCount();

            PrefabSingleton.Instance.ActionAfterEffactsHandler.PassActions(ActionContainer);

            PrepareNewMember();
        }

        /// <summary>
        /// A new Player will be added.
        /// </summary>
        public void AddProspectToGang()
        {
            if (DebugSingleton.Instance.IsEnabled)
            {
                _actualMember = CharacterSingleton.Instance.GenerateAIPlayer(_pubLevel);
                CharacterSingleton.Instance.AddAIPlayer(_actualMember);
            }
            else
            {
                PrepareNewMember();
                CharacterSingleton.Instance.AddAIPlayer(_actualMember);
            }
        }

        /// <summary>
        /// Do not accept Prospect.
        /// </summary>
        public void DeclineProspect()
        {
            PrepareNewMember();
        }

        /// <summary>
        /// Close this shit.
        /// </summary>
        public void Leave()
        {
            PrefabSingleton.Instance.ActionAfterEffactsHandler.SwitchAfterEffectsPanel(false);
        }

        /// <summary>
        /// Prepares a new member
        /// </summary>
        private void PrepareNewMember()
        {
            _actualMember = CharacterSingleton.Instance.GenerateAIPlayer(_pubLevel);

            if (_refillStarts >= DateTime.Now)
            {
                // No members are present.
                _textPart1 = ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "NewMemberFailPart1");
                _textPart2 = String.Empty;
                ActionContainer.MethodButtonsActive[0] = false;
                ActionContainer.MethodButtonsActive[1] = false;
                return;
            }

            ActionContainer.MethodButtonsActive[0] = true;
            ActionContainer.MethodButtonsActive[1] = true;
            _textPart1 = ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "NewMemberPart1");            

            var type = _actualMember.GetType();
            var properties = type.GetProperties();
            List<string> blackList = new List<string> { "Name", "ActiveStreeName", "StreetName" };

            _textPart2 = ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "NewMemberPart2");
            _textPart2 = String.Concat(_textPart2, "\n", "Name: ", "\t\t\t", _actualMember.Name,
                "\n", "Nickname:", "\t\t", _actualMember.ActiveStreeName);

            foreach (var prop in properties)
            {
                if (blackList.Contains(prop.Name))
                {
                    // Do not show Blacklist entries.
                    continue;
                }

                var translation = ResourceSingleton.Instance.CreateActionText("GangMember", prop.Name);
                string tabs = "\t";
                if (translation.Length < 6)
                {
                    tabs += "\t\t";
                }                    
                else if (translation.Length < 7)
                {
                    tabs += "\t";
                }

                _textPart2 = String.Concat(_textPart2, "\n", translation, ": ", tabs);
                var number = UnityEngine.Random.Range(0, 3);
                _textPart2 = String.Concat(_textPart2, number > 1 ? prop.GetValue(_actualMember, null) : "<n/a>");
            }

            EvaluateMemberCount();
            _membersCreated++;
        }

        /// <summary>
        /// Evaluate member count
        /// </summary>
        private void EvaluateMemberCount()
        {
            if (_membersCreated >= _amountProspects)
            {
                _refillStarts = DateTime.Now.AddMinutes(_refillMinutes);
                _membersCreated = 0;
            }

            Debug.Log("Start: " + _refillStarts);
        }
    }
}
