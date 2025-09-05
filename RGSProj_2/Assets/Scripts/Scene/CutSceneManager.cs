using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Cinemachine;
using TMPro;
using DialogueSystem;
public class CutSceneManager : MonoBehaviour
{
    [Header("LandingProtocol")]
    [SerializeField] private ParticleSystem boost;
    [SerializeField] private ParticleSystem PS;
    [SerializeField] private float fadeDuration = 1f; // 1�� ���� ���̱�

    [Header("Connection")]
    [SerializeField] private SpriteRenderer connectSR;

    [Header("Warning")]
    [SerializeField] private Image warningImg;
    [SerializeField] private CinemachineBasicMultiChannelPerlin CBMCP;
    [SerializeField] private GameObject planet;
    [SerializeField] private GameObject spaceShip;
    [SerializeField] private GameObject boom;

    private bool isStory=true;
    private bool storyStart;
    [SerializeField] private GameObject isStoryPanel;
    [SerializeField] private TMP_Text YesText;
    [SerializeField] private TMP_Text NoText;
    [SerializeField] private DialogueHolder DH;
    private void Start()
    {
        isStory = true;
        UpdatePanel();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            isStory = !isStory;
            UpdatePanel();  
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { 
            isStory=!isStory;
            UpdatePanel();
        }
        if (Input.GetKeyDown(KeyCode.Return)&&!storyStart)
        {
            storyStart = true;
            if (isStory) {
                isStoryPanel.SetActive(false);
                DH.StartDialogue();
            }
            else
            {
                Loader.Load(Loader.Scene.GameScene);
            }
        }
    }
    private void UpdatePanel()
    {
        if (isStory)
        {
            YesText.color = Color.red;
            NoText.color = Color.white;
        }
        else
        {
            YesText.color = Color.white;
            NoText.color = Color.red;
        }
    }
    public void LandingProtocol()
    {
        var PSMain = PS.emission;
        PSMain.rateOverTime=5;
        StartCoroutine(FadeToTarget());
    }

    public void ConnectTry()
    {
        connectSR.gameObject.SetActive(true);
        connectSR.color = Color.yellow;
    }

    public void ConnectFailed()
    {
        connectSR.color = Color.red;
    }

    public void ConnectOff()
    {
        connectSR.gameObject.SetActive(false);
    }

    public void RadioActive()
    {
        warningImg.gameObject.SetActive(true);
        StartCoroutine(FadeWarningLoop());
        CBMCP.FrequencyGain = 1;
        CBMCP.AmplitudeGain = 1;
    }

    public void Error()
    {
        CBMCP.FrequencyGain = 3;
        CBMCP.AmplitudeGain = 3;
    }

    public void Crash()
    {
        PS.Pause();
        StartCoroutine(PlanetCrashSequence());
    }
    private IEnumerator FadeWarningLoop()
    {
        float alphaDuration = 1f; // �� �� �������� �ٲ�� �� �ɸ��� �ð�
        Color baseColor = warningImg.color;

        while (true)
        {
            // Fade In: ���� 0 �� 1
            float time = 0f;
            while (time < alphaDuration)
            {
                time += Time.deltaTime;
                float t = time / alphaDuration;
                float alpha = Mathf.Lerp(0f, 1f, t);
                warningImg.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                yield return null;
            }

            // Fade Out: ���� 1 �� 0
            time = 0f;
            while (time < alphaDuration)
            {
                time += Time.deltaTime;
                float t = time / alphaDuration;
                float alpha = Mathf.Lerp(1f, 0f, t);
                warningImg.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                yield return null;
            }
        }
    }
    private IEnumerator FadeToTarget()
    {
        float time = 0f;

        // ���۰�
        float startVelocityX = -10f;
        float startLifetime = 1.0f;

        // ��ǥ��
        float targetVelocityX = -5f;
        float targetLifetime = 0.5f;

        // �ʱ� ����
        var vel = PS.velocityOverLifetime;
        vel.x = new ParticleSystem.MinMaxCurve(startVelocityX);

        var mainBoost = boost.main;
        mainBoost.startLifetime = new ParticleSystem.MinMaxCurve(startLifetime);

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            float currentVelocityX = Mathf.Lerp(startVelocityX, targetVelocityX, t);
            float currentLifetime = Mathf.Lerp(startLifetime, targetLifetime, t);

            var tempVel = PS.velocityOverLifetime;
            tempVel.x = new ParticleSystem.MinMaxCurve(currentVelocityX);

            var tempBoost = boost.main;
            tempBoost.startLifetime = new ParticleSystem.MinMaxCurve(currentLifetime);

            yield return null;
        }

        // ������ ����
        var finalVel = PS.velocityOverLifetime;
        finalVel.x = new ParticleSystem.MinMaxCurve(targetVelocityX);

        var finalBoost = boost.main;
        finalBoost.startLifetime = new ParticleSystem.MinMaxCurve(targetLifetime);
    }
    private IEnumerator PlanetCrashSequence()
    {
        // �غ� �ܰ�
        planet.SetActive(true);
        spaceShip.SetActive(true);

        // ��ġ �ʱ�ȭ
        Vector3 planetStartPos = new Vector3(15f, 1f, 0f); // ������ �ٱ�
        Vector3 planetTargetPos = new Vector3(11.35f, 1f, 0f); // �߾�

        Vector3 shipStartPos = spaceShip.transform.position;
        Vector3 shipTargetPos = new Vector3(6.36f,1,0);
        Quaternion shipStartRot = spaceShip.transform.rotation;
        Quaternion shipTargetRot = Quaternion.Euler(0f, 0f, 200f); // 2���� ȸ��
        float totalRotation = 1440*1.4f; // z������ 720�� ȸ��

        float duration = 3f; // ��ü ���� �ð�
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // �༺ �̵�
            planet.transform.position = Vector3.Lerp(planetStartPos, planetTargetPos, t);

            float currentZRotation = Mathf.Lerp(0f, totalRotation, t);
            spaceShip.transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation);
            spaceShip.transform.position = Vector3.Lerp(shipStartPos, shipTargetPos, t); // �༺�� ���� �̵�

            yield return null;
        }

        StartCoroutine(ScaleBoomOverTime(boom.transform));
    }
    private IEnumerator ScaleBoomOverTime(Transform boom, float duration = 1f, float maxScale = 40f)
    {
        float time = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one * maxScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // �ε巯�� ��¡ (������ ���� �� ������ �� �ٽ� ������)
            t = Mathf.SmoothStep(0f, 1f, t);

            boom.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        boom.localScale = endScale; // ��Ȯ�� ����

        yield return new WaitForSeconds(1f);

        Loader.Load(Loader.Scene.GameScene);
    }
}
