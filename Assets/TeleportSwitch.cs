using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportSwitch : MonoBehaviour
{
    private XRRayInteractor _interactor;
    private LineRenderer _renderer;
    private XRController _controller;
    private XRInteractorLineVisual _line;
    void Start()
    {
        _interactor = GetComponent<XRRayInteractor>();
        _renderer = GetComponent<LineRenderer>();
        _controller = GetComponent<XRController>();
        _line = GetComponent<XRInteractorLineVisual>();
    }

    void Enable()
    {
        _interactor.enabled = true;
        _renderer.enabled = true;
        _controller.enabled = true;
        _line.enabled = true;
    }

    void Disable()
    {
        _interactor.enabled = false;
        _renderer.enabled = false;
        _controller.enabled = false;
        _line.enabled = false;
    }
}
