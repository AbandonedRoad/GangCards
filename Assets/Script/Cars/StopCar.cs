using UnityEngine;
using System.Linq;
using Singleton;
using Cars;

public class StopCar : MonoBehaviour
{
    public bool PlayerOnly;

    private ActionContainer _container;

    void Start()
    {
        _container = this.GetComponent<ActionContainer>();
    }

    /// <summary>
    /// Stops a car.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Car" && other.gameObject.tag != "PlayersCar")
        {
            // Collision was made with something what is not car.
            return;
        }
        else if (other.gameObject.tag == "Car" && PlayerOnly)
        {
            // AI car was hit, but this is a Player Only WayPoint
            return;
        }

        var carScript = other.gameObject.GetComponent<MoveCars>();
        carScript.Speed = 0;

        if (_container != null && _container.Actions.Any())
        {
            PrefabSingleton.Instance.BuildingActionsHandler.SwitchBuildingActionsPanel(true);
            PrefabSingleton.Instance.BuildingActionsHandler.PassActions(_container);
        }
    }
}
