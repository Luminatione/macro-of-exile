#include <ntddk.h>
#include <wdf.h>
#include <initguid.h>

#include "device.h"
#include "queue.h"
#include "trace.h"

EXTERN_C_START

#define DEVICE_NAME L"\\Device\\VirtualDevice"
#define DEVICE_LINK_NAME L"\\DosDevices\\VirtualDeviceDriver"

#define IOCTL_MOUSE_INPUT_SEND CTL_CODE(FILE_DEVICE_UNKNOWN, 0x800, METHOD_BUFFERED, FILE_ANY_ACCESS)
#define IOCTL_KEYBOARD_INPUT_SEND CTL_CODE(FILE_DEVICE_UNKNOWN, 0x801, METHOD_BUFFERED, FILE_ANY_ACCESS)

DRIVER_INITIALIZE DriverEntry;
EVT_WDF_DRIVER_DEVICE_ADD VirtualDeviceDriverEvtDeviceAdd;
EVT_WDF_OBJECT_CONTEXT_CLEANUP VirtualDeviceDriverEvtDriverContextCleanup;

EXTERN_C_END
