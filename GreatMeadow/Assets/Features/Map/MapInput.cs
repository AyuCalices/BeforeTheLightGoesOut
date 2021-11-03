// GENERATED AUTOMATICALLY FROM 'Assets/Input/MapInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MapInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MapInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MapInput"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""164203bf-936b-4850-beaf-0dbd21495568"",
            ""actions"": [
                {
                    ""name"": ""Drawing"",
                    ""type"": ""PassThrough"",
                    ""id"": ""75f58e7e-f666-49ed-89bf-7aaf7d32d223"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c4823f5d-f449-4bcc-83c5-a256f3c253b3"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drawing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_Drawing = m_Mouse.FindAction("Drawing", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Mouse
    private readonly InputActionMap m_Mouse;
    private IMouseActions m_MouseActionsCallbackInterface;
    private readonly InputAction m_Mouse_Drawing;
    public struct MouseActions
    {
        private @MapInput m_Wrapper;
        public MouseActions(@MapInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Drawing => m_Wrapper.m_Mouse_Drawing;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @Drawing.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnDrawing;
                @Drawing.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnDrawing;
                @Drawing.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnDrawing;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Drawing.started += instance.OnDrawing;
                @Drawing.performed += instance.OnDrawing;
                @Drawing.canceled += instance.OnDrawing;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);
    public interface IMouseActions
    {
        void OnDrawing(InputAction.CallbackContext context);
    }
}
