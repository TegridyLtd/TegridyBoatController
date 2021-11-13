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
namespace Tegridy.Tools
{
    public class MeshTools
    {
        public static List<Vector3> SliceIntoVoxels(MeshCollider mCollider, int slicesPerAxis, Transform transform)
        {
            List<Vector3> points = new List<Vector3>(slicesPerAxis * slicesPerAxis * slicesPerAxis);
            for (int ix = 0; ix < slicesPerAxis; ix++)
            {
                for (int iy = 0; iy < slicesPerAxis; iy++)
                {
                    for (int iz = 0; iz < slicesPerAxis; iz++)
                    {
                        float x = mCollider.bounds.min.x + mCollider.bounds.size.x / slicesPerAxis * (0.5f + ix);
                        float y = mCollider.bounds.min.y + mCollider.bounds.size.y / slicesPerAxis * (0.5f + iy);
                        float z = mCollider.bounds.min.z + mCollider.bounds.size.z / slicesPerAxis * (0.5f + iz);
                        if (!mCollider.convex)
                        {
                            Vector3 p = transform.InverseTransformPoint(new Vector3(x, y, z));
                            if (PointIsInsideMeshCollider(mCollider, p)) points.Add(p);
                        }
                        else
                        {
                            Vector3 p = transform.InverseTransformPoint(new Vector3(x, y, z));
                            points.Add(p);
                        }
                    }
                }
            }
            if (points.Count == 0) points.Add(mCollider.bounds.center);
            return points;
        }
        private static bool PointIsInsideMeshCollider(Collider c, Vector3 p)
        {
            Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
            foreach (Vector3 ray in directions)
            {
                if (c.Raycast(new Ray(p - ray * 1000, ray), out _, 1000f) == false) return false;
            }
            return true;
        }
        public static void FindClosestPoints(IList<Vector3> list, out int firstIndex, out int secondIndex)
        {
            float minDistance = float.MaxValue, maxDistance = float.MinValue;
            firstIndex = 0;
            secondIndex = 1;

            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    float distance = Vector3.Distance(list[i], list[j]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        firstIndex = i;
                        secondIndex = j;
                    }
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                    }
                }
            }
        }
        public static void WeldPoints(IList<Vector3> list, int targetCount)
        {
            if (list.Count <= 2 || targetCount < 2)
            {
                return;
            }

            while (list.Count > targetCount)
            {
                MeshTools.FindClosestPoints(list, out int first, out int second);

                Vector3 mixed = (list[first] + list[second]) * 0.5f;
                list.RemoveAt(second); // the second index is always greater that the first => removing the second item first
                list.RemoveAt(first);
                list.Add(mixed);
            }
        }
    }
}