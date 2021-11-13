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
namespace Tegridy.Cam
{
    public class TegridyCamSimpleFollow : MonoBehaviour
{
    public GameObject cameraHolder;
    public GameObject target;
    public Vector3 offset;

    public bool followX;
    public bool followY;
    public bool followZ;
    public bool lookAt;

    private void Start()
    {
        if (followX || followY || followZ) cameraHolder.transform.position = offset;
    }

    void LateUpdate()
    {
        if(lookAt) cameraHolder.transform.LookAt(target.transform.position);

        Vector3 newPos = cameraHolder.transform.position;
        if (followX) newPos.x = target.transform.position.x + offset.x;
        if (followY) newPos.y = target.transform.position.y + offset.y;
        if (followZ) newPos.z = target.transform.position.z + offset.z;

        cameraHolder.transform.position = newPos;
    }
}
}