''' <summary>
''' Processes UI dependant tasks on the UI thread when the application is idle.
''' </summary>
Public Class IdleWorker
    Inherits Disposable

    ''' <summary>
    ''' Maintains a collection on tasks to perform when the application is idle.
    ''' </summary>
    Private ReadOnly tasks As New Queue(Of MethodInvoker)
    ''' <summary>
    ''' Indicates when the queue of tasks to perform is empty.
    ''' </summary>
    Private ReadOnly empty As New Threading.ManualResetEvent(True)
    ''' <summary>
    ''' A control, from which the UI thread can be invoked.
    ''' </summary>
    Private ReadOnly control As Control

    ''' <summary>
    ''' Initializes a new instance of the <see cref="IdleWorker"/> class on the current thread.
    ''' </summary>
    Public Sub New()
        control = New Control()
        control.CreateControl()
        AddHandler Application.Idle, AddressOf RunTask
        AddHandler Application.ThreadExit, Sub(sender, e) Dispose()
    End Sub

    ''' <summary>
    ''' Queues a task for execution when the UI thread is next idle.
    ''' </summary>
    ''' <param name="task">The task which will be executed once other queued tasks have been processed and the UI thread is idle.</param>
    Public Sub QueueTask(task As MethodInvoker)
        SyncLock tasks
            tasks.Enqueue(task)
            ' If there were previously no tasks in the queue, the application may already be an an idle state.
            ' We will post a dummy event to the message queue, so that the idle event can be raised once the message queue is cleared.
            If tasks.Count = 1 Then
                control.BeginInvoke(Sub()
                                    End Sub)
                empty.Reset()
            End If
        End SyncLock
    End Sub

    ''' <summary>
    ''' Dequeues and invokes a task on the UI thread.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">Data about the event.</param>
    Private Sub RunTask(sender As Object, e As EventArgs)
        SyncLock tasks
            If Not Disposed Then
                If tasks.Count > 0 Then
                    tasks.Dequeue().Invoke()
                    If tasks.Count = 0 Then empty.Set()
                End If
            End If
        End SyncLock
    End Sub

    ''' <summary>
    ''' Waits until all tasks queued by this worker have been processed.
    ''' </summary>
    Public Sub WaitOnAllTasks()
        empty.WaitOne()
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler Application.Idle, AddressOf RunTask
        If disposing Then
            SyncLock tasks
                empty.Dispose()
                If control.InvokeRequired Then control.Invoke(Sub() control.Dispose())
            End SyncLock
        End If
    End Sub
End Class
