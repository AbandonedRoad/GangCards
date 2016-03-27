using UnityEngine;
using System.Collections;

public class TrackObject : MonoBehaviour
{
    public GameObject ObjectToFollow;

    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (ObjectToFollow != null)
        {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, ObjectToFollow.transform.position);
            Vector2 result;
            var result2 = RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, pos, Camera.main, out result);
            rt.position = result;
            Debug.Log(result2);
        }
    }
}
