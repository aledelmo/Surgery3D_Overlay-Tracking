import io
import zmq
import base64
import numpy as np
from PIL import Image

context = zmq.Context()
socket = context.socket(zmq.REP)

with socket.bind("tcp://*:12345"):
    while True:
        message_rx = socket.recv()
        if len(message_rx):
            dec = base64.b64decode(message_rx)
            stream = io.BytesIO(dec)
            im = np.array(Image.open(stream))
            socket.send_string("Mean frame voxels value: {}". format(np.mean(im)))

"""
import asyncio
import zmq
import time
import random
import zmq.asyncio

import time
from concurrent.futures import ThreadPoolExecutor
_executor = ThreadPoolExecutor(1)

loop = asyncio.get_event_loop()
ctx = zmq.asyncio.Context()
sock = ctx.socket(zmq.REP)

def sync_blocking():
    time.sleep(2)

def async_process(msg) -> str:
    print(f"Received request: {msg}")
    #  do something
    # time.sleep(1)

    #  reply to client
    message = str(random.uniform(-1.0, 1.0)) + " " + str(random.uniform(-1.0, 1.0))
    print(message)
    return message


async def recv_and_process():
    sock.bind("tcp://*:12345")
    msg = await sock.recv()  # waits for msg to be ready
    # reply = await async_process(msg)
    await loop.run_in_executor(_executor, sync_blocking)
    await sock.send_string("ciao")

asyncio.ensure_future(recv_and_process())
loop.run_forever()

"""
