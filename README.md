# Surgery3D - *Overlay & Tracking*
System for 3D models overlay and real-time surgical video tracking of organs.

![gif](https://i.imgur.com/wru91KV.gif)

This is an [Unity3D](https://unity.com) project that communicates with a local Python server in order to overlay patients 3D models generated 
with [TensorFlow](https://www.tensorflow.org) (check out this repo [aledelmo/APMRI-DNN](https://github.com/aledelmo/APMRI-DNN)) to the corresponding surgical operation video. Communication to the server established using [ZeroMQ](https://www.zeromq.org). Organs
movements are real-time tracked from video frames using [OpenCV](http://www.opencv.org). 3D models position is adjusted accordingly.

## Running a test

Before building the project download models and video using this link *NOT_AVAILABLE* and put them in Assets/Data folder.
1. Build the app in Unity3D.
2. Create the Python environment and install requirements
2. Start the ØMQ server.
3. Establish connection between app and server.

## System requirements

Unity3D 2019.4.18f1 or later

Python 3.8.6

## Known Issue

If connection to the server is lost during tracking, the app must be restarted.

## Contact

[Alessandro Delmonte - alessandro.delmonte@institutimagine.org](mailto:alessandro.delmonte@institutimagine.org)

## License

This project is licensed under the [Apache License 2.0](LICENSE.md) - see the [LICENSE.md](LICENSE.md) file for
details
