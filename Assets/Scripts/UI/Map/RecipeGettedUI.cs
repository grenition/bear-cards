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

        public override void InteractComplited()
        {
            
            // TODO: give recipe here
        }
    }
}
