using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// ���� ������ �ð� �帧�� �����մϴ�.
    /// dontMoveStack ���� 0�� ���� Time.timeScale�� 1�� �����ϰ�,
    /// 0���� ũ�� �Ͻ�����(0) ���·� ����ϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class TimeScaleManager : MonoBehaviour
    {
        #region Singleton

        /// <summary>
        /// TimeScaleManager �̱��� �ν��Ͻ��Դϴ�.
        /// </summary>
        public static TimeScaleManager Instance { get; private set; }

        #endregion

        #region Fields

        /// <summary>
        /// �Ͻ����� ��û ���� ī��Ʈ�Դϴ�.
        /// 0�� ���� �ð� �帧�� ����˴ϴ�.</summary>
        [Tooltip("0�� ���� �ð� �帧�� ����˴ϴ�.")]
        public int dontMoveStack = 0;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// �̱��� �ν��Ͻ��� �����ϰ� �ߺ� �� �ı��մϴ�.
        /// �� ��ȯ �ÿ��� �ı����� �ʵ��� �����մϴ�.
        /// </summary>
        private void Awake()
        {
            // �̱��� ����
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// ���� �� �Ͻ����� ������ �ʱ�ȭ�մϴ�.
        /// </summary>
        private void Start()
        {
            dontMoveStack = 0;
        }

        /// <summary>
        /// �� �����Ӹ��� dontMoveStack ���� �˻��Ͽ�
        /// Time.timeScale�� 0 �Ǵ� 1�� �����մϴ�.
        /// </summary>
        private void Update()
        {
            if (dontMoveStack == 0)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// �Ͻ����� ��û�� �ϳ� �߰��Ͽ�
        /// timeScale�� ���ߴ� �������� ����ϴ�.
        /// </summary>
        public void TimeStopStackPlus()
        {
            dontMoveStack++;
        }

        /// <summary>
        /// �Ͻ����� ��û�� �ϳ� �����Ͽ�
        /// timeScale�� ����ϴ� �������� ����ϴ�.
        /// </summary>
        public void TimeStopStackMinus()
        {
            dontMoveStack--;
        }

        #endregion
    }
}