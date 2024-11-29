using UnityEngine;

namespace Project.UI.Battle
{
    public class UIRotatingZAxis : MonoBehaviour
    {
        [SerializeField] public float rotationSpeed = 400f;

        private void Update()
        {
            transform.localEulerAngles += Vector3.forward * (rotationSpeed * Time.deltaTime);
        }
    }
}