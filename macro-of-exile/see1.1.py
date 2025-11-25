import cv2
import mss
import numpy as np
import pyperclip
import time
import sys

CAPTURE_MONITOR_INDEX = 1
SCALE = 1 / 5

TARGET_W = int(130 * SCALE)
TARGET_H = int(40 * SCALE)

PURPLE_BGR = (255, 0, 255)
PURPLE_TOL = (14, 10, 14)
YELLOW_BGR = (0, 255, 255)  # Yellow in BGR
YELLOW_TOL = (14, 10, 14)

# Restricted rectangles: (left, top, right, bottom)
RESTRICTED_RECTS = [
    (1370, 950, 1690, 1080),  # First restricted area
    (230, 950, 640, 1055)     # Second restricted area
]

def adjust_coordinate_if_restricted(x, y):
    """
    Check if coordinates are inside restricted rectangles.
    If so, return the closest point above the rectangle (same X, Y = top - 1).
    Otherwise, return original coordinates.
    """
    for left, top, right, bottom in RESTRICTED_RECTS:
        if left <= x <= right and top <= y <= bottom:
            # Point is inside restricted rectangle, return point above
            return x, top - 1
    # Point is not in any restricted rectangle
    return x, y

def scan_once(target_bgr, tolerance, return_rect_info=False):
    # Handle both single value and tuple tolerance
    if isinstance(tolerance, (int, float)):
        tol = np.array([tolerance, tolerance, tolerance])
    else:
        tol = np.array(tolerance)

    t_np = np.uint8([[list(target_bgr)]])
    hsv_target = cv2.cvtColor(t_np, cv2.COLOR_BGR2HSV)[0][0]

    lower = np.array([
        max(0, int(hsv_target[0]) - int(tol[0])),
        max(0, int(hsv_target[1]) - int(tol[1])),
        max(0, int(hsv_target[2]) - int(tol[2]))
    ])

    upper = np.array([
        min(179, int(hsv_target[0]) + int(tol[0])),
        min(255, int(hsv_target[1]) + int(tol[1])),
        min(255, int(hsv_target[2]) + int(tol[2]))
    ])

    with mss.mss() as sct:
        monitor = sct.monitors[CAPTURE_MONITOR_INDEX]
        frame = np.array(sct.grab(monitor))[:, :, :3]

        resized = cv2.resize(
            frame,
            (int(frame.shape[1] * SCALE), int(frame.shape[0] * SCALE)),
            interpolation=cv2.INTER_AREA
        )

        hsv = cv2.cvtColor(resized, cv2.COLOR_BGR2HSV)
        mask = cv2.inRange(hsv, lower, upper)

        contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

        result_x, result_y = -1, -1
        rect_width = -1

        for cnt in contours:
            x, y, w, h = cv2.boundingRect(cnt)
            if w > TARGET_W - 10 and h > TARGET_H - 10:
                cx = int(x + w / 2)
                cy = int(y + h / 2)

                result_x = int(cx / SCALE + monitor["left"])
                result_y = int(cy / SCALE + monitor["top"])
                rect_width = int(w / SCALE)  # Store actual width in screen coordinates
                
                # Adjust coordinates if they fall in restricted area
                result_x, result_y = adjust_coordinate_if_restricted(result_x, result_y)
                break

        if return_rect_info:
            return result_x, result_y, rect_width
        return result_x, result_y


if __name__ == "__main__":
    # Check for purple rectangle first (higher priority)
    purple_x, purple_y = scan_once(PURPLE_BGR, PURPLE_TOL)
    if purple_x != -1 and purple_y != -1:
        # Found purple rectangle - output with 'p' prefix
        pyperclip.copy(f"p,{purple_x},{purple_y}")
    else:
        # Check for yellow square
        yellow_x, yellow_y, yellow_width = scan_once(YELLOW_BGR, YELLOW_TOL, return_rect_info=True)
        if yellow_x != -1 and yellow_y != -1:
            # Found yellow square - calculate offset position
            # Offset horizontally: rectangle width + reasonable margin (e.g., 50% of width)
            margin = int(yellow_width * 0.5)
            offset_x = yellow_x + yellow_width + margin
            offset_y = yellow_y  # Keep same Y coordinate
            
            # Adjust offset coordinates if they fall in restricted area
            offset_x, offset_y = adjust_coordinate_if_restricted(offset_x, offset_y)
            
            # Output with 'y' prefix and offset coordinates
            pyperclip.copy(f"y,{offset_x},{offset_y}")
        else:
            # No squares found
            pyperclip.copy("-1,-1")
