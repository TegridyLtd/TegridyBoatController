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
    public class TegridyBoatNewInput : MonoBehaviour
    {
        TegridyBoat ship;
        PlayerInput controls;
        [SerializeField] bool active = false;
        public void StartUp(PlayerInput input, TegridyBoat boat)
        {
            ship = boat;
            controls = input;
            controls.Enable();
            active = true;
        }
        void Update()
        {
            if (active)
            {
                //set the rudded
                if (controls.Boat.Move.ReadValue<Vector2>().x < 0) ship.RudderLeft();
                else if (controls.Boat.Move.ReadValue<Vector2>().x > 0) ship.RudderRight();

                //set the throttle
                if (controls.Boat.Move.ReadValue<Vector2>().y > 0) ship.ThrottleUp();
                else if (controls.Boat.Move.ReadValue<Vector2>().y < 0) ship.ThrottleDown();

                //switch proppeller direction
                if (controls.Boat.Reverse.triggered) ship.Reverse();
            }
        }
    }
}