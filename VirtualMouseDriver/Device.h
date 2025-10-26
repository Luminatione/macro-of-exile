#include "public.h"
#include <vhf.h>

EXTERN_C_START

#define MOUSE_REPORT_ID 1
#define KEYBOARD_REPORT_ID 2

static UCHAR InputReportDescriptor[] =
{
		0x05, 0x01,    // UsagePage(Generic Desktop[0x0001])
		0x09, 0x02,    // UsageId(Mouse[0x0002])
		0xA1, 0x01,    // Collection(Application)
		0x85, MOUSE_REPORT_ID,    //     ReportId(1)
		0x09, 0x01,    //     UsageId(Pointer[0x0001])
		0xA1, 0x00,    //     Collection(Physical)
		0x09, 0x30,    //         UsageId(X[0x0030])
		0x09, 0x31,    //         UsageId(Y[0x0031])
		0x15, 0x00,       // LogicalMinimum(0)
		0x26, 0xFF, 0x7F, // LogicalMaximum(32767)
		0x95, 0x02,    //         ReportCount(2)
		0x75, 0x10,    //         ReportSize(16)
		0x81, 0x02,    //         Input (Data, Var, Relative)
		0x05, 0x09,    //         UsagePage(Button[0x0009])
		0x19, 0x01,    //         UsageIdMin(Button 1[0x0001])
		0x29, 0x03,    //         UsageIdMax(Button 3[0x0003])
		0x15, 0x00,    //         LogicalMinimum(0)
		0x25, 0x01,    //         LogicalMaximum(1)
		0x95, 0x03,    //         ReportCount(3)
		0x75, 0x01,    //         ReportSize(1)
		0x81, 0x03,    //         Input(Data, Variable, Absolute, NoWrap, Linear, PreferredState, NoNullPosition, BitField)
		0xC0,          //     EndCollection()
		0x95, 0x01,    //     ReportCount(1)
		0x75, 0x05,    //     ReportSize(5)
		0x81, 0x03,    //     Input(Constant, Variable, Absolute, NoWrap, Linear, PreferredState, NoNullPosition, BitField)
		0xC0,          // EndCollection()
		0x05, 0x01,    // UsagePage(Generic Desktop[0x0001])
		0x09, 0x06,    // UsageId(Keyboard[0x0006])
		0xA1, 0x01,    // Collection(Application)
		0x85, KEYBOARD_REPORT_ID,    //     ReportId(2)
		0x05, 0x07,        //   Usage Page (Keyboard)
		0x19, 0xE0,        //   Usage Minimum (Left Control)
		0x29, 0xE7,        //   Usage Maximum (Right GUI)
		0x15, 0x00,        //   Logical Minimum (0)
		0x25, 0x01,        //   Logical Maximum (1)
		0x75, 0x01,        //   Report Size (1)
		0x95, 0x08,        //   Report Count (8)
		0x81, 0x02,        //   Input (Data, Variable, Absolute) ; Modifier byte
		// Reserved byte
		0x75, 0x08,        //   Report Size (8)
		0x95, 0x01,        //   Report Count (1)
		0x81, 0x01,        //   Input (Constant) ; Reserved
		// Key array (6 bytes)
		0x05, 0x07,        //   Usage Page (Keyboard)
		0x19, 0x00,        //   Usage Minimum (0)
		0x29, 0x65,        //   Usage Maximum (101)
		0x15, 0x00,        //   Logical Minimum (0)
		0x25, 0x65,        //   Logical Maximum (101)
		0x75, 0x08,        //   Report Size (8)
		0x95, 0x06,        //   Report Count (6)
		0x81, 0x00,        //   Input (Data, Array) ; Key array

		0xC0               // End Collection
};

typedef struct
{
	VHFHANDLE VhfHandle;
	PUCHAR Data;

} DEVICE_CONTEXT, * PDEVICE_CONTEXT;

#pragma pack(push, 1)

typedef struct
{
	UCHAR reportId;

	SHORT xAxis;
	SHORT yAxis;

	UCHAR button1 : 1;
	UCHAR button2 : 1;
	UCHAR button3 : 1;
	UCHAR padding : 5;
} MouseHidReport;

typedef struct
{
	UCHAR reportId;
	UCHAR modifiers;
	UCHAR reserved;
	UCHAR key[6];
} KeyboardHidReport;

#pragma pack (pop)

WDF_DECLARE_CONTEXT_TYPE_WITH_NAME(DEVICE_CONTEXT, DeviceGetContext)

NTSTATUS VirtualDeviceDriverCreateDevice(_Inout_ PWDFDEVICE_INIT DeviceInit);

EXTERN_C_END
