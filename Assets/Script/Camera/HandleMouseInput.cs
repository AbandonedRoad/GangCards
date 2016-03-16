using UnityEngine;
using System.Collections;
using Singleton;

public class HandleMouseInput : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit result;
            if (Physics.Raycast(ray, out result))
            {
                if (result.transform.gameObject.tag == "Untagged")
                {
                    return;
                }

                var topMost = HelperSingleton.Instance.GetTopMostGO(result.transform.gameObject, true);
                HelperSingleton.Instance.SelectedObject = topMost;

                Debug.Log("Jop " + topMost.name);

                var container = HelperSingleton.Instance.SelectedObject.GetComponent<ActionContainer>();
                if (container != null)
                {
                    PrefabSingleton.Instance.ActionsHandler.PassActions(container);
                }
            }
            else
            {
                HelperSingleton.Instance.SelectedObject = null;
            }
        }


    }
}
