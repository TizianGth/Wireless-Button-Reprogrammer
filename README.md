## Table of contents
* [General info](#general-info)
* [Warning](#warning)
* [Supported devices](#supported-devices)
* [Setup](#setup)
* [Latest update](#latest-update)
* [Technologies](#technologies)
## General info
Repurposes the mute button on the HyperX Cloud II Wireless (DTS) which you can use now to do 3 different actions that you can programm yourself. Now you can
walk around and still pause/skip music!

## Warning
Because the mute functionality is handled on the headset it self you'll still mute/unmute your headset's microphone.
Because of that I would advise you to only use this tool if you can live without your headset microphone.

## Supported devices
* Windows only
* HyperX Cloud II Wireless (DTS Version)

Sadly it's only one at the moment, but if you want to use this tool and your device is not supported, feel free to contact me (e.g open an issue here on github).

## Setup
### Download
You can donload the .exe here on github under "Releases"
### How to find you Vendor and Product ID:
To find your 2 IDs you basically have 2 ways of doing so: 1. Trial and error using your device manager (not advised) or 2. using a third party tool like [Busdog](https://github.com/djpnewton/busdog). Here you will only find documentation how to find your IDs via busdog .\

1. Install and open [Busdog](https://github.com/djpnewton/busdog). Here you will have to check "Automaticly trace new Devices" on the bottom left hand corner. \
![BusdogTraceNew](https://github.com/GuthiYT/hyperxrebutton/blob/main/doc/img/busdog_trace_new.png)

2. Make sure every box is unchecked then un- and replug your HyperX USB dongle. Now one Device should be checked with its multiple "sub-devices". Now hover over one of them, look for the 4 letterrs after "VID_" and "PID_" and write them down somewhere. \
![BusdogDevice](https://github.com/GuthiYT/hyperxrebutton/blob/main/doc/img/busdog_device.png)

4. Now just input your VID and PID in the program, click "Apply" and "Start".

### How to change keycodes
To change what happens after each click, refer to this [Site](https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes) to get your desired
keycodes, then enter them in the program, click "Apply" and "Start".

## Latest update:
### 0.1.2:
* Added function to configure the Volume step on the headset. Before you could only go in steps of 2 now you can set this to what ever you want.

## Technologies
* Visual Studio's WPF
* .NET 4.7.2
* [AudioManager](https://gist.github.com/sverrirs/d099b34b7f72bb4fb386)
* [HIDLibrary](https://github.com/mikeobrien/HidLibrary)
	
