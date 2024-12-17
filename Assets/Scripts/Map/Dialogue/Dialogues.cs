using GreonAssets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project
{
    public class Dialogues
    {
        public bool DialogueActive { get; private set; }

        private Dictionary<string, DialogueCondition> _conditionsMap = new();
        private DialogueConfig _currentConfig;

        private DialogueController _prefabsDialogues;
        private List<DialogueConfig> _dialogueConfig = new List<DialogueConfig>();
        private List<string> _keyComplitedDialogues;
        private DialogueController _dialogueController;

        private LocationVariabelsData _locationVariabelsData;
        public void Init()
        {
            _prefabsDialogues = Resources.Load<DialogueController>("Map/Dialogues/Prefabs/Dialogues");
            _dialogueConfig = Resources.LoadAll<DialogueConfig>("Map/Dialogues/Config").ToList();

            _locationVariabelsData = DialoguesStatic.LoadData();
            _dialogueConfig.ForEach(config =>
            {
                _currentConfig = config;
                _conditionsMap.Add(config.name, CreateCondition(GetComand(config)));
            });

            var data = DialoguesStatic.LoadData();
            _keyComplitedDialogues = data.KeyDialogueWasComplited.ToList();
            UpdateConditionData();
        }

        public void DialoguesUpdate()
        {
            UpdateConditionData();

            foreach (var dialog in _conditionsMap)
            {
                if (dialog.Value.GetResult())
                {
                    var dialogConfig = _dialogueConfig.Find(value => value.name == dialog.Key);
                    if (StartDialogue(dialogConfig))
                        return;
                }
            }
        }

        public void DialogueComplited() => DialogueActive = false;

        private bool StartDialogue(DialogueConfig config)
        {
            var data = DialoguesStatic.LoadData();

            if (data.KeyDialogueWasComplited.Contains(config.name) && !config.StartUnlimited)
                return false;

            if (_dialogueController == null)
                _dialogueController ??= GameObject.FindAnyObjectByType<DialogueController>(FindObjectsInactive.Include);
            DialogueActive = true;
            _dialogueController.Initialize(config);
            return true;
        }

        private void UpdateConditionData()
        {
            var data = DialoguesStatic.LoadData();

            _conditionsMap.Values.ForEach(conditions =>
            {
                conditions.Update(data);
            });
        }

        private string GetComand(DialogueConfig config)
        {
            var comand = config.Comand;

            if (comand == "")
                return comand;

            var finalComand = comand.Substring(0, comand.IndexOf('('));

            return finalComand;
        }

        private string FindField()
        {
            var comand = _currentConfig.Comand;
            int startIndex = comand.IndexOf("(") + 1;
            int endIndexByEndCommand = comand.IndexOf(")", startIndex);
            int endIndexByEndField = comand.IndexOf(",", startIndex);

            int endIndex = endIndexByEndField != -1 ? endIndexByEndField : endIndexByEndCommand;

            return comand.Substring(startIndex, endIndex - startIndex);
        }

        private int FindInteger()
        {
            var comand = _currentConfig.Comand;
            var finalComand = comand.Substring(2, comand.IndexOf(','));

            int startIndex = comand.IndexOf(',') + 1;
            int endIndex = comand.IndexOf(")", startIndex);

            return Int32.Parse(comand.Substring(startIndex, endIndex - startIndex));
        }

        public DialogueCondition CreateCondition(string comand)
        {
            DialogueCondition condition = comand switch
            {
                "Equals" => new ConditionEquales(FindField(), FindInteger()),
                "OnUpdateInteger" => new IntegerUpdateCondition(FindField(), _locationVariabelsData),
                "OnUpdateBoolean" => new BooleanUpdateCondition(FindField()),
                "None" => new ConditionNull(),
                _ => new ConditionNull(),
            };

            return condition;
        }
    }
}
