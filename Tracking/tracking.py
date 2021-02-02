#!/usr/bin/env python3
# coding:utf-8

# Copyright 2021 Alessandro Delmonte. All Rights Reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# =============================================================================

"""
Real-time 3D organ tracking from video feedback. 0MQ server-side for Unity-Python communication.

Receiving: Current organs position + 3D Scene parameters + surgical operation video-frames.
Sending: Adjusted organs position

Contact : alessandro.delmonte@institutimagine.org
"""

import os.path
import utils
import server


if __name__ == "__main__":

    conf = utils.get_config_from_file(os.path.abspath("../config.yml"))

    server.Server(conf['port']).start_server()
