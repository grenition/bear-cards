using System;

namespace Project
{
    public abstract class DialogueCondition
    {
        public abstract void Update(LocationVariabelsData locationVariabelsData);
        public abstract bool GetResult();
    }
}
