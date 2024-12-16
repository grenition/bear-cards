using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using R3;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class DialogueController : MonoBehaviour
    {
        [Header("Left Actor")]
        [SerializeField] private Actor _leftActor;

        [Header("Left Actor")]
        [SerializeField] private Actor _rightActor;

        [Header("Other")]
        [SerializeField] private TMP_Text _dialogues;
        [SerializeField] private Button _nextStep;

        private int _numberStep;
        private DIalogueConfig _config;

        private void Start()
        {
            _nextStep.onClick.Bind(() => UpdateDialogue()).AddTo(this);
        }

        public void Initialize(DIalogueConfig dIalogueConfig)
        {
            _config = dIalogueConfig;
            _numberStep = 0;
            UpdateDialogue();
        }

        private void UpdateDialogue()
        {
            if (_config.Dialogues.Length == _numberStep)
            {
                var data = DialoguesStatic.LoadData();
                var levelWasComplited = data.KeyDialogueWasComplited.ToList();

                if (!levelWasComplited.Contains(_config.name))
                {
                    levelWasComplited.Add(_config.name);
                    data.KeyDialogueWasComplited = levelWasComplited.ToArray();
                    DialoguesStatic.SaveData(data);
                }
                return;
            }

            _dialogues.text = _config.Dialogues[_numberStep].TextDialogue;

            if (_config.Dialogues[_numberStep].ActorPosition == DIalogueConfig.Actor.LeftActor)
            {
                UpdateActor(_leftActor, _rightActor, _config.Dialogues[_numberStep].ActorConfig.Name,
                    _config.Dialogues[_numberStep].ActorConfig.Icon);
            }
            else
            {
                UpdateActor(_rightActor, _leftActor, _config.Dialogues[_numberStep].ActorConfig.Name,
                    _config.Dialogues[_numberStep].ActorConfig.Icon);
            }
            _numberStep++;
        }

        private void UpdateActor(Actor activeActor, Actor deactivateActor, string name, Sprite icon)
        {
            activeActor.NameActor.text = name;
            activeActor.ImageActor.sprite = icon;
            //activeActor.AnimatorActor?.SetTrigger("active");
            //deactivateActor.AnimatorActor?.SetTrigger("disable");
        }

        [Serializable]
        private class Actor
        {
            public TMP_Text NameActor;
            public Animator AnimatorActor;
            public Image ImageActor;
        }
    }
}
