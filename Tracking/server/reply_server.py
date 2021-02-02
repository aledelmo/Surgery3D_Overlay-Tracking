import io
import zmq
import json
import base64
import processing
import collections
from PIL import Image


class Server:
    def __init__(self, port: int) -> None:
        self.port = str(port)
        self.address = "tcp://*:" + self.port
        context = zmq.Context()
        self.socket = context.socket(zmq.REP)
        self.q = collections.deque([], 2)
        self.tracking = processing.Tracking(self.q)

    def __str__(self) -> str:
        return f'ZeroMQ Server at port {self.port}'

    def __repr__(self) -> str:
        return f'Server(port={self.port})'

    def start_server(self) -> None:
        """
            0MQ server startup and communication management
        """
        with self.socket.bind(self.address):
            print("ZeroMQ Server listening at {}".format(self.address))
            while True:
                payload_rx = self.socket.recv(flags=0)
                if payload_rx:
                    self.decode_payload(payload_rx)
                    self.socket.send_string(self.reply(), flags=0, copy=False)

    def decode_payload(self, payload: bytes) -> None:
        """
            Messaging decoder. In Payload: X, Y, Z, Image
            :param payload: encoded message received from Unity3D application
        """
        packet = json.loads(payload)

        image = packet['Image']
        dec = base64.b64decode(image)
        stream = io.BytesIO(dec)
        image = Image.open(stream)

        frame = processing.Frame(packet['X'], packet['Y'], packet['Z'], image)
        self.q.append(frame)

    def reply(self) -> str:
        """
            Payload encoding carrying translation and rotation indications
            :return: payload containing processing results
            """
        x, y, z, rx, ry, rz = self.tracking.get_coords()

        results = {
            "X": x,
            "Y": y,
            "Z": z,
            "Rx": rx,
            "Ry": ry,
            "Rz": rz,
        }

        return json.dumps(results)
