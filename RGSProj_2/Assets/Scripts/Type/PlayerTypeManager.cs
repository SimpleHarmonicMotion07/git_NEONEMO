using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Type
{
    /// <summary>
    /// �÷��̾��� Ÿ�� ��ȯ�� �����ϰ�,
    /// SpriteRenderer �� ParticleSystem ������ �����մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerTypeManager : MonoBehaviour
    {
        #region Fields

        /// <summary>���� Ȱ��ȭ�� BaseType</summary>
        public BaseType BT;

        /// <summary>��� ������ Ÿ�� �ڵ� ���</summary>
        public List<int> NowType = new List<int>();

        /// <summary>Ÿ�� �ڵ忡 �����ϴ� BaseType ����Ʈ</summary>
        public List<BaseType> Types = new List<BaseType>();

        /// <summary>Ÿ�Ժ� ���� �迭 (���� 4)</summary>
        public Color[] Colors = new Color[4];

        /// <summary>Ÿ�Ժ� �׶���Ʈ �迭 (���� 4)</summary>
        public Gradient[] Gradients = new Gradient[4];

        /// <summary>������ Ÿ�� ���� �ð� (�� ����)</summary>
        private float lastChangeTime;

        /// <summary>Ÿ�� ���� ��Ÿ�� (��)</summary>
        [SerializeField] private float changeCool;

        /// <summary>���� ���濡 ����� SpriteRenderer</summary>
        [SerializeField] private SpriteRenderer SR;

        /// <summary>���� ���濡 ����� ParticleSystem</summary>
        [SerializeField] private ParticleSystem PS;

        /// <summary>���� ���õ� Ÿ�� �ε���</summary>
        public int idx = 0;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// MonoBehaviour ���� �� ȣ��˴ϴ�.
        /// �ʱ�ȭ ������ �ʿ��� ��� ���⿡ �����ϼ���.
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// �� ������ Ÿ�� ��ȯ Ű(A/D) �Է��� �����Ͽ�
        /// ������ ��Ÿ�� ���Ŀ� <see cref="ChangeColor"/>�� ȣ���մϴ�.
        /// </summary>
        private void Update()
        {

            if (Time.time>lastChangeTime+changeCool) {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    idx = (idx - 1 + NowType.Count) % NowType.Count;
                    ChangeColor();
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    idx = (idx + 1) % NowType.Count;
                    ChangeColor();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// ���ο� Ÿ�� �ڵ带 ����մϴ�.
        /// </summary>
        /// <param name="typeCode">�߰��� Ÿ�� �ڵ�</param>
        public void AddType(int typeCode)
        {
            NowType.Add(typeCode);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ���� ���õ� Ÿ�� �ε����� �����ϴ� ������
        /// SpriteRenderer �� ParticleSystem�� �����ϰ�
        /// ������ ���� �ð��� �����մϴ�.
        /// </summary>
        private void ChangeColor()
        {
            lastChangeTime = Time.time;
            BT = Types[NowType[idx]];

            // SpriteRenderer ���� ����
            SR.color = Colors[BT.typeCode];

            // ParticleSystem �׶���Ʈ ����
            var colorOverLifetime = PS.colorOverLifetime;
            colorOverLifetime.color = Gradients[BT.typeCode];
        }

        #endregion
    }
}