using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Unity.Services.Mediation
{
    /// <summary>
    /// Mediation API for the Unity Mediation SDK.
    /// </summary>
    public static class MediationService
    {
        internal static MediationServiceImpl s_Instance;

        /// <summary>
        /// Single entry point to all Mediation service features.
        /// </summary>
        [Obsolete("Make sure to initialize with IronSource.Agent.init(appKey).")]
        public static IMediationService Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new MediationServiceImpl();
                }
                return s_Instance;
            }
        }

        /// <summary>
        /// The initialization state of the mediation sdk.
        /// </summary>
        [Obsolete("LevelPlay does not support check of initialization state. Make sure to initialize with IronSource.Agent.init(appKey).")]
        public static InitializationState InitializationState { get { return s_Instance == null ? (InitializationState.Uninitialized) : InitializationState.Initialized; } }

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void EditorReset()
        {
            s_Instance = null;
        }
#endif
    }
}
