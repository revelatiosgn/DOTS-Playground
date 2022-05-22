using System.Collections;
using System.Collections.Generic;
using GPUInstance;
using TMPro;
using UnityEngine;
using Unity.Entities;

public class DotsSceneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private List<GPUSkinnedMeshComponent> characters;

    [SerializeField] private float _dist = 10f;
    [SerializeField] private int _count = 10;
    [SerializeField] private int _countPerFrame = 10;
    [SerializeField] private double _globalTimeSpeed = 1.0;
    [SerializeField] private Camera _frustumCullingCamera;
    [SerializeField] private TMP_Text _currentText;

    private MeshInstancer m;
    private PathArrayHelper p;
    private int _current;

    private EntityManager _manager;
    private BlobAssetStore _blobAssetStore;
    private Entity _entityPrefab;

    private InstanceData<InstanceProperties>[] _dataArray;

    private static DotsSceneSpawner _instance;
    public static DotsSceneSpawner Instance => _instance;

    public GameObject BotPrefab => botPrefab;
    public List<GPUSkinnedMeshComponent> Characters => characters;
    public Camera FrustumCullingCamera => _frustumCullingCamera;

    private void Awake()
    {
        _instance = this;

        // _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        // _blobAssetStore = new BlobAssetStore();
        // GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
        // _entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(botPrefab, settings);
    }

    private void OnDestroy()
    {
        // _blobAssetStore.Dispose();
    }

    private void Start()
    {
        // StartCoroutine(SpawnCo(_count));
    }

    private IEnumerator SpawnCo(int count)
    {
        while (_current < count)
        {
            for (int i = 0; i < _countPerFrame; i++)
            {
                Entity newEntity = _manager.Instantiate(_entityPrefab);
                
                _manager.AddComponentData(newEntity, new MoveData { Speed = 1f });
                _manager.AddComponentData(newEntity, new AnimationData {});

                _current++;

                if (_current >= count)
                {
                    _currentText.text = _current.ToString();
                    yield break;
                }
            }

            _currentText.text = _current.ToString();

            yield return null;
        }
    }

    private void Update()
    {
        // for (int i = 0; i < _dataArray.Length; i++)
        // {
        //     var pos = _dataArray[i].position;
        //     pos.x += 1f * Time.deltaTime;
        //     _dataArray[i].position = pos;
        //     _dataArray[i].DirtyFlags = DirtyFlag.Position | DirtyFlag.Rotation;
        // }

        // m.AppendMany(_dataArray);


        // Ticks.GlobalTimeSpeed = this._globalTimeSpeed;
        // m.FrustumCamera = this._frustumCullingCamera;
        // m.Update(Time.deltaTime);
    }

    Vector3 GetRand(float d)
    {
        return new Vector3 (
            Random.Range(-d, d),
            0f,
            Random.Range(-d, d)
        );
    }
}
