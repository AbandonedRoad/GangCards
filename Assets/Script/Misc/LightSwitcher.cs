using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class LightSwitcher : MonoBehaviour
{
    private Light _rightFront;
    private Light _rightBack;
    private Light _leftFront;
    private Light _leftBack;
    private bool _isRight;
    private bool _isExecuting = true;
    private DateTime? _startAt;

    public float Speed;

    // Use this for initialization
    void Start ()
    {
        float wait = UnityEngine.Random.Range(1000f, 4000f);
        _startAt = DateTime.Now.AddMilliseconds(wait);

        _isRight = true;

        var lights = this.GetComponentsInChildren<Light>().ToList();

        _rightFront = lights.FirstOrDefault(lg => lg.gameObject.name == "RightLightFront");
        _rightBack = lights.FirstOrDefault(lg => lg.gameObject.name == "RightLightBack");
        _leftFront = lights.FirstOrDefault(lg => lg.gameObject.name == "LeftLightFront");
        _leftBack = lights.FirstOrDefault(lg => lg.gameObject.name == "LeftLightBack");

        _leftBack.gameObject.SetActive(false);
        _leftFront.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_startAt.HasValue && _startAt.Value < DateTime.Now)
        {
            _startAt = null;
            _isExecuting = false;
        }

        if (_isExecuting)
        {
            return;
        }

        StartCoroutine(WaitMore());
	}

    private IEnumerator WaitMore()
    {
        _isExecuting = true;

        yield return new WaitForSeconds(Speed);

        _leftBack.gameObject.SetActive(!_isRight);
        _leftFront.gameObject.SetActive(!_isRight);
        _rightBack.gameObject.SetActive(_isRight);
        _rightFront.gameObject.SetActive(_isRight);

        _isRight = !_isRight;

        _isExecuting = false;
    }
}
