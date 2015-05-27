using UnityEngine;
using System.Collections;

public enum ChildIdentificationMode {

	IDENTIFY_BY_ORDER = 0, //useful if children hierarchy doesn't change during the game and/or children have same names
	IDENTIFY_BY_NAME = 1 //useful if children order changes during the game, give all children different names for this to work
}
