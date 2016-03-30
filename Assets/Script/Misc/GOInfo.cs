using Enum;
using System;
using UnityEngine;

namespace Misc
{
    [Serializable]
    public class GOInfo
    {
        public string PrefabName { get; set; }
        public GOType GOType { get; set; }
        public string PrefabRoot { get { return GetPrefabRoot(); } }

        float[] _rotation = new float[3];
        private float[] _position = new float[3];

        /// <summary>
        /// Initializes a new instance of the <see cref="Game.GOInfo"/> class.
        /// </summary>
        public GOInfo()
        {}

        /// <summary>
        /// Gets the vector.
        /// </summary>
        /// <returns>The vector.</returns>
        public Vector3 GetVector(GOVectorProperty property)
        {
            float[] var = null;

            switch (property)
            {
                case GOVectorProperty.Rotation:
                    var = _rotation;
                    break;
                case GOVectorProperty.Position:
                    var = _position;
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
        public void SetVector(GOVectorProperty property, Vector3 vector)
        {
            float[] var = new float[3] { vector.x, vector.y, vector.z };

            switch (property)
            {
                case GOVectorProperty.Rotation:
                    _rotation = var;
                    break;
                case GOVectorProperty.Position:
                    _position = var;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Evaluates root
        /// </summary>
        /// <returns></returns>
        private string GetPrefabRoot()
        {
            string result = String.Empty;
            switch (GOType)
            {
                case GOType.Car:
                    result = @"\Prefabs\Cars";
                    break;
                default:
                    Debug.LogError("Type " + GOType.ToString() + " unknown");
                    break;
            }

            return result;
        }
    }

    public enum GOVectorProperty
    {
        Rotation,
        Position
    }
}