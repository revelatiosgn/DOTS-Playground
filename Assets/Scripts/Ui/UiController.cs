using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using UnityEngine.SceneManagement;

namespace Playground.Ui
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private Button _exit;
        [SerializeField] private Button _spawn;
        [SerializeField] private Button _clear;
        [SerializeField] private TMP_InputField _countInput;
        [SerializeField] private TMP_InputField _densityInput;
        [SerializeField] private TMP_InputField _botSpeedInput;
        [SerializeField] private TMP_Text _fpsText;
        [SerializeField] private TMP_Text _frameTimeText;
        [SerializeField] private BaseSpawner _baseSpawner;
        [SerializeField] private TMP_Dropdown _sceneDropdown;

        [SerializeField] private float _statsUpdateDelay = 5f;
        private float _statsUpdateTime = 0f;
        private int _statsUpdateFrames = 0;

        private void Awake()
        {
            _exit.onClick.AddListener(() => Application.Quit());
            _spawn.onClick.AddListener(SpawnEntities);
            _clear.onClick.AddListener(ClearEntities);

            _sceneDropdown.SetValueWithoutNotify(SceneManager.GetActiveScene().buildIndex);
        }
        
        private void OnDestroy()
        {
        }

        private void Update()
        {
            if (_statsUpdateTime < _statsUpdateDelay)
            {
                _statsUpdateTime += Time.deltaTime;
                _statsUpdateFrames++;
            }
            else
            {
                DisplayStats();
                _statsUpdateTime = 0f;
                _statsUpdateFrames = 0;
            }
        }

        private void SpawnEntities()
        {
            if (int.TryParse(_countInput.text, out int count) &&
                float.TryParse(_densityInput.text, out float density) &&
                float.TryParse(_botSpeedInput.text, out float speed))
            {
                if (count > 0 && density > 0f && speed >= 0f)
                {
                    BaseSpawner.SpawnSettings settings = new BaseSpawner.SpawnSettings {
                        Count = count,
                        Density = density,
                        BotSpeed = speed
                    };

                    Debug.Log($"Spawn {settings.Count} entities");
                    _baseSpawner.Spawn(settings);
                }
            }
        }

        private void ClearEntities()
        {
           _baseSpawner.Clear();
        }

        private void DisplayStats()
        {
            int frameTime = (int) (_statsUpdateTime * 1000f / (float) (_statsUpdateFrames));
            _frameTimeText.text = $"{frameTime} ms";
            int fps = (int) ((float) (_statsUpdateFrames) / _statsUpdateTime);
            _fpsText.text = $"{fps} fps";
        }

        public void ChangeScene(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}