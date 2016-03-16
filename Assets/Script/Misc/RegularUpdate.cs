using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RegularUpdate : MonoBehaviour
{
    private Dictionary<int, Action> _listeners;

    void Start()
    {
        _listeners = new Dictionary<int, Action>();
    }

	// Update is called once per frame
	void Update ()
    {
        foreach (var pair in _listeners)
        {
            pair.Value.Invoke();
        }
	}

    /// <summary>
    /// Add to List
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="newAction"></param>
    public void AddListenener(object sender, Action newAction)
    {
        if (!_listeners.ContainsKey(sender.GetHashCode()))
        {
            _listeners.Add(sender.GetHashCode(), newAction);
        }        
    }

    /// <summary>
    /// Remove from list
    /// </summary>
    /// <param name="sender"></param>
    public void RemoveListenener(object sender)
    {
        var hash = sender.GetHashCode();
        if (_listeners.ContainsKey(hash))
        {
            _listeners.Remove(hash);
        }
    }
}