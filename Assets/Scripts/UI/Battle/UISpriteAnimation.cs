using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Battle
{
    public class UISpriteAnimation : MonoBehaviour
    {

        public Image m_Image;

        public Sprite[] m_SpriteArray;
        public float m_Speed = .5f;

        private int m_IndexSprite;

        public void Func_PlayUIAnim()
        {
            while (m_IndexSprite >= m_SpriteArray.Length)
            {
                UniTask.WaitForSeconds(m_Speed);
                m_Image.overrideSprite = m_SpriteArray[m_IndexSprite];
                m_IndexSprite += 1;
            }
        }
    }
}