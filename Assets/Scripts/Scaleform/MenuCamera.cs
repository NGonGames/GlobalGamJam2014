
/**********************************************************************

Filename    :   MyCamera.cs
Content     :   Inherits from SFCamera
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Collections;
using Scaleform;

/* The user should override SFCamera and add methods for creating movies whenever specific events take place in the game.
*/
public class MenuCamera : SFCamera {
	
	////////////////
    
	//
	new public void Awake()
	{
		
	}
	
    // Hides the Start function in the base SFCamera. Will be called every time the ScaleformCamera (Main Camera game object)
    // is created. Use new and not override, since return type is different from that of base::Start()
    new public  IEnumerator Start()
    {
			// The eval key must be set before any Scaleform related classes are loaded, other Scaleform Initialization will not 
			// take place.
			#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR) && !UNITY_WP8
			SF_SetKey("ES6F23QQA2UAQ81QUROOOYJQ6EWAYEQ4VNATLQ8IMQI2Q6IASZT4O3KM87PYD9R");
			#elif UNITY_IPHONE
			SF_SetKey("");
			#elif UNITY_ANDROID
			SF_SetKey("");
			#elif UNITY_WP8
			sf_setKey("");
			#endif
			
			//For GL based platforms - Sets a number to use for Unity specific texture management.  Adjust this number if
			//you start to experience black and/or mssing textures.
			#if UNITY_WP8
			sf_setTextureCount(500);
			#else
			SF_SetTextureCount(500);
			#endif
			return base.Start();
    }

    // Application specific code goes here
    new public void Update()
    {
		base.Update ();
    }

	public MainMenu AddMovie(String swfName) {		
		SFMovieCreationParams creationParams = CreateMovieCreationParams(swfName);
		//     creationParams.TheScaleModeType  = ScaleModeType.SM_ShowAll;
		creationParams.IsInitFirstFrame = false;
		return new MainMenu(this, SFMgr, creationParams);
	}

}