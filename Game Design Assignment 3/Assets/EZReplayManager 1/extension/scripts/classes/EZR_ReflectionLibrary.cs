using UnityEngine;
using System.Collections;

public static class EZR_ReflectionLibrary {

	//source: http://answers.unity3d.com/questions/295327/if-function-exists-in-script.html
	public static bool functionExist(string functionName) {
	 	System.Type T = EZReplayManager.get.GetType();
		foreach(System.Reflection.MethodInfo _m in T.GetMethods())
		{
			if(_m.Name == functionName)
			{
				//_m.Invoke(EZReplayManager.get,null);
				return true;
			}
		}
		return false;
	}	
}
