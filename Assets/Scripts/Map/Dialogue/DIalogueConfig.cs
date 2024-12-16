using System;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Configs/Map/Dialogues/Dialog")]
    public class DIalogueConfig : ScriptableObject
    {
        [field: SerializeField] public ActorDialogue[] Dialogues { get; private set; }
        [field: SerializeField] public string Comand {  get; private set; }
        [field: SerializeField] public bool StartUnlimited { get; private set; }

        public bool IsComplited;

        public enum Actor
        {
            LeftActor,
            RightActor
        }

        [Serializable]
        public class ActorDialogue
        {
            public Actor ActorPosition;
            public ActorConfig ActorConfig;
            public string TextDialogue;
        }
    }
}
