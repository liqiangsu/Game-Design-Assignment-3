//#define IS_UNLICENSED

using UnityEngine;
#if UNITY_EDITOR || !(UNITY_STANDALONE_OSX || UNITY_DASHBOARD_WIDGET || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY || UNITY_WP8 || UNITY_METRO)
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection.Emit;

/*
 * @author SoftRare - www.softrare.eu
 * This class handles all global settings and executes intervals between updates. Please change values one at a time if you must, debug, and change the next if you are really sure everything is working,. Changes can have significant consequences for performance and overall functionality.
 * You may only use and change this code if you purchased it in a legal way.
 * Please read readme-file, included in this package, to see further possibilities on how to use/execute this code.
 */
#if !IS_UNLICENSED
public class EZReplayManager : MonoBehaviour {
#else
public sealed class EZReplayManager : MonoBehaviour {	
#endif
		
	//these icons can be changed freely
	public Texture2D EZRicon;
	public Texture2D playIcon;
	public Texture2D pauseIcon;
	public Texture2D startRecordIcon;
	public Texture2D stopRecordIcon;
	public Texture2D replayIcon;
	public Texture2D stopIcon;
	public Texture2D closeIcon;
	public Texture2D rewindIcon;
	
	//these values have no influence on what the user sees:
	public const string S_PARENT_NAME= "EZReplayM_sParent"; //default: EZReplayM_sParent
	protected const string S_EZR_ASSET_PATH= "EZReplayManagerAssetPrefabs";
	
	public const bool showErrors = true; //default: true
	public const bool showWarnings = true; //default: true
	public const bool showHints = false; //default: false
	
	public const string EZR_VERSION = "1.5";
	
	//don't change these manually unless you know what you are doing:
	protected ViewMode currentMode = ViewMode.LIVE;
	protected ActionMode currentAction = ActionMode.STOPPED;
	protected int recorderPosition = 0;
	protected int maxPositions = 0;
	protected float playingInterval;	
	protected int recorderPositionStep = 1; //default: 1
	protected int orgRecorderPositionStep;
	protected bool exitOnFinished = false; //default: false
	protected float surplus = 0.0f; 
	protected float timeelapsed = 0.0f; 
	protected bool continueCallingUpdate = false;
	protected bool showPrecachingMandatoryMessage = false;
#if IS_UNLICENSED
	private bool showingStoppedRecordingMsg = false; 
#endif
	
	//these values determine what GUIs will be seen by the user:
	public bool showHintsForImportantOptions = true;
	public bool useRecordingGUI = true; //default: true
	public bool useReplayGUI = true; //default: true	
	public bool useDarkStripesInReplay = true; //default: true
	public bool precacheGameobjects = true; //default: true
	public bool sendCallbacks = false; //default: false
	public List<string> callbacksToExecute = new List<string>(); 
#if IS_UNLICENSED
	public bool showFullVersionLicensingInfo = true; //default: true
#endif
	public bool autoDeactivateLiveObjectsOnReplay = true; //default: true	
	
	//change these to configure the overall performance of the script:
	public float recordingInterval = 0.05f;  //default: 0.05f (20fps) .. if your replay is not fluent enough lower this value step by step.. try 0.04f (25fps) first, lower it then
	public const int maxSpeedSliderValue = 3; //default: 3
	public const int minSpeedSliderValue = -3; //default: -3
	protected int speedSliderValue = 0; //default: 0
	protected int speedSliderValueBackup = 0; //default: 0

	protected Dictionary<GameObject, Object2PropertiesMapping> gOs2propMappingsRecordingSlot = new Dictionary<GameObject, Object2PropertiesMapping>();
	protected Dictionary<GameObject, Object2PropertiesMapping> gOs2propMappingsLoadingSlot = new Dictionary<GameObject, Object2PropertiesMapping>();
	
	//here all mappings are done from the original game objects to their replay-counterpart clones
	public Dictionary<GameObject, Object2PropertiesMapping> gOs2propMappings;
	//these are the game objects to mark for recording. This way coding can be avoided.
	public List<GameObject> gameObjectsToRecord = new List<GameObject>(); 
	//fill these with names of components and scripts (strings). These will not be removed on replay.
	public List<string> componentsAndScriptsToKeepAtReplay = new List<string>(); 
	
	protected List<GameObject> markedGameObjects = new List<GameObject>(); //don't fill on your own.
	public Dictionary<int,GameObject> instanceIDtoGO = new Dictionary<int,GameObject>(); //don't fill on your own. Only usable in replay mode
	
	protected static EZReplayManager singleton;
	
	//instantiation actions go here: DO NOT hit EZReplayManager.record() in "Awake()" function. Will fail. Call record in "Start()" instead.
	protected void instantiateSingleton() {
		useRecordingSlot();
	}
	
	// use to replay what you have just recorded in the same session
	protected void useRecordingSlot() {
		gOs2propMappings = gOs2propMappingsRecordingSlot;
	}
	
	public bool isRecordingSlotInUse() {
		return (gOs2propMappings == gOs2propMappingsRecordingSlot);
	}
	
	//use to replay what was saved to file earlier
	protected void useLoadingSlot() {
		gOs2propMappings = gOs2propMappingsLoadingSlot;
	}	
	
	public bool isLoadingSlotInUse() {
		return (gOs2propMappings == gOs2propMappingsLoadingSlot);
	}	
	
	protected void sendCallback2All(string functionName, object parameter) {
		
		if (sendCallbacks && callbacksToExecute.Contains(functionName)) {
			GameObject[] gameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
			
			foreach(GameObject go in gameObjects) {
				go.SendMessage(functionName,parameter,SendMessageOptions.DontRequireReceiver);
			}
		}
	}	
		
#if !IS_UNLICENSED	
	//serialize and save (do not call directly, call save() instead)
	protected void SerializeObject(string filename, Object2PropertiesMappingListWrapper objectToSerialize)
	{
		Stream stream = File.Open(filename, FileMode.Create);
		BinaryFormatter bFormatter = new BinaryFormatter();
		bFormatter.Serialize(stream, objectToSerialize);
		stream.Close();
	}	
	
	//deserialize and return for loading (do not call directly, call load() instead)
	protected Object2PropertiesMappingListWrapper DeSerializeObject(string filename)
	{
		Object2PropertiesMappingListWrapper objectToSerialize = null;
		Stream stream = File.Open(filename, FileMode.Open);
		BinaryFormatter bFormatter = new BinaryFormatter();

		objectToSerialize = (Object2PropertiesMappingListWrapper)bFormatter.Deserialize(stream);
		//print("System.GC.GetTotalMemory(): "+System.GC.GetTotalMemory(false));
		stream.Close();
		
		return objectToSerialize;
	}		
		
	//wrapper for saving to file
	public void saveToFile(string filename) {
		
		if (!precacheGameobjects) {
			StartCoroutine(startShowPrecachingMandatoryMessage(8.5f));
			return;	
		}
			
		stop ();
			
		Object2PropertiesMappingListWrapper o2pMappingListW = new Object2PropertiesMappingListWrapper();
		foreach (var entry in gOs2propMappings) {
			o2pMappingListW.addMapping(entry.Value);		
		}
		o2pMappingListW.recordingInterval = recordingInterval;
		SerializeObject(filename,o2pMappingListW);							
		
	}
	
	//wrapper for loading from file
	public void loadFromFile(string filename) {
			
		if (!precacheGameobjects) {
			StartCoroutine(startShowPrecachingMandatoryMessage(8.5f));
			return;	
		}			
	
		stop ();
			
		if (autoDeactivateLiveObjectsOnReplay) 
			foreach(var entry in markedGameObjects) {
				if (entry != null)  //not sure why, but it seems that sometimes this condition is neccessary, will research
					entry.SetActive(false);
				//else 
				//	print ("entry: "+entry);
			}
		
		useLoadingSlot();
		
		Object2PropertiesMappingListWrapper reSerialized = (Object2PropertiesMappingListWrapper)DeSerializeObject(filename);
		
		gOs2propMappings.Clear();
		maxPositions = 0;
		recorderPosition = 0;
		recordingInterval = reSerialized.recordingInterval; //if you load a replay with a different recording interval, 
																	  //you have to reset it to the earlier value afterwards YOURSELF!
		
		if (reSerialized.EZR_VERSION != EZR_VERSION) {
			if (showWarnings)
				print("EZReplayManager WARNING: The EZR version with which the file has been created differs from your version of the EZReplayManager. This can cause unintended behaviour.");			
		}
		
		foreach(var entry in reSerialized.object2PropertiesMappings) {

			if (entry.isParent()) {
				entry.prepareObjectForReplay();
				GameObject goClone = entry.getGameObjectClone();
				gOs2propMappings.Add(goClone,entry);
				
				foreach(KeyValuePair<int,SavedState> stateEntry in entry.savedStates) {
					if (stateEntry.Key > maxPositions)
						maxPositions = stateEntry.Key;
				}

			}
		}	
		
		foreach(var entry in reSerialized.object2PropertiesMappings) {

			if (!entry.isParent()) {
				entry.prepareObjectForReplay();
				GameObject goClone = entry.getGameObjectClone();
				gOs2propMappings.Add(goClone,entry);
			}
			
			foreach(KeyValuePair<int,SavedState> stateEntry in entry.savedStates) {
				if (stateEntry.Key > maxPositions)
					maxPositions = stateEntry.Key;
			}			
		}

		currentMode = ViewMode.REPLAY;
		
		//switchModeTo(ViewMode.REPLAY); //happens in play:
		play(0);
		
	}
#endif	
	
	//create an empty prefab at predefined location
#if UNITY_EDITOR || !(UNITY_STANDALONE_OSX || UNITY_DASHBOARD_WIDGET || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY || UNITY_WP8 || UNITY_METRO)
	protected UnityEngine.Object createEmptyEZRPrefab(string filepath) {
		return PrefabUtility.CreateEmptyPrefab("Assets/Resources/"+filepath+".prefab");	
	}
	
	protected void cacheGOinResources(GameObject go, string prefabLoadPath) {
	
		prefabLoadPath = generateCachePath(go.name, prefabLoadPath);
		
		//IMPORTANT: always execute a full replay of the scene within Unity Editor before exporting (i.e. if you added new models)
		if (Resources.Load(prefabLoadPath) == null) {
			//object has not been assigned to a specific prefab-path. Create our own:			
			
			if (!Directory.Exists("Assets/Resources")) {
				AssetDatabase.CreateFolder("Assets", "Resources");
			}							
			
			if (!Directory.Exists("Assets/Resources/"+S_EZR_ASSET_PATH)) {
				AssetDatabase.CreateFolder("Assets/Resources", S_EZR_ASSET_PATH);
			}
		
			UnityEngine.Object asset = createEmptyEZRPrefab(prefabLoadPath);
		
			PrefabUtility.ReplacePrefab(go,asset);	
		}	
		
	
		
	}
#endif
	
	public string generateCachePath(string gameObjectName, string prefabLoadPath) {
		string standardFilename = "go_"+gameObjectName;
		if (prefabLoadPath == "") {
			prefabLoadPath = EZReplayManager.S_EZR_ASSET_PATH+"/"+standardFilename;
		}
		
		return prefabLoadPath;
	}
	
	//private object precachingLock = new object();
	
	//mark an object for recording, can be done while recording and while game is running, but not while replaying a recording
	//v1.5: Specify "prefabLoadPath" if you know exactly where "go" is located as a prefab in "Resources" directory.
	public void mark4Recording(GameObject go, string prefabLoadPath, ChildIdentificationMode childIdentificationMode) {
		if (currentMode == ViewMode.LIVE) {
			if (currentAction != ActionMode.PLAY) {

				if (!gOs2propMappings.ContainsKey(go)) { //if not already existant	
					
					if (!precacheGameobjects) {
						//if you dont need to precache:
						gOs2propMappings.Add(go, new Object2PropertiesMapping(go,true,null,0,"",childIdentificationMode));
					} else {
					
						//lock (precachingLock) {
							//prepare to precache:
							prefabLoadPath = generateCachePath(go.name, prefabLoadPath);
							
#if UNITY_EDITOR || !(UNITY_STANDALONE_OSX || UNITY_DASHBOARD_WIDGET || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY || UNITY_WP8 || UNITY_METRO)
							
							cacheGOinResources(go,prefabLoadPath);
						
							Transform[] children = go.transform.GetComponentsInChildren<Transform>();
						
							foreach(Transform t in children) {
								
								if (t != go.transform)
									cacheGOinResources(t.gameObject,"");
							}
#endif							

							//add to map
							gOs2propMappings.Add(go, new Object2PropertiesMapping(go,true,null,0, prefabLoadPath,childIdentificationMode));
						
						//}
						
					}
				
					if (!markedGameObjects.Contains(go)) { //still needed?
						markedGameObjects.Add(go);
					}					
				
				} else 
					if (showHints)
						print("EZReplayManager HINT: GameObject '"+go+"' has already been marked for recording.");
	
			} else 
				if (showWarnings)
					print("EZReplayManager WARNING: You cannot mark GameObject '"+go+"' for recording while a recording is being played.");
		}	else {
			if (showWarnings)
				print("EZReplayManager WARNING: You cannot mark GameObject '"+go+"' for recording while in replay mode.");
			
		}
	}		
		
	//mark an object for recording, can be done while recording and while script is nonactive, but not while replaying a recording
	//v1.5: legacy function for not saving the recording
	public void mark4Recording(GameObject go, string prefabLoadPath) {
		mark4Recording(go, prefabLoadPath, ChildIdentificationMode.IDENTIFY_BY_ORDER);
	}
	
	//mark an object for recording, can be done while recording and while script is nonactive, but not while replaying a recording
	//v1.5: legacy function for not saving the recording
	public void mark4Recording(GameObject go) {
		mark4Recording(go, "");
	}
	
	public int getMaxFrames(Dictionary<GameObject, Object2PropertiesMapping> go2o2pm) {
		int maxframes = 0;
		foreach(var entry in go2o2pm) {
			int tmp = entry.Value.getMaxFrames();
			if (tmp > maxframes)
				maxframes = tmp;
		}
		return maxframes;
	}
	
	public int getMaxFrames(Object2PropertiesMapping o2pm) {
		return o2pm.getMaxFrames();
	}	
	
	//switch to different mode.. so far there are MODE_LIVE for viewing a normal game action and MODE_REPLAY for viewing a replay of a recording
	public void switchModeTo(ViewMode newMode) {
		
		if (newMode == ViewMode.LIVE ) {
				
			sendCallback2All("__EZR_live_prepare",null);
			
			//reset game object (i.e. rigidbody state)
			foreach(KeyValuePair<GameObject,Object2PropertiesMapping> entry in gOs2propMappings) {
				
				//GameObject go = entry.Key;
				Object2PropertiesMapping propMapping = entry.Value;
				if (propMapping.getGameObject() != null)
					propMapping.resetObject();
				
			}	
			
			bool tmpWasLoading = false;
			if (isLoadingSlotInUse()) {
				tmpWasLoading = true;
			}
			
			useRecordingSlot();
			
			if (tmpWasLoading) //repeat to avoid a bug
				switchModeTo(ViewMode.LIVE);
			
			//COUNTFRAMES
			maxPositions = getMaxFrames(gOs2propMappings);
				
			
			sendCallback2All("__EZR_live_ready",null);
			
		} else {
				
			sendCallback2All("__EZR_replay_prepare",null);
			
			if (maxPositions > 0) {
				//prepare parents first
				foreach(KeyValuePair<GameObject,Object2PropertiesMapping> entry in gOs2propMappings) {
					//GameObject go = entry.Key;
					Object2PropertiesMapping propMapping = entry.Value;
					if (propMapping.isParent()) {
						//if (propMapping.getGameObject() != null)
							propMapping.prepareObjectForReplay();
					}
				}		
				//..then childs
				foreach(KeyValuePair<GameObject,Object2PropertiesMapping> entry in gOs2propMappings) {
					//GameObject go = entry.Key;
					Object2PropertiesMapping propMapping = entry.Value;
					if (!propMapping.isParent()) {
						//if (propMapping.getGameObject() != null)
							propMapping.prepareObjectForReplay();	
					}
				}	
					
				sendCallback2All("__EZR_replay_ready",null);	
					
			} else {
				newMode = ViewMode.LIVE;
				if (showWarnings)
					print("EZReplayManager WARNING: You have not recorded anything yet. Will not replay.");
			}
			
		}
		
		currentMode = newMode;
		stop();
	}
	
	//stopping is essential to all other actions for resetting settings before switching
	public void stop() {
		
		continueCallingUpdate = false;
		currentAction = ActionMode.STOPPED;
		timeelapsed=0.0f;
		if (orgRecorderPositionStep < 0)
			recorderPosition = maxPositions;
		else
			recorderPosition = 0;

		surplus = 0.0f;
		
		
		execRecorderAction();		
		
		sendCallback2All("__EZR_stop",null);			
	}
	
	//mark the game objects from the public gui list for recording. Can be filled in Unity3d GUI to avoid coding.
	protected void markPredefinedObjects4Recording() {
		
		for (int i=0;i<gameObjectsToRecord.Count;i++) {
			mark4Recording(gameObjectsToRecord[i]);
		}

	} 	
	
	//this starts the recording of objects, which have been marked for recoring previously or while recording in progress
	public void record() {		
		
		//markPredefinedObjects4Recording(); //mark the game objects from the public gui list for recording
		
		if (currentMode == ViewMode.LIVE) {
			if (currentAction == ActionMode.STOPPED) {
				
				//remove a previous recording
				foreach(KeyValuePair<GameObject,Object2PropertiesMapping> entry in gOs2propMappings) {
					//GameObject go = entry.Key;
					Object2PropertiesMapping propMapping = entry.Value;
					propMapping.clearStates();
				}
				
				//reset everything to standard values
				recorderPosition = 0;
				recorderPositionStep = 1;
				orgRecorderPositionStep = 1;
				//set new action
				currentAction = ActionMode.RECORD;
				continueCallingUpdate = true;
				updateRecording(currentAction);
				
				sendCallback2All("__EZR_record",null);
				
			} else {
				if (showWarnings)
					print("EZReplayManager WARNING: Ordered to record when recorder was not in stopped-state. Will not start recording.");
			}
		} else {
				if (showWarnings)
					print("EZReplayManager WARNING: Ordered to record when recorder was in replay mode. Will not start recording.");			
		}
		
		
	}	
	//halt a replay
	public void pause() {
		if (currentMode == ViewMode.REPLAY) {
			currentAction = ActionMode.PAUSED;
			setReplaySpeed(minSpeedSliderValue);
			
			sendCallback2All("__EZR_pause",null);
		} else
			if (showWarnings)
				print("EZReplayManager WARNING: Ordered to pause when recorder was not in replay mode. Will not pause.");
		
	}
	//simple wrapper for the play-method
	public void play(int speed) {
		play(speed,false,false,false);
	}
	
	//replays a recording. 
	public void play(int speed, bool playImmediately, bool backwards, bool exitOnFinished) {
		
#if IS_UNLICENSED
		speed = 0;			
#endif
			
		//switch to correct mode
		if (currentMode!=ViewMode.REPLAY) {
			switchModeTo(ViewMode.REPLAY);
		}
		
		if (speed>=minSpeedSliderValue && speed<=maxSpeedSliderValue)
			speedSliderValue = speed;
		else
			speedSliderValue = 0;

		//revert playing direction if neccessary
		if ((backwards && orgRecorderPositionStep > 0) || (!backwards && orgRecorderPositionStep < 0)) {
			orgRecorderPositionStep *= -1;
		}
		//set playing speed
		setReplaySpeed(speedSliderValue);
				
		if (currentAction == ActionMode.STOPPED || currentAction == ActionMode.PAUSED) {

			if (currentAction != ActionMode.PAUSED)
				stop();
			
			if (playImmediately)
				currentAction = ActionMode.PLAY;
			
			this.exitOnFinished = exitOnFinished;
			continueCallingUpdate = playImmediately;
			updateRecording(currentAction);			
			
			sendCallback2All("__EZR_play",null);
			
		} else 
			if (showHints)
				print("EZReplayManager HINT: Ordered to play when not in stopped or paused state.");
	}
	
	public int getCurrentPosition() {
		return recorderPosition;
	}
	
	public ActionMode getCurrentAction() {
		return currentAction;
	}	
	
	public ViewMode getCurrentMode() {
		return currentMode;
	}
	
	void Awake() {
		instantiateSingleton();	
			
		orgRecorderPositionStep = recorderPositionStep;
			
		//useRecordingSlot();
		
		markPredefinedObjects4Recording();
	}
	
	// Use this for initialization
	void Start () {
		//markPredefinedObjects4Recording();
		
	}
	
	//execute one cycle of the current action on the current recorder position
	protected void execRecorderAction() {
		
		if (gOs2propMappings != null)
		foreach(KeyValuePair<GameObject,Object2PropertiesMapping> entry in gOs2propMappings) {
			GameObject go = entry.Key;
			Object2PropertiesMapping propMapping = entry.Value;
			
			if (currentAction == ActionMode.RECORD && currentMode == ViewMode.LIVE) { //if recording
				
#if IS_UNLICENSED
                if (recorderPosition <= (100 * (int)ActionMode.STOPPED) / (int)ActionMode.PAUSED )
                {
#endif
                    maxPositions = recorderPosition;
                    propMapping.insertStateAtPos(recorderPosition);
#if IS_UNLICENSED

                }
                else
                {

                    showingStoppedRecordingMsg = true;
                    StartCoroutine(exitStoppedRecordingMsg(5f));
                    stop();

                }
#endif
				
			} else if (currentMode == ViewMode.REPLAY) { //if replaying
				//if in between start and finish position
				if ((recorderPosition <= maxPositions && orgRecorderPositionStep>0) || (recorderPosition > 0 && orgRecorderPositionStep<0) ) {
					
					//lerping not integrated yet
					//float updateSyncTime = Time.realtimeSinceStartup;
					//float lerpInterval = interval - ((updateSyncTime - updateStartingTime) % interval) ;
					if (propMapping.getGameObjectClone() != null)
						propMapping.synchronizeProperties(recorderPosition);
						
				} else { //else if reached the finishing position
					stop();
					
					if (exitOnFinished)
						switchModeTo(ViewMode.LIVE);
				}
			}
		}		
	}
	
#if IS_UNLICENSED
    private IEnumerator exitStoppedRecordingMsg(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        showingStoppedRecordingMsg = false;
    }	
#endif
		
	protected IEnumerator startShowPrecachingMandatoryMessage(float seconds)
    {
		showPrecachingMandatoryMessage = true;
        yield return new WaitForSeconds(seconds);
        showPrecachingMandatoryMessage = false;
    }
	
	//update recording/replaying position and start timer to next update
	protected void updateRecording(ActionMode action) {

		float updateStartingTime = Time.realtimeSinceStartup;
		bool mayBeNull = false;
		
		float interval = recordingInterval;		
				
		if (currentAction != ActionMode.STOPPED && action == currentAction) { //if action has not changed sinds last update
			
			if (currentMode == ViewMode.REPLAY) {
				interval = playingInterval;
			}			
			//execute current recorder action
			execRecorderAction();
			
			float updateEndingTime = Time.realtimeSinceStartup;
	
			if ((updateEndingTime - updateStartingTime) < interval) { // if updating didn't take longer than the current interval
				//substract surplus during more than one frame cycle to come to zero surplus
				
				float surplusToEliminate = 0.0f;
				if (surplus > 0.0f) { //if there is interval surplus..
					//..it has to be eliminated
					surplusToEliminate = (interval - (updateEndingTime - updateStartingTime));
					
					if (surplusToEliminate > surplus)
						surplusToEliminate = surplus;
				}
				//determine interval to next update
				interval -= (updateEndingTime - updateStartingTime) - surplusToEliminate;

				mayBeNull = true;
				if (surplusToEliminate > 0.0f) { 
					surplus -= surplusToEliminate;
					
					if (surplus < 0.0f)
						surplus = 0.0f;
					
				}
				
			} else { //if updating took longer than the interval
				surplus += (updateEndingTime - updateStartingTime) - interval; //..add to surplus

				//update immediately
				interval = 0.0f;
				mayBeNull = true;
			}

			/*timeelapsed += (updateEndingTime - updateStartingTime) + surplus; //<-- uncomment to make timeelapsed global
			print("timeelapsed: "+timeelapsed);*/
			//timeelapsed = (updateEndingTime - updateStartingTime); //<-- uncomment to make timeelapsed only about this cycle
			
			
			if ((((recorderPosition + recorderPositionStep > -1) ) ||  
				((recorderPosition + recorderPositionStep < maxPositions) )) && continueCallingUpdate) //if in the "middle of something"
				//should be the only place where to increase recorderPosition
				recorderPosition+=recorderPositionStep;	
			else
				stop();	//stop on finishing an action
			
			if ((interval > 0.0f || mayBeNull) && currentAction!=ActionMode.PAUSED 
				&& currentAction!=ActionMode.STOPPED && continueCallingUpdate) { //if another update can be done
				
				StartCoroutine(waitForNewUpdate(interval,timeelapsed,action)); //don't ignore surplus
				//StartCoroutine(waitForNewUpdate(playingInterval,timeelapsed,action)); //ignore surplus
			} 	
		}

	}
	
	//there is an interval to wait for before the update will be done
	protected IEnumerator waitForNewUpdate(float delay, float timeelapsed, ActionMode action) {
		yield return new WaitForSeconds(delay);
		
		if (currentAction != ActionMode.STOPPED) 
			timeelapsed += delay;
		//print("timeelapsed: "+timeelapsed);
		if (continueCallingUpdate)
			singleton.updateRecording(action);

	}
	
	//HINT: lerping is still highly experimental, to use/develope enable in SavedState.cs
	//source: http://answers.unity3d.com/questions/14279/make-an-object-move-from-point-a-to-point-b-then-b.html
	public IEnumerator MoveTo(Transform transform, Vector3 target, float time)
	{
	    Vector3 start = transform.position;
	    float t = 0;
		time = playingInterval;	
	
	    while(t < 1)
	    {
	        yield return null;
	        t += Time.deltaTime / time;
	        transform.position = Vector3.Lerp(start, target, t);
	    }
	    transform.position = target;
	}		
	
	//bad function name: not only sets replay speed but also returns a string describing the replaying speed relative to the recording speed
	protected string setReplaySpeed(int speed) {
		
		string ret="";

		if (speed == minSpeedSliderValue) {
			
			playingInterval = 0.0f;
			recorderPositionStep = orgRecorderPositionStep;
			currentAction = ActionMode.PAUSED;
			ret = "Paused";
		} else if (speed > speedSliderValueBackup) {
			
			playingInterval = singleton.recordingInterval;
			
			int increaser = 1;
			if (orgRecorderPositionStep < 0)
				increaser = -1;
			
			recorderPositionStep = (orgRecorderPositionStep * speed)+increaser;
			int multiplicator = (int)Mathf.Round(recorderPositionStep / orgRecorderPositionStep);
			ret = "~ "+multiplicator + "x faster";
		} else if (speed < speedSliderValueBackup) {
			
			playingInterval =  singleton.recordingInterval * ((speed-1)*-2);
			recorderPositionStep = orgRecorderPositionStep;
			int divisor = (int)Mathf.Round(playingInterval /singleton.recordingInterval);
			ret = "~ "+divisor + "x slower";
		}  else /*if (speed == sliderValueBackup)*/ {
			
			playingInterval = singleton.recordingInterval;
			recorderPositionStep = orgRecorderPositionStep;
			ret = "~ Recording speed";
		}

		return ret;
	}

	//build EZ Replay Manager GUI 
	void  OnGUI (){
		
        GUIStyle style = GUI.skin.GetStyle("box");
        style.fontStyle = FontStyle.Bold;		
			
#if IS_UNLICENSED	
		if (showFullVersionLicensingInfo) {
		
		    if (showingStoppedRecordingMsg)
	        {
	            GUI.Box(new Rect(Screen.width / 2 - (400 / 2), Screen.height / 2 - (40 / 2), 400, 40), "LIMITED VERSION - stopped recording after 150 frames", style);
			}
					
			GUI.Box(new Rect(0, Screen.height - 50, Screen.width, 25), "EZ Replay Manager LIMITED VERSION - Hide this watermark by disabling \"Show licensing info\" in the EZReplayManager-prefab");
			
            if(GUI.Button(new Rect(0, Screen.height - 25, Screen.width, 30), "Click here for full unsealed source code, unlimited recording time, changing replay speed, and saving whole replays to filesystem.")) {
			    Application.OpenURL("https://www.assetstore.unity3d.com/#/content/690");	
			}
            
        }
			
#endif		
			
		if (showPrecachingMandatoryMessage) {
				
			GUI.Box(new Rect(Screen.width / 2 - (650 / 2), Screen.height / 2 - (40 / 2), 650, 40), "To use this functionality, please enable precaching of game objects (in EZReplayManager prefab)", style);	
		}
		
		if (currentMode == ViewMode.LIVE && useRecordingGUI) {
			Rect r0;
			
			if (currentAction == ActionMode.RECORD || (maxPositions > 0))
				r0 = new Rect(10,20,150,200);
			else
				r0 = new Rect(10,20,150,170);
			
			GUI.Box ( r0, EZRicon);
			
			Rect r1 = new Rect(20,160,130,20);
			Rect r2 = new Rect(20,190,130,20);
				

			if (currentAction == ActionMode.STOPPED) {
				
				if (GUI.Button ( r1, startRecordIcon)) {
					record ();
				}	
				
			} else if (currentAction == ActionMode.RECORD) {
				
				if (GUI.Button ( r1, stopRecordIcon)) {
					stop ();
				}

			}
			
			if (maxPositions > 0)
			if (GUI.Button ( r2, replayIcon)) {
				stop();
				play(0, false,false,false);
			}		
			
			
		} else if (currentMode == ViewMode.REPLAY) {

			if (useDarkStripesInReplay) {
				GUI.Box (new Rect (0,0,Screen.width , 100),EZRicon);
				
				GUI.Box (new Rect (0,Screen.height-100, Screen.width , Screen.height-100),"");	
			}
			
			if (useReplayGUI) {
					
				//<!-- SPEED SLIDER
				bool stopped = false;
				
				if (speedSliderValue <= minSpeedSliderValue)
					 stopped = true;		
#if !IS_UNLICENSED				
				speedSliderValue = (int)GUI.HorizontalSlider(new Rect(10,60,120,70),speedSliderValue,minSpeedSliderValue,maxSpeedSliderValue);
#endif				
				//setReplaySpeed: bad function name
				string speedIndicator = setReplaySpeed(speedSliderValue);
				
				if ( stopped && playingInterval > 0.0f) {
					 stopped = false;
				}
				
				GUI.Box (new Rect (10,10,130, 45),"Replay speed:\n"+speedIndicator); //+ "\n, interval: "+playingInterval+", step: "+recorderPositionStep);
		
				// SPEED SLIDER //--> 
	
				//<!-- TIME SLIDER 
				int recorderPositionTemp = (int)GUI.HorizontalSlider(new Rect(150,60,240,10),recorderPosition,0,maxPositions);

				if (recorderPositionTemp != recorderPosition) {
					pause();
					recorderPosition = recorderPositionTemp;
					singleton.execRecorderAction();
				}
				
				float percentage = Mathf.Round(((float)recorderPosition / (float)maxPositions) * 100.0f);
				GUI.Box (new Rect (150,10,240, 45),"Replay position: "+percentage+"%");
				// TIME SLIDER //--> 
				
				//<!-- POSITION MANIPULATION TOOLS 
				if (GUI.Button (new Rect (158,72,40, 23),playIcon)) {
					play(speedSliderValue,true,false,false);
				}
			
				if (GUI.Button (new Rect (203,72,40, 23),rewindIcon)) {
					play(speedSliderValue,true,true,false);
				}
			
				if (GUI.Button (new Rect (248,72,40, 23),pauseIcon)) {
					pause();
				}	
					
				if (GUI.Button (new Rect (293,72,40, 23),stopIcon)) {
					stop();
				}				
				
				if (GUI.Button (new Rect (338,72,40, 23),closeIcon)) {
					switchModeTo(ViewMode.LIVE);
				}				
				// POSITION MANIPULATION TOOLS //-->
			}
			
		}
	}
	
	void OnLevelWasLoaded() {
		stop();		
	}	
	
	//returns the only instance of EZReplayManager. Don't put this script on just some GameObject but instantiate the prefab which comes with the package.
    public static EZReplayManager get
    {
        get
        {
            if (singleton == null)
            {
				GameObject ezr = GameObject.Find("EZReplayManager");
                singleton = ezr.GetComponent(typeof(EZReplayManager)) as EZReplayManager;
            }
            return singleton;
        }
    }	
}