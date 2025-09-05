using TMPro;
using UnityEngine;
using System.Collections;

public class TypeManager : MonoBehaviour
{
    public TextMeshProUGUI textUI;     // ����� TextMeshPro �ؽ�Ʈ
    public string fullText; // ��ü ����
    public float delay = 0.15f;        // ���� �� ������ �ð�

    public void ShowText()
    {
        StartCoroutine(ShowTextLetterByLetter());
    }
    public void DeleteText()
    {
        textUI.text = "";
    }
    IEnumerator ShowTextLetterByLetter()
    {
        textUI.text = "";
        foreach (char letter in fullText)
        {
            textUI.text += letter;
            yield return new WaitForSecondsRealtime(delay);

        }
    }
}