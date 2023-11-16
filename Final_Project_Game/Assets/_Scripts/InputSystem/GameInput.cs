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
        public static event Action<int> onPlayerRequestItem;
        public static event Action onPlayerPressInteract;
        public static event Action onPlayerShoot;
        public static event Action onPlayerStopShooting;
        #endregion
        private static GameInputSystem _gameInputSystem;

        public static void Init()
        {
            _gameInputSystem = new GameInputSystem();
            _gameInputSystem.Player.Enable();

            _gameInputSystem.Player.Move.performed += (InputAction.CallbackContext callbackContext) => onPlayerPressMoveVector2?.Invoke(callbackContext.ReadValue<Vector2>());
            _gameInputSystem.Player.Move.canceled += (InputAction.CallbackContext callbackContext) => onPlayerPressMoveVector2?.Invoke(callbackContext.ReadValue<Vector2>());

            _gameInputSystem.Player.Interact.performed += (InputAction.CallbackContext callbackContext) => onPlayerPressInteract?.Invoke();

            _gameInputSystem.Player.Inventory1.performed += (InputAction.CallbackContext callbackContext) => onPlayerRequestItem?.Invoke(0);
            _gameInputSystem.Player.Inventory2.performed += (InputAction.CallbackContext callbackContext) => onPlayerRequestItem?.Invoke(1);
            _gameInputSystem.Player.Inventory3.performed += (InputAction.CallbackContext callbackContext) => onPlayerRequestItem?.Invoke(2);
            _gameInputSystem.Player.Inventory4.performed += (InputAction.CallbackContext callbackContext) => onPlayerRequestItem?.Invoke(3);
            _gameInputSystem.Player.Inventory5.performed += (InputAction.CallbackContext callbackContext) => onPlayerRequestItem?.Invoke(4);
            _gameInputSystem.Player.Inventory6.performed += (InputAction.CallbackContext callbackContext) => onPlayerRequestItem?.Invoke(5);
            _gameInputSystem.Player.Inventory7.performed += (InputAction.CallbackContext callbackContext) => onPlayerRequestItem?.Invoke(6);
            _gameInputSystem.Player.Inventory8.performed += (InputAction.CallbackContext callbackContext) => onPlayerRequestItem?.Invoke(7);

            _gameInputSystem.Player.Shoot.performed += (InputAction.CallbackContext callbackContext) => onPlayerShoot?.Invoke();
            _gameInputSystem.Player.Shoot.canceled += (InputAction.CallbackContext callbackContext) => onPlayerStopShooting?.Invoke();
        }
    }

}
