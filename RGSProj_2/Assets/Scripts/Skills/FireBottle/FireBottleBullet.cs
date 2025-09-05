using UnityEngine;
using Core;

public class FireBottleBullet : MonoBehaviour
{
    public float moveDuration = 1.0f;
    public float arcHeight = 0.3f;

    private Vector2 startPos;
    private Vector2 endPos;
    private float elapsedTime = 0f;
    private bool isMoving = false;
    private SO_FireBottleData skilData;
    private int nowLevel;
    private Skill_FireBottle SF;

    private void Awake()
    {
    }
    public void Attack(Vector2 targetPosition, SO_FireBottleData data, int level, Skill_FireBottle si)
    {
        startPos = transform.position;
        endPos = targetPosition;
        elapsedTime = 0f;
        isMoving = true;
        skilData = data;
        nowLevel = level;
        SF = si;
    }

    void Update()
    {
        if (!isMoving) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveDuration);
        transform.rotation = Quaternion.Euler(0, 0,t * 720);
        // ���� ��ġ ���� (X�� Y)
        Vector2 flatPos = Vector2.Lerp(startPos, endPos, t);

        // ������ ���� ��� (Y���� ����ó�� ��)
        float arc = arcHeight * t * (1 - t); // ������ ����

        // ���� ��ġ: flatPos�� X�� Y, arc�� ���̷ν� Y�࿡ �߰�
        transform.position = new Vector3(flatPos.x, flatPos.y + arc, 0f);

        if (t >= 1f)
        {
            isMoving = false;
            transform.position = new Vector3(endPos.x, endPos.y, 0f); // ��Ȯ�� ���� ��ġ ����
            GameObject incenArea = GameManager.Instance.poolManager.Get(26);
            incenArea.transform.position = transform.position;
            incenArea.GetComponent<FireBottleArea>().Init(skilData.incenDuration[nowLevel], skilData.attackDamage[nowLevel], skilData.incenRadius[nowLevel], SF);
            gameObject.SetActive(false);
        }
    }
}
