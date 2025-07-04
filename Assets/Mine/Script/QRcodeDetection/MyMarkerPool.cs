// MyMarkerPool.cs
using System.Collections.Generic;
using UnityEngine;

public class MyMarkerPool : MonoBehaviour
{
    [SerializeField] private GameObject markerPrefab;
    private Dictionary<string, MyMarkerController> markerMap = new Dictionary<string, MyMarkerController>();

    /// <summary>
    /// ��ȡ�Ѵ��ڻ��½��Ķ�ά���Ƕ���
    /// </summary>
    /// <param name="id">��ά��������ΪΨһ��ʶ</param>
    /// <returns>���õ� MyMarkerController ʵ��</returns>
    public MyMarkerController GetOrCreateMarker(string id)
    {
        if (!markerMap.ContainsKey(id))
        {
            GameObject markerObj = Instantiate(markerPrefab, transform);
            MyMarkerController marker = markerObj.GetComponent<MyMarkerController>();
            if (marker == null)
            {
                Debug.LogError("MarkerPrefab ��ȱ�� MyMarkerController �ű������");
                return null;
            }
            markerMap[id] = marker;
        }

        var resultMarker = markerMap[id];
        resultMarker.gameObject.SetActive(true);
        return resultMarker;
    }

    /// <summary>
    /// �������ж�ά����
    /// </summary>
    public void HideAll()
    {
        foreach (var kvp in markerMap)
        {
            if (kvp.Value != null)
                kvp.Value.gameObject.SetActive(false);
        }
    }
}
