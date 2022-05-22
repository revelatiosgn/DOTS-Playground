using System.Collections;
using System.Collections.Generic;
using GPUInstance;
using TMPro;
using UnityEngine;

public class AnimationSceneSpawner : MonoBehaviour
{
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

    private SkinnedMesh _skinnedMesh;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCo(_count));
    }

    private IEnumerator SpawnCo(int count)
    {
        // Initialize character mesh list
        int hierarchy_depth, skeleton_bone_count;
        var controllers = GPUSkinnedMeshComponent.PrepareControllers(characters, out hierarchy_depth, out skeleton_bone_count);

        // Initialize GPU Instancer
        this.m = new MeshInstancer();
        this.m.Initialize(max_parent_depth: hierarchy_depth + 2, num_skeleton_bones: skeleton_bone_count, pathCount: 2);
        this.p = new PathArrayHelper(this.m);

        // Add all animations to GPU buffer
        this.m.SetAllAnimations(controllers);

        // Add all character mesh types to GPU Instancer
        foreach (var character in this.characters)
            this.m.AddGPUSkinnedMeshType(character);

        string[] animNames = new string[] {
            "walk", 
            "Mutant Run", 
            "Mutant Breathing Idle", 
            "Mutant Punch", 
            "Mutant Walking"
        };

        _current = 0;

        while (_current < count)
        {
            for (int i = 0; i < _countPerFrame; i++)
            {
                var mesh = characters[Random.Range(0, characters.Count)];
                var animName = animNames[Random.Range(0, animNames.Length)];
                var anim = mesh.anim.namedAnimations[animName];
                // var anim = mesh.anim.namedAnimations["walk"];
                var skinnedMesh = new SkinnedMesh(mesh, this.m);
                skinnedMesh.mesh.position = GetRand(_dist);
                skinnedMesh.SetRadius(1.75f); // set large enough radius so model doesnt get culled to early
                skinnedMesh.Initialize();

                skinnedMesh.SetAnimation(anim, speed: 1.4f, start_time: Random.Range(0.0f, 1.0f)); // set animation

                // var path = GetNewPath(); // create new path
                // skinnedMesh.mesh.SetPath(path, this.m); // assign path to instance
                // paths[i, j] = path;

                skinnedMesh.UpdateAll();

                if (_current == 0)
                    _skinnedMesh = skinnedMesh;

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
        Vector3 pos = _skinnedMesh.mesh.position;
        pos.x += 1f * Time.deltaTime;
        _skinnedMesh.mesh.position = pos;
        _skinnedMesh.mesh.DirtyFlags = DirtyFlag.Position | DirtyFlag.Rotation;
        this.m.Append(ref _skinnedMesh.mesh);

        Ticks.GlobalTimeSpeed = this._globalTimeSpeed;
        this.m.FrustumCamera = this._frustumCullingCamera;
        this.m.Update(Time.deltaTime);
        
        // _skinnedMesh.UpdateMesh();
        // _skinnedMesh.UpdateRoot();
        // _skinnedMesh.UpdateAll();
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
