import cv2
import mss
import numpy as np
import pyperclip
import argparse
import sys

CAPTURE_MONITOR_INDEX = 2
SCALE = 1 / 5

# original size ~260x80 → scaled size
TARGET_W = int(150 * SCALE)
TARGET_H = int(10 * SCALE)

# Default color (blue/cyan) in BGR format
DEFAULT_BGR = (255, 200, 0)  # Approximate blue in BGR
DEFAULT_TOL = (25, 80, 80)  # Default HSV tolerance

def calculate_hsv_bounds(target_bgr, tolerance):
    """
    Calculate HSV lower and upper bounds from BGR color and tolerance.
    
    Args:
        target_bgr: Target color in BGR format (tuple of 3 integers)
        tolerance: Color tolerance (int, float, or tuple of 3 values for HSV)
    
    Returns:
        Tuple of (lower, upper) HSV bounds
    """
    # Handle both single value and tuple tolerance
    if isinstance(tolerance, (int, float)):
        tol = np.array([tolerance, tolerance, tolerance])
    else:
        tol = np.array(tolerance)
    
    # Convert BGR to HSV
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
    
    return lower, upper

if __name__ == "__main__":
    # Parse command-line arguments
    parser = argparse.ArgumentParser(description="Check if color contour is found on screen")
    parser.add_argument("--region", nargs=4, type=int, metavar=("LEFT", "TOP", "WIDTH", "HEIGHT"),
                        help="Screen region to capture: left top width height (e.g., --region 100 200 800 600)")
    parser.add_argument("--color", nargs=3, type=int, metavar=("B", "G", "R"),
                        help="Color to search for in BGR format: B G R (e.g., --color 255 0 255 for purple)")
    args = parser.parse_args()
    
    # Determine target color and tolerance
    if args.color:
        target_bgr = tuple(args.color)
        tolerance = (50, 50, 50)
    else:
        # Use default color and tolerance
        target_bgr = DEFAULT_BGR
        tolerance = DEFAULT_TOL
    
    # Calculate HSV bounds
    lower, upper = calculate_hsv_bounds(target_bgr, tolerance)
    
    # Build region dict if provided, otherwise None (whole screen)
    region = None
    if args.region:
        region = {
            "left": args.region[0],
            "top": args.region[1],
            "width": args.region[2],
            "height": args.region[3]
        }
    
    with mss.mss() as sct:
        monitor = sct.monitors[CAPTURE_MONITOR_INDEX]
        
        # Use region if provided, otherwise use whole monitor
        if region is not None:
            capture_area = {
                "left": region["left"],
                "top": region["top"],
                "width": region["width"],
                "height": region["height"]
            }
            region_left = region["left"]
            region_top = region["top"]
        else:
            capture_area = monitor
            region_left = monitor["left"]
            region_top = monitor["top"]
        
        # Capture single frame
        frame = np.array(sct.grab(capture_area))[:, :, :3]

        resized = cv2.resize(
            frame,
            (int(frame.shape[1] * SCALE), int(frame.shape[0] * SCALE)),
            interpolation=cv2.INTER_AREA
        )

        hsv = cv2.cvtColor(resized, cv2.COLOR_BGR2HSV)

        mask = cv2.inRange(hsv, lower, upper)
        contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

        # Find the largest contour that meets size criteria
        max_contour = None
        max_area = 0
        
        for cnt in contours:
            x, y, w, h = cv2.boundingRect(cnt)
            
            if w > TARGET_W - 10 and h > TARGET_H - 10:
                area = cv2.contourArea(cnt)
                if area > max_area:
                    max_area = area
                    max_contour = cnt
        
        # Output to clipboard: "yes" if contour found, otherwise "no"
        if max_contour is not None:
            pyperclip.copy("yes")
        else:
            pyperclip.copy("no")



