import cv2
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
            Video frame plotting
        """
        plt.imshow(self.im)
        plt.show()


class Tracking:
    def __init__(self, q: deque):
        """
            Surgical video tracking using SIFT features
            :param q: queue containing two consecutive frames
        """
        self.q = q

        self.min_match_count = 10
        self.sift = cv2.SIFT_create()
        flann_index_kdtree = 1
        index_params = dict(algorithm=flann_index_kdtree, trees=5)
        search_params = dict(checks=50)
        self.flann = cv2.FlannBasedMatcher(index_params, search_params)

    def __str__(self) -> str:
        return f'Automatic landmark tracking between consecutive frames'

    def __repr__(self) -> str:
        return f'Tracking(q={self.q})'

    def get_coords(self) -> list:
        """
            Analyze the displacements between two consecutive frames and return the correspondent translations and
            rotations for 3D models adjustments. // renvoie les changements de position de la caméra
            :return: x, y, z translations and rx, ry, rz Euler rotation angles
        """
        if len(self.q) < 2:
            return [0., 0., 0., 0., 0., 0.]
        else:
            kp1, des1 = self.sift.detectAndCompute(self.q[0].im, None)
            kp2, des2 = self.sift.detectAndCompute(self.q[1].im, None)
            matches = self.flann.knnMatch(des1, des2, k=2)
            good = []
            for m, n in matches:
                if m.distance < 0.7 * n.distance:
                    good.append(m)
            if len(good) > self.min_match_count:
                src_pts = np.float32([kp1[m.queryIdx].pt for m in good]).reshape(-1, 1, 2)
                dst_pts = np.float32([kp2[m.trainIdx].pt for m in good]).reshape(-1, 1, 2)
                m, mask = cv2.findHomography(src_pts, dst_pts, cv2.RANSAC, 5.0)

                # TODO: implement homography decomposition to translation vector and rotation matrix
                return np.random.uniform(-10, 10, 6) * (np.mean(self.q[1].im) - np.mean(self.q[0].im))
            else:
                return [0., 0., 0., 0., 0., 0.]
