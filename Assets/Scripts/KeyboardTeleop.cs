//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Scripts/KeyboardTeleop.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @KeyboardTeleop : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @KeyboardTeleop()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""KeyboardTeleop"",
    ""maps"": [
        {
            ""name"": ""Common"",
            ""id"": ""1b3b4bfa-7c31-47f1-9314-4800c86b85e1"",
            ""actions"": [
                {
                    ""name"": ""ChangeActionSet"",
                    ""type"": ""Button"",
                    ""id"": ""a1012cb8-44bc-480d-98fb-2d9051388259"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0a0782ad-1b83-4161-b9dd-c6648c4b754b"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeActionSet"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""id"": ""6f09051f-0fea-4db3-bf33-a9862957dd6b"",
            ""actions"": [
                {
                    ""name"": ""GoToGoal"",
                    ""type"": ""Button"",
                    ""id"": ""b5d09e85-214d-4212-85f5-cb8ded627c4f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveRobotForward"",
                    ""type"": ""Button"",
                    ""id"": ""ca87562f-6081-495b-bd2f-4c885b6dadbb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TurnRobot"",
                    ""type"": ""Button"",
                    ""id"": ""24fbe219-b6d1-4e13-817a-c8e79a927a67"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TurnCam"",
                    ""type"": ""Button"",
                    ""id"": ""8e289044-3978-47a2-b28e-67672772a634"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""StopRobot"",
                    ""type"": ""Button"",
                    ""id"": ""cfa7caaa-8866-4796-a0a6-2ce3cfcab016"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TiltCam"",
                    ""type"": ""Button"",
                    ""id"": ""5c5fb9e9-b6f6-4de5-a424-c49113ae95ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""aafc0529-fed1-4e92-ade0-ec6cdfb6c5d5"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GoToGoal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""1ff4b28d-4de4-469c-86f7-2817f526b226"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRobotForward"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""73e3d57f-c9ac-47fa-96eb-e44fd4b7aeed"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRobotForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""d53fd7d9-90bf-4123-b543-11266aeee568"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRobotForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""ee2c7b94-254c-408e-a454-4e455bf23d83"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TurnRobot"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0f1d7a6a-4ee4-43b2-80ba-fead549ed6e4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TurnRobot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6216cef2-d32a-4aac-96d8-301fadde687f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TurnRobot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""c2183306-cf6a-4c1c-adaa-994e2cc8bfd5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TurnCam"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""439651d6-9046-4682-91e2-d27eb556ebfa"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TurnCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1f92140f-91b4-4b07-8922-ae0b89872217"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TurnCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f3831494-0104-40f1-a34f-9beafedee158"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StopRobot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""91e7ec9d-c3b6-4e52-8e14-69cf7156e996"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TiltCam"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""9a93c641-8cd6-41b3-af8a-e4a3861516ca"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TiltCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""e353eab0-f097-47ba-b169-1aa3700570be"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TiltCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Arm"",
            ""id"": ""ebe21060-b688-4947-8db4-71dd35398810"",
            ""actions"": [
                {
                    ""name"": ""FBLR"",
                    ""type"": ""Value"",
                    ""id"": ""ce197f90-40b2-426d-8b59-53382dcaf2a1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""UpDown"",
                    ""type"": ""Button"",
                    ""id"": ""8455f04a-2576-4dc5-844d-f4791a441461"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""adfeba32-4bd3-472b-947b-9825bf466d88"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FBLR"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6ae40d00-8410-4a3d-b33b-634d1eb7192a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FBLR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""664a10a0-ad4a-471e-83b1-f4521525bba0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FBLR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d22044d6-a013-435d-a7b7-122a432c73d8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FBLR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1ce8317c-43f2-4557-946a-1435d03cf4d4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FBLR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""18ac96af-2450-4cbd-9e5b-91365960e10f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpDown"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b4be864a-65f9-4d44-9c00-dbd43d065c99"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""48e54738-4884-4aa6-a9b8-d140b746e137"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Common
        m_Common = asset.FindActionMap("Common", throwIfNotFound: true);
        m_Common_ChangeActionSet = m_Common.FindAction("ChangeActionSet", throwIfNotFound: true);
        // Keyboard
        m_Keyboard = asset.FindActionMap("Keyboard", throwIfNotFound: true);
        m_Keyboard_GoToGoal = m_Keyboard.FindAction("GoToGoal", throwIfNotFound: true);
        m_Keyboard_MoveRobotForward = m_Keyboard.FindAction("MoveRobotForward", throwIfNotFound: true);
        m_Keyboard_TurnRobot = m_Keyboard.FindAction("TurnRobot", throwIfNotFound: true);
        m_Keyboard_TurnCam = m_Keyboard.FindAction("TurnCam", throwIfNotFound: true);
        m_Keyboard_StopRobot = m_Keyboard.FindAction("StopRobot", throwIfNotFound: true);
        m_Keyboard_TiltCam = m_Keyboard.FindAction("TiltCam", throwIfNotFound: true);
        // Arm
        m_Arm = asset.FindActionMap("Arm", throwIfNotFound: true);
        m_Arm_FBLR = m_Arm.FindAction("FBLR", throwIfNotFound: true);
        m_Arm_UpDown = m_Arm.FindAction("UpDown", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Common
    private readonly InputActionMap m_Common;
    private ICommonActions m_CommonActionsCallbackInterface;
    private readonly InputAction m_Common_ChangeActionSet;
    public struct CommonActions
    {
        private @KeyboardTeleop m_Wrapper;
        public CommonActions(@KeyboardTeleop wrapper) { m_Wrapper = wrapper; }
        public InputAction @ChangeActionSet => m_Wrapper.m_Common_ChangeActionSet;
        public InputActionMap Get() { return m_Wrapper.m_Common; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CommonActions set) { return set.Get(); }
        public void SetCallbacks(ICommonActions instance)
        {
            if (m_Wrapper.m_CommonActionsCallbackInterface != null)
            {
                @ChangeActionSet.started -= m_Wrapper.m_CommonActionsCallbackInterface.OnChangeActionSet;
                @ChangeActionSet.performed -= m_Wrapper.m_CommonActionsCallbackInterface.OnChangeActionSet;
                @ChangeActionSet.canceled -= m_Wrapper.m_CommonActionsCallbackInterface.OnChangeActionSet;
            }
            m_Wrapper.m_CommonActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ChangeActionSet.started += instance.OnChangeActionSet;
                @ChangeActionSet.performed += instance.OnChangeActionSet;
                @ChangeActionSet.canceled += instance.OnChangeActionSet;
            }
        }
    }
    public CommonActions @Common => new CommonActions(this);

    // Keyboard
    private readonly InputActionMap m_Keyboard;
    private IKeyboardActions m_KeyboardActionsCallbackInterface;
    private readonly InputAction m_Keyboard_GoToGoal;
    private readonly InputAction m_Keyboard_MoveRobotForward;
    private readonly InputAction m_Keyboard_TurnRobot;
    private readonly InputAction m_Keyboard_TurnCam;
    private readonly InputAction m_Keyboard_StopRobot;
    private readonly InputAction m_Keyboard_TiltCam;
    public struct KeyboardActions
    {
        private @KeyboardTeleop m_Wrapper;
        public KeyboardActions(@KeyboardTeleop wrapper) { m_Wrapper = wrapper; }
        public InputAction @GoToGoal => m_Wrapper.m_Keyboard_GoToGoal;
        public InputAction @MoveRobotForward => m_Wrapper.m_Keyboard_MoveRobotForward;
        public InputAction @TurnRobot => m_Wrapper.m_Keyboard_TurnRobot;
        public InputAction @TurnCam => m_Wrapper.m_Keyboard_TurnCam;
        public InputAction @StopRobot => m_Wrapper.m_Keyboard_StopRobot;
        public InputAction @TiltCam => m_Wrapper.m_Keyboard_TiltCam;
        public InputActionMap Get() { return m_Wrapper.m_Keyboard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardActions set) { return set.Get(); }
        public void SetCallbacks(IKeyboardActions instance)
        {
            if (m_Wrapper.m_KeyboardActionsCallbackInterface != null)
            {
                @GoToGoal.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnGoToGoal;
                @GoToGoal.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnGoToGoal;
                @GoToGoal.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnGoToGoal;
                @MoveRobotForward.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMoveRobotForward;
                @MoveRobotForward.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMoveRobotForward;
                @MoveRobotForward.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMoveRobotForward;
                @TurnRobot.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnTurnRobot;
                @TurnRobot.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnTurnRobot;
                @TurnRobot.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnTurnRobot;
                @TurnCam.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnTurnCam;
                @TurnCam.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnTurnCam;
                @TurnCam.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnTurnCam;
                @StopRobot.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnStopRobot;
                @StopRobot.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnStopRobot;
                @StopRobot.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnStopRobot;
                @TiltCam.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnTiltCam;
                @TiltCam.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnTiltCam;
                @TiltCam.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnTiltCam;
            }
            m_Wrapper.m_KeyboardActionsCallbackInterface = instance;
            if (instance != null)
            {
                @GoToGoal.started += instance.OnGoToGoal;
                @GoToGoal.performed += instance.OnGoToGoal;
                @GoToGoal.canceled += instance.OnGoToGoal;
                @MoveRobotForward.started += instance.OnMoveRobotForward;
                @MoveRobotForward.performed += instance.OnMoveRobotForward;
                @MoveRobotForward.canceled += instance.OnMoveRobotForward;
                @TurnRobot.started += instance.OnTurnRobot;
                @TurnRobot.performed += instance.OnTurnRobot;
                @TurnRobot.canceled += instance.OnTurnRobot;
                @TurnCam.started += instance.OnTurnCam;
                @TurnCam.performed += instance.OnTurnCam;
                @TurnCam.canceled += instance.OnTurnCam;
                @StopRobot.started += instance.OnStopRobot;
                @StopRobot.performed += instance.OnStopRobot;
                @StopRobot.canceled += instance.OnStopRobot;
                @TiltCam.started += instance.OnTiltCam;
                @TiltCam.performed += instance.OnTiltCam;
                @TiltCam.canceled += instance.OnTiltCam;
            }
        }
    }
    public KeyboardActions @Keyboard => new KeyboardActions(this);

    // Arm
    private readonly InputActionMap m_Arm;
    private IArmActions m_ArmActionsCallbackInterface;
    private readonly InputAction m_Arm_FBLR;
    private readonly InputAction m_Arm_UpDown;
    public struct ArmActions
    {
        private @KeyboardTeleop m_Wrapper;
        public ArmActions(@KeyboardTeleop wrapper) { m_Wrapper = wrapper; }
        public InputAction @FBLR => m_Wrapper.m_Arm_FBLR;
        public InputAction @UpDown => m_Wrapper.m_Arm_UpDown;
        public InputActionMap Get() { return m_Wrapper.m_Arm; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ArmActions set) { return set.Get(); }
        public void SetCallbacks(IArmActions instance)
        {
            if (m_Wrapper.m_ArmActionsCallbackInterface != null)
            {
                @FBLR.started -= m_Wrapper.m_ArmActionsCallbackInterface.OnFBLR;
                @FBLR.performed -= m_Wrapper.m_ArmActionsCallbackInterface.OnFBLR;
                @FBLR.canceled -= m_Wrapper.m_ArmActionsCallbackInterface.OnFBLR;
                @UpDown.started -= m_Wrapper.m_ArmActionsCallbackInterface.OnUpDown;
                @UpDown.performed -= m_Wrapper.m_ArmActionsCallbackInterface.OnUpDown;
                @UpDown.canceled -= m_Wrapper.m_ArmActionsCallbackInterface.OnUpDown;
            }
            m_Wrapper.m_ArmActionsCallbackInterface = instance;
            if (instance != null)
            {
                @FBLR.started += instance.OnFBLR;
                @FBLR.performed += instance.OnFBLR;
                @FBLR.canceled += instance.OnFBLR;
                @UpDown.started += instance.OnUpDown;
                @UpDown.performed += instance.OnUpDown;
                @UpDown.canceled += instance.OnUpDown;
            }
        }
    }
    public ArmActions @Arm => new ArmActions(this);
    public interface ICommonActions
    {
        void OnChangeActionSet(InputAction.CallbackContext context);
    }
    public interface IKeyboardActions
    {
        void OnGoToGoal(InputAction.CallbackContext context);
        void OnMoveRobotForward(InputAction.CallbackContext context);
        void OnTurnRobot(InputAction.CallbackContext context);
        void OnTurnCam(InputAction.CallbackContext context);
        void OnStopRobot(InputAction.CallbackContext context);
        void OnTiltCam(InputAction.CallbackContext context);
    }
    public interface IArmActions
    {
        void OnFBLR(InputAction.CallbackContext context);
        void OnUpDown(InputAction.CallbackContext context);
    }
}
