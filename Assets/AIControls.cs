using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControls : MonoBehaviour
{
    enum AIState
    {
        PickingAngle,
        Angling,
        Pausing
    }

    // Angling state
    float _currentAngle = 0f;
    float _targetAngle = 0f;
    float _rotationSpeed = 40f;
    float _divergenceThreshold = 0.1f;

    // Pausing state
    float _pauseDuration = 3f;
    float _pauseTimeElapsed = 0f;

    AIState _state = AIState.Pausing;

    public Tent tent;


    void Update()
    {
        if (_state == AIState.PickingAngle)
        {
            _targetAngle = PickAngle();
            _state = AIState.Angling;
            return;
        }

        if (_state == AIState.Angling)
        {
            if (Mathf.Abs(_currentAngle - _targetAngle) > _divergenceThreshold)
            {
                float polarity = _currentAngle > _targetAngle ? -1 : 1;
                _currentAngle += polarity * _rotationSpeed * Time.deltaTime;
                tent.AISetAngle(_currentAngle);
            }
            else
            {
                List<int> useableAmmo = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    if (tent.playerAmmo.AmmoAtIndexFireable(i))
                    {
                        useableAmmo.Add(i);
                    }
                }

                if (useableAmmo.Count > 0)
                {
                    tent.SelectAmmo(Random.Range(0, useableAmmo.Count));
                    tent.BeginChargingAttack();
                    tent.FireChargedAttack();

                    _state = AIState.Pausing;
                }
            }

            return;
        }

        if (_state == AIState.Pausing)
        {
            _pauseTimeElapsed += Time.deltaTime;

            if (_pauseTimeElapsed >= _pauseDuration)
            {
                _pauseTimeElapsed = 0f;
                _state = AIState.PickingAngle;
            }

            return;
        }
    }

    float PickAngle()
    {
        return Random.Range(10f, 50f);
    }
}
