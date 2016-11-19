/**
 * Adapted from johny3212
 * Written by Matt Oskamp
 */
//using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using WobbrockLib;
using WobbrockLib.Extensions;
using WobbrockLib.Types;

namespace OptitrackManagement
{
	
	// marker
	public class Marker
	{
		public int ID = -1;
        public float pos;      
	}
	
	// Rigidbody
	public class OptiTrackRigidBody
	{
		public string name = "";
		public int ID = -1;
		public int parentID = -1;
        public TimePointF position;     // contains the frame index as well
		//public Quaternion orientation;
	}
}