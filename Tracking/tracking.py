#!/usr/bin/env python3
# coding:utf-8

"""
Real-time 3D organ tracking from video feedback. 0MQ server-side for Unity-Python communication.

Receiving: Current organs position + 3D Scene parameters + surgical operation video-frames.
Sending: Adjusted organs position

Author : Alessandro Delmonte
Contact : alessandro.delmonte@institutimagine.org
"""

import os.path
import utils
import server


if __name__ == "__main__":

    conf = utils.get_config_from_file(os.path.abspath("../config.yml"))

    server.Server(conf['port']).start_server()
