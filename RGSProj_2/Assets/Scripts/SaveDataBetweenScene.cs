using UnityEngine;

public class SaveDataBetweenScene : MonoBehaviour
{
    public static SaveDataBetweenScene Instance;
    void Awake()
    {
        // �̱��� �ߺ� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // �� �̵� �� �ı����� ����
    }
}
