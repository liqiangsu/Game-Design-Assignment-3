using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;

[Serializable()]
public class SerVector3: ISerializable  {
	
	//serialization constructor
	protected SerVector3(SerializationInfo info,StreamingContext context) {
		x = (float)info.GetValue("x",typeof(float));
		y = (float)info.GetValue("y",typeof(float));
		z = (float)info.GetValue("z",typeof(float));
	}
	
	public SerVector3(Vector3 vec3) {
		this.x = vec3.x;
		this.y = vec3.y;
		this.z = vec3.z;
	}	
	
	public SerVector3(float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
	
	public bool isDifferentTo(SerVector3 other) {
		bool changed = false;
		
		if (!changed && x != other.x )
			changed = true;
			
		if (!changed && y != other.y )
			changed = true;
		
		if (!changed && z != other.z )
			changed = true;
		
		return changed;
	}	
	
	public float x; 
	public float y; 
	public float z; 
	
	public Vector3 getVector3() {
		return new Vector3(this.x,this.y,this.z);
	}
	
	/*[SecurityPermissionAttribute(
	            SecurityAction.Demand,
	            SerializationFormatter = true)]		*/
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("x", this.x);
		info.AddValue("y", this.y);
		info.AddValue("z", this.z);
	}	
}
