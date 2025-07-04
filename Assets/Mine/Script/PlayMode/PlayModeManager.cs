using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayModeManager : MonoBehaviour
{
    [Header("References")]
    public GameObject mainMenuUI;
    public GameObject blackScreenPanel;
    public GameObject tipText;
    public ArmSwingLocomotion armSwingScript;
    public Transform player;
    public Vector3 resetPosition = new Vector3(19f, 0f, -66f);

    private bool isRoamingMode = false;
    private bool waitingForAKey = false;

    void Update()
    {
        // �ȴ� A�� ��ʼ����
        if (waitingForAKey && OVRInput.GetDown(OVRInput.Button.One))  // A��
        {
            waitingForAKey = false;
            StartCoroutine(FadeFromBlackAndStartRoaming());
        }

        // Roaming ģʽ�а��� B�� �˳�
        if (isRoamingMode && OVRInput.GetDown(OVRInput.Button.Two))  // B��
        {
            StartCoroutine(ExitRoamingMode());
        }
    }

    public void EnterRoamingMode()
    {
        StartCoroutine(StartRoamingMode());
    }

    private IEnumerator StartRoamingMode()
    {
        // 1. �������˵�
        mainMenuUI.SetActive(false);

        // 2. ���� + ��ʾ
        blackScreenPanel.SetActive(true);
        tipText.SetActive(true);

        // 3. �ȴ� A��
        waitingForAKey = true;
        yield return null;
    }

    private IEnumerator FadeFromBlackAndStartRoaming()
    {
        // �رպ�������ʾ
        blackScreenPanel.SetActive(false);
        tipText.SetActive(false);

        // �����ƶ��ű�
        armSwingScript.enableMovement = true;
        isRoamingMode = true;

        yield return null;
    }

    private IEnumerator ExitRoamingMode()
    {
        // ���� + ֹͣ�ƶ�
        blackScreenPanel.SetActive(true);
        tipText.SetActive(false);

        yield return new WaitForSeconds(1f);

        // ����λ��
        player.position = resetPosition;

        armSwingScript.enableMovement = false;
        isRoamingMode = false;

        // ��ʾ���˵�
        mainMenuUI.SetActive(true);
        blackScreenPanel.SetActive(false);
    }
}
