//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Input_ActionSet_base_movement p_base_movement;
        
        private static SteamVR_Input_ActionSet_arm_movement p_arm_movement;
        
        private static SteamVR_Input_ActionSet_common_controls p_common_controls;
        
        public static SteamVR_Input_ActionSet_base_movement base_movement
        {
            get
            {
                return SteamVR_Actions.p_base_movement.GetCopy<SteamVR_Input_ActionSet_base_movement>();
            }
        }
        
        public static SteamVR_Input_ActionSet_arm_movement arm_movement
        {
            get
            {
                return SteamVR_Actions.p_arm_movement.GetCopy<SteamVR_Input_ActionSet_arm_movement>();
            }
        }
        
        public static SteamVR_Input_ActionSet_common_controls common_controls
        {
            get
            {
                return SteamVR_Actions.p_common_controls.GetCopy<SteamVR_Input_ActionSet_common_controls>();
            }
        }
        
        private static void StartPreInitActionSets()
        {
            SteamVR_Actions.p_base_movement = ((SteamVR_Input_ActionSet_base_movement)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_base_movement>("/actions/base_movement")));
            SteamVR_Actions.p_arm_movement = ((SteamVR_Input_ActionSet_arm_movement)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_arm_movement>("/actions/arm_movement")));
            SteamVR_Actions.p_common_controls = ((SteamVR_Input_ActionSet_common_controls)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_common_controls>("/actions/common_controls")));
            Valve.VR.SteamVR_Input.actionSets = new Valve.VR.SteamVR_ActionSet[] {
                    SteamVR_Actions.base_movement,
                    SteamVR_Actions.arm_movement,
                    SteamVR_Actions.common_controls};
        }
    }
}
