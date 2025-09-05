using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Core;

using Random = UnityEngine.Random;

namespace Core
{
    /// <summary>
    /// ���ּ� ���� �̴ϰ����� �����մϴ�.
    /// ���� ���ۺ��� ����, ���� ���� �� UI ���� ������ �����մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class RepairManager : MonoBehaviour
    {
        #region Fields


        [Tooltip("���� ���� �� �غ� ����")]
        [SerializeField] private TMP_Text ReadyTxt;

        [Header("UI Panels")]
        [Tooltip("���� ���� �� ǥ���� �г�")]
        [SerializeField] private GameObject gameStartPanel;
        [Tooltip("�غ� ���¸� ǥ���� �ؽ�Ʈ(TMP)")]
        [SerializeField] private TMP_Text readyText;

        [Header("Gameplay Settings")]
        [Tooltip("������ ��� ������")]
        [SerializeField] private GameObject target;
        [Tooltip("�̴ϰ��� ���� �ð�(��)")]
        [SerializeField] private float gameTime;
        [Tooltip("������ Ÿ�� ��")]
        [SerializeField] private int numOfTarget;

        [Header("Particles")]
        [Tooltip("���� ����Ʈ ������")]
        [SerializeField] private GameObject explodeParticle;
        [Tooltip("�� ����Ʈ ������")]
        [SerializeField] private GameObject healParticle;
        [Tooltip("���� ����Ʈ ���� �׶���Ʈ �迭")]
        [SerializeField] private Gradient[] explodeGradient;

        [Header("Time UI")]
        [Tooltip("���� �ð� ���־� �̹���")]
        [SerializeField] private Image leftTime;
        [Tooltip("���� �ð� �ؽ�Ʈ(TMP)")]
        [SerializeField] private TMP_Text leftTimeTxt;

        [Header("Health UI")]
        [Tooltip("���� ü�� �� �̹���")]
        [SerializeField] private Image leftHealthImg;
        [Tooltip("�÷��� �ؽ�Ʈ ���� ��ġ")]
        [SerializeField] private Transform spawnFloatingPos;

        [Header("Heal Settings")]
        [Tooltip("�� �� ȸ����")]
        [SerializeField] private int healAmount;
        [Tooltip("�ִ� ü�� ������")]
        [SerializeField] private int maxHealAmount;

        [Header("Result UI")]
        [Tooltip("�̴ϰ��� ���� �� ��� �г�")]
        [SerializeField] private GameObject resultPanel;
        [Tooltip("�� ���� �� �ؽ�Ʈ(TMP)")]
        [SerializeField] private TMP_Text resultHitTxt;
        [Tooltip("���� �Ϸ� �� �ؽ�Ʈ(TMP)")]
        [SerializeField] private TMP_Text resultRepairTxt;
        [Tooltip("���� �ؽ�Ʈ(TMP)")]
        [SerializeField] private TMP_Text resultHealTxt;
        [Tooltip("�ִ� ü�� ������ �ؽ�Ʈ(TMP)")]
        [SerializeField] private TMP_Text resultMaxTxt;

        private int resultHit;
        private int resultRepair;
        private int resultHeal;
        private int resultMax;

        /// <summary>
        /// �̴ϰ��� ���� �� �ܺο� �˸� �̺�Ʈ�Դϴ�.
        /// </summary>
        public static event Action OnGameFinished;

        /// <summary>
        /// ü�� ���� Ʈ���ſ� �̺�Ʈ
        /// </summary>
        private UnityEvent increaseHealth;

        /// <summary>
        /// �̴ϰ��� ���� �� ���� �÷���
        /// </summary>
        private bool isGaming;

        /// <summary>
        /// ���� ������ Ÿ�� ����
        /// </summary>
        private int leftNum;

        /// <summary>
        /// ���� ���� �ð�(��)
        /// </summary>
        private float nowGameTime;

        #endregion

        #region Unity Callbacks


        private void Update()
        {
            if (!isGaming)
                return;

            // ���� �ð��� ���ҽ�Ű�� UI ������Ʈ
            nowGameTime -= Time.deltaTime;
            leftTime.fillAmount = nowGameTime / gameTime;
            leftTimeTxt.text = nowGameTime.ToString("F2");

            if (nowGameTime <= 0f)
            {
                isGaming = false;
                leftTimeTxt.text = "00:00";
                StartCoroutine(FinishGame());
            }

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// �̴ϰ����� �غ��ϰ� �����մϴ�.
        /// </summary>
        /// <param name="currentHP">�÷��̾� ���� ü�� (�ʱ� ü�� ǥ�ÿ�)</param>
        public void RepairStart(float currentHP)
        {
            UpdateHealth();
            resultHit = 0;
            resultRepair=0;
            resultHeal = 0;
            resultMax = 0;

            gameStartPanel.SetActive(true);
            resultPanel.SetActive(false);
            
            StartCoroutine(GameStart());
        }

        #endregion

        #region Coroutines


        /// <summary>
        /// ���� ���� �� �غ� �ܰ�(ī��Ʈ�ٿ�)�� ó���մϴ�.
        /// </summary>
        IEnumerator GameStart()
        {
            readyText.text = "���ּ��� �����ϼ���!";
            yield return new WaitForSeconds(1f);

            for (int i = 3; i > 0; i--)
            {
                readyText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }

            readyText.text = "����!";
            yield return new WaitForSeconds(1f);

            isGaming = true;
            gameStartPanel.SetActive(false);

            // Ÿ�� ����
            nowGameTime = gameTime;
            for (int i = 0; i < numOfTarget; i++)
                SpawnEnemy();

        }

        /// <summary>
        /// ���� ���� ó�� �� ��� UI ���� ǥ�ø� �����մϴ�.
        /// </summary>
        private IEnumerator FinishGame()
        {
            OnGameFinished?.Invoke();
            yield return new WaitForSeconds(1.5f);
            resultHitTxt.text = resultHit.ToString();
            resultRepairTxt.text = resultRepair.ToString(); 
            resultMaxTxt.text = "+"+resultMax.ToString();
            resultHealTxt.text = "+"+resultHeal.ToString();
            resultHitTxt.gameObject.SetActive(false);
            resultRepairTxt.gameObject.SetActive(false);
            resultMaxTxt.gameObject.SetActive(false);
            resultHealTxt.gameObject.SetActive(false);
            resultPanel.SetActive(true);
            yield return WaitForMouseClick();
            resultHitTxt.gameObject.SetActive(true);
            yield return WaitForMouseClick();
            resultRepairTxt.gameObject.SetActive(true);
            yield return WaitForMouseClick();
            resultHealTxt.gameObject.SetActive(true);
            yield return WaitForMouseClick();
            resultMaxTxt.gameObject.SetActive(true);
            yield return WaitForMouseClick();
            GameManager.Instance.EndRepair();
        }

        /// <summary>
        /// ���콺 Ŭ���� ����մϴ�.
        /// </summary>
        IEnumerator WaitForMouseClick()
        {
            // ���콺 ������ �ִ� ���̸� ��ٸ��� (��ư�� ������ ������)
            yield return new WaitUntil(() => !Input.GetMouseButton(0));

            // �ٽ� ���� ������ ��ٸ� (��Ȯ�� "Ŭ��" ����)
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Ÿ�� ������Ʈ�� ���� ��ġ�� �����մϴ�.
        /// </summary>
        public void SpawnEnemy()
        {
            Vector2 randPoint = new Vector2(Random.Range(2f, 6f), Random.Range(1f, 3.5f));
            Vector2 startPoint = new Vector2(Random.Range(0.01f, 0.2f), Random.Range(0.01f, 0.2f));
            if (Random.value < 0.5f)
            {
                randPoint.x *= -1;
                startPoint.x *= -1;
            }
            if (Random.value < 0.5f)
            {
                randPoint.y *= -1;
                startPoint.y *= -1;
            }
            GameObject GO=Instantiate(target,Vector2.zero,Quaternion.identity);
            GO.GetComponent<LineARInitalizer>().InitObj(randPoint, startPoint, Random.Range(1, 4));

        }

        /// <summary>
        /// �� ����Ʈ�� �����ϰ� ȸ���� ���踦 ������ŵ�ϴ�.
        /// </summary>
        /// <param name="pos">���� ��ġ</param>
        public void SpawnParticles(Vector2 pos)
        {
            resultRepair++;
            Instantiate(healParticle,pos, Quaternion.identity);
        }

        /// <summary>
        /// ���� ����Ʈ�� �����ϰ� ���� ���� ������ŵ�ϴ�.
        /// </summary>
        /// <param name="hp">���� ���� HP �ܰ� �ε���</param>
        /// <param name="pos">���� ��ġ</param>
        public void SpawnExplodeParticles(int hp,Vector2 pos)
        {
            resultHit++;
            ParticleSystem PS=explodeParticle.GetComponent<ParticleSystem>();
            var PSGrad = PS.colorOverLifetime;
            PSGrad.color = explodeGradient[hp];
            Instantiate(explodeParticle, pos, Quaternion.identity);
        }

        /// <summary>
        /// �÷��̾ ȸ���ϰų� �ִ� ü���� ������Ű��,
        /// �÷��� �ؽ�Ʈ�� ǥ���մϴ�.
        /// </summary>
        public void HealPlayer()
        {
            GameObject floatingTxt = GameManager.Instance.poolManager.Get(3);
            floatingTxt.transform.position = spawnFloatingPos.position;
            floatingTxt.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));
            TMP_Text floatingTxt_TMP = floatingTxt.GetComponentInChildren<TextMeshPro>();

            if (GameManager.Instance.player.playerCurrentHealth >= GameManager.Instance.player.playerMaxHealth)
            {
                GameManager.Instance.player.IncreaseMaxHealth(maxHealAmount);
                floatingTxt_TMP.text = "+" + maxHealAmount.ToString();
                floatingTxt_TMP.fontSize = 6.5f;
                floatingTxt_TMP.color = Color.red;
                resultMax += maxHealAmount;
            }
            else
            {
                GameManager.Instance.player.IncreaseHealth(healAmount);
                floatingTxt_TMP.text = "+" + healAmount.ToString();
                floatingTxt_TMP.fontSize = 6.5f;
                floatingTxt_TMP.color = Color.green;
                resultHeal += healAmount;
            }

            UpdateHealth();
        }

        /// <summary>
        /// ü�� UI fillAmount�� ���� ü�� ������ �����մϴ�.
        /// </summary>
        public void UpdateHealth()
        {
            leftHealthImg.fillAmount=GameManager.Instance.player.playerCurrentHealth/GameManager.Instance.player.playerMaxHealth;
        }

        #endregion
    }
}