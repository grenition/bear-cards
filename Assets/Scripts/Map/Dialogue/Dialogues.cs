using GreonAssets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project
{
    public class Dialogues
    {
        private Dictionary<string, DialogueCondition> _conditionsMap = new();
        private DIalogueConfig _currentConfig;

        private DialogueController _prefabsDialogues;
        private List<DIalogueConfig> _dialogueConfig = new List<DIalogueConfig>();
        private List<string> _keyComplitedDialogues;

        private LocationVariabelsData _locationVariabelsData;
        public void Init()
        {
            _prefabsDialogues = Resources.Load<DialogueController>("Map/Dialogues/Prefabs/Dialogues");

            _dialogueConfig = Resources.LoadAll<DIalogueConfig>("Map/Dialogues/Config").ToList();
            _locationVariabelsData = DialoguesStatic.LoadData();
            _dialogueConfig.ForEach(config =>
            {
                _currentConfig = config;
                _conditionsMap.Add(config.name, CreateCondition(GetComand(config)));
            });

            var data = DialoguesStatic.LoadData();
            _keyComplitedDialogues = data.KeyDialogueWasComplited.ToList();
            UpdateDialogues();
        }

        public void DialoguesUpdate()
        {
            UpdateDialogues();

            foreach (var dialog in _conditionsMap)
            {
                if (dialog.Value.GetResult())
                {
                    var dialogConfig = _dialogueConfig.Find(value => value.name == dialog.Key);
                    StartDialogue(dialogConfig);
                }
            }
        }

        private void StartDialogue(DIalogueConfig config)
        {
            var data = DialoguesStatic.LoadData();

            if (data.KeyDialogueWasComplited.Contains(config.name) && !config.StartUnlimited)
                return;

            var panelialogues = GameObject.Instantiate(_prefabsDialogues);
            panelialogues.Initialize(config);
        }

        private void UpdateDialogues()
        {
            var data = DialoguesStatic.LoadData();

            _conditionsMap.Values.ForEach(conditions =>
            {
                conditions.Update(data);
            });
        }

        private string GetComand(DIalogueConfig config)
        {
            var comand = config.Comand;
            var finalComand = comand.Substring(0, comand.IndexOf('('));

            return finalComand;
        }

        private string FindField()
        {
            var comand = _currentConfig.Comand;
            int startIndex = comand.IndexOf("(") + 1;
            int endIndexByEndCommand = comand.IndexOf(")", startIndex);
            int endIndexByEndField = comand.IndexOf(",", startIndex);

            int endIndex = endIndexByEndField != -1? endIndexByEndField: endIndexByEndCommand;

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

        private DialogueCondition CreateCondition(string comand)
        {
            DialogueCondition condition = comand switch
            {
                "Equals" => new ConditionEquales(FindField(), FindInteger()),
                "OnUpdateInteger" => new IntegerUpdateCondition(FindField(), _locationVariabelsData),
                "OnUpdateBoolean" => new BooleanUpdateCondition(FindField()),
                _ => throw new KeyNotFoundException("Command not present in map"),
            };

            return condition;
        }
    }
}
