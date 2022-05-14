using Audio;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Util
{
    public class NavigationDialog : MonoBehaviour
    {
        public string Title => titileText.text;
        public string Message => messageText.text;

        private float OpenDialogAnimationSec => 1.0f;
        private float DuringTheOpenDialogAnimationWaitTime => 1.0f;

        private float HoldDialogAnimationSec => 1.0f;
        private float CloseDialogAnimationSec => 1.0f;
        private float OpenDialogHeight => 10.0f;

        private const int MaxHeight = 140;
        private const int MaxWidth = 600;

        [SerializeField]
        private RectTransform maskRectTransform;
        [SerializeField]
        private TextMeshProUGUI titileText;
        [SerializeField]
        private TextMeshProUGUI messageText;
        [SerializeField]
        private Transform moveToPoint;
        [SerializeField]
        private CanvasGroup canvasGroup;

        private bool isProcess = false;

        //private void OnValidate()
        //{
        //    maskRectTransform = GetComponentInChildren<RectMask2D>().GetComponent<RectTransform>();
        //    titileText = GetComponentInChildren<Util.UIParts.Title>().GetComponent<TextMeshProUGUI>();
        //    messageText = GetComponentInChildren<Util.UIParts.Message>().GetComponent<TextMeshProUGUI>();
        //    moveToPoint = transform.GetChild(0).Find("MoveToPoint");
        //    canvasGroup = GetComponentInChildren<CanvasGroup>();

        //}

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            OpenDialog("aaa", "bbbb");
        }

        public void Init()
        {
            maskRectTransform.sizeDelta = new Vector2(0, 0);
            transform.localPosition = Vector3.zero;
            canvasGroup.alpha = 1;
        }

        public void OpenDialog(string inputTitle, string inputMessage)
        {
            StartCoroutine(CoOpenAnimation(inputTitle, inputMessage));
        }

        public void OpenDialog_Textonly(string inputTitle, string inputMessage, float displayDuration, bool playSE)
        {
            StartCoroutine(CoOpenTextAnimation(inputTitle, inputMessage, displayDuration, playSE));
        }

        public Transform GetDialogCanvasTransform()
        {
            return canvasGroup.transform;
        }

        public IEnumerator CoOpenAnimation(string inputTitle, string inputMessage)
        {
            while (isProcess)
            {
                yield return null;
            }

            //AudioManager.SE.Play(AudioDataPath.OpenDialog, transform.position);// SE

            Init();

            titileText.text = inputTitle;

            messageText.text = inputMessage;


            isProcess = true;

            maskRectTransform.sizeDelta = new Vector2(0, OpenDialogHeight);

            // 少し高さを表示して横目いっぱい表示
            float elapsedTime = 0.0f;
            while (true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / OpenDialogAnimationSec;
                float toWidth = Mathf.Lerp(0, 1.0f, t) * MaxWidth;
                maskRectTransform.sizeDelta = new Vector2(toWidth, maskRectTransform.sizeDelta.y);

                if (t >= 1.0f) { break; }

                yield return null;
            }

            yield return new WaitForSeconds(DuringTheOpenDialogAnimationWaitTime);

            // 下まで目いっぱい表示
            float convertHeightClamp1 = MaxHeight / maskRectTransform.sizeDelta.x;
            elapsedTime = 0.0f;
            while (true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / OpenDialogAnimationSec;
                float toWidth = (Mathf.Lerp(0, 1.0f - convertHeightClamp1, t) + convertHeightClamp1) * MaxHeight;
                maskRectTransform.sizeDelta = new Vector2(maskRectTransform.sizeDelta.x, toWidth);

                if (t >= 1.0f) { break; }

                yield return null;
            }

            yield return StartCoroutine(CoMoveHeaderToFooterAnimation());
            yield return StartCoroutine(CoCloseDialog());
        }

        public IEnumerator CoOpenTextAnimation(string inputTitle, string inputMessage, float displayDuration, bool playSE)
        {
            while (isProcess)
            {
                yield return null;
            }

            //if(playSE)
            //AudioManager.SE.Play(AudioDataPath.OpenDialog, transform.position);// SE

            Init();

            titileText.text = inputTitle;

            messageText.text = inputMessage;


            isProcess = true;

            maskRectTransform.sizeDelta = new Vector2(0, OpenDialogHeight);

            // 少し高さを表示して横目いっぱい表示
            float elapsedTime = 0.0f;
            while (true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / OpenDialogAnimationSec;
                float toWidth = Mathf.Lerp(0, 1.0f, t) * MaxWidth;
                maskRectTransform.sizeDelta = new Vector2(toWidth, maskRectTransform.sizeDelta.y);

                if (t >= 1.0f) { break; }

                yield return null;
            }

            yield return new WaitForSeconds(DuringTheOpenDialogAnimationWaitTime);

            // 下まで目いっぱい表示
            float convertHeightClamp1 = MaxHeight / maskRectTransform.sizeDelta.x;
            elapsedTime = 0.0f;
            while (true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / OpenDialogAnimationSec;
                float toWidth = (Mathf.Lerp(0, 1.0f - convertHeightClamp1, t) + convertHeightClamp1) * MaxHeight;
                maskRectTransform.sizeDelta = new Vector2(maskRectTransform.sizeDelta.x, toWidth);

                if (t >= 1.0f) { break; }

                yield return null;
            }

            // 音声終了まで待機
            yield return new WaitForSeconds(displayDuration);

            yield return StartCoroutine(CoMoveHeaderToFooterAnimation());
            yield return StartCoroutine(CoCloseDialog());
        }

        private IEnumerator CoMoveHeaderToFooterAnimation()
        {
            // 見えやすいよう下にダイアログを移動
            float elapsedTime = 0.0f;
            while (true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / HoldDialogAnimationSec;

                transform.localPosition = Vector3.Lerp(Vector3.zero, moveToPoint.localPosition, t);

                if (t >= 1.0f)
                {
                    break;
                }

                yield return null;
            }

            isProcess = false;
        }

        //public void CloseDialog()
        //{
        //    StartCoroutine(CoCloseDialog());
        //}

        public IEnumerator CoCloseDialog()
        {
            while (isProcess)
            {
                yield return null;
            }

            isProcess = true;

            float elapsedTime = 0.0f;
            float tmpCanvasAlpha = canvasGroup.alpha;
            while (true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / CloseDialogAnimationSec;

                canvasGroup.alpha = Mathf.Lerp(tmpCanvasAlpha, 0.0f, t);
                if (t >= 1.0f)
                {
                    break;
                }

                yield return null;
            }

            isProcess = false;
            Init();
        }
    }
}