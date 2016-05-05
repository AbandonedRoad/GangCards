using Enum;
using Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    [Serializable]
    public class SceneData
    {
        public List<GOInfo> GameObjects { get; set; }
        public List<IGangMember> GangMembers { get; set; }
        public float Money { get; set; }
        public Gangs GangOfPlayer { get; set; }

        public int ActualScene;

        private float[] _mainCameraPosition;
        private float[] _mainCameraRotation;
        private float[] _miniMapCameraPosition;
        private float[] _miniMapCameraRotation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game.SceneData"/> class.
        /// </summary>
        public SceneData()
        {
            GameObjects = new List<GOInfo>();
            GangMembers = new List<IGangMember>();
        }

        /// <summary>
        /// Gets the vector.
        /// </summary>
        /// <returns>The vector.</returns>
        public Vector3 GetVector(SceneVectorProperty property)
        {
            float[] var = null;

            switch (property)
            {
                case SceneVectorProperty.MainCameraPosition:
                    var = _mainCameraPosition;
                    break;
                case SceneVectorProperty.MainCameraRotation:
                    var = _mainCameraRotation;
                    break;
                case SceneVectorProperty.MiniMapCameraPosition:
                    var = _miniMapCameraPosition;
                    break;
                case SceneVectorProperty.MiniMapCameraRotation:
                    var = _miniMapCameraRotation;
                    break;

                default:
                    break;
            }

            return new Vector3(var[0], var[1], var[2]);
        }

        /// <summary>
        /// Sets the vector.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="vector">Vector.</param>
        public void SetVector(SceneVectorProperty property, Vector3 vector)
        {
            float[] var = new float[3] { vector.x, vector.y, vector.z };

            switch (property)
            {
                case SceneVectorProperty.MainCameraPosition:
                    _mainCameraPosition = var;
                    break;
                case SceneVectorProperty.MainCameraRotation:
                    _mainCameraRotation = var;
                    break;
                case SceneVectorProperty.MiniMapCameraPosition:
                    _miniMapCameraPosition = var;
                    break;
                case SceneVectorProperty.MiniMapCameraRotation:
                    _miniMapCameraRotation = var;
                    break;
                default:
                    break;
            }
        }
    }

    public enum SceneVectorProperty
    {
        MainCameraPosition,
        MainCameraRotation,
        MiniMapCameraPosition,
        MiniMapCameraRotation,
    }
}

