using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AppButtonClick : MonoBehaviour
{
    private Transform parent;
    public GameObject targetPrefab;
    public float scaleDuration = 0.2f;
    private BaseWindowsOperation targetBWO;
    public TMP_Text numText;
    public List<MessageButton> messageButtons = new List<MessageButton>();

    public void UpdateNum()
    {
        int num = 0;
        foreach (var item in messageButtons)
        {
            num += int.Parse(item.numText.text);
        }
        numText.text = num.ToString();
        if(num==0)
        {
            numText.transform.parent.gameObject.SetActive(false);
        }
    }


    private void Awake()
    {
        parent = transform.parent.parent.parent;
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    private void Start()
    {
        //设置消息数量
        List<Character> characters = ChatGLMManager.Instance.otherCharacters;
        int num = 0;
        foreach (var C in characters)
        {
            int length = C.messages.Count;
            for (int i = length - 1; i >= 0 ; i--)
            {
                if(C.messages[i].role=="assistant")
                {
                    num++;
                }
                else
                {
                    break;
                }
            }
        }
        numText.text = num.ToString();
    }

    private void OnButtonClicked()
    {
        if (targetBWO != null)
        {
            RectTransform tempRT = targetBWO.GetComponent<RectTransform>();
            if (targetBWO.isHide)
            {
                tempRT.DOLocalMove(Vector3.zero, scaleDuration);
                tempRT.DOScale(Vector3.one, scaleDuration);
                targetBWO.isHide = false;
            }
        }
        else
        {
            targetBWO = Instantiate(targetPrefab, parent).GetComponent<BaseWindowsOperation>();
            targetBWO.InitAppLink(this);
            targetBWO.GetComponentInChildren<ShowMessageController>().ABC = this;
            RectTransform tempRT = targetBWO.GetComponent<RectTransform>();
            tempRT.DOMove(this.GetComponent<RectTransform>().position, scaleDuration).From();
            tempRT.DOScale(Vector3.zero, scaleDuration).From();
        }
    }

    public void ClearTargetBWO()
    {
        targetBWO = null;
    }
}
