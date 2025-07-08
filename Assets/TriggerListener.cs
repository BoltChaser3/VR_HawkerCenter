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
        Debug.Log($"[���������] {gameObject.name} ���� {other.name}");
        controller?.OnTriggerChanged(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        controller?.OnTriggerChanged(other, false);
    }
}
