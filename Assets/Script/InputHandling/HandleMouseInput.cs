using UnityEngine;
using System.Collections;
using Singleton;
using Debugor;

namespace InputHandling
{
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

                    var actionContainer = HelperSingleton.Instance.SelectedObject.GetComponent<ActionContainer>();
                    if (actionContainer != null)
                    {
                        PrefabSingleton.Instance.ActionsHandler.PassActions(actionContainer);
                    }

                    var debugContainer = HelperSingleton.Instance.SelectedObject.GetComponent<IDebug>();
                    if (debugContainer != null)
                    {
                        debugContainer.Execute();
                    }
                }
                else
                {
                    HelperSingleton.Instance.SelectedObject = null;
                }
            }
        }
    }
}