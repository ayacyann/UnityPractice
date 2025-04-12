using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseWindowsOperation : MonoBehaviour
{
    public AppButtonClick targetABC;
    public Transform HLayout;
    public bool isHide;
    private float scaleDuration = 0.2f;
    private RectTransform RT;
    private Vector2 defaultSize;
    private bool isMaximize;

    private void Start()
    {
        RT = this.GetComponent<RectTransform>();
        defaultSize = RT.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        HLayout.GetChild(0).GetComponent<Button>().onClick.AddListener(MinimizeWindows);
        HLayout.GetChild(1).GetComponent<Button>().onClick.AddListener(MaximizeWindows);
        HLayout.GetChild(2).GetComponent<Button>().onClick.AddListener(CloseWindows);
    }

    public void InitAppLink(AppButtonClick appButtonClick)
    {
        targetABC = appButtonClick;
    }

    private void MinimizeWindows()
    {
        if(!isHide)
        {
            RT.DOMoveY(0, scaleDuration);
            RT.DOScale(Vector3.zero, scaleDuration);
            isHide = true;
        }
    }

    private void MaximizeWindows()
    {
        Debug.Log("在当前接入ChatGLM模型项目中有bug，别用！");
        return;
        // RectTransform windows = RT.GetChild(0).GetComponent<RectTransform>();
        // if (isMaximize)
        // {
        //     Vector2 targetSize = defaultSize;
        //     windows.DOSizeDelta(targetSize, scaleDuration);
        //     windows.DOLocalMove(Vector3.zero, scaleDuration);
        //     isMaximize = false;
        // }
        // else
        // {
        //     Vector2 targetSize = new Vector2(Screen.width, Screen.height);
        //     windows.DOSizeDelta(targetSize, scaleDuration);
        //     windows.DOLocalMove(Vector3.zero, scaleDuration);
        //     isMaximize = true;
        // }
        // Transform max = HLayout.GetChild(1);
        // max.GetChild(0).gameObject.SetActive(!isMaximize);
        // max.GetChild(1).gameObject.SetActive(isMaximize);
    }

    private void CloseWindows()
    {
        RT.DOMove(targetABC.GetComponent<RectTransform>().position, scaleDuration);
        RT.DOScale(Vector3.zero, scaleDuration+0.05f).OnComplete(()=>
        {
            targetABC.ClearTargetBWO();
            Destroy(this.gameObject);
            targetABC.messageButtons.Clear();
        });
    }
}
