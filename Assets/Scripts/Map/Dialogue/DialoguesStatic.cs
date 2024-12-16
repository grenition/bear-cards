using System.Linq;
using UnityEngine;

namespace Project
{
    public static class DialoguesStatic
    {
        private static Dialogues _dialogues;
        private static LocationVariableLoader _locationVariabelsoader;

        private static string[] _recepts;

        public static string[] GetNewRecepts()
        {
            if (_recepts == null)
                _recepts = Resources.LoadAll<ScriptableObject>("Gameplay/Crafts/").ToList().Select(obj => obj.ToString()).ToArray();

            var data = LoadData();
            var newRecept = _recepts.Where(recept => !data.Recepts.Contains(recept));

            return _recepts;
        }

        public static void SaveRecept(string[] newRecepts)
        {
            Initialize();
            var data = LoadData();
            var recepts = data.Recepts.ToList();
            recepts.AddRange(newRecepts);
            data.Recepts = recepts.ToArray();

            _locationVariabelsoader.SaveVariables(data);
        }

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
