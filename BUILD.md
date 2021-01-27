# Building Guide

## Dependencies

The dependencies is split into two categories by license.

### Free (Libre) & Open Source (F(L)OSS)

| Name                  | Author           | License     | Install From                                                 |
| --------------------- | ---------------- | ----------- | ------------------------------------------------------------ |
| Random Name Generator | m4bwav           | Expat (MIT) | NuGet Restore                                                |
| JSON.NET              | James Newtonking | Expat (MIT) | NuGet Restore                                                |
| RAGE Native UI        | alexguirre       | Expat (MIT) | [GitHub Repo](https://github.com/alexguirre/RAGENativeUI) (Included) |

### Proprietary, or Closed Source (Non-Free)

| Name                 | Author          | License     | Install From                                  |
| -------------------- | --------------- | ----------- | --------------------------------------------- |
| RAGE Plugin Hook SDK | MulleDK19 & LMS | Proprietary | [Official Website](http://ragepluginhook.net) |

### How to Install Dependencies 

* If the "Install From" option shows Included, then you don't have to do anything - it's already installed with the project.
* If the "Install From" option is NuGet Restore, then the only thing you need to do to install that dependency is simply restore NuGet packages.

* If the "Install From" option is a link, then you need to download it from that link and extract the corresponding assembly to the corresponding folder:
  * If it's license is some kind of FLOSS license (either OSI approved license or these "free" licenses listed by FSF), copy it into `OpenSourceReferences` folder.
  * If it's license is some kind of proprietary license (prohibits redistribution), you need to create a `References` folder (if it does not exists), and place it into that folder.
  * It's recommended to copy the `xml` file as the same name of the assembly (for example, RAGE plugin hook SDK has `RagePluginHookSDK.xml`) to receive IntelliSense documentations.

## Building

You have two options, to either:

* Build the project from Visual Studio IDE
* Build the project via command-line

### Build from IDE

Double-click the solution file to open it in the IDE, and Build the project.\

### Build from Command Line

1. Open the Developer Command Prompt from Start Menu.
2. Navigate to the project folder.
3. Type `msbuild`.

