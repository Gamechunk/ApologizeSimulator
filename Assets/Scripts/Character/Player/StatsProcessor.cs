using ProcessSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum CharacterStats
{
    CP,//confidence points
    CritChance,
    StumbledResistance,
    Damage,
    DamageSpread,
    AdditionalDamage,
}

public class StatsProcessor : MonoBehaviour, IProcess
{
    [field: SerializeField] public bool Activated { get; set; } = true;

    [field:Range(0, 1000)]
    [field: SerializeField] public float CP { get; private set; } = 0; //confidence points - очки уверенности

    [field: SerializeField] public float Crit { get; private set; } = 100; // crit value in percent

    [field:Range(0, 100)]
    [field: SerializeField] public float CritChance { get; private set; } = 10; // crit chance when clicking to get CP in percent

    [field:Range(0, 100)]
    [field: SerializeField] public float StumbledResistance { get; private set; } = 10; // resistance when meet stumble in percent

    [field: SerializeField] public float Damage { get; private set; } = 10; // base damage
    [field: SerializeField] public float DamageSpread { get; private set; } = 2; // base damage spread - разброс урона

    [field: SerializeField] public float AdditionalDamage { get; private set; } = 0; // additional damage




    //Here stored all changes with parameters
    private Dictionary<CharacterStats, Dictionary<GUID, StatDelta>> _deltaParametersStore = new Dictionary<CharacterStats, Dictionary<GUID, StatDelta>>();


    public void AwakeMe()
    {
    }

    public void StartMe()
    {
    }

    public void UpdateMe()
    {
    }

    public void DestroyMe()
    {
        _deltaParametersStore.Clear();
    }

    public void ChangeStatPermanent(CharacterStats parameter, float delta)
    {
        switch (parameter)
        {
            case CharacterStats.CP:
                CP += delta;
                break;
            case CharacterStats.CritChance:
                CritChance += delta;
                break;
            case CharacterStats.StumbledResistance:
                StumbledResistance += delta;
                break;
            case CharacterStats.Damage:
                Damage += delta;
                break;
            case CharacterStats.DamageSpread:
                DamageSpread += delta;
                break;
            case CharacterStats.AdditionalDamage:
                AdditionalDamage += delta;
                break;
            default:
                break;
        }
    }

    public GUID ChangeStat(CharacterStats parameter, float delta)
    {
        var guid = GUID.Generate();
        var stat = new StatDelta() { delta = delta, parameter = parameter };
        //create stroke of dont contains
        if (!_deltaParametersStore.TryGetValue(parameter, out var dict))
        {
            var d = new Dictionary<GUID, StatDelta>();

            d.Add(guid, stat);
            _deltaParametersStore.Add(parameter, d);
        }
        else
            dict.Add(guid, stat);

        ChangeStatPermanent(parameter, stat.EnableDelta());

        return guid;
    }

    public void ReturnStat(CharacterStats parameter, GUID guid)
    {
        if (_deltaParametersStore.TryGetValue(parameter, out var dict))
        {
            if (dict.TryGetValue(guid, out var statDelta))
            {
                ChangeStatPermanent(parameter, statDelta.DisableDelta());
                dict.Remove(guid);
            }
        }
    }

    public class StatDelta
    {
        public float delta;
        public CharacterStats parameter;

        public float EnableDelta()
        {
            return delta;
        }

        public float DisableDelta()
        {
            return -delta;
        }
    }
}

