using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

public class UIAlertWindow : MonoBehaviour
{
    public struct AlertWindowContent
    {
        public string title;
        public string content;
        public string[] buttonLabels;
        public UnityAction[] buttonActions;

        public AlertWindowContent(string title, string content,
                            string[] buttonLabels, UnityAction[] buttonActions)
        {
            this.title = title;
            this.content = content;
            this.buttonLabels = buttonLabels;
            this.buttonActions = buttonActions;
        }
    }

    static UIAlertWindow _instance;
    static readonly Queue<AlertWindowContent> _alertContentQueue = new Queue<AlertWindowContent>();

    public static bool IsWindowDisplayed
    {
        get
        {
            if (_instance == null)
                return false;
            if (_instance._windowTransform == null)
                return false;
            return _instance._windowTransform.gameObject.activeSelf;
        }
    }

    public static void CreateAlertWindow()
    {
        if (_instance == null)
        {
            GameObject obj = Resources.Load<GameObject>("AlertWindow");
            obj = Instantiate(obj);
            _instance = obj.GetComponent<UIAlertWindow>();
        }

        _instance.transform.localPosition = Vector3.zero;
        _instance.transform.localScale = Vector3.one;
    }

    public static void Show(string title, string content,
                            string[] buttonLabels, UnityAction[] buttonActions)
    {
        CreateAlertWindow();
        if (_instance._windowTransform.gameObject.activeSelf)
        {
            _alertContentQueue.Enqueue(new AlertWindowContent(title, content, buttonLabels, buttonActions));
            return;
        }
        //  윈도우 GameObject 켜기
        _instance._windowTransform.gameObject.SetActive(true);
        _instance._dimTransform.gameObject.SetActive(true);

        //  타이틀, 내용 텍스트 설정
        _instance._contentText.text = content;

        // 버튼 라벨 텍스트 설정, 버튼 On Off 설정, 버튼 이벤트 설정
        for (int i = 0; i < _instance._buttons.Length; i++)
        {
            _instance._buttons[i].gameObject.SetActive(i < buttonLabels.Length);
            if (i < buttonLabels.Length)
            {
                _instance._buttonLabels[i].text = buttonLabels[i];
                if (buttonActions[i] != null)
                    _instance._buttons[i].onClick.AddListener(buttonActions[i]);
                _instance._buttons[i].onClick.AddListener(_instance.CloseWindow);
            }
        }
    }

    public static void Show(string title, string content,
                            UnityAction closeAction)
    {
        closeAction += () => _instance.CloseWindow();

        Show(title, content,
             new string[] { "닫기" },
             new UnityAction[] { closeAction });
    }

    public static void Show(string title, string content,
                            UnityAction yesAction, UnityAction noAction)
    {
        noAction += () => _instance.CloseWindow();

        Show(title, content,
             new string[] { "예", "아니오" }, 
             new UnityAction[] { yesAction, noAction });
    }

    public static void Close()
    {
        if (_instance != null)
        {
            _instance.CloseWindow();
        }
    }

    Transform _dimTransform;
    Transform _windowTransform;
    TextMeshProUGUI _contentText;

    const int MaxButtonCount = 3;
    Button[] _buttons;
    TextMeshProUGUI[] _buttonLabels;

    void Awake()
    {
        _dimTransform = transform.Find("Dim");
        _windowTransform = transform.Find("Window");

        _contentText = transform.Find("Window/ContentText").GetComponent<TextMeshProUGUI>();

        _buttons = new Button[MaxButtonCount]; // 3개 버튼
        _buttonLabels = new TextMeshProUGUI[MaxButtonCount];
        Transform buttonsParent = transform.Find("Window/Buttons");

        for (int i = 0; i < MaxButtonCount; i++)
        {
            _buttons[i] = buttonsParent.GetChild(i).GetComponent<Button>();
            _buttonLabels[i] = _buttons[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        _dimTransform.gameObject.SetActive(false);
        _windowTransform.gameObject.SetActive(false);
    }

    void CloseWindow()
    {
        _contentText.text = "";

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].onClick.RemoveAllListeners();
        }

        _windowTransform.gameObject.SetActive(false);
        _dimTransform.gameObject.SetActive(false);

        if (_alertContentQueue.Count > 0)
        {
            var content = _alertContentQueue.Dequeue();
            Show(content.title, content.content, content.buttonLabels, content.buttonActions);
        }
    }
}
