using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _damagedText;
    [SerializeField]
    private Transform _golem;
    
    private Golem_Controller golem_Controller;
    [SerializeField]
    private TitanController titanController;

    [SerializeField]
    private Transform _camera;
    void Start()
    {

    }
    private void OnEnable()
    {
        golem_Controller = GetComponentInParent<Golem_Controller>();
    }


    void Update()
    {
        if (golem_Controller.onDamaged)
        {
            InstantiateDamagedUI();
        }

        if (titanController.onDamaged)
        {
            InstantiateDamagedUITitan();
        }
    }
    [SerializeField]
    private Vector3 _offset;
    private void InstantiateDamagedUI()
    {
        Instantiate(_damagedText, (golem_Controller.hitPosition + _offset), _camera.rotation, transform);
        Debug.Log($"damagedText Transform: {_damagedText.transform}");
    }
    private void InstantiateDamagedUITitan()
    {
        Instantiate(_damagedText, (titanController.hitPosition + _offset), _camera.rotation, transform);
        Debug.Log($"damagedText Transform: {_damagedText.transform}");
    }
}
