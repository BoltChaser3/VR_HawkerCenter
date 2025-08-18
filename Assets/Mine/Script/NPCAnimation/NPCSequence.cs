using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TryAR.MarkerTracking;

public class NPCSequence : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;

    public Transform servePoint;
    public Transform pickupPoint;
    public GameObject tray;
    public GameObject paper;

    [HideInInspector]
    public bool hasServed = false;
    private bool isServing = false;

    void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
        animator.applyRootMotion = true;
    }

    public void StartServing()
    {   
        paper.SetActive(false);
        if (isServing) return;
        StartCoroutine(ServeRoutine());
    }

    private IEnumerator ServeRoutine()
    {
        isServing = true;
        hasServed = false;

        FindObjectOfType<ArUcoTrackingAppCoordinator>().ForceHideMarkers();

        Vector3 direction = (pickupPoint.position - transform.position).normalized;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // ��ǰ����
        Quaternion startRotation = transform.rotation;

        // ���š��Ӿ�ת������
        animator.SetTrigger("Turn");

        // �ڶ�������ʱ������֡��ֵת��Ŀ�귽��
        float turnDuration = 1.19f; // ���ݶ�����������
        float elapsed = 0f;
        while (elapsed < turnDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / turnDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ȷ�����շ���׼ȷ
        transform.rotation = targetRotation;

        // �ȴ������������
        yield return StartCoroutine(WaitForAnimation("Turn"));

        agent.SetDestination(pickupPoint.position);
        animator.SetBool("Walking", true);
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance + 1f)
            yield return null;
        animator.SetBool("Walking", false);

        // ȡ�˶���
        animator.SetTrigger("PickUp");
        yield return StartCoroutine(WaitForAnimation("PickUp"));


        // �ٴ�ת������ servePoint�����Ȳ��Ŷ�����Ȼ��ƽ��ת��
        Vector3 serveDirection = (servePoint.position - transform.position).normalized;
        serveDirection.y = 0f;
        Quaternion serveTargetRotation = Quaternion.LookRotation(serveDirection);
        Quaternion serveStartRotation = transform.rotation;

        animator.SetTrigger("Turn"); // ������ͬ��ת�����������������һ��������
        float turnBackDuration = 1.19f; // ���ݶ�����������һ��
        float elapsedBack = 0f;
        while (elapsedBack < turnBackDuration)
        {
            transform.rotation = Quaternion.Slerp(serveStartRotation, serveTargetRotation, elapsedBack / turnBackDuration);
            elapsedBack += Time.deltaTime;
            yield return null;
        }
        transform.rotation = serveTargetRotation;

        tray.SetActive(true);

        // �����ƶ��������
        agent.SetDestination(servePoint.position);
        animator.SetBool("IsHoldingTray", true);
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance + 1f)
            yield return null;
        animator.SetBool("IsHoldingTray", false);
        animator.ResetTrigger("Turn");
        // ���¶���
        animator.SetTrigger("Place");
        yield return StartCoroutine(WaitForAnimation("Place"));

        tray.SetActive(false);
        paper.SetActive(true);

        hasServed = true;
        isServing = false;
    }

    private IEnumerator WaitForAnimation(string stateName)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;
    }

    void OnAnimatorMove()
    {
        if (agent.enabled)
        {
            agent.nextPosition = animator.rootPosition;
            transform.position = agent.nextPosition;
            transform.rotation = animator.rootRotation;
        }
    }
}
