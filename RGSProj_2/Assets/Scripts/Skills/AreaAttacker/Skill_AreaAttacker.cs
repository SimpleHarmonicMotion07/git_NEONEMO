using UnityEngine;

public class Skill_AreaAttacker : BaseSkill
{
    [SerializeField] private GameObject[] areaAttackers;
    [HideInInspector]public SO_AreaAttackerData skillData;
    [Header("������")]
    public float damageReduceAmount = 0.7f;
    [Header("������")]
    public float baseAttackDmg = 4.6f;
    [Header("������")]
    public float stunPercent = 1f;
    public float stunTime = 0.5f;
    private void Awake()
    {
        if (baseSkillData.GetType() == typeof(SO_AreaAttackerData))
        {
            skillData = (SO_AreaAttackerData)baseSkillData;
        }
        else
        {
            Debug.LogError("Wrong Downcasting!");
        }
    }
    private void Update()
    {
        float size = skillData.sizeByLevel[itemLevel] * GameManager.instance.SM.GetFinalValue("AoESize");
        transform.localScale = new Vector3(size, size, size);
        for (int i= 0;i < areaAttackers.Length;i++)
        {
            areaAttackers[i].transform.Rotate(Vector3.forward, 1*Mathf.Pow(-1,i));
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
        float size = skillData.sizeByLevel[itemLevel] * GameManager.instance.SM.GetFinalValue("AoESize");
        transform.localScale = new Vector3(size,size,size);
    }
}
