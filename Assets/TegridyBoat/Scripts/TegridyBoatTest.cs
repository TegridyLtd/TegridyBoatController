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
using Tegridy.Water;
namespace Tegridy.Boat
{
    public class TegridyBoatTest : MonoBehaviour
    {
        public TegridyBoat boat;

        public TegridyBoatUIControlHolder gui;
        public TegridyBoatUIControl guiControl;
        
        public TegridyBoatNewInput newInputControl;
        public TegridyBoatOldInput oldInputControl;

        public TegridyBuoyancySettings buoyancySettings;
        TegridyBuoyancy[] buoyancies;

        void Start()
        {
            //set all objects to the default settings
            buoyancies = FindObjectsOfType<TegridyBuoyancy>();
            for (int i = 0; i < buoyancies.Length; i++) buoyancies[i].config = buoyancySettings;
            
            //if we have controllers start them up, why not?
            if(guiControl != null) guiControl.StartShipUI(boat, gui, null);
            if(newInputControl != null) newInputControl.StartUp(new PlayerInput(), boat);
            if (oldInputControl != null) oldInputControl.StartUp(boat);
        }
    }
}