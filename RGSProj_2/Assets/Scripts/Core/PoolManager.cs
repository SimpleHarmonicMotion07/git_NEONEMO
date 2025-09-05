using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// �̸� ������ �������� Ǯ���Ͽ� �����ϰ�,
    /// �ʿ� �� �ڵ����� PoolTimeTracker�� �ٿ� ���� ��� �ð��� �����մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class PoolManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// �ε������� Ǯ���� ������ �迭 (Inspector���� �Ҵ�)  
        /// </summary>
        [Tooltip("Ǯ���� �������� �ε��� ������� �Ҵ��ϼ���.")]
        public GameObject[] prefabs;

        /// <summary>
        /// �ε����� ������ ������Ʈ ����Ʈ</summary>
        List<GameObject>[] pools;

        #endregion

        #region Unity Callbacks


        /// <summary>
        /// Awake ������ Ǯ ����Ʈ�� �ʱ�ȭ�մϴ�.
        /// </summary>
        private void Awake()
        {
            // prefabs ���̸�ŭ �� ����Ʈ ����
            pools = new List<GameObject>[prefabs.Length];
            for (int idx = 0; idx < pools.Length; idx++)
            {
                pools[idx] = new List<GameObject>();
            }
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// ������ �ε����� ������Ʈ�� ��ȯ�մϴ�.
        /// ��Ȱ��ȭ�� ������Ʈ �� ���� ���� �ð��� ���� ù ��° �������� Ȱ��ȭ�Ͽ� ��ȯ�ϸ�,
        /// ������ ���� �ν��Ͻ�ȭ�մϴ�.
        /// </summary>
        /// <param name="index">prefabs �迭�� �ε���</param>
        /// <returns>Ǯ���� ���� GameObject �ν��Ͻ�</returns>
        public GameObject Get(int index)
        {
            float now = Time.time;
            GameObject select = null;

            foreach (GameObject item in pools[index])
            {
                if (!item.activeSelf)
                {
                    PoolTimeTracker tracker = item.GetComponent<PoolTimeTracker>();
                    if (tracker != null)
                    {
                        if (now - tracker.lastDisabledTime < 1f)
                            continue; // ���� ���� ����
                    }

                    select = item;
                    select.SetActive(true);
                    break;
                }
            }

            if (!select)
            {
                select = Instantiate(prefabs[index], transform);
                pools[index].Add(select);

                // �ڵ����� PoolTimeTracker ���̱�
                if (select.GetComponent<PoolTimeTracker>() == null)
                {
                    select.AddComponent<PoolTimeTracker>();
                }
            }

            // BaseItem �ʱ�ȭ�� ������ ������ �� ���� (�ʿ��� ��쿡��)
            BaseItem isItem = select.GetComponent<BaseItem>();
            if (isItem != null)
            {
                isItem.isCollected = false;
            }

            return select;
        }

        /// <summary>
        /// ������ �ε����� ������Ʈ�� �ҷ�����, ũ�⸦ �����Ͽ� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="index">prefabs �迭�� �ε���</param>
        /// <param name="size">���� �����Ϸ� ������ ũ��</param>
        /// <returns>Ǯ���� ���� GameObject �ν��Ͻ�</returns>
        public GameObject Get(int index, Vector3 size)
        {
            GameObject obj = Get(index);
            if (obj != null)
            {
                obj.transform.localScale = size;
            }
            return obj;
        }

        /// <summary>
        /// �ڽ� Ʈ�������� ���� ��� GameObject�� ��Ȱ��ȭ�մϴ�.
        /// </summary>
        public void DeActiveAllChild()
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        #endregion
    }
}