using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;

namespace UI.LevelUp
{
    /// <summary>
    /// ������ �� ��ų�������Ƽ ���׷��̵� UI �帧�� �����մϴ�.
    /// ������ �г� �� ��ų/�����Ƽ ���� �� ���� ���׷��̵� �г� ������ 
    /// ȭ�� ��ȯ�� �Է� ó��, ���� ��� �ݿ�, TimeScale ��� �����մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class LevelManager : MonoBehaviour
    {
        #region Fields

        /// <summary>���׷��̵� �ɼ� Ŭ�� ��� ���� ����</summary>
        private bool canClick = false;

        /// <summary>���� ������ ���μ��� ������ ����</summary>
        public bool isUpgrading { get; private set; }

        [Header("Panels")]
        [SerializeField] private GameObject totalPanel;           // ��ü ������ UI
        [SerializeField] private GameObject levelUpPanel;         // ������ ���� �г�
        [SerializeField] private GameObject skillUpgradePanel;    // ���� ��ų/�����Ƽ ���׷��̵� �г�

        [Tooltip("������ �ɼ� ǥ�ÿ� �г�(3��)")]
        [SerializeField] private GameObject[] panels = new GameObject[3];
        [Tooltip("���� ���׷��̵� �ɼ� ǥ�ÿ� �г�(3��)")]
        [SerializeField] private GameObject[] upgradePanels = new GameObject[3];

        [Header("Skill & Ability Pools")]
        [Tooltip("�⺻ ��ų Ǯ")]
        [SerializeField] private BaseSkill[] skills;
        [Tooltip("�� �Ӽ� ��ų Ǯ")]
        [SerializeField] private BaseSkill[] fireSkills;
        [Tooltip("�ٶ� �Ӽ� ��ų Ǯ")]
        [SerializeField] private BaseSkill[] windSkills;
        [Tooltip("�� �Ӽ� ��ų Ǯ")]
        [SerializeField] private BaseSkill[] lightSkills;
        [Tooltip("�պ� ��ų Ǯ(�ʱ� ��ų)")]
        [SerializeField] private BaseSkill[] totalSkills;
        [Tooltip("�����Ƽ Ǯ")]
        [SerializeField] private BaseType[] typeClasses;

        /// <summary>���� ���� ���׷��̵� ������(3��)</summary>
        private MaxLevelData[] maxLevelDatas = new MaxLevelData[3];

        /// <summary>���� ȭ�鿡 �Ѹ� ��ų �ɼ� �迭</summary>
        private BaseSkill[] selectedSkills;
        /// <summary>���� ȭ�鿡 �Ѹ� �����Ƽ �ɼ� �迭</summary>
        private BaseType[] selectedAbilities;
        /// <summary>�� �г��� ��ų(true) �Ǵ� �����Ƽ(false) ��������</summary>
        private bool[] isSkill = new bool[3];

        [Header("Configuration")]
        [Tooltip("������ �ɼ� �� ���� ���õ� �ε���")]
        [SerializeField] private int selectedNum = -1;
        [Tooltip("���� ���׷��̵� �ɼ� �� ���õ� �ε���")]
        [SerializeField] private int maxSkillSelectedNum = -1;
        [Tooltip("�����Ƽ Ȯ��(0~1)")]
        [SerializeField] private float abilityPercent;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            // ó���� ��� ��Ȱ��ȭ
            totalPanel.SetActive(false);
            levelUpPanel.SetActive(false);
            skillUpgradePanel.SetActive(false);
            skills = skills.Concat(totalSkills).ToArray();
            foreach (GameObject panel in upgradePanels)
            {
                panel.SetActive(false);
            }
            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }

            selectedNum = -1;
            maxSkillSelectedNum = -1;
            for(int i = 0; i < totalSkills.Length; i++)
            {
                GameManager.Instance.UIM.AddSkill(totalSkills[i]);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)&&selectedNum==-1)
            {
                for (int i = 0; i < panels.Length; i++)
                {
                    if (panels[i].GetComponent<hoveringUI>().isHovering)
                    {
                        selectedNum = i;
                        break;
                    }
                }
                if (selectedNum >= 0)
                {
                    if (isSkill[selectedNum])
                    {
                        if (panels[selectedNum].GetComponent<hoveringUI>().isFinalUpgrade)
                        {
                            //������ ���׷��̵���
                        }
                        else
                        {
                            if (selectedSkills[selectedNum].itemLevel < 0)
                            {
                                selectedSkills[selectedNum].gameObject.SetActive(true);
                                GameManager.Instance.UIM.AddSkill(selectedSkills[selectedNum]);
                            }
                            selectedSkills[selectedNum].GetComponent<IUpgradable>().Upgrade();
                        }
                    }
                    else
                    {
                        if (panels[selectedNum].GetComponent<hoveringUI>().isFinalUpgrade)
                        {

                        }
                        else
                        {
                            if (selectedAbilities[selectedNum].typePassiveLevel<0)
                            {
                                selectedAbilities[selectedNum].gameObject.SetActive(true);
                            }
                            selectedAbilities[selectedNum].GetComponent<IUpgradable>().Upgrade();

                        }

                    }


                    StartCoroutine(DisappearPanel());
                }
            }

            if (Input.GetMouseButtonDown(0) && maxSkillSelectedNum == -1 && canClick)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (upgradePanels[i].GetComponent<hoveringUI>().isHovering)
                    {
                        maxSkillSelectedNum = i;
                        break;
                    }
                }

                if (maxSkillSelectedNum >= 0)
                {
                    canClick = false; // �Է� ����
                    selectedSkills[selectedNum].reinforcedNum = maxSkillSelectedNum + 1;
                    selectedSkills[selectedNum].GetComponent<IUpgradable>().Upgrade();
                    StartCoroutine(DisappearUpgradePanel());
                }
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// ������ ���μ����� �����մϴ�.
        /// ȭ�� ��ȯ, ���� �ɼ� ����, TimeScale ������ ó���մϴ�.
        /// </summary>
        public void LevelUp()
        {
            Debug.Log("PLUSED");
            TimeScaleManager.Instance.TimeStopStackPlus();
            isUpgrading = true;
            totalPanel.SetActive(true);
            levelUpPanel.SetActive(true);
            skillUpgradePanel.SetActive(false);
            selectedNum = -1;
            maxSkillSelectedNum = -1;
            selectedSkills = GetRandomSkills(3);
            selectedAbilities = GetRandomAbillity(3);
            for (int i = 0; i < selectedSkills.Length; i++) {
                if (UtilClass.GetPercent(abilityPercent))
                {
                    panels[i].GetComponent<hoveringUI>().SetPanel(selectedAbilities[i]);
                    isSkill[i] = false;
                }
                else
                {
                    panels[i].GetComponent<hoveringUI>().SetPanel(selectedSkills[i]);
                    isSkill[i] = true;
                }
            }
            StartCoroutine(ShowPanel());
        }

        /// <summary>�� �Ӽ� ��ų�� Ǯ�� �߰��մϴ�.</summary>
        public void AddFireSkill() 
        {
            skills = skills.Concat(fireSkills).ToArray();
        }
        /// <summary>�ٶ� �Ӽ� ��ų�� Ǯ�� �߰��մϴ�.</summary>
        public void AddWindSkill()
        {
            skills = skills.Concat(windSkills).ToArray();
        }
        /// <summary>�� �Ӽ� ��ų�� Ǯ�� �߰��մϴ�.</summary>
        public void AddLightSkill()
        {
            skills = skills.Concat(lightSkills).ToArray();
        }

        #endregion

        // ���� ���� ���� region ���ó�� X

        private IEnumerator ShowPanel()
        {
            panels[0].SetActive(true);
            yield return new WaitForSecondsRealtime(0.22f);
            panels[2].SetActive(true);
            yield return new WaitForSecondsRealtime(0.22f);
            panels[1].SetActive(true);
        }

        private IEnumerator ShowUpgradedPanel()
        {
            upgradePanels[0].SetActive(true);
            yield return new WaitForSecondsRealtime(0.22f);
            upgradePanels[2].SetActive(true);
            yield return new WaitForSecondsRealtime(0.22f);
            upgradePanels[1].SetActive(true);

            yield return new WaitForSecondsRealtime(0.1f); // �߰� ���� (optional)
            canClick = true;
        }

        private IEnumerator DisappearPanel()
        {
            if (panels[selectedNum].GetComponent<hoveringUI>().isFinalUpgrade)
            {

                if (isSkill[selectedNum])
                {
                    foreach (GameObject panel in panels)
                    {
                        panel.SetActive(false);
                    }
                    levelUpPanel.SetActive(false);
                    maxLevelDatas = selectedSkills[selectedNum].baseSkillData.levelDatas;
                    skillUpgradePanel.SetActive(true);

                    for (int i = 0; i < 3; i++)
                    {
                        upgradePanels[i].GetComponent<hoveringUI>().SetPanel(maxLevelDatas[i]);
                    }
                    StartCoroutine(ShowUpgradedPanel());
                }
                else
                {
                    selectedAbilities[selectedNum].GetComponent<IUpgradable>().Upgrade();
                    panels[selectedNum].GetComponent<Animator>().SetTrigger("Disappear");
                    yield return new WaitForSecondsRealtime(0.4f);
                    levelUpPanel.SetActive(false);
                    totalPanel.SetActive(false);

                    EndUpgrade();
                }

            }
            else
            {
                panels[selectedNum].GetComponent<Animator>().SetTrigger("Disappear");
                yield return new WaitForSecondsRealtime(0.4f);
                levelUpPanel.SetActive(false);
                totalPanel.SetActive(false);

                EndUpgrade();
            }
        }

        private IEnumerator DisappearUpgradePanel()
        {
            upgradePanels[maxSkillSelectedNum].GetComponent<Animator>().SetTrigger("Disappear");
            RemoveSkill(selectedSkills[selectedNum]);
            yield return new WaitForSecondsRealtime(0.4f);
            skillUpgradePanel.SetActive(false);
            levelUpPanel.SetActive(false);
            totalPanel.SetActive(false);

            foreach (GameObject panel in upgradePanels)
            {
                panel.SetActive(false);
            }

            canClick = false; // �ٽ� ������
            EndUpgrade();
        }

        private BaseSkill[] GetRandomSkills(int count)
        {
            var availableSkills = skills
                .Where(skill => skill != null && !skill.isMaxLevel) // null üũ�� �Բ�!
                .OrderBy(x => Random.value)
                .Take(count)
                .ToArray();
            return availableSkills;
        }

        private BaseType[] GetRandomAbillity(int count)
        {
            var availableSkills = typeClasses
                .Where(skill => skill != null) // null üũ�� �Բ�!
                .OrderBy(x => Random.value)
                .Take(count)
                .ToArray();
            return availableSkills;
        }

        public void RemoveSkill(BaseSkill skillToRemove)
        {
            skills = skills.Where(skill => skill != skillToRemove).ToArray();
        }

        public void RemoveAbility(BaseType typeToRemove)
        {

            typeClasses=typeClasses.Where(type=>type!=typeToRemove).ToArray();
            Debug.Log("removed");
        }

        private void EndUpgrade()
        {
            if (!isUpgrading) return;
            isUpgrading = false;
            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }
            GameManager.Instance.UIM.UpdateAll();
            TimeScaleManager.Instance.TimeStopStackMinus();
        }
    }
}