using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class UserControl : MonoBehaviour
{
    private Touch _touch;
    private Camera _mainCamera;

    private List<Planets.Base> _selectablePlanets;
    private Planets.Base _destination=null;
    public UnityEvent onSelectPlanet;
    public event Action<Planets.Base> cancelSelect;
    public event Action<Planets.Base> launchUnit;
    
    public void Start()
    {
        _mainCamera=Camera.main;
        if (_mainCamera == null)
            throw new MyException("can't get camera: " + name);
        _selectablePlanets = new List<Planets.Base>();
        cancelSelect += DecreaseScale;
        cancelSelect += RemoveFromList;
        launchUnit = cancelSelect;
    }

    public void Update()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            HandleTouch();
        }
    }

    private void HandleTouch()
    {
        switch (_touch.phase)   
        {
            case TouchPhase.Began:
            {
                HandleClick();
                break;
            }
            case TouchPhase.Ended:
            {
                HandleRelease();
                break;
            }
            case TouchPhase.Moved:
            {
                HandleMultipleSelection();
                break;
            }
        }
        
    }

    private void HandleClick()
    {
        var planet = RaycastForPlanet();
        if (planet != null)
        {
            if (_selectablePlanets.Contains(planet)==false)
            {
                planet.transform.localScale *= 1.5f;
                _selectablePlanets.Add(planet);
            }
        }
    }

    private void HandleRelease()
    {
        var planet = RaycastForPlanet();
        if (planet == null)
        {
            CancelSelection();
        }
        int count = _selectablePlanets.Count;
        if (count > 1)
        {
            _destination = _selectablePlanets[count - 1];
            _selectablePlanets.RemoveAt(count-1);
            LaunchToDestination(_destination);
            _destination.transform.localScale /= 1.5f;
        }
    }

    private void CancelSelection()
    {
        foreach (var planet in _selectablePlanets)
        {
            cancelSelect?.Invoke(planet);
            //planet.transform.localScale /= 1.5f;
        }
        //_selectablePlanets.Clear();
    }

    private Planets.Base RaycastForPlanet()
    {
        var ray = _mainCamera.ScreenPointToRay(_touch.position);
        return Physics.Raycast(ray, out var hit) ? hit.collider.GetComponentInParent<Planets.Base>() : null;
    }
    
    private void HandleMultipleSelection()
    {
        var planet = RaycastForPlanet();
        if (planet != null)
        {
            if (_selectablePlanets.Contains(planet))
            {
                _selectablePlanets.Remove(planet);
            }
            else
            {
                planet.transform.localScale *= 1.5f;
            }
            _selectablePlanets.Add(planet);
        }
    }
    
    private void LaunchToDestination(Planets.Base destination)
    {
        foreach (var planet in _selectablePlanets)
        {
            planet.LaunchUnit(destination);
            planet.transform.localScale /= 1.5f;
        }

        _selectablePlanets.Clear();
    }

    private void DecreaseScale(Planets.Base unit)
    {
        unit.transform.localScale /= 1.5f;
    }

    private void RemoveFromList(Planets.Base unit)
    {
        _selectablePlanets.Remove(unit);
    }
    
    //handle ui selection
    //handle skill selection
    
    
    
}