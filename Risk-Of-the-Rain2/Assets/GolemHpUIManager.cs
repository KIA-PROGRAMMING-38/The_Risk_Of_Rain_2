
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

using System.Threading;
using UniRx;
using System;
using UnityEngine.Animations;

public class GolemHpUIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _slider;

    [SerializeField]
    private GameObject _hpPart;
    [SerializeField]
    private Image _hpFill;

    [SerializeField]
    private Color initialColor;
    [SerializeField]
    private Color DamangedColor;

    private Slider sliderHP;
    private Golem_Controller _golemController;

    public float hpBarDuration;
    private bool isonDamaged;
    CancellationTokenSource _damageSouce = new();

    void Start()
    {
        _golemController = GetComponentInParent<Golem_Controller>();
        sliderHP = GetComponentInChildren<Slider>();

        _slider.SetActive(false);
        sliderHP.image.color = initialColor;
    }


    void Update()
    {

        if (_golemController.onDamaged)
        {
            isonDamaged = true;
            ShowHpBar();
        }

    }

    async private UniTaskVoid ShowHpBar()
    {
        _slider.SetActive(true);

        if (sliderHP != null)
        {
            _hpFill.color = DamangedColor;
            if (_golemController.hp != 0)
            {
                sliderHP.value = Utils.Percent(_golemController.HP, _golemController.maxHp);
                Debug.Log($"{_golemController.HP}");
            }
            if (_golemController.hp < 0)
            {
                _hpPart.SetActive(false);
            }

        }

        await UniTask.Delay((int)(hpBarDuration * 2000), cancellationToken: _damageSouce.Token);
        isonDamaged = false;
        if (_golemController.onDamaged == false && isonDamaged == false)
        {
            _slider.SetActive(false);
        }
        else
        {
            _damageSouce.Cancel();
        }

    }



}
