using InventorySystem;
using TMPro;
using UnityEngine;

public class SetToolTip : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameTxt;
    [SerializeField] private TMP_Text itemDescTxt;
    [SerializeField] private TMP_Text itemSynergeTxt;
    [SerializeField] private TMP_Text itemTagTxt;
    [SerializeField] private TMP_Text itemRarityTxt;
    [SerializeField] private GameObject dojingObj;
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void ShowToolTip(Transform pos, InventoryItem IISE)
    {
        // �ؽ�Ʈ ����
        itemNameTxt.text = IISE.GetSkillEvent().itemName;
        itemDescTxt.text = IISE.GetSkillEvent().itemAbility;
        itemSynergeTxt.text = IISE.GetSkillEvent().synergeString;
        itemTagTxt.text = UtilClass.ToLocalizedString(IISE.GetTags());
        itemRarityTxt.text = UtilClass.ToLocalizedString(IISE.GetRarity());

        // ������Ʈ ��ġ�� ĵ���� ��ǥ�� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Camera.main.WorldToScreenPoint(pos.position),
            canvas.worldCamera,
            out Vector2 localPoint
        );

        // offset ���� (��¦ ���� ����)
        localPoint += new Vector2(-20f, 20f);

        // ���� ũ�� / ĵ���� ũ��
        Vector2 tooltipSize = rectTransform.sizeDelta;
        Vector2 canvasSize = (canvas.transform as RectTransform).sizeDelta;

        float halfH = canvasSize.y / 2;

        Debug.Log(localPoint.y + tooltipSize.y + " " + halfH);
        // y Ŭ����
        if (localPoint.y + tooltipSize.y > halfH)
        {
            localPoint.y = halfH - tooltipSize.y;
        }

        // ���� ����
        rectTransform.localPosition = localPoint;

        dojingObj.SetActive(true);
    }




    public void HideToolTip()
    {
        dojingObj.SetActive(false);
    }
}