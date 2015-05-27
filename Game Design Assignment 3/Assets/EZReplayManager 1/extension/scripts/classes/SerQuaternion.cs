using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;

[Serializable()]
public class SerQuaternion: ISerializable {
	
	//serialization constructor
	protected SerQuaternion(SerializationInfo info,StreamingContext context) {
		x = (float)info.GetValue("x",typeof(float));
		y = (float)info.GetValue("y",typeof(float));
		z = (float)info.GetValue("z",typeof(float));
		w = (float)info.GetValue("w",typeof(float));
	}	
	
	public SerQuaternion(Quaternion quat) {
		this.x = quat.x;
		this.y = quat.y;
		this.z = quat.z;
		this.w = quat.w;
	}		
	
	public SerQuaternion(float x, float y, float z, float w) {
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}	

	public float x; 
	public float y; 
	public float z; 
	public float w; 
	
	public bool isDifferentTo(SerQuaternion other) {
		bool changed = false;
		
		if (!changed && x != other.x )
			changed = true;
			
		if (!changed && y != other.y )
			changed = true;
		
		if (!changed && z != other.z )
			changed = true;
		
		if (!changed && w != other.w )
			changed = true;
				
		
		return changed;
	}
	
	public Quaternion getQuaternion() {
		return new Quaternion(this.x,this.y,this.z,this.w);
	}	
	
	/*[SecurityPermissionAttribute(
	            SecurityAction.Demand,
	            SerializationFormatter = true)]		*/
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("x", this.x);
		info.AddValue("y", this.y);
		info.AddValue("z", this.z);
		info.AddValue("w", this.w);
	}	
}
