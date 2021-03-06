﻿using UnityEngine;
using System.Collections;
using Enum;
using System.Collections.Generic;
using System;

namespace Cars
{
    public class MoveCars : MonoBehaviour
    {
        private Vector3 _lastPlayerCarPosition;
        private Quaternion _lastPlayerCarRotation;
        private bool _isPlayersCar;
        private bool _isTurning;
        private Vector3 _newDirectionRotation;
        private float _startSpeed;

        public float Speed;
        public bool AllowExecute { get; set; }
        public List<Directions> AllowedDirections { get; set; }
        public Directions NewDirection { get; private set; }
        public int BodyCountInCar { get; set; }

        /// <summary>
        /// Starts the instance
        /// </summary>
        void Start()
        {
            _startSpeed = Speed;

            _newDirectionRotation = this.transform.rotation.eulerAngles;
            _isPlayersCar = this.gameObject.tag == "PlayersCar";
            NewDirection = Directions.Forward;
            AllowedDirections = new List<Directions>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0)
            {
                return;
            }

            this.transform.position = this.transform.position + (transform.forward * Speed);

            if (Speed > 0)
            {
                MoveToNewDirection();
            }
        }

        /// <summary>
        /// Translate desired direction
        /// </summary>
        /// <param name="newDirection"></param>
        public void TranslateNewDirection(Directions newDirection)
        {
            if (_isTurning || (!AllowedDirections.Contains(newDirection) && _isPlayersCar))
            {
                // Car is already turing or we are the players car and the desired direction is not alllowed - do not take a new request for a turn
                return;
            }

            NewDirection = newDirection;
            var actRot = transform.rotation.eulerAngles;
            _newDirectionRotation = actRot;
            switch (newDirection)
            {
                case Directions.Left:
                    _newDirectionRotation = new Vector3(actRot.x, actRot.y - 90, actRot.z);
                    break;
                case Directions.Right:
                    _newDirectionRotation = new Vector3(actRot.x, actRot.y + 90, actRot.z);
                    break;
                case Directions.Forward:
                    break;
                case Directions.TurnAround:
                    _newDirectionRotation = new Vector3(actRot.x, actRot.y + 180, actRot.z);
                    break;
                default:
                    break;
            }

            if (_newDirectionRotation.y > 360)
            {
                _newDirectionRotation = new Vector3(_newDirectionRotation.x, _newDirectionRotation.y - 360, _newDirectionRotation.z);
            }
            else if (_newDirectionRotation.y < 0)
            {
                _newDirectionRotation = new Vector3(_newDirectionRotation.x, _newDirectionRotation.y + 360, _newDirectionRotation.z);
            }

            AllowedDirections.Clear();
        }

        /// <summary>
        /// Sets the car back on the road.
        /// </summary>
        public void SetCarBackOnTheRoad(Vector3? position = null, Quaternion? rotation = null)
        {
            if (position.HasValue && rotation.HasValue)
            {
                _lastPlayerCarPosition = position.Value;
                _lastPlayerCarRotation = rotation.Value;
            }
            else
            {
                this.transform.position = _lastPlayerCarPosition;
                this.transform.rotation = _lastPlayerCarRotation;

                NewDirection = Directions.Forward;
                _newDirectionRotation = transform.rotation.eulerAngles;
            }
        }

        /// <summary>
        /// Sets the car back on the road, but car is turned and is now facing opposite direction
        /// </summary>
        public void SetCarBackOnTheRoadWithTurn()
        {
            float newY = 0;
            var moveFactor = 4.3f;
            switch ((int)Math.Abs(_lastPlayerCarRotation.eulerAngles.y))
            {
                case 0:
                    newY = 180;
                    _lastPlayerCarPosition = new Vector3(_lastPlayerCarPosition.x - moveFactor, _lastPlayerCarPosition.y, _lastPlayerCarPosition.z);
                    break;
                case 90:
                    newY = 270;
                    _lastPlayerCarPosition = new Vector3(_lastPlayerCarPosition.x, _lastPlayerCarPosition.y, _lastPlayerCarPosition.z - moveFactor);
                    break;
                case 180:
                    newY = 0;
                    _lastPlayerCarPosition = new Vector3(_lastPlayerCarPosition.x + moveFactor, _lastPlayerCarPosition.y, _lastPlayerCarPosition.z);
                    break;
                case 270:
                    newY = 90;
                    _lastPlayerCarPosition = new Vector3(_lastPlayerCarPosition.x, _lastPlayerCarPosition.y, _lastPlayerCarPosition.z + moveFactor);
                    break;
                default:
                    Debug.LogError("Rotation is unknown!");
                    break;
            }

            this.transform.position = _lastPlayerCarPosition;
            this.transform.rotation = Quaternion.Euler(new Vector3(_lastPlayerCarRotation.eulerAngles.x, newY, _lastPlayerCarRotation.eulerAngles.z));

            NewDirection = Directions.Forward;
            _newDirectionRotation = transform.rotation.eulerAngles;
        }

        /// <summary>
        /// Move car to next direciton.
        /// </summary>
        private void MoveToNewDirection()
        {
            if (_isPlayersCar && !AllowExecute)
            {
                // Only allow the execute of the direction change in a certain time spot, if this is the players car.
                return;
            }

            var difference = Mathf.Abs(this.transform.rotation.eulerAngles.y - _newDirectionRotation.y);
            if (difference < 2)
            {
                _isTurning = false;
                this.transform.rotation = Quaternion.Euler(_newDirectionRotation);
                Speed = 0.2f;
                AllowExecute = false;
                NewDirection = Directions.Forward;
                return;
            }
            else
            {
                _isTurning = true;
                Speed = NewDirection == Directions.Left ? 0.15f : 0.05f;
                var damping = 2;
                var rotation = Quaternion.Euler(Vector3.zero);
                rotation *= Quaternion.Euler(0, _newDirectionRotation.y, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
        }
    }
}