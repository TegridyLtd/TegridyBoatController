/////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2021 Tegridy Ltd                                          //
// Author: Darren Braviner                                                 //
// Contact: db@tegridygames.co.uk                                          //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// This program is free software; you can redistribute it and/or modify    //
// it under the terms of the GNU General Public License as published by    //
// the Free Software Foundation; either version 2 of the License, or       //
// (at your option) any later version.                                     //
//                                                                         //
// This program is distributed in the hope that it will be useful,         //
// but WITHOUT ANY WARRANTY.                                               //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// You should have received a copy of the GNU General Public License       //
// along with this program; if not, write to the Free Software             //
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,              //
// MA 02110-1301 USA                                                       //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////

using UnityEngine;
namespace Tegridy.Boat
{
    public class TegridyBoat : MonoBehaviour
    {
        [Header("Rudder Config")]
        public Transform[] rudder;
        public float rudderMin = -90f;
        public float rudderMax = 90f;
        public float rudderChange = 0.9f;
        public float rudderReturn = 0.02f;

        [Header("Proppeller Config")]
        public Transform[] propellers;
        public float maxRPM = 600.0F;
        public float acceleration = 0.001F;
        public float propellerForce = 0.6F;

        [Header("Fuel/Eficiency")]
        public float fuel;
        public float fuelConsumption;
        public float drag = 0.0001F;

        [Header("Audio")]
        public AudioClip engineSound; 

        [Header("Info")]
        public float rpm;
        public float throttle;
        public int direction = 1;
        public float angle;

        [HideInInspector] public Rigidbody rb;
        AudioSource audioSource;
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.clip = engineSound;
            audioSource.loop = true;
            audioSource.Play();
            rpm = 0F;
            throttle = 0F;
        }
        void Update()
        {
            if (fuel > 0)
            {
                //find out how fast we are pushing
                float frame_rpm = rpm * Time.deltaTime;

                //add force at prop position
                for (int i = 0; i < propellers.Length; i++)
                {
                    propellers[i].localRotation = Quaternion.Euler(propellers[i].localRotation.eulerAngles + new Vector3(0, 0, -frame_rpm));
                    rb.AddForceAtPosition(Quaternion.Euler(0, angle, 0) * propellers[i].forward * propellerForce * frame_rpm * 15, propellers[i].position);
                }

                //calculate rpm for next frame
                throttle *= (1.0F - drag);
                rpm = throttle * maxRPM * direction;
                fuel -= fuelConsumption * throttle;

                //set our engine note
                audioSource.pitch = 0.6f + (throttle * 0.5f);
            }

            //get our angle for the rudder
            angle = Mathf.Lerp(angle, 0.0F, rudderReturn);
            //transform rudders rotion
            for (int i = 0; i < rudder.Length; i++)
                rudder[i].localRotation = Quaternion.Euler(0, angle, 0);
        }
        void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.relativeVelocity.magnitude + " " + collision.gameObject.tag);
        }
        public void ThrottleUp()
        {
            if (throttle == 0) GetComponent<AudioSource>().Play();
            throttle += acceleration;
            if (throttle > 1) throttle = 1;
        }
        public void ThrottleDown()
        {
            throttle -= acceleration;
            if (throttle < 0) throttle = 0;
            if (throttle == 0) GetComponent<AudioSource>().Stop();
        }
        public void Brake()
        {
            throttle *= 0.9F;
        }
        public void Reverse()
        {
            direction *= -1;
        }
        public void RudderRight()
        {
            angle -= rudderChange;
            angle = Mathf.Clamp(angle, rudderMin, rudderMax);
        }
        public void RudderLeft()
        {
            angle += rudderChange;
            angle = Mathf.Clamp(angle, rudderMin, rudderMax);
        }
    }
}