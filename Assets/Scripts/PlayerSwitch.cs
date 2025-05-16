using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private GameObject[] _players;
    [SerializeField] private GameObject _currentPlayer;

    public void SpawnPlayer(int index) 
    {
        if (index >= _players.Length || index < 0) { return; }
        var spawnPosition = _currentPlayer ? _currentPlayer.transform.position : Vector3.zero;
        if (_currentPlayer) { Destroy(_currentPlayer); }

        _currentPlayer = Instantiate(_players[index], spawnPosition, Quaternion.identity);

        if (_camera) { _camera.Follow = _currentPlayer.transform; }
    }

}
