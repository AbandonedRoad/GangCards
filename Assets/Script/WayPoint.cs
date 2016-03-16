using UnityEngine;
using System.Collections;
using Enum;
using System.Linq;
using Singleton;

public class WayPoint : MonoBehaviour
{
    public Directions[] AvailableDirections;
    public bool PlayerOnly;

    private GameObject _planeRight;
    private GameObject _planeLeft;
    private GameObject _planeForward;

    // Use this for initialization
    void Start()
    {
        var arrows = this.GetComponentsInChildren<Transform>();

        if (arrows.Any(tr => tr.gameObject.name.Contains("Arrow")))
        {
            _planeRight = arrows.FirstOrDefault(tr => tr.gameObject.name == "ArrowRightPlane").gameObject;
            _planeLeft = arrows.FirstOrDefault(tr => tr.gameObject.name == "ArrowLeftPlane").gameObject;
            _planeForward = arrows.FirstOrDefault(tr => tr.gameObject.name == "ArrowForwardPlane").gameObject;

            SetArrows(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Occurs if the 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        var carScript = other.gameObject.GetComponent<MoveCars>();
        if (other.gameObject.tag == "Car" && PlayerOnly)
        {
            // AI car was hit, but this is a Player Only WayPoint
            return;
        }
        if (other.gameObject.tag == "PlayersCar")
        {
            // This is the Players Car.
            carScript.AllowExecute = false;
            carScript.AllowedDirections = AvailableDirections.ToList();

            if (_planeRight != null && _planeLeft != null && _planeForward != null)
            {
                _planeRight.SetActive(AvailableDirections.Contains(Directions.Right));
                _planeLeft.SetActive(AvailableDirections.Contains(Directions.Left));
                _planeForward.SetActive(AvailableDirections.Contains(Directions.Forward));
            }

            return;
        }
    }

    /// <summary>
    /// Trigger has been left.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        var carScript = other.gameObject.GetComponent<MoveCars>();
        if (other.gameObject.tag == "Car" && PlayerOnly)
        {
            // AI car was hit, but this is a Player Only WayPoint
            return;
        }
        if (other.gameObject.tag == "PlayersCar")
        {
            // This is the Players Car.
            PrefabSingleton.Instance.PlayersCarScript.SetCarBackOnTheRoad(other.gameObject.transform.position, other.gameObject.transform.rotation);
            carScript.AllowExecute = true;
            SetArrows(false);
            return;
        }
        
        var possibleDirs = AvailableDirections.Where(dir => dir != Directions.NotSet);
        if (possibleDirs.Any())
        {
            var newDirection = possibleDirs.ElementAt(Random.Range(0, possibleDirs.Count()));
            carScript.TranslateNewDirection(newDirection);
        }
    }

    /// <summary>
    /// Sets all Arrows active/inactive.
    /// </summary>
    /// <param name="active"></param>
    private void SetArrows(bool active)
    {
        _planeRight.SetActive(active);
        _planeLeft.SetActive(active);
        _planeForward.SetActive(active);
    }
}