## BROKEN CURRENTLY (26/12/2023 21:03), currently fixing: Please come back tomorrow! 

## Table of contents
* [General info](#general-info)
* [Warning](#warning)
* [Supported devices](#supported-devices)
* [How to get your device to be supported](#how-to-get-your-device-to-be-supported)
* [Setup](#setup)
* [Technologies](#technologies)
## General info
Repurposes any button on (theoretically) any wireless headset ([Supported devices](#supported-devices)) to do 3 different actions that you can programm yourself. My personal favourite is that I can now
walk around my room and still pause/skip music!

## Warning
The original functionality is handled locally on the headset itself so it still gets triggered.
Because of that I would advise you to only use this tool if you can live without the origianal functionality. For example, I don't use my microphone on my headset, so I can tolerate not having the original mute function.

## Supported devices
[How to get your device to be supported](#how-to-get-your-device-to-be-supported)
* Windows only
* HyperX Cloud II Wireless (DTS Version)

Sadly it's only one at the moment, but if you want to use this tool and your device is not supported, feel free to contact me because I have to adjust the application to each device individually (you could e.g open an issue here on github).

## How to get your device to be supported
* Execute WBR-Tester.exe ([Releases](https://github.com/TizianGuth/Wireless-Button-Reprogrammer/releases)) and follow the instruction.
* Contact me as told in the instructions.

## Setup
### Download
You can donload the .exe here on github under "Releases"
### How to find you Vendor and Product ID:
To find your 2 IDs you basically have 2 ways of doing so: 1. Trial and error using your device manager (not advised) or 2. using a third party tool like [Busdog](https://github.com/djpnewton/busdog). Here you will only find documentation how to find your IDs via busdog .\

1. Install and open [Busdog](https://github.com/djpnewton/busdog). Here you will have to check "Automaticly trace new Devices" on the bottom left hand corner. \
![BusdogTraceNew](https://github.com/GuthiYT/hyperxrebutton/blob/main/doc/img/busdog_trace_new.png)

2. Make sure every box is unchecked then un- and replug your headset's USB dongle. Now one Device should be checked with its multiple "sub-devices". Now hover over one of them, look for the 4 letterrs after "VID_" and "PID_" and write them down somewhere. \
![BusdogDevice](https://github.com/GuthiYT/hyperxrebutton/blob/main/doc/img/busdog_device.png)

4. Now just input your VID and PID in the program, click "Apply" and "Start".

### How to change keycodes
To change what happens after each click, refer to this [Site](https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes) to get your desired
keycodes, then enter them in the program, click "Apply" and "Start".


## Technologies
* Visual Studio's WPF
* .NET 4.7.2
* [AudioManager](https://gist.github.com/sverrirs/d099b34b7f72bb4fb386)
* [HIDLibrary](https://github.com/mikeobrien/HidLibrary)
	
