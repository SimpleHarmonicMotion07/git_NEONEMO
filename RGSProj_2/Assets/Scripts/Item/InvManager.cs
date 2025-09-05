using InventorySystem;
using UnityEngine;
using Core;

namespace Item
{
    /// <summary>
    /// �κ��丮 �г��� ����/�ݱ�� 
    /// ���� Ÿ�ӽ�����(�Ͻ�����) ��� ����մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class InvManager : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// ����� �κ��丮 UI �г� ������Ʈ
        /// </summary>
        [Tooltip("�κ��丮 UI �г��� �巡���Ͽ� �Ҵ��ϼ���.")]
        [SerializeField] private GameObject inventoryObj;

        /// <summary>
        /// �κ��丮 ���� �� �÷��̾� �̵��� ��Ȱ��ȭ�ϱ� ���� ������Ʈ
        /// </summary>
        [Tooltip("PlayerMove ������Ʈ�� �巡���Ͽ� �Ҵ��ϼ���.")]
        [SerializeField] private PlayerMove PM;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// ���� ���� �� �⺻ �������� �κ��丮�� �߰��մϴ�.
        /// InventoryController �ν��Ͻ��� ������ ������ ����մϴ�.
        /// </summary>
        private void Start()
        {
            InventoryController.instance.AddItem("Inventory", "Ruby", 2);
        }

        /// <summary>
        /// �� ������ "I" Ű �Է��� Ȯ���ϰ� �κ��丮 ����� ȣ���մϴ�.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ShowPausePanel();
            }
        }

        /// <summary>
        /// �κ��丮 �г��� ���ų� �ݰ�, TimeScaleManager�� ���� �Ͻ�����/�簳�� ó���մϴ�.
        /// </summary>
        public void ShowPausePanel()
        {
            inventoryObj.SetActive(!inventoryObj.activeSelf);
            if (inventoryObj.activeSelf)
            {
                TimeScaleManager.Instance.TimeStopStackPlus();
            }
            else
            {
                TimeScaleManager.Instance.TimeStopStackMinus();
            }
        }

        #endregion
    }
}