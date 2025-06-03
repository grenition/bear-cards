using UnityEngine;

namespace Project
{
    public class CardUseLocker : MonoBehaviour
    {
        [SerializeField] private GameObject Lock;

        public void UnLock()
        {
            Lock.SetActive(false);
        }
    }
}
