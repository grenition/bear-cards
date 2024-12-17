using System;
using GreonAssets.Extensions;
using GreonAssets.UI.Extensions;
using Project.UI.Common.Extensions;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Tutorial
{
    public class UITutorial : MonoBehaviour
    {
        [SerializeField] private string _tutorialId;
        [SerializeField] private GameObject _mainContainer;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _previousPageButton;
        [SerializeField] private Button _nextPageButton;
        [SerializeField] private GameObject[] _pages;
        [SerializeField] private int _startPage = 0;

        private int _pageIndex = 0;
        
        private void Awake()
        {
            _closeButton.Bind(() =>
            {
                _mainContainer.CloseWithChildrensAnimation();
            }).AddTo(this);

            _previousPageButton.Bind(() =>
            {
                SetPage(_pageIndex - 1);
            }).AddTo(this);

            _nextPageButton.Bind(() =>
            {
                SetPage(_pageIndex + 1);
            }).AddTo(this);
        }

        private void Start()
        {
            _previousPageButton.gameObject.SetActive(false);
            _nextPageButton.gameObject.SetActive(false);
            _pages.ForEach(x => x.gameObject.SetActive(false));
            SetPage(_startPage);

            if (PlayerPrefs.GetInt($"tutorial_played_{_tutorialId}") == 1)
            {
                _mainContainer.SetActive(false);
                return;
            }
            _mainContainer.SetActive(true);
            PlayerPrefs.SetInt($"tutorial_played_{_tutorialId}", 1);
        }

        public void SetPage(int index)
        {
            if(index < 0 || index >= _pages.Length) return;

            _pageIndex = index;
            for (int i = 0; i < _pages.Length; i++)
            {
                _pages[i].SetActiveWithChildrensAnimation(i == _pageIndex);
            }

            _previousPageButton.SetActiveWithAnimation(PreviosPageAvailable());
            _nextPageButton.SetActiveWithAnimation(NextPageAvailable());
        }
        public bool PreviosPageAvailable() => _pageIndex > 0;
        public bool NextPageAvailable() => _pageIndex + 1 < _pages.Length;

        public void Show() => _mainContainer.SetActive(true);
        public void Hide() => _mainContainer.CloseWithChildrensAnimation();
    }
}
