# Project 41: Gesture set development for an AR/VR environment for building site consenting

# Prerequisite 
* Follow this [tutorial](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/unity/choosing-unity-version#unity-20203-lts-recommended), but install **Unity 2019**.
* Follow this installation [checklist](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/install-the-tools) for the **windows 10** and **Visual Studio** set up. An emulator is not required.
# Build and Deployment
## To Build
1) Open project in Unity.
2) File > Build Settings
3) Add the open scene to the build and press build

## To Deploy
1) Open build solution file that was generated from Unity in Visual Studio.
2) In the solution explorer, right click, Publish > App Packages
3) Select sideloading ![image](https://user-images.githubusercontent.com/52563454/127789465-10a35a51-b5da-4a62-935d-35b5e0e612cc.png)
4) 
![image](https://user-images.githubusercontent.com/52563454/127789449-d3d79583-2180-4377-81a8-e2a3bd6b5ada.png)
5) Select only ARM64 and Always for bundle and press Create ![image](https://user-images.githubusercontent.com/52563454/127789568-7d63ac9b-e632-4b7d-8aab-f8c213310747.png)
6) In Device Portal, Apps>Local Storage, select the generated .appxbundle file then Install

# Extracting Tutorial Learning Times
1) Open the Device Portal, File Explorer > LocalAppData > Project41... > LocalState
![image](https://user-images.githubusercontent.com/52563454/137607925-0b32ee4d-901e-49fa-b6e2-62ecb0fb87e4.png)
2) Download the files, sometimes the download won't work on Chrome so use another browser such as Firefox or Internet Explorer.

# Release
User testing release is on branch  `release/user-testing-scenario`.

It is also under the release tab https://github.com/annithinggoes/project-41/releases/tag/v1.0
