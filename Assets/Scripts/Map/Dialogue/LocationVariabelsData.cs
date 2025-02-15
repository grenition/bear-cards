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
            CountCardCraftComming = 0;
            CountReceptComming = 0;
            CountHillComming = 0;
            GameWinCount = 0;
            GameFail = 0;
            CountLocationOneUpdate = 0;
            CountLocationTwoUpdate = 0;
            CountLocationThreeUpdate = 0;
            CountBossOneUpdate = 0;
            CountBossTwoUpdate = 0;
            CountBossThreeUpdate = 0;


            KeyDialogueWasComplited = new string[0];
            Recepts = new string[1] { "craft_vodorod_kislorod" };
        }

        public bool FirstStart;
        public int CountEnemyComming;
        public int CountCardGiveComming;
        public int CountReceptComming;
        public int CountHillComming;
        public int CountCardCraftComming;
        public int GameWinCount;
        public int GameFail;
        public int CountLocationOneUpdate;
        public int CountLocationTwoUpdate;
        public int CountLocationThreeUpdate;
        public int CountBossOneUpdate;
        public int CountBossTwoUpdate;
        public int CountBossThreeUpdate;

        public string[] Recepts;
        public string[] KeyDialogueWasComplited;
    }
}
