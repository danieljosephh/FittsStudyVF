/**
 * Adapted from johny3212
 * Written by Matt Oskamp
 */
//using UnityEngine;
/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using OptitrackManagement;
using WobbrockLib;
using WobbrockLib.Extensions;*/

using System;
using System.Collections;
using System.Collections.Generic;
using OptitrackManagement;
using System.Drawing;
using WobbrockLib;
using WobbrockLib.Extensions;
using WobbrockLib.Types;

// public class OptiTrackManager : MonoBehaviour
public class OptiTrackManager
{
	public string myName;
	public float scale = 20.0f;
	private static OptiTrackManager instance;
	public float origin = 0.0f; // set this to wherever you want the center to be in your scene

    public PointF p;    // used as return variable for 'getPosition'

	public static OptiTrackManager Instance
	{
		get { return instance; } 
	}

	void Awake()
	{
		instance = this;
	}

	~OptiTrackManager ()
	{
        Console.WriteLine("OptitrackManager: Destruct");
		OptitrackManagement.DirectMulticastSocketClient.Close();
	}
	
	public void Start () 
	{
        Console.WriteLine(myName + ": Initializing");
		
		OptitrackManagement.DirectMulticastSocketClient.Start();
		//Application.runInBackground = true;
	}

	public OptiTrackRigidBody getOptiTrackRigidBody(int index)
	{
		// only do this if you want the raw data
		if(OptitrackManagement.DirectMulticastSocketClient.IsInit())
		{
			DataStream networkData = OptitrackManagement.DirectMulticastSocketClient.GetDataStream();
			return networkData.getRigidbody(index);
		}
		else
		{
			OptitrackManagement.DirectMulticastSocketClient.Start();
			return getOptiTrackRigidBody(index);
		}
	}

	public TimePointF getPosition(int rigidbodyIndex)
	{
		if(OptitrackManagement.DirectMulticastSocketClient.IsInit())
		{
			DataStream networkData = OptitrackManagement.DirectMulticastSocketClient.GetDataStream();

            return networkData.getRigidbody(rigidbodyIndex).position;

            //p = networkData.getRigidbody(rigidbodyIndex).position;

            //return networkData;

            /********************* EDIT START *********************/
			// original (&& test 1)
            //float pos = origin + networkData.getRigidbody(rigidbodyIndex).position * scale;
            
            // test 2
            //float[] pos = new float[2];
            //pos[0] = origin + networkData.getRigidbody(rigidbodyIndex).position[0] * scale;
            //pos[1] = origin + networkData.getRigidbody(rigidbodyIndex).position[1] * scale;
            //float posX = origin + networkData.getRigidbody(rigidbodyIndex).position * scale;
            //float posY = origin + networkData.getRigidbody(rigidbodyIndex).position * scale;
            //float posZ = origin + networkData.getRigidbody(rigidbodyIndex).position * scale;
			//pos.x = -pos.x; // not really sure if this is the best way to do it
			//pos.y = pos.y; // these may change depending on your configuration and calibration
			//pos.z = -pos.z;

			//return p;

            /********************* EDIT END *********************/
		}
		else
		{
            // using 'float[]' as a return type
            //return new float[2];

            // using a DataStream as a return type
            //return null;

            // using PointF as a return type
            return new TimePointF(0, 0, 0);
		}
	}


    /*

	public Quaternion getOrientation(int rigidbodyIndex)
	{
		// should add a way to filter it
		if(OptitrackManagement.DirectMulticastSocketClient.IsInit())
		{
			DataStream networkData = OptitrackManagement.DirectMulticastSocketClient.GetDataStream();
			Quaternion rot = networkData.getRigidbody(rigidbodyIndex).orientation;

			// change the handedness from motive
			//rot = new Quaternion(rot.z, rot.y, rot.x, rot.w); // depending on calibration
			
			// Invert pitch and yaw
			Vector3 euler = rot.eulerAngles;
			rot.eulerAngles = new Vector3(euler.x, -euler.y, euler.z); // these may change depending on your calibration

			return rot;
		}
		else
		{
			return Quaternion.identity;
		}
	}
     * 
     */

	public void DeInitialize()
	{
		OptitrackManagement.DirectMulticastSocketClient.Close();
	}

	// Update is called once per frame
	void Update () 
	{

	}
}