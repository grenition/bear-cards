using Project.Gameplay.Battle.Model.Cards;
using UnityEngine;

namespace Project
{
    public class CraftDynamicTutorial : MonoBehaviour
    {
        [SerializeField] private GameObject All;

        [SerializeField] private CardModel SiliciumCard;

        [SerializeField] private GameObject[] Pages;
        [SerializeField] private int CurrentPage;

        private void Start()
        {
            if (PlayerPrefs.GetInt($"tutorial_played_craft_tutorial") == 1)
            {
                All.SetActive(false);
                return;
            }
            All.SetActive(true);
        }

        public void NextPage()
        {
            CurrentPage++;

            for (int i = 0; i < Pages.Length; i++)
            {
                Pages[i].SetActive(CurrentPage == i);
            }

            if (CurrentPage >= Pages.Length)
            {
                PlayerPrefs.SetInt($"tutorial_played_craft_tutorial", 1);
                PlayerPrefs.Save();
                All.SetActive(false);
            }
        }
    }
}
