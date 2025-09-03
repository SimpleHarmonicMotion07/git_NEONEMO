using UnityEngine;

public class CellBox : MonoBehaviour
{
    [SerializeField] private GameObject destroyParticle;

    private ItemRarity itemRarity;  
    private bool isArrived;
    private Vector2 arrivePos;

    private Vector2 startPos;
    private float moveTime = 0.15f;   // �̵��� �ɸ��� �ð� (��)
    private float elapsedTime;     // ��� �ð�

    public void Init(Transform pos,ItemRarity itemR)
    {
        startPos = transform.position; // ���� ��ġ ���
        arrivePos = pos.position;      // ���� ��ġ ���
        elapsedTime = 0f;              // �ð� �ʱ�ȭ
        isArrived = false;
        itemRarity= itemR;
    }

    private void Update()
    {
        if (isArrived) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveTime);

        // t�� �ε巴�� �� ������ ������ ���� ������
        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        transform.position = Vector2.Lerp(startPos, arrivePos, smoothT);

        if (t >= 1f)
        {
            isArrived = true;
        }
    }
    private void OnMouseDown()
    {
        if (!isArrived) return;
        Instantiate(destroyParticle,transform.position,Quaternion.identity);
        SpawnItem();
        Destroy(gameObject);
    }
    private void SpawnItem()
    {

    }
}