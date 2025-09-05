using UnityEngine;
using InventorySystem;

namespace InventorySystem
{
    /// <summary>
    /// ���Կ� ����� �������� �ʱ�ȭ�ϰ� �����մϴ�.
    /// Slot ������Ʈ�� ĳ���Ͽ� ���� ���� ������ ������ �� �ֵ��� �غ��մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class SlotItemManager : MonoBehaviour
    {
        #region Fields


        /// <summary>
        /// Slot ������Ʈ ����
        /// </summary>
        private Slot slot;

        #endregion

        #region Unity Callbacks


        /// <summary>
        /// Awake �� Slot ������Ʈ�� ������ ĳ���մϴ�.
        /// Slot ������Ʈ�� ���� ��� ������ ����մϴ�.
        /// </summary>
        private void Awake()
        {
            slot = GetComponent<Slot>();
            if (slot == null)
            {
                Debug.LogError($"Slot ������Ʈ�� ã�� �� �����ϴ�. GameObject: {gameObject.name}", this);
            }

        }

        #endregion
    }
}

