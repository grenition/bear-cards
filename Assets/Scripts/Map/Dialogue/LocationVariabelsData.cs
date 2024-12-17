using System;

namespace Project
{
    public class LocationVariabelsData
    {
        public LocationVariabelsData()
        {
            FirstStart = false;
            CountEnemyComming = 0;
            CountCardGiveComming = 0;
            CountReceptComming = 0;
            CountHillComming = 0;
            GameWinCount = 0;
            GameFail = 0;
            CountLocationOneUpdate = 0;
            CountLocationTwoUpdate = 0;
            CountLocationThreeUpdate = 0;

            KeyDialogueWasComplited = new string[0];
            Recepts = new string[0];
        }

        public bool FirstStart;
        public int CountEnemyComming;
        public int CountCardGiveComming;
        public int CountReceptComming;
        public int CountHillComming;
        public int GameWinCount;
        public int GameFail;
        public int CountLocationOneUpdate;
        public int CountLocationTwoUpdate;
        public int CountLocationThreeUpdate;

        public string[] Recepts;
        public string[] KeyDialogueWasComplited;
    }
}
