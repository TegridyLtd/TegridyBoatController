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
using Tegridy.Tools;
namespace Tegridy.Boat
{
    public class TegridyBoatUIControl : MonoBehaviour
    {
        TegridyBoat ship;
        TegridyBoatUIControlHolder gui;

        float maxFuel;
        bool active = false;
        GameObject host;

        void Update()
        {
            if (active) UpdateDisplay();
        }
        public void StartShipUI(TegridyBoat boat, TegridyBoatUIControlHolder thisGUI, GameObject hostMenu)
        {
            //keep track of the object we need
            host = hostMenu;
            ship = boat;
            gui = thisGUI;

            //Setup the buttons
            gui.direction.onClick.AddListener(() => ship.Reverse());
            UITools.SetButtonText(gui.direction, TegridyBoatLanguage.changeDirection);
            gui.throttleDown.onClick.AddListener(() => ship.ThrottleDown());
            UITools.SetButtonText(gui.throttleDown, TegridyBoatLanguage.throttleDown);
            gui.throttleUp.onClick.AddListener(() => ship.ThrottleUp());
            UITools.SetButtonText(gui.throttleUp, TegridyBoatLanguage.throttleUp);
            gui.rudderLeft.onClick.AddListener(() => ship.RudderLeft());
            UITools.SetButtonText(gui.rudderLeft, TegridyBoatLanguage.rudderLeft);
            gui.rudderRight.onClick.AddListener(() => ship.RudderRight());
            UITools.SetButtonText(gui.rudderRight, TegridyBoatLanguage.rudderRight);
            gui.close.onClick.AddListener(() => CloseShipUI(1));
            UITools.SetButtonText(gui.close, TegridyBoatLanguage.close);

            //start the UI
            maxFuel = ship.fuel; //get this before we start using any fuel
            active = true;
            gui.gameObject.SetActive(true);
            if (host != null) host.SetActive(false);
        }
        public void CloseShipUI(int reason)
        {
            //reasons 1 = Menu Close // 2 = ... 
            active = false;
            gui.direction.onClick.RemoveAllListeners();
            gui.throttleDown.onClick.RemoveAllListeners();
            gui.throttleUp.onClick.RemoveAllListeners();
            gui.rudderLeft.onClick.RemoveAllListeners();
            gui.rudderRight.onClick.RemoveAllListeners();
            gui.gameObject.SetActive(false);

            //decide what to do after closing
            if (host != null) host.SetActive(true);
            else Application.Quit();
        }
        private void UpdateDisplay()
        {
            //buid the text info
            gui.displayText.text = TegridyBoatLanguage.fuel + ship.fuel / (maxFuel / 100) + "%<br>";
            gui.displayText.text += TegridyBoatLanguage.speed + ((ship.rb.velocity.magnitude * 2.237) * 2).ToString("F2") + "mph<br>";
            gui.displayText.text += TegridyBoatLanguage.throttle + (ship.throttle * 100).ToString("F0") + "%<br>";
            gui.displayText.text += TegridyBoatLanguage.rudderPos + ship.angle.ToString("F2") + "<br>";


            int rpm = (int)ship.rpm;
            if (rpm < 0) rpm *= -1;
            gui.displayText.text += TegridyBoatLanguage.rpm + rpm + "<br>" + TegridyBoatLanguage.maxRPM + ship.maxRPM.ToString("F0") + "<br>";

            //decide what direction we are going
            if(ship.direction == 1) gui.displayText.text += TegridyBoatLanguage.gear + TegridyBoatLanguage.forward;
            else gui.displayText.text += TegridyBoatLanguage.gear + TegridyBoatLanguage.reverse;
        }
    }
}