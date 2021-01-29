import numpy as np
from PIL import Image
from collections import deque
import matplotlib.pyplot as plt


class Frame:
    def __init__(self, x: float, y: float, z: float, im: Image) -> None:
        self.x = x
        self.y = y
        self.z = z
        self.im = np.array(im)

    def __str__(self) -> str:
        return f'VideoPlayer frame and Scene params'

    def __repr__(self) -> str:
        return f'Frame(x={self.x}, y={self.y}, z={self.z}, im={self.im})'

    def show_image(self):
        """
            Frame plotting
        """
        plt.imshow(self.im)
        plt.show()


def get_coords(q: deque) -> (float, float, float):
    """
        Tracking logic
        :param q: queue containing two successive frames
        :return: triplet of adjusted 3d model coordinates
    """
    if len(q) < 2:
        return 0., 0., 0.
    else:
        # TODO: implement tracking
        displacement = np.random.uniform(-10, 10, 3) * (np.mean(q[1].im) - np.mean(q[0].im))

        return displacement
