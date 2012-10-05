using UnityEngine;
using System.Collections;

public class WindowIDManager{
	public static int ID{
		get{
			return id++;
		}
	}
	static int id = 0;
}
