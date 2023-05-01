using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum NumberType
{
    Simple,
    Crit
}

public class NumbersUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _numberTexts;

    [SerializeField] private float _moveDistanceNumber;
    [SerializeField] private float _moveDurationNumber;
    [SerializeField] private Color _simpleNumberColor;
    [SerializeField] private Color _critNumberColor;

    private Vector3 _originPosition;

    private void Awake()
    {
        DOTween.Init();
        _originPosition = _numberTexts[0].rectTransform.localPosition;
    }

    public void EnableNumber(NumberType type, string numberValue)
    {
        var number = GetNumberFromPool();
        if (number == null)
            return;

        number.gameObject.SetActive(true);
        number.rectTransform.localPosition = _originPosition;
        number.text = numberValue;

        //set color and scale
        switch (type)
        {
            case NumberType.Simple:
                number.color = _simpleNumberColor;
                number.transform.localScale = new Vector3(1f, 1f, 1f);

                break;
            case NumberType.Crit:
                number.color = _critNumberColor;
                number.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            default:
                break;
        }

        StartCoroutine(NumberAnimation(number));
    }

    private TextMeshProUGUI GetNumberFromPool()
    {
        for (int i = 0; i < _numberTexts.Length; i++)
        {
            if (_numberTexts[i].gameObject.activeSelf)
                continue;

            return _numberTexts[i];
        }

        return null;
    }

    private IEnumerator NumberAnimation(TextMeshProUGUI num)
    {
        //start move
        num.rectTransform.DOMoveY(_moveDistanceNumber, _moveDurationNumber);
        //start unfade
        num.DOFade(1, 0.5f);
        yield return new WaitForSeconds(.5f);
        num.DOFade(0, 0.5f).OnComplete(() => num.gameObject.SetActive(false));
        //start fade

    }
}
