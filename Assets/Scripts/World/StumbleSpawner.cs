using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StumbleType
{
    Down,
    Middle
}

public class StumbleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _stumblePrefabs;

    [field: SerializeField] private float SpawnTick { get; set; } = 5;
    [field:Range(0, 100)]
    [field:SerializeField] private int SpawnChance { get; set; } = 30;
    [field:Range(0, 100)]
    [field:SerializeField] private int SpawnDownStumbleChance { get; set; } = 40;
    [SerializeField] private float _spawnOffset = 30;
    private int SpawnMiddleStumbleChance { get => 100 - SpawnDownStumbleChance; }

    private Coroutine _updateCoro;


    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<ClickerProcessor>().transform;
    }

    private void Start()
    {
        StartSpawn();
    }

    private void OnDestroy()
    {
        StopSpawn();
    }

    public void StartSpawn()
    {
        _updateCoro = StartCoroutine(ExtendCoroutines.CycleAction(SpawnTick, Tick));

    }

    public void StopSpawn()
    {
        if (_updateCoro != null)
            StopCoroutine(_updateCoro);
        _updateCoro = null;
    }

    private void Tick()
    {
        if (!RandomFunctions.CheckChance(SpawnChance))
            return;

        Transform p = default;
        if (RandomFunctions.CheckChance(SpawnDownStumbleChance))
            p = SpawnStumble(StumbleType.Middle);
        else
            p = SpawnStumble(StumbleType.Down);

        //set position
        p.localPosition = new Vector3(0, 0.6f, _playerTransform.position .z + _spawnOffset);
    }

    private Transform SpawnStumble(StumbleType type)
    {
        Transform p = default;
        switch (type)
        {
            case StumbleType.Down:
                p = Instantiate(_stumblePrefabs[0]).transform;
                break;
            case StumbleType.Middle:
                p = Instantiate(_stumblePrefabs[1]).transform;
                break;
            default:
                break;
        }

        return p.transform;
    }


}
