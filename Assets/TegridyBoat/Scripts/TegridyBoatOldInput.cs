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
    public class TegridyBoatOldInput : MonoBehaviour
    {
        TegridyBoat ship;
        [SerializeField] bool active = false;
        public void StartUp(TegridyBoat boat)
        {
            ship = boat;
            active = true;
        }
        void Update()
        {
            if (active)
            {
                //set the rudder
                float horizontal = Input.GetAxis("Horizontal");
                if (horizontal < 0) ship.RudderLeft();
                else if (horizontal > 0) ship.RudderRight();

                //set the throttle
                float vertical = Input.GetAxis("Vertical");
                if (vertical > 0) ship.ThrottleUp();
                else if (vertical < 0) ship.ThrottleDown();

                //switch proppeller direction
                if (Input.GetButtonDown("Fire1")) ship.Reverse();
            }
        }
    }
}