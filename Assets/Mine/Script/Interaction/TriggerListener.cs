using UnityEngine;

public class TriggerListener : MonoBehaviour
{
    public GenerateBitmask controller;

    public void Init(GenerateBitmask ctrl)
    {
        controller = ctrl;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (controller != null)
        {
            controller.OnTriggerChanged(other, true);
            //controller.OnCollisionLog($"?? ���봥��: {gameObject.name} -> {other.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (controller != null)
        {
            controller.OnTriggerChanged(other, false);
            //controller.OnCollisionLog($"?? �뿪����: {gameObject.name} -> {other.name}");
        }
    }

    // ��ѡ����ӳ����������
    private void OnTriggerStay(Collider other)
    {
        if (controller != null)
        {
            //controller.OnTriggerStayDetected(other);
        }
    }
}
