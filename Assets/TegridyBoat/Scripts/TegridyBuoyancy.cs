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

using System.Collections.Generic;
using UnityEngine;
using Tegridy.Tools;
namespace Tegridy.Water
{
   [System.Serializable] public class TegridyBuoyancySettings
    {
        [Header("Object Config")]
        public float density = 500;
        public int slicesPerAxis = 2;
        public int voxelsLimit = 16;
        public bool getCOM = true;
        public Vector3 centerOfMass;

        [Header("WaterConfig")]
        public float waterLevel;
        public float waterDrag = 0.1f;
        public float waterDensity = 1000;
    }


    public class TegridyBuoyancy : MonoBehaviour
    {
        public TegridyBuoyancySettings config;
        //hull stuff
        private float voxelHeight;
        private float voxelHalfHeight;
        private Vector3 archimedesForce;
        private List<Vector3> voxels;

        private Rigidbody rb;
        private MeshCollider mCollider;

        private void Start()
        {
            // Store original rotation and position
            Transform startPosition = transform;
            transform.rotation = Quaternion.identity;
            transform.position = Vector3.zero;

            //Find the collider
            mCollider = GetComponent<MeshCollider>();

            //find the voxelHeight
            if (mCollider.bounds.size.x < mCollider.bounds.size.y) voxelHalfHeight = mCollider.bounds.size.x;
            else voxelHalfHeight = mCollider.bounds.size.y;
            if (mCollider.bounds.size.z < voxelHalfHeight) voxelHalfHeight = mCollider.bounds.size.z;

            voxelHalfHeight /= 2 * config.slicesPerAxis;
            voxelHeight = voxelHalfHeight * 2;
            
            //Setup the rigidy body / if none found set center of mass
            rb = GetComponent<Rigidbody>();

            if (config.getCOM) rb.centerOfMass = new Vector3(0, -mCollider.bounds.extents.y * 0f, 0) + transform.InverseTransformPoint(mCollider.bounds.center);
            else rb.centerOfMass = config.centerOfMass;

            voxels = MeshTools.SliceIntoVoxels(mCollider, config.slicesPerAxis, transform);

            // Restore original rotation and position
            transform.rotation = startPosition.rotation;
            transform.position = startPosition.position;

            MeshTools.WeldPoints(voxels, config.voxelsLimit);
            archimedesForce = new Vector3(0, config.waterDensity * Mathf.Abs(Physics.gravity.y) * (rb.mass / config.density), 0) / voxels.Count;
        }
        private void FixedUpdate()
        {
            foreach (Vector3 point in voxels)
            {
                Vector3 wp = transform.TransformPoint(point);
                if (wp.y - voxelHalfHeight < config.waterLevel)
                {
                    //bob arround
                    float k = (config.waterLevel - wp.y) / voxelHeight + 0.5f;
                    if (k > 1)
                    {
                        k = 1f;
                    }
                    else if (k < 0)
                    {
                        k = 0f;
                    }
                    Vector3 velocity = rb.GetPointVelocity(wp);
                    Vector3 localDampingForce = -velocity * config.waterDrag * rb.mass;
                    Vector3 force = localDampingForce + Mathf.Sqrt(k) * archimedesForce;
                    rb.AddForceAtPosition(force, wp);
                }
            }
        }
    }
}