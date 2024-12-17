using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using R3;
using System;
using System.Linq;
using GreonAssets.UI.Extensions;
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
        [SerializeField] private float _typewriterEffectDuration = 2f;

        private int _numberStep;
        private DialogueConfig _config;

        private void Start()
        {
            _nextStep.onClick.Bind(() => UpdateDialogue()).AddTo(this);
        }

        public void Initialize(DialogueConfig dialogueConfig)
        {
            _leftActor.NameActor.text = dialogueConfig.LeftActorStart.Name;
            _leftActor.ImageActor.sprite = dialogueConfig.LeftActorStart.Icon;
            _rightActor.NameActor.text = dialogueConfig.RightActorStart.Name;
            _rightActor.ImageActor.sprite = dialogueConfig.RightActorStart.Icon;

            _config = dialogueConfig;
            _numberStep = 0;
            UpdateDialogue();
        }

        private async void UpdateDialogue()
        {
            if (_config.Dialogues.Length <= _numberStep)
            {
                var data = DialoguesStatic.LoadData();
                var levelWasComplited = data.KeyDialogueWasComplited.ToList();

                if (!levelWasComplited.Contains(_config.name))
                {
                    levelWasComplited.Add(_config.name);
                    data.KeyDialogueWasComplited = levelWasComplited.ToArray();
                    DialoguesStatic.SaveData(data);
                }

                await gameObject.CloseWithChildrensAnimationAsync();
                Destroy(gameObject);
                DialoguesStatic.DialogueComplited();
                return;
            }

            var dialogueText = _config.Dialogues[_numberStep].TextDialogue;
            _dialogues.TypewriterEffect(dialogueText, _typewriterEffectDuration);

            if (_config.Dialogues[_numberStep].ActorPosition == DialogueConfig.Actor.LeftActor)
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
            deactivateActor.Root.SetActiveWithAnimation(false);
            activeActor.Root.SetActiveWithAnimation(true);
            activeActor.NameActor.text = name;
            activeActor.ImageActor.sprite = icon;
        }

        [Serializable]
        private class Actor
        {
            public GameObject Root;
            public TMP_Text NameActor;
            public Image ImageActor;
        }
    }
}
