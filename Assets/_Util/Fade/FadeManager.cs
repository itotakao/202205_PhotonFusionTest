using UnityEngine;
using UnityEngine.UI;

namespace _Util
{
    public class FadeManager : MonoBehaviour
    {
        public static FadeManager Current { get; private set; }

        public bool isFadeOut = false;  //フェードアウト処理の開始、完了を管理するフラグ
        public bool isFadeIn = false;

        [SerializeField]
        float alpha = 0;
        [SerializeField]
        float speed = 0.015f;

        [SerializeField]
        private Image fadeImage;

        void Awake()
        {
            Current = this;
            fadeImage.ExChangeAlpha(alpha);
        }

        void Update()
        {
            if (isFadeOut)
            {
                StartFadeOut();
                return;
            }

            if (isFadeIn)
            {
                StartFadeIn();
            }
        }

        void StartFadeIn()
        {
            alpha -= speed;
            alpha = Mathf.Clamp01(alpha);
            fadeImage.ExChangeAlpha(alpha);
            if (alpha <= 0)
            {
                isFadeIn = false;
                fadeImage.enabled = false;
            }
        }

        void StartFadeOut()
        {
            fadeImage.enabled = true;
            alpha += speed;
            alpha = Mathf.Clamp01(alpha);
            fadeImage.ExChangeAlpha(alpha);
            if (alpha >= 1)
            {
                isFadeOut = false;
            }
        }
    }
}