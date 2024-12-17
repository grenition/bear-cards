using System;

namespace Project
{
    public class RecipeGettedUI : MapPanelUI
    {
        public event Action<string[]> OnReceptUpdate;
        private string[] _keyRecepts; 

        public void SetRecepts(string[] recepts)
        {
            _keyRecepts = recepts;
            OnReceptUpdate?.Invoke(recepts);
        }

        public void Apper(Action action)
        {
            OnInteractComplitedAction = action;
        }

        public override void InteractComplited()
        {
        }
    }
}
