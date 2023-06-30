using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class DamageUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _damagedText;
    [SerializeField]
    private GameObject _damagedTextUltimate;
    [SerializeField]
    private Transform _golem;

    [SerializeField]
    private Golem_Controller golem_Controller;
    [SerializeField]
    private TitanController titanController;

    [SerializeField]
    private Transform _camera;

    [SerializeField]
    private Transform _player;
    void Start()
    {

    }
    private void OnEnable()
    {
        golem_Controller = GetComponentInParent<Golem_Controller>();
    }

    private bool isInstantiating;
    
    void Update()
    {
        if (golem_Controller.onDamaged)
        {
            if(isInstantiating==false)
            InstantiateDamagedUI();
        }

        if (titanController.onDamaged)
        {
           
               // InstantiateDamagedUITitan();
        }
    }
 
    [SerializeField]
    public float _offsetQuantityTitan;
    [SerializeField]
    public float _offsetQuantityGolem;
    private void InstantiateDamagedUI()
    {
        isInstantiating = true;
        Instantiate(_damagedText, (golem_Controller.hitPosition + _offsetQuantityGolem* ((golem_Controller.hitPosition - _player.position).normalized)), _camera.rotation, transform);
        Debug.Log($"damagedText Transform: {_damagedText.transform}");
        isInstantiating = false;
    }
    //async private UniTaskVoid InstantiateDamagedUITitan()
    //{
    //    if (isInstantiating == false)
    //    {
    //        isInstantiating = true;
    //        Instantiate(_damagedTextUltimate, (titanController.hitPosition + +_offsetQuantityTitan * ((golem_Controller.hitPosition - _player.position).normalized)), _camera.rotation, transform);
    //        Debug.Log($"damagedText Transform: {_damagedText.transform}");
    //        await UniTask.Delay(150);
    //        isInstantiating = false;
    //    }
         
    //}
}
