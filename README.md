# LBPMusicReader
Tool to read and convert LBP2/LBP3 Sequencer data 

## How to use
This tool is VERY VERY Work in progress so there WILL be issues

First, compile this tool using Visual Studio.

Using [Craftworld Toolkit](https://github.com/ennuo/toolkit), import the relevant data into it. For most cases, this will be your save game that has the object you want to get the sequencer data out of.

Find the object either under the `items > user_object` or `levels` folders, right click, and go to Extract, and click JSON. Save that to a folder of your choosing.

Then, drag that JSON into the LBPMusicReader.exe file. If everything goes well, the tool should generate one or move sequencer_x.mid files.