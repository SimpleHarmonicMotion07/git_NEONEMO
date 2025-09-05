using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    #region Data Structures

    /// <summary>
    /// ���� ������ �̸��� �⺻���� �����ϴ� ������ Ŭ�����Դϴ�.
    /// Inspector���� �ʱ� ���� ���� ������ �� ����մϴ�.
    /// </summary>
    [Serializable]
    public class StatData
    {
        /// <summary>���� �ĺ��� �̸� (��: "Health", "Attack").</summary>
        public string statName;

        /// <summary>�ش� ������ �⺻��.</summary>
        public float baseValue;
    }

    /// <summary>
    /// ���ȿ� ����Ǵ� ������(Modifier)�� �����մϴ�.
    /// ���� �ð�, ����(additive) �� ����(multiplier) ȿ���� �����ϴ�.
    /// </summary>
    public class StatModifier
    {
        /// <summary>���� ��� ���� Ű (��: "attack", "magic", "crit").</summary>
        public string type;

        /// <summary>�⺻���� �������� ��.</summary>
        public float additive;

        /// <summary>�⺻�� �� ���� ���� �������� ����.</summary>
        public float multiplier;

        /// <summary>�����ڰ� �����Ǵ� �� ���� �ð�(��). 0�̸� ������.</summary>
        public float duration;

        /// <summary>������ ���� ������ Ÿ�ӽ�����.</summary>
        public float startTime;

        /// <summary>������ �׷� �ĺ��� �±� (��: "buff", "debuff").</summary>
        public string tag;
    }

    #endregion

    /// <summary>
    /// ������ �⺻���� ���� ���� �����ڸ� �����ϰ�,
    /// ���� ������ ���� ���� ����Ͽ� �����մϴ�.
    /// </summary>
    public class StatsManager : MonoBehaviour
    {
        #region Fileds


        [Header("�ʱ� ���� (Inspector ����)")]
        /// <summary>Inspector���� ������ �ʱ� ���� ����Ʈ.</summary>
        public List<StatData> initialStats = new List<StatData>();

        /// <summary>���� �̸� �� �⺻�� ���� ��ųʸ�.</summary>
        private Dictionary<string, float> baseValues = new Dictionary<string, float>();

        /// <summary>���� ���� ���� ��� ���� ������ ���.</summary>
        private List<StatModifier> modifiers = new List<StatModifier>();

        #endregion

        #region Unity Callbacks


        /// <summary>
        /// Awake �� �ʱ�Stats�� baseValues ��ųʸ��� �����մϴ�.
        /// </summary>
        void Awake()
        {
            // �ʱ� ������ Dictionary�� ��ȯ
            foreach (var stat in initialStats)
            {
                baseValues[stat.statName] = stat.baseValue;
            }
        }

        /// <summary>
        /// �� �����Ӹ��� ���� �ð��� ����� �����ڸ� �����մϴ�.
        /// </summary>
        void Update()
        {
            float now = Time.time;
            modifiers.RemoveAll(mod => mod.duration > 0 && now - mod.startTime >= mod.duration);
        }

        #endregion

        #region Public API

        /// <summary>
        /// ������ �⺻���� ���� �����մϴ�. �������� ������ ���� �߰��մϴ�.
        /// </summary>
        /// <param name="type">���� Ű</param>
        /// <param name="value">�� �⺻��</param>
        public void SetBase(string type, float value)
        {
            baseValues[type] = value;
        }

        /// <summary>
        /// ���ο� ���� �����ڸ� �߰��մϴ�.
        /// </summary>
        /// <param name="type">���� ��� ���� Ű</param>
        /// <param name="additive">���� ��</param>
        /// <param name="multiplier">���� ����</param>
        /// <param name="duration">���� �ð�(��), 0 �̸� ������</param>
        /// <param name="tag">������ ���п� �±�</param>
        public void AddModifier(string type, float additive = 0f, float multiplier = 1f, float duration = 0f, string tag = null)
        {
            modifiers.Add(new StatModifier
            {
                type = type,
                additive = additive,
                multiplier = multiplier,
                duration = duration,
                startTime = Time.time,
                tag = tag
            });
        }

        /// <summary>
        /// ������ �±׷� ��ϵ� ��� �����ڸ� �����մϴ�.
        /// </summary>
        /// <param name="tag">������ ������ �±�</param>
        public void RemoveModifiersByTag(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return;

            modifiers.RemoveAll(mod => mod.tag == tag);
        }

        /// <summary>
        /// �ش� ���ȿ� ���� ���� ��� �����ڸ� ������
        /// ���� ���� ���� ���� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="type">���� Ű</param>
        /// <returns>�⺻���� ���� �հ�� ���� ������ ������ ���</returns>
        public float GetFinalValue(string type)
        {
            float baseValue = baseValues.ContainsKey(type) ? baseValues[type] : 0f;
            float additiveSum = 0f;
            float multiplierProduct = 1f;

            foreach (var mod in modifiers)
            {
                if (mod.type != type) continue;
                additiveSum += mod.additive;
                multiplierProduct *= mod.multiplier;
            }

            return (baseValue + additiveSum) * multiplierProduct;
        }

        #endregion
    }
}

