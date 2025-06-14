# OARPriorityRemapper
2 console applications to assist in reordering the priority of Open Animation Replacer mods.
OARPriorityReader reads and outputs base files for each of the found OAR mods. You can then edit those outputted files to have OARPriorityRemapper to remap the OAR config files.

## Requirements
- [.Net 9 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) its the same as Synthesis

## Installation
1. Download and install [.Net 6 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Extract this anywhere except into Skyrim's Data folder
3. If using MO2 set both exes as a executables and add your data folder path to the arguments

## How to Use
1. Run OARPriorityReader.exe and let it generate all the OAR files
2. Open "[Install folder]\OARBaseData".
3. Select any of the _OARPriorirty.csv and copy them to the other created folder "[Install folder]\OAROverrideData".
4. Open and edit them using notepad or anything you want.
   - I prefer Excel, Excel can automate changing the numbers.
5. Run OARPriorityRemapper.exe
   - It will check if the mod was updated and/or has already had OARPriorityRemapper change something.
   - If it was updated and the folder names or prioritys were changed it will not run the _OARPriorirty.csv
   - If OARPriorityRemapper has already changed something it will not run the _OARPriorirty.csv

## Tools
- Visual Studio
- Notepad++
- Excel
