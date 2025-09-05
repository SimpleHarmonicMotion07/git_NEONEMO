using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Item
{
    /// <summary>
    /// ��ȭ �̴ϰ��� UI �� ������ ó���� ����մϴ�.
    /// ��ū ����� ���� �پ��� �ɷ�ġ�� ������Ű��,
    /// �Ϸ� �� GameManager�� ����� �����մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class ReinforceManager : MonoBehaviour
    {
        #region Fields

        /// <summary>��ȭ�� ����� �ʱ� ��ū ����</summary>
        [SerializeField] private int numOfToken;

        /// <summary>���� ��ū ������ ǥ���� �ؽ�Ʈ</summary>
        [SerializeField] private TMP_Text leftTokenNum;

        /// <summary>��ȭ ������ ǥ���� �ؽ�Ʈ �迭</summary>
        [SerializeField] private TMP_Text[] texts;

        /// <summary>�� ��ȭ �׸� ��ư �迭</summary>
        [SerializeField] private Button[] buttons;

        /// <summary>��ȭ�� �Ϸ��� �� Ȱ��ȭ�� ��ư</summary>
        [SerializeField] private Button FinishButton;

        /// <summary>�� �׸� ��ȭ Ƚ���� �����ϴ� �迭</summary>
        private int[] reinforceDataDict = new int[8];

        #endregion

        #region Properties

        /// <summary>���� ���� ��ū ����</summary>
        public int LeftToken { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// ��ȭ ���� ���۵� �� ȣ��˴ϴ�.
        /// �ʱ� ��ū ����, �ؽ�Ʈ �ʱ�ȭ, ���� ��ȭ ������ �ε尡 ����˴ϴ�.
        /// </summary>
        public void StartReinforce(int[] data)
        {
            FinishButton.interactable = false;
            LeftToken = numOfToken;
            UpdateTokenNumTxt();
            reinforceDataDict = (int[])data.Clone();
            InitText();
        }

        /// <summary>
        /// ��ȭ�� �Ϸ�Ǹ� ȣ��˴ϴ�.
        /// GameManager�� ��� �����͸� �����ϰ� ���� �����մϴ�.
        /// </summary>
        public void EndReinforce()
        {
            GameManager.Instance.EndReinforce(reinforceDataDict);
        }

        /// <summary>
        /// �ε����� �ش��ϴ� ��ȭ �׸��� 1 ���� �ø���,
        /// ��ū�� �����ϸ� UI, ���� �����ڸ� ������Ʈ�մϴ�.
        /// </summary>
        /// <param name="index">��ȭ �׸� �ε��� (0~7)</param>
        public void UpdateValue(int index)
        {
            LeftToken--;
            UpdateTokenNumTxt();
            if (index == 0)
            {
                GameManager.Instance.player.IncreaseMaxHealth(15);
            }
            else if (index == 1)
            {
                GameManager.Instance.SM.AddModifier("AtkMul",additive:0.08f,tag:"Reinforce_Attack");
            }
            else if (index == 2)
            {
                GameManager.Instance.SM.AddModifier("NaturalHeal", additive: 0.1f, tag: "Reinforce_Heal");
            }
            else if (index == 3)
            {
                GameManager.Instance.SM.RemoveModifiersByTag("Reinforce_Dodge");
                GameManager.Instance.SM.AddModifier("dodgeMul", multiplier: 1 - 0.05f * (reinforceDataDict[3]+1), tag: "Reinforce_Dodge");
            }
            else if (index == 4)
            {
                GameManager.Instance.SM.RemoveModifiersByTag("Reinforce_Defence");
                GameManager.Instance.SM.AddModifier("defenceRate", multiplier: 1 - (0.05f * (reinforceDataDict[3] + 1)), tag: "Reinforce_Defence");
            }
            else if (index == 5)
            {
                GameManager.Instance.SM.AddModifier("PlayerSpeed", additive: 0.05f, tag: "Reinforce_Speed");
            }
            else if (index == 6)
            {
                GameManager.Instance.SM.AddModifier("ItemRange", additive: 0.1f, tag: "Reinforce_ItemR");
            }
            else if (index == 7)
            {
                GameManager.Instance.SM.AddModifier("xpMul", additive: 0.08f, tag: "Reinforce_ItemX");
            }
            if (LeftToken <= 0)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].interactable = false;
                }
                FinishButton.interactable = true;
            }
            reinforceDataDict[index]++;
            InitText();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ��ȭ ���� �ؽ�Ʈ�� ���� �����Ϳ� ���� �����ϰ�,
        /// �ִ� ����(5) �޼� �� ��ư ��Ȱ��ȭ �� ���� ������ ó���մϴ�.
        /// </summary>
        private void InitText()
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = "LV "+reinforceDataDict[i].ToString();
                if (reinforceDataDict[i] >= 5)
                {
                    buttons[i].interactable = false;
                    texts[i].color = Color.red;
                    texts[i].text = "MAX";
                }
            }
        
        }

        /// <summary>
        /// ���� ��ū ���� �ؽ�Ʈ�� �����մϴ�.
        /// </summary>
        private void UpdateTokenNumTxt()
        {
            leftTokenNum.text=LeftToken.ToString();
        }

        #endregion
    }
}