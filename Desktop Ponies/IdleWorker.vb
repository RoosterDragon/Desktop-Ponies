Imports System.Threading

''' <summary>
''' Processes UI dependant tasks on the UI thread when it becomes idle.
''' </summary>
''' <remarks>When the control is disposed, any tasks queued for execution are abandoned.</remarks>
Public Class IdleWorker
    ''' <summary>
    ''' A method that does nothing.
    ''' </summary>
    Private Shared ReadOnly DummyCallback As New MethodInvoker(Sub()
                                                               End Sub)
    ''' <summary>
    ''' Indicates if work should be done when idle, otherwise work will be asynchronously invoked.
    ''' </summary>
    Private Shared ReadOnly UseIdlePooling As Boolean = Not Runtime.IsMono

    ''' <summary>
    ''' Maintains a collection of tasks to perform when the application is idle.
    ''' </summary>
    Private ReadOnly tasks As New Queue(Of MethodInvoker)()
    ''' <summary>
    ''' Indicates when the queue of tasks to perform is empty.
    ''' </summary>
    Private ReadOnly empty As New Threading.ManualResetEvent(True)
    ''' <summary>
    ''' The control from which the UI thread is to be invoked.
    ''' </summary>
    Private ReadOnly control As Control
    ''' <summary>
    ''' Keeps track of how long the current batch of tasks has taken.
    ''' </summary>
    Private ReadOnly runWatch As New Diagnostics.Stopwatch()

    ''' <summary>
    ''' Indicates if the control has been disposed.
    ''' </summary>
    Private ReadOnly Property controlDisposed As Boolean
        Get
            Return control.Disposing OrElse control.IsDisposed
        End Get
    End Property
    ''' <summary>
    ''' Async result returned from dummy callback, held so the wait handle may be disposed.
    ''' </summary>
    Private dummyAsyncResult As IAsyncResult

    ''' <summary>
    ''' Initializes a new instance of the <see cref="IdleWorker"/> class for the specified control.
    ''' </summary>
    ''' <param name="control">The control on which tasks are dependant.</param>
    ''' <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is null.</exception>
    ''' <exception cref="T:System.ArgumentException"><paramref name="control"/> has been disposed.</exception>
    Public Sub New(control As Control)
        Me.control = Argument.EnsureNotNull(control, "control")
        AddHandler control.Disposed, AddressOf Control_Disposed
        If controlDisposed Then Throw New ArgumentException("control must not be disposed.", "control")
        If UseIdlePooling Then
            control.SmartInvoke(Sub() AddHandler Application.Idle, AddressOf RunTask)
        End If
    End Sub

    ''' <summary>
    ''' Queues a task for execution when the UI thread is next idle.
    ''' </summary>
    ''' <param name="task">The task which will be executed once other queued tasks have been processed and the UI thread is idle.</param>
    Public Sub QueueTask(task As MethodInvoker)
        Argument.EnsureNotNull(task, "task")
        SyncLock tasks
            ' If the control is disposed or the handle has been lost, we will drop all new tasks since they can't be processed anyway.
            If controlDisposed OrElse Not control.IsHandleCreated Then
                Return
            End If

            If UseIdlePooling Then
                tasks.Enqueue(task)
                ' If there were previously no tasks in the queue, the application may already be an an idle state.
                ' We will post a dummy event to the message queue, so that the idle event can be raised once the message queue is
                ' cleared.
                If tasks.Count = 1 Then
                    dummyAsyncResult = control.BeginInvoke(DummyCallback)
                    empty.Reset()
                End If
            Else
                ' Mono does not handle the idle event in the same way. Instead we'll just lump the request onto the message queue. This
                ' means the caller is still not blocked, but that user interaction will be delayed behind queued tasks. This becomes an
                ' issue if a lot of tasks are added under Mono, since they must complete before the UI becomes responsive again.
                control.BeginInvoke(task)
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
            If controlDisposed Then Return
            runWatch.Restart()
            ' For efficiency, run a batch of tasks whilst idle.
            ' This reduces the message loop overhead in the case of lots of very short tasks.
            While tasks.Count > 0 AndAlso runWatch.ElapsedMilliseconds < 33
                tasks.Dequeue().Invoke()
                If tasks.Count = 0 Then TaskQueueCleared()
            End While
        End SyncLock
    End Sub

    ''' <summary>
    ''' Waits until all tasks queued by this worker have been processed.
    ''' </summary>
    Public Sub WaitOnAllTasks()
        If Not control.InvokeRequired Then
            ' We are on the UI thread, invoke tasks until all are complete.
            If UseIdlePooling Then
                SyncLock tasks
                    If controlDisposed Then Return
                    While tasks.Count > 0
                        tasks.Dequeue.Invoke()
                    End While
                    TaskQueueCleared()
                End SyncLock
            Else
                Application.DoEvents()
            End If
        Else
            SyncLock tasks
                If controlDisposed Then Return
            End SyncLock
            ' We are on another thread, wait on the UI thread to finish processing our tasks.
            Try
                If UseIdlePooling Then
                    empty.WaitOne()
                Else
                    control.SmartInvoke(DummyCallback)
                End If
            Catch ex As ObjectDisposedException
                ' If the control is disposed after our check, we swallow the exception. We have finished waiting on tasks since the control
                ' is closing down.
            End Try
        End If
    End Sub

    ''' <summary>
    ''' This method should be called when the task queue is emptied, in order to reset state.
    ''' </summary>
    Private Sub TaskQueueCleared()
        empty.Set()
        If dummyAsyncResult IsNot Nothing Then
            Try
                control.EndInvoke(dummyAsyncResult)
            Finally
                dummyAsyncResult.AsyncWaitHandle.Dispose()
                dummyAsyncResult = Nothing
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Disposes of the worker, if the current thread is exiting.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">Data about the event.</param>
    Private Sub Control_Disposed(sender As Object, e As EventArgs)
        SyncLock tasks
            RemoveHandler Application.Idle, AddressOf RunTask
            empty.Set()
            empty.Dispose()
            If dummyAsyncResult IsNot Nothing Then dummyAsyncResult.AsyncWaitHandle.Dispose()
        End SyncLock
    End Sub
End Class
