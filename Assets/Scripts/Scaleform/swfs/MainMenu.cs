/**********************************************************************

Filename    :	UI_Scene_Demo1.cs
Content     :  
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/

using System;
using System.Collections;
using UnityEngine;
using Scaleform;
using Scaleform.GFx;

public class MainMenu : Movie
{
    protected Value	theMovie = null;
	private MenuCamera parent = null;
    
    public MainMenu(MenuCamera parent, SFManager sfmgr, SFMovieCreationParams cp) :
        base(sfmgr, cp)
    {
		this.parent = parent;
        SFMgr = sfmgr;
        this.SetFocus(false);
    }

	public void StartServer() {
		GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartServer();
	}
	public void StartClient() {		
		GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartClient();
	}

	public void Ready() {
		GameObject.Find("NetworkManager").GetComponent<NetworkManager>().networkView.RPC("ReadyHandshake",RPCMode.All);
	}
	public void ReadyChecked() {
		theMovie.GetMember("waiting").SetBool(true);
	}
	public void ReadyGo() {
		GameObject.Find("NetworkManager").GetComponent<NetworkManager>().currentState = (int)NetworkManager.NetworkState.playing;
	}
}
	
	