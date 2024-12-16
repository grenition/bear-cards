namespace Project
{
    public static class DialoguesStatic
    {
        private static Dialogues _dialogues;
        private static LocationVariableLoader _locationVariabelsoader;

        public static void SaveData(LocationVariabelsData data)
        {
            Initialize();
            _locationVariabelsoader.SaveVariables(data);

            _dialogues.DialoguesUpdate();
        }

        public static LocationVariabelsData LoadData()
        {
            Initialize();
            return _locationVariabelsoader.LoadVaribels();
        }

        public static void Deleted(LocationVariabelsData data)
        {
            Initialize();
            _locationVariabelsoader.Deleted();
        }

        private static void Initialize()
        {
            if (_dialogues == null)
            {
                _dialogues = new Dialogues();
                _dialogues.Init();
            }

            _locationVariabelsoader ??= new LocationVariableLoader();
        }
    }
}
