using ProcessSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerProcessor : MonoBehaviour, IProcess
{
    [field: SerializeField] public bool Activated { get; set; } = true;


    [SerializeField] private float _slapInFaceCD = 2;
    [SerializeField] private float _useBoosterCD = 2;

    private InputProcessor _inputProcessor;
    private StatsProcessor _statsProcessor;
    private AnimatorProcessor _animatorProcessor;
    private NumbersUIController _numberUIController;

    public void AwakeMe()
    {
        _inputProcessor = GetComponent<InputProcessor>();
        _statsProcessor = GetComponent<StatsProcessor>();
        _animatorProcessor = GetComponent<AnimatorProcessor>();
        _numberUIController = FindObjectOfType<NumbersUIController>();
    }

    public void StartMe()
    {
        _inputProcessor.OnInputAction += OnInputAction;
    }

    public void UpdateMe()
    {

    }

    public void DestroyMe()
    {
        _inputProcessor.OnInputAction -= OnInputAction;

    }

    private void OnInputAction(InputType inputType)
    {
        print(inputType);
        switch (inputType)
        {
            //use booster
            case InputType.SwipeUp:
                _animatorProcessor.SetTrigger(_animatorProcessor.DRINK_BOOSTER_HASH);
                break;
            case InputType.SwipeDown:
                break;
            //slap in face
            case InputType.SwipeRight:
                _animatorProcessor.SetTrigger(_animatorProcessor.SLAP_IN_FACE_HASH);
                break;
            //slap in face
            case InputType.SwipeLeft:
                _animatorProcessor.SetTrigger(_animatorProcessor.SLAP_IN_FACE_HASH);
                break;
            //get CP
            case InputType.PointerDown:
                var damage = AddCP(out var isCrit);

                _numberUIController.EnableNumber(
                    isCrit ? NumberType.Crit : NumberType.Simple,
                    ((int)damage).ToString());

                CorrectWalkAnimation();
                //correct walk animation
                break;
            case InputType.PointerUp:
                break;
            case InputType.PointerClick:
                break;
            default:
                break;
        }
    }

    private float _lastProcSlapTime;
    private void SlapInFaceAction()
    {
        //check CD
        if (Time.time < _lastProcSlapTime + _slapInFaceCD)
            return;

        //start animation
        _animatorProcessor.SetTrigger(_animatorProcessor.SLAP_IN_FACE_HASH);

        //get CP
        _statsProcessor.ChangeStatPermanent(CharacterStats.CP, 200); //for test

        _lastProcSlapTime = Time.time;
    }

    private float _lastProcDrinkTime;
    private void UseBoosterAction()
    {
        //check CD
        if (Time.time < _lastProcDrinkTime + _useBoosterCD)
            return;

        //start animation
        _animatorProcessor.SetTrigger(_animatorProcessor.DRINK_BOOSTER_HASH);

        //get CP
        _statsProcessor.ChangeStatPermanent(CharacterStats.CP, 100); //for test

        _lastProcDrinkTime = Time.time;
    }

    private float AddCP(out bool isCrit)
    {
        isCrit = false;
        //get CP
        //check damage spread
        var min = _statsProcessor.Damage - _statsProcessor.DamageSpread * 2;
        var max = _statsProcessor.Damage;
        var damage = Random.Range(min, max);
        //check crit chance
        if (RandomFunctions.CheckChance((int)_statsProcessor.CritChance))
        {
            var critValue = RandomFunctions.GetPercentedValueFromNumber((int)_statsProcessor.Crit, damage);
            damage += critValue;
            isCrit = true;
        }

        _statsProcessor.ChangeStatPermanent(CharacterStats.CP, damage);
        return damage;
    }

    private void CorrectWalkAnimation()
    {
        var maxCP = 1500;
        var p = _statsProcessor.CP / maxCP;
        p *= 2; //привел к шкале 0 - 2

        _animatorProcessor.SetBlend(_animatorProcessor.WALK_BLEND_HASH, p);
    }


}
