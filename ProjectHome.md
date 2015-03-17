Introduction

This article shows you how to use video and sound information to detect intruder. Image data and sound data are continually collected from the environment respectively through webcam and microphone. Once the conditions of surrounding environment is changing, Corresponding alarm will be raised immediately.

The package contains:

DirectShowLib: Manipulate the webcam, grab images. I have wrapped functions related to IntruderDetection in class CManipulateWebcam. More information about DirectShowLib, please see directshow.net.

ManipulateMicrophone: Manipulate the microphone, grab sound, play sound. Functions related to IntruderDetection is wrapped in class CManipulateMicrophone. More information about ManipulateMicrophone please see the part "Using the code" of the article: Sending and playing microphone audio over network.

WavStream: Read .wav file, save data into .wav file.  The class CManipulateMicrophone uses WavStream.dll to save sound data into .wav file.

IntruderDetection: The main application, needs (DirectShowLib.dll, ManipuateMicrophone.dll, WavStream.dll).

DirectShowLib, ManipulateMicrophone and WavStream are independent projects. You can use DirectShowLib in your application to interact with webcam. Use ManipulateMicrophone to interact with microphone. Use WavStream to interact with .wav file. Of course, use IntruderDetection to detect intruder.

How does the IntruderDetection work

> Video detection: Check the box "Video Detection" to open video detection. If the box "Video Alarm" is checked, the alarm will be activated. Once there is something, abnormal information (0-3 second) will be automatically recorded in the folder: IntruderDetection>>bin>>Debug>>imgae>>(yyyy-MM-dd-H), filename (mm-ss-ff).jpg.

> Sound detection: Check the box "Sound Detection" to open sound detection. If the box "Sound Alarm" is checked, the alarm will be activated. However, if your speaker is close to your microphone, the alarm may be treated as abnormal sound. Check the box "Sound Echo" to play the sound from microphone. In the same way, abnormal information (0-3 second) will be automatically recorded in the folder: IntruderDetection>>bin>>Debug>>sound>>(yyyy-MM-dd-H), filename (mm-ss-ff).wav.

Background knowledge of IntruderDetection

Video detection

For video, define the distance of two image a(i,j) (ra,ga,ba) and  b(i,j)(rb,gb,bb) as

D1=∑((|ra-rb|+|ga-gb|+|ba-bb|)/3)  i=1,2...; j=1,2,... or

D2=∑((|ra-rb|2+|ga-gb|2+|ba-bb|2)/3)  i=1,2...; j=1,2,...  or

D3=max((|ra-rb|+|ga-gb|+|ba-bb|)/3)  i=1,2...; j=1,2,...

If the D1,D2 or D3 is bigger than a threshold value, we believe something is happening.

In default case, pointer is not allowed in C#. You need to open the Properties of IntruderDetection, click Build, check the box "Allow unsafe code". You may adjust the threshold values to adapt your machine.

Sound Detection

Calculate the sound energy every 25 millisecond. If the energy is bigger than a threshold value, something is happening.

Conclusion

This article shows how to use video and sound information to detect intruder (dig into IntruderDetection).  In this process, some useful intermediate technology is used, including how to manipulate webcam (dig into DirectShowLib), how to manipulate microphone (dig into ManipulateMicrophone), how to interact with .wav file (dig into WavStream). Have fun!