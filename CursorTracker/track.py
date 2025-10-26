import tkinter as tk
import pyautogui
import time
import threading

def update_label():
    while True:
        x, y = pyautogui.position()
        label.config(text=f"X={x}  Y={y}")
        time.sleep(0.05)

root = tk.Tk()
root.overrideredirect(True)            # Remove window borders
root.wm_attributes("-topmost", True)   # Always on top
root.wm_attributes("-transparentcolor", "black")  # Make background transparent
root.configure(bg="black")

label = tk.Label(root, fg="lime", bg="black", font=("Consolas", 14))
label.pack(anchor="nw")

threading.Thread(target=update_label, daemon=True).start()
root.geometry("+10+10")  # position top-left corner
root.mainloop()
