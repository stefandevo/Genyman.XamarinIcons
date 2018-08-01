# Genyman.XamarinIcons
Based upon a single SVG source create all icons for iOS, Android, UWP, ... with a Genyman generator


```
Class: Configuration
--------------------

Name           Type                 Req      Description      
--------------------------------------------------------------
Platforms      PlatformClass[]      *        List of platforms

Class: PlatformClass
--------------------

Name                Type                  Req      Description                                  
------------------------------------------------------------------------------------------------
Type                Platforms (Enum)      *        Platform                                     
ProjectPath         String                *        Path to the platform, do not include filename
IconFileName        String                *        Filename (svg) of the file to be used        
AndroidOptions      AndroidOptions                 Extra Android options                        

Enum: Platforms
---------------

Name            Description
---------------------------
iOS             iOS        
Android         Android    
UWP             UWP        
AppleWatch      Apple Watch
MacOs           Mac OS     

Class: AndroidOptions
---------------------

Name                   Type                              Req      Description                   
------------------------------------------------------------------------------------------------
AssetFolderPrefix      AndroidResourceFolder (Enum)               The folder where resources are

Enum: AndroidResourceFolder
---------------------------

Name          Description         
----------------------------------
mipmap        Use mipmap folders  
drawable      Use drawable folders

```