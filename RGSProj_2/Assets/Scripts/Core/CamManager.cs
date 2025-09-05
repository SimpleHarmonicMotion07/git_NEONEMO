using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

namespace Core
{
    /// <summary>
    /// Cinemachine Virtual Camera�� Basic Multi-Channel Perlin ����� ����Ͽ�
    /// ī�޶� ��鸲 ȿ���� ó���մϴ�.
    /// �ش� GameObject���� CinemachineVirtualCamera��
    /// CinemachineBasicMultiChannelPerlin ������Ʈ�� �־�� �մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class CamManager : MonoBehaviour
    {
    
        private CinemachineCamera virtualCam;
        private CinemachineBasicMultiChannelPerlin perlin;

        /// <summary>
        /// Awake ������ CinemachineVirtualCamera�� Perlin ������ ������Ʈ�� ĳ���մϴ�.
        /// </summary>
        private void Awake()
        {
            virtualCam = GetComponent<CinemachineCamera>();
            perlin=GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        /// <summary>
        /// ������ ����� ������, ���� �ð����� ī�޶� ��鸲�� �����մϴ�.
        /// </summary>
        /// <param name="amplitude">��鸲 �����Դϴ�.</param>
        /// <param name="frequency">��鸲 �������Դϴ�.</param>
        /// <param name="duration">��鸲 ���� �ð�(��)�Դϴ�.</param>
        public void Shake(float amplitude, float frequency, float duration)
        {
            if(perlin == null)
            {
                Debug.LogWarning("Shake ȣ�� �� Perlin ������ ������Ʈ�� null�Դϴ�.", this);
                return;
            }

            StopAllCoroutines(); // ���� ��鸲 ����
            StartCoroutine(ShakeCoroutine(amplitude, frequency, duration));
        }

        /// <summary>
        /// ������ �Ű������� ������ ��, ������ �ð� �Ŀ� ���� �ʱ�ȭ�ϴ� �ڷ�ƾ�Դϴ�.
        /// </summary>
        /// <param name="amplitude">������ ������ �����Դϴ�.</param>
        /// <param name="frequency">������ ������ �������Դϴ�.</param>
        /// <param name="duration">�ʱ�ȭ �� ��� �ð�(��)�Դϴ�.</param>
        /// <returns>�ڷ�ƾ�� IEnumerator ��ü�� ��ȯ�մϴ�.</returns>
        private IEnumerator ShakeCoroutine(float amplitude, float frequency, float duration)
        {
            perlin.AmplitudeGain = amplitude;
            perlin.FrequencyGain = frequency;

            yield return new WaitForSeconds(duration);

            perlin.AmplitudeGain = 0f;
            perlin.FrequencyGain = 0f;
        }
    }

}

