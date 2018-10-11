
Photon Unity Networking (PUN)
	This package includes the Photon Unity Networking Api, the Realtime Api (on which PUN is based).
	Also included: A setup wizard, demo scenes, documentation and Editor extensions.


PUN Free versus PUN+
	While the PUN package is essentially free, we offer a PUN+ package which includes a Photon Cloud 
    "Subscription" at for a one-time fee. See below how this is applied.
    The content of the PUN Free and PUN+ packages are identical.


UnityScript / JavaScript
    PUN is written with C# in mind primarily.
    We do not support UnityScript with PUN v2 and up.


Help and Documentation
	Please read the included chm (or pdf).
	Exit Games Forum:       https://forum.photonengine.com/categories/unity-networking-plugin-pun
	Online documentation:   https://doc.photonengine.com/en-us/pun/v2
    Unity Forum Thread:     https://forum.unity3d.com/threads/photon-unity-networking.101734/


Integration
	This package adds an Editor window "PUN Wizard" for connection setup:
		Menu -> Window -> Photon Unity Networking (shortcut: ALT+P)
	It also adds a commonly used component "PhotonView" to this menu:
		Menu -> Component -> Miscellaneous -> PhotonView (shortcut: ALT+V)
	When imported into a new, empty project, the "PunStartup" script opens the "demo hub" and setup scenes to build.


Clean PUN Import (no demos)
	To get a clean import of PUN, just skip all folders named "Demos" and "UtilityScripts".
    UtilityScripts can be useful for rapid prototyping but are optional to use.


Server
	Exit Games Photon can be run on your servers or you can subscribe to the Photon Cloud for managed servers.
	
	The window "Photon Unity Networking" will help you setup a Photon Cloud account.
	This service is geared towards room-based games and the server cannot be modified.
	Read more about it: https://www.photonengine.com

	Alternatively, download the Server SDK and run your own Photon Server.
	The SDK has the binaries to run immediately but also includes the source code and projects
	for the game logic. You can use that as basis to modify and extend it.
	A 100 concurrent user license is free (also for commercial use) per game.
	Read more about it: https://www.photonengine.com/en-us/OnPremise


PUN+ Subscriptions
    Follow these steps when you bought an asset that includes a Photon Cloud subscription:
        • Use an existing Photon Cloud Account or register.     https://www.photonengine.com/Account/SignUp
        • Sign in and open the Dashboard.                       https://dashboard.photonengine.com
        • Select the Subscription to upgrade and click "Apply Unity Purchase".
        • Enter your Unity Invoice Number and Photon App ID.
        
        • You find the App ID on: https://dashboard.photonengine.com
        • You find your Unity Invoice Number in the Unity AssetStore: 
            https://www.assetstore.unity3d.com/en/#!/account/transactions
            Or while logged in to the Asset Store, click on your name on the top right. 
            From the drop-down select the payment method you used in your purchase.
            Navigate to your purchase and copy the number following the "#" symbol (excluding the "#" and spaces).


Important Files

	Documentation
		PhotonNetwork-Documentation.chm (a pdf is also included)
		changelog.txt

	The server-setup will be saved as file (can be moved into any Resources folder and edited in inspector)
		Photon\PhotonUnityNetworking\Resources\PhotonServerSettings.asset

	Demos
		All demos are in separate folders in Photon\PhotonUnityNetworking\Demos\. Delete this folder in your projects.
		Each has a Demo<name>-Scene.
