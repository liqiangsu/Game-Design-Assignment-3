using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

[Serializable()]
public class Object2PropertiesMappingListWrapper : ISerializable {
	
	public List<Object2PropertiesMapping> object2PropertiesMappings = new List<Object2PropertiesMapping>();
	public string EZR_VERSION = EZReplayManager.EZR_VERSION;
	public float recordingInterval;

	//serialization constructor
	protected Object2PropertiesMappingListWrapper(SerializationInfo info,StreamingContext context) {
	    object2PropertiesMappings = (List<Object2PropertiesMapping>)info.GetValue("object2PropertiesMappings",typeof(List<Object2PropertiesMapping>));
		EZR_VERSION = info.GetString("EZR_VERSION");
		recordingInterval = (float)info.GetValue("recordingInterval",typeof(float));
	}
	
	public Object2PropertiesMappingListWrapper(List<Object2PropertiesMapping> mappings) {
		object2PropertiesMappings = mappings;
	}
	
	public Object2PropertiesMappingListWrapper() {

	}
	
	public void addMapping(Object2PropertiesMapping mapping) {
		object2PropertiesMappings.Add(mapping);
	}
	
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("object2PropertiesMappings", this.object2PropertiesMappings);
		info.AddValue("EZR_VERSION", EZR_VERSION);
		info.AddValue("recordingInterval", this.recordingInterval);
		//base.GetObjectData(info, context);
	}	
}
