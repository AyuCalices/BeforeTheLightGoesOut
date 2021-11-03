// GENERATED AUTOMATICALLY FROM 'Assets/Input/MazeInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MazeInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MazeInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MazeInput"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""0cfdd667-6ebe-476d-9e07-28e944901b50"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b40a3bf8-a7ee-4e9a-89e8-9ddb2d606f18"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""67f1b8d1-b783-49b2-9854-957b91b3a612"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""0f21bccf-deea-4046-992e-ca0aaf1fb4aa"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f0648ace-3b9b-4c75-b800-76538f991771"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""09d536cd-4048-4ebd-b46c-5e6082ec7832"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8179c19d-f37c-42be-b16d-33c6450731b6"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2085bd03-8000-49bb-a644-46a29015f5b5"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Actions"",
            ""id"": ""0dd6f048-a510-4111-ba97-48b5a734235b"",
            ""actions"": [
                {
                    ""name"": ""Hello"",
                    ""type"": ""Button"",
                    ""id"": ""458302a2-11f9-4827-a1ba-d70d15bfa46b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9607a234-982d-46ca-ab44-d02329f0be50"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hello"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Newaction = m_Movement.FindAction("New action", throwIfNotFound: true);
        // Actions
        m_Actions = asset.FindActionMap("Actions", throwIfNotFound: true);
        m_Actions_Hello = m_Actions.FindAction("Hello", throwIfNotFound: true);
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

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_Newaction;
    public struct MovementActions
    {
        private @MazeInput m_Wrapper;
        public MovementActions(@MazeInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Movement_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Actions
    private readonly InputActionMap m_Actions;
    private IActionsActions m_ActionsActionsCallbackInterface;
    private readonly InputAction m_Actions_Hello;
    public struct ActionsActions
    {
        private @MazeInput m_Wrapper;
        public ActionsActions(@MazeInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Hello => m_Wrapper.m_Actions_Hello;
        public InputActionMap Get() { return m_Wrapper.m_Actions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionsActions set) { return set.Get(); }
        public void SetCallbacks(IActionsActions instance)
        {
            if (m_Wrapper.m_ActionsActionsCallbackInterface != null)
            {
                @Hello.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnHello;
                @Hello.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnHello;
                @Hello.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnHello;
            }
            m_Wrapper.m_ActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Hello.started += instance.OnHello;
                @Hello.performed += instance.OnHello;
                @Hello.canceled += instance.OnHello;
            }
        }
    }
    public ActionsActions @Actions => new ActionsActions(this);
    public interface IMovementActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
    public interface IActionsActions
    {
        void OnHello(InputAction.CallbackContext context);
    }
}
