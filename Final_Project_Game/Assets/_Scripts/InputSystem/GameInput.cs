using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public static class GameInput 
    {
        #region Events
        public static event Action<Vector2> onPlayerPressMoveVector2;
        #endregion
        private static GameInputSystem _gameInputSystem;

        public static void Init()
        {
            _gameInputSystem = new GameInputSystem();
            _gameInputSystem.Player.Enable();

            _gameInputSystem.Player.Move.performed += (InputAction.CallbackContext callbackContext) => onPlayerPressMoveVector2?.Invoke(callbackContext.ReadValue<Vector2>());
            _gameInputSystem.Player.Move.canceled += (InputAction.CallbackContext callbackContext) => onPlayerPressMoveVector2?.Invoke(callbackContext.ReadValue<Vector2>());
        }
    }

}
