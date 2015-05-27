using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

/*
 * SoftRare - www.softrare.eu
 * This class is mapped to a single game object in EZReplayManager.cs map gOs2propMappings. It takes control of how to handle the scene if a recording is replayed and set back to normal game view.
 * You may only use and change this code if you purchased it in a legal way.
 * Please read readme-file, included in this package, to see further possibilities on how to use/execute this code. 
 */
[Serializable()]
public class Object2PropertiesMapping : ISerializable{

	//saved states belonging to one game object
	public SerializableDictionary<int,SavedState> savedStates = new SerializableDictionary<int,SavedState>();
	//the game object this mapping class object is being created for
	protected GameObject gameObject;
	//the clone belonging to the gameObject
	protected GameObject gameObjectClone;
	//is it a parent game object?
	protected bool isParentObj = false;
	//mapping of parent game object
	protected Object2PropertiesMapping parentMapping;
	//child no. parents have 0
	public int childNo;
	//prefab load path (for saving recordings)
	protected string prefabLoadPath = "";	
	//last frame where changes where recognized
	protected int lastChangedFrame = -1;
	//first frame where changes where recognized
	protected int firstChangedFrame = -1;
	//InstanceID of original gameObject
	protected int gameObjectInstanceID = -1;
	//name of original gameObject
	protected string gameObjectName = "name_untraceable";
	//way of identifying children of gameobjects in game scene hierarchy
	protected ChildIdentificationMode childIdentificationMode = ChildIdentificationMode.IDENTIFY_BY_ORDER;
	
	//serialization constructor
	protected Object2PropertiesMapping(SerializationInfo info,StreamingContext context) {
	    savedStates = (SerializableDictionary<int,SavedState>)info.GetValue("savedStates",typeof(SerializableDictionary<int,SavedState>));
	    isParentObj = info.GetBoolean("isParentObj");
		parentMapping = (Object2PropertiesMapping)info.GetValue("parentMapping",typeof(Object2PropertiesMapping));
		childNo = info.GetInt32("childNo");
		prefabLoadPath = info.GetString("prefabLoadPath");
		lastChangedFrame = info.GetInt32("lastChangedFrame");
		firstChangedFrame = info.GetInt32("firstChangedFrame");
		try {
			childIdentificationMode = (ChildIdentificationMode)info.GetValue("childIdentificationMode",typeof(ChildIdentificationMode));
		} catch (SerializationException e) {
			//file was recorded using old version of this plugin
			childIdentificationMode = ChildIdentificationMode.IDENTIFY_BY_ORDER;
		}		
		try {
			gameObjectName = info.GetString("gameObjectName");
		} catch (SerializationException e) {
			//file was recorded using old version of this plugin	
			childIdentificationMode = ChildIdentificationMode.IDENTIFY_BY_ORDER;
			gameObjectName = "name_untraceable";
		}
	}
	
	public Object2PropertiesMapping(GameObject go,bool isParent, Object2PropertiesMapping parentMapping, int childNo, string prefabLoadPath, ChildIdentificationMode childIdentificationMode) : this (go,isParent,parentMapping, childNo, prefabLoadPath) {
		this.childIdentificationMode = childIdentificationMode;
		
		if (isParentObj) { // if gameObject is a parent..
			//..instantiate mappings for all children too
			Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>() ;
			for(int i=0;i<allChildren.Length;i++) {
				GameObject child = allChildren[i].gameObject;
				
				if (!EZReplayManager.get.gOs2propMappings.ContainsKey(child)) {
					
					if (child!=gameObject)
						EZReplayManager.get.gOs2propMappings.Add(child, new Object2PropertiesMapping(child,false,this,i,"",childIdentificationMode));
					
				} else 
					if (EZReplayManager.showHints)
						MonoBehaviour.print("EZReplayManager HINT: GameObject '"+child+"' is already being recorded. Will not be marked for recording again.");				
				
			}
		}		
	}	
	
	public Object2PropertiesMapping(GameObject go,bool isParent, Object2PropertiesMapping parentMapping, int childNo, string prefabLoadPath) : this (go,isParent,parentMapping, childNo) {
		this.prefabLoadPath = prefabLoadPath;
	}	
	
	//as this is not derived from MonoBehaviour, we have a constructor
	public Object2PropertiesMapping(GameObject go,bool isParent, Object2PropertiesMapping parentMapping, int childNo) {
		//setting instance variables
		this.gameObject = go;
		this.isParentObj = isParent;		
		this.parentMapping = parentMapping;
		this.childNo = childNo;
		this.gameObjectInstanceID=go.GetInstanceID();
		this.gameObjectName = go.name;
	

	}
	
	public bool isParent() {
		return isParentObj;
	}
	
	public GameObject getGameObject() {
		return gameObject;
	}		
	
	public GameObject getGameObjectClone() {
		return gameObjectClone;
	}
	
	public int getLastChangedFrame() {
		return lastChangedFrame;	
	}
	
	public void setLastChangedFrame(int lastChangedFrame) {
		this.lastChangedFrame = lastChangedFrame;
	}
	
	//executed before each replay
	public void prepareObjectForReplay() {
		
		//spawn super object which gets all replay manager objects as children
		GameObject superParent = GameObject.Find(EZReplayManager.S_PARENT_NAME);
	
		//create super parent if has not happened. The super parent keeps the scene clean
		if (superParent == null) {
			superParent = new GameObject(EZReplayManager.S_PARENT_NAME);		
			superParent.transform.position = Vector3.zero;
			superParent.transform.rotation = Quaternion.identity;
			superParent.transform.localScale = Vector3.one;
		}
		
		if (isParentObj) { //if is a parent gameObject mapping 
			
			if (prefabLoadPath == "") {
				gameObjectClone = (GameObject)GameObject.Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
				
			} else {
				gameObjectClone = (GameObject)GameObject.Instantiate(Resources.Load(prefabLoadPath));
			}
			
			gameObjectClone.transform.parent = superParent.transform;
		
		} else { // if is a child (can also be a parent in game scene hierachy but "EZReplayManager.mark4recording()" has not been called for this object specifically, so we handle it as a child
			
			GameObject myParentClone = parentMapping.getGameObjectClone();
			Transform[] allChildren = myParentClone.GetComponentsInChildren<Transform>(true) ;
			
			for(int i=0;i<allChildren.Length;i++) {
				GameObject child = allChildren[i].gameObject;
				//map child to order number or go-name
				if ((childIdentificationMode == ChildIdentificationMode.IDENTIFY_BY_ORDER && i == childNo) ||
					(childIdentificationMode == ChildIdentificationMode.IDENTIFY_BY_NAME && gameObjectName == child.name)) {
					gameObjectClone = child;
					break;
				}
			}			
			
			if (gameObjectClone == null) { //child was destroyed along the way while recording
				if (EZReplayManager.get.precacheGameobjects)
					gameObjectClone = (GameObject)GameObject.Instantiate(Resources.Load(EZReplayManager.get.generateCachePath(gameObjectName,"")));

			}
			
		}
		
		gameObjectClone.name = gameObjectInstanceID+"_"+gameObjectClone.GetInstanceID()+"_"+gameObjectClone.name;
		
		if (gameObjectInstanceID > -1) // can happen when file was loaded. obviously this doesn't work with loaded files yet.
			EZReplayManager.get.instanceIDtoGO.Add(gameObjectInstanceID,gameObject);
	
		// kill all unneccessary scripts on gameObjectClone
		Component[] allComps = gameObjectClone.GetComponentsInChildren<Component>(true);
		
		List<Component> componentsToKill = new List<Component>();
		foreach (Component comp in allComps) {
			
			//Exclude scripts and components from removal: (this is done to preserve basic functionality and renderers)
			if (comp != comp.GetComponent<Transform>() 
				&& comp != comp.GetComponent<MeshFilter>() 
				&& comp != comp.GetComponent<MeshRenderer>() 
				&& comp != comp.GetComponent<SkinnedMeshRenderer>() 
				&& comp != comp.GetComponent<ParticleEmitter>()
				&& comp != comp.GetComponent<ParticleAnimator>()
				&& comp != comp.GetComponent<ParticleRenderer>()
				&& comp != comp.GetComponent<Camera>()
				&& comp != comp.GetComponent<GUILayer>()
				&& comp != comp.GetComponent<AudioListener>()
				&& comp != comp.GetComponent("FlareLayer")
				) {

						bool found = false;
						// take exceptions from public array "EZReplayManager.componentsAndScriptsToKeepAtReplay"
						for (int i=0;i<EZReplayManager.get.componentsAndScriptsToKeepAtReplay.Count;i++) {
							if (comp == comp.GetComponent(EZReplayManager.get.componentsAndScriptsToKeepAtReplay[i])) {
								found = true;
								break;
							}
						}
						
						if (!found) {
							componentsToKill.Add(comp);
						}
				} 
		}	
		//uses multiple cycles to kill components which are required by others
		int cycles = 0;
		do {
			List<Component> componentsToKillNew = componentsToKill;
			for(int i=0;i<componentsToKill.Count;i++) {
				Component comp = componentsToKill[i];

				try {
					
					GameObject.DestroyImmediate(comp);
				} finally {
					if (comp == null) {
						componentsToKillNew.RemoveAt(i);
					} else { //change order
						componentsToKillNew.Remove(comp);
						componentsToKillNew.Add(comp);
					}
				}
			}
			
			componentsToKill = componentsToKillNew;
			cycles++;
		} while (componentsToKill.Count > 0 && cycles <= 10);
		
		EZR_Clone thisCloneScript = gameObjectClone.AddComponent<EZR_Clone>();
		thisCloneScript.origInstanceID = gameObjectInstanceID;
		thisCloneScript.cloneInstanceID = gameObjectClone.GetInstanceID();		
		
		if (EZReplayManager.get.autoDeactivateLiveObjectsOnReplay && gameObject != null) {
			/*if (gameObject.rigidbody) { //if needed, please do this yourself (i.e. via callback __EZR_replay_prepare)
				gameObject.rigidbody.Sleep();
			}*/			
			
			gameObject.SetActive( false );
		}
		
		SavedState mostRecent = null;

		for(int i= firstChangedFrame;i<=lastChangedFrame;i++) {
			if (savedStates.ContainsKey(i)) {
				mostRecent = savedStates[i];
			} else {
				savedStates.Add(i,mostRecent);
			}
		}
		
	}
	
	public int getMaxFrames() {
		int maxframes = 0;
		foreach(KeyValuePair<int,SavedState> stateEntry in savedStates) {
			if (stateEntry.Key > maxframes)
				maxframes = stateEntry.Key;
		}		
		return maxframes;
	}
	
	//executed just before stopping a replay
	public void resetObject() {
		
		
		GameObject superParent = GameObject.Find(EZReplayManager.S_PARENT_NAME);
		//destroy superParent if not yet done
		if (superParent != null)
			GameObject.Destroy(superParent);
		//clear clones list
		if (EZReplayManager.get.instanceIDtoGO.Count > 0)
			EZReplayManager.get.instanceIDtoGO.Clear();
		
		//reactivate gameObject
		if (gameObject != null && EZReplayManager.get.autoDeactivateLiveObjectsOnReplay) {
			if (savedStates.ContainsKey(lastChangedFrame) && lastChangedFrame > -1) {
				
					gameObject.SetActive( savedStates[lastChangedFrame].isActive );
				
			} else 
				gameObject.SetActive( true );
		}
		
	}	
	
	// insert a new state at certain position
	public void insertStateAtPos(int recorderPosition) {
		
		SavedState newState = new SavedState(gameObject);	
		bool insertFrame = true;
		if (lastChangedFrame > -1) {

			if (savedStates.ContainsKey(lastChangedFrame) && !newState.isDifferentTo(savedStates[lastChangedFrame])) {
			
				insertFrame = false;
			}
		}
		try {
			if (insertFrame) {
				
				savedStates.Add(recorderPosition,newState);
				
				lastChangedFrame = recorderPosition;
				
				if (firstChangedFrame == -1)
					firstChangedFrame = recorderPosition;
			}
		} catch {
			if (EZReplayManager.showErrors)
				MonoBehaviour.print("EZReplayManager ERROR: You probably already inserted at position '"+recorderPosition+"' for game object '"+gameObject+"'.");
		}
	}
	
	//synchronize gameObjectClone to a certain state at a certain  recorderPosition
	public void synchronizeProperties(int recorderPosition) { 
		
		if (firstChangedFrame > -1 && recorderPosition >= firstChangedFrame )  {
				if (recorderPosition <= lastChangedFrame ) {			
					
					try {
						//gameObjectClone.active = true;
						getStateAtPos(recorderPosition).synchronizeProperties(gameObjectClone);
						
					} catch (NullReferenceException e ) {
						
					}
				
				} else {
					getStateAtPos(lastChangedFrame).synchronizeProperties(gameObjectClone); 
				}
				
		} else if (gameObjectClone.activeInHierarchy) {
			gameObjectClone.SetActive (false);	
		}
		
		
	}
	
	protected SavedState getStateAtPos(int recorderPosition) {
		return savedStates[recorderPosition];
	}	
	
	public int getAmountStates() {
		return savedStates.Count;
	}
	
	public void clearStates() {
		savedStates.Clear();
	}
	
	/*[SecurityPermissionAttribute(
	            SecurityAction.Demand,
	            SerializationFormatter = true)]	*/
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("savedStates", this.savedStates);
		info.AddValue("isParentObj", this.isParentObj);
		info.AddValue("parentMapping", this.parentMapping);
		info.AddValue("childNo", this.childNo);
		info.AddValue("prefabLoadPath", this.prefabLoadPath);
		info.AddValue("lastChangedFrame", this.lastChangedFrame);
		info.AddValue("firstChangedFrame", this.firstChangedFrame);
		info.AddValue("gameObjectName", this.gameObjectName);
		info.AddValue("childIdentificationMode", this.childIdentificationMode);
		//base.GetObjectData(info, context);
	}		
	
}