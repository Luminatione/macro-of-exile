EXTERN_C_START

typedef struct
{
    PULONG VhfHandle;
    PUCHAR Data;

} QUEUE_CONTEXT, * PQUEUE_CONTEXT;

typedef struct
{
    SHORT xAxis;
    SHORT yAxis;
    LONG buttons;
    LONG key;
    LONG keyState;
    UCHAR modifiers;
} INPUT_MESSAGE, * PINPUT_MESSAGE;

#define LMB_MESSAGE_BIT 1 << 1
#define RMB_MESSAGE_BIT 1 << 2
#define MMB_MESSAGE_BIT 1 << 3

WDF_DECLARE_CONTEXT_TYPE_WITH_NAME(QUEUE_CONTEXT, QueueGetContext)

NTSTATUS VirtualDeviceDriverQueueInitialize(_In_ WDFDEVICE Device);

EVT_WDF_IO_QUEUE_IO_DEVICE_CONTROL VirtualDeviceDriverEvtIoDeviceControl;
EVT_WDF_IO_QUEUE_IO_STOP VirtualDeviceDriverEvtIoStop;

EXTERN_C_END
