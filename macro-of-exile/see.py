import cv2
import mss
import numpy as np
import pyperclip
import time

CAPTURE_MONITOR_INDEX = 2
SCALE = 1 / 5

TARGET_W = int(260 * SCALE)
TARGET_H = int(80 * SCALE)

def scan_once():
    with mss.mss() as sct:
        monitor = sct.monitors[CAPTURE_MONITOR_INDEX]
        frame = np.array(sct.grab(monitor))[:, :, :3]

        resized = cv2.resize(
            frame,
            (int(frame.shape[1] * SCALE), int(frame.shape[0] * SCALE)),
            interpolation=cv2.INTER_AREA
        )

        hsv = cv2.cvtColor(resized, cv2.COLOR_BGR2HSV)

        lower = np.array([140, 150, 150])
        upper = np.array([165, 255, 255])

        mask = cv2.inRange(hsv, lower, upper)
        contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

        for cnt in contours:
            x, y, w, h = cv2.boundingRect(cnt)
            if w > TARGET_W - 10 and h > TARGET_H - 10:
                cx = int(x + w / 2)
                cy = int(y + h / 2)

                screen_x = int(cx / SCALE + monitor["left"])
                screen_y = int(cy / SCALE + monitor["top"])

                pyperclip.copy(f"{screen_x},{screen_y}")
                return
            else:
                pyperclip.copy(f"{-1},{-1}")
scan_once()