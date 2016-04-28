using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class WaitController : MonoBehaviour
{
    private float _oldSpeed;
    private float _waitDelay;
    private Coroutine _changeSpeed;
    void Awake()
    {
        StateController.WaitController = this;
        //StartCoroutine(Waiting(waitDelay));
    }

    void Update()
    {
    }

    /// <summary>
    /// Изменение скорости
    /// </summary>
    /// <param name="oldSpeed"></param>
    /// <param name="waitDelay"></param>
    public void ChangeSpeed(float oldSpeed, float waitDelay)
    {

        _waitDelay = waitDelay;
        // Если уже замедлена/ускорена змейка, тогда сбросить счётчик
        if (_changeSpeed != null)
        {
            StopCoroutine(_changeSpeed);
        }
        else
        {
            _oldSpeed = oldSpeed;
        }
        _changeSpeed = StartCoroutine(_changeSpeedCoroutine());
        print("ChangeSpeed");
    }


    private IEnumerator _changeSpeedCoroutine()
    {
        yield return new WaitForSeconds(_waitDelay);
        _changeSpeed = null;
        StateController.MoveSnakeState.Speed = _oldSpeed;
    }
}
