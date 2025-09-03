using UnityEngine;

public class SpinAround : MonoBehaviour
{
    private Transform Target;
    public float radius;
    public float angularVelocity;

    private float angle; // ���� ���� (�� ����)
    private bool angleInitialized = false;

    private void Awake()
    {
        Target = GameManager.instance.player.transform;
    }

    public void SetInitialAngle(float angleDeg)
    {
        this.angle = angleDeg;
        angleInitialized = true;
    }

    void Start()
    {
        if (!angleInitialized)
        {
            // �ܺο��� �ʱ�ȭ �������� �ڵ� ���
            Vector2 direction = transform.position - Target.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }

    void Update()
    {
        // ���� ����
        angle += angularVelocity * Time.deltaTime;

        // �� ��ġ ���
        float rad = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
        transform.position = (Vector2)Target.position + offset;
    }
}
