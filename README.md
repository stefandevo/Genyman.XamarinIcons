# Stefandevo.Genyman.XamarinIcons
Based upon a single SVG source create all icons for iOS, Android, UWP
## Getting Started
Stefandevo.Genyman.XamarinIcons is a **[genyman](http://genyman.net)** code generator. If you haven't installed **genyman** run following command:
```
dotnet tool install -g genyman
```
_Genyman is a .NET Core Global tool and thereby you need .NET Core version 2.1 installed._
## New Configuration file 
```
genyman new Stefandevo.Genyman.XamarinIcons
```
## Sample Configuration file 
```
{
    "genyman": {
        "packageId": "Stefandevo.Genyman.XamarinIcons",
        "info": "This is a Genyman configuration file - https://genyman.github.io/docs/"
    },
    "configuration": {
        "platforms": [
            {
                "type": "iOS",
                "projectPath": "YourProject.iOS",
                "iconFileName": "YourIOSIconFile.svg"
            },
            {
                "type": "Android",
                "projectPath": "YourProject.Droid",
                "iconFileName": "YourAndroidIconFile.svg",
                "androidOptions": {
                    "assetFolderPrefix": "mipmap"
                }
            }
        ]
    }
}
```
## Documentation 
### Class Configuration
| Name | Type | Req | Description |
| --- | --- | :---: | --- |
| Platforms | PlatformClass[] | * | List of platforms |
### Class PlatformClass
| Name | Type | Req | Description |
| --- | --- | :---: | --- |
| Type | Platforms (Enum) | * | Platform |
| ProjectPath | String | * | Path to the platform, do not include filename |
| IconFileName | String | * | Filename (svg) of the file to be used |
| AndroidOptions | AndroidOptions |  | Extra Android options |
### Enum Platforms
| Name | Description |
| --- | --- |
| iOS | iOS |
| Android | Android |
| UWP | UWP |
| AppleWatch | Apple Watch |
| MacOs | Mac OS |
### Class AndroidOptions
| Name | Type | Req | Description |
| --- | --- | :---: | --- |
| AssetFolderPrefix | AndroidResourceFolder (Enum) |  | The folder where resources are |
### Enum AndroidResourceFolder
| Name | Description |
| --- | --- |
| mipmap | Use mipmap folders |
| drawable | Use drawable folders |
