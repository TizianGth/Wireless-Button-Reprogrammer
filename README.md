## Table of contents
* [General info](#general-info)
* [Warning](#warning)
* [Supported devices](#supported-devices)
* [Setup](#setup)
* [Technologies](#technologies)
## General info
Repurposes the mute button on the HyperX Cloud II Wireless (DLS). You can now do 3 different actions, which you can programm yourself,
but the default behaviour is:
* 1 click = Pause/Resume
* 2 clicks = Next
* 3 clicks = Previous

## Warning
Because the mute functionality is handled on the headset it self you'll still mute/unmute your headset microphone.
Because of that I would advise you to only use this tool if you can live without your headset microphone.

## Supported devices
* Windows only
* HyperX Cloud II Wireless (DLS Version)

Sadly it's only one at the moment, but if you want to use this tool but your device is not supported, feel free to contact me (e.g open an issue here on github).

## Setup
### How to find you Vendor and Product ID:
1. Open your Device Manager and expand "Human Interface Devices".\
![device manager](https://github.com/GuthiYT/hyperx/blob/main/doc/img/device_manager.png)

2. Unplug and Re-plug your USB dongle from your computer. Now your desired device should be the last one from the "HID-compliant consumer control" devices. \
![HIDs](https://github.com/GuthiYT/hyperx/blob/main/doc/img/hid.png)

3. Right click and select "Properties", then switch to "Details". There you should select "Hardware IDs" from the "Property" drop down menu. Now you can already
see the Vendor ID (the 4 numbers after "VID_") and the Product ID (the 4 numbers after "PID_"). \
![VPID](https://github.com/GuthiYT/hyperx/blob/main/doc/img/vid_pid.png)

4. Now just input these in the program, click "Apply" and "Start".

### How to change keycodes
To change what happens after each click, refer to this [Site](https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes) to get your desired
keycodes, then enter them in the program, click "Apply" and "Start".

## Technologies
* Visual Studio's WPF
* .NET 4.7.2
* [HIDLibrary](https://github.com/mikeobrien/HidLibrary)
	
