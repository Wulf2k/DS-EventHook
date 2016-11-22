Imports System.IO
Imports System.Reflection
Imports System.Threading

Public Class Form1

    Shared Version As String
    Shared VersionCheckUrl As String = "http://wulf2k.ca/souls/DS-EventHook-ver.txt"

    Private WithEvents refTimer As New System.Windows.Forms.Timer()

    Private Declare Function OpenProcess Lib "kernel32.dll" (ByVal dwDesiredAcess As UInt32, ByVal bInheritHandle As Boolean, ByVal dwProcessId As Int32) As IntPtr
    Private Declare Function ReadProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, ByVal iSize As Integer, ByRef lpNumberOfBytesRead As Integer) As Boolean
    Private Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, ByVal iSize As Integer, ByVal lpNumberOfBytesWritten As Integer) As Boolean
    Private Declare Function CloseHandle Lib "kernel32.dll" (ByVal hObject As IntPtr) As Boolean
    Private Declare Function VirtualAllocEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal flAllocationType As Integer, ByVal flProtect As Integer) As IntPtr
    Private Declare Function VirtualProtectEx Lib "kernel32.dll" (hProcess As IntPtr, lpAddress As IntPtr, ByVal lpSize As IntPtr, ByVal dwNewProtect As UInt32, ByRef dwOldProtect As UInt32) As Boolean
    Private Declare Function VirtualFreeEx Lib "kernel32.dll" (hProcess As IntPtr, lpAddress As IntPtr, ByVal dwSize As Integer, ByVal dwFreeType As Integer) As Boolean
    Private Declare Function CreateRemoteThread Lib "kernel32" (ByVal hProcess As Integer, ByVal lpThreadAttributes As Integer, ByVal dwStackSize As Integer, ByVal lpStartAddress As Integer, ByVal lpParameter As Integer, ByVal dwCreationFlags As Integer, ByRef lpThreadId As Integer) As Integer

    Public Const PROCESS_VM_READ = &H10
    Public Const TH32CS_SNAPPROCESS = &H2
    Public Const MEM_COMMIT = 4096
    Public Const MEM_RELEASE = &H8000
    Public Const PAGE_READWRITE = 4
    Public Const PAGE_EXECUTE_READWRITE = &H40
    Public Const PROCESS_CREATE_THREAD = (&H2)
    Public Const PROCESS_VM_OPERATION = (&H8)
    Public Const PROCESS_VM_WRITE = (&H20)
    Public Const PROCESS_ALL_ACCESS = &H1F0FFF

    Dim isHooked As Boolean = False
    Dim exeVER As String = ""

    Dim hook1mem As IntPtr
    Dim hook2mem As IntPtr
    Dim getflagfuncmem As IntPtr
    Dim setflagfuncmem As IntPtr

    Dim hooks As Hashtable
    Dim dbgHooks As Hashtable = New Hashtable
    Dim rlsHooks As Hashtable = New Hashtable
    
    Private _targetProcess As Process = Nothing 'to keep track of it. not used yet.
    Private _targetProcessHandle As IntPtr = IntPtr.Zero 'Used for ReadProcessMemory

    Private Async Sub updatecheck()
        Try
            Dim client As New Net.WebClient()
            Dim content As String = Await client.DownloadStringTaskAsync(VersionCheckUrl)

            Dim lines() As String = content.Split({vbCrLf, vbLf}, StringSplitOptions.None)
            Dim latestVersion = lines(0)
            Dim latestUrl = lines(1)

            If latestVersion > Version.Replace(".", "") Then
                btnUpdate.Tag = latestUrl
                btnUpdate.Visible = True
            End If


        Catch ex As Exception

        End Try
    End Sub

    Public Function ScanForProcess(ByVal windowCaption As String, Optional automatic As Boolean = False) As Boolean
        Dim _allProcesses() As Process = Process.GetProcesses
        For Each pp As Process In _allProcesses
            If pp.MainWindowTitle.ToLower.Equals(windowCaption.ToLower) Then
                'found it! proceed.
                Return TryAttachToProcess(pp, automatic)
            End If
        Next
        Return False
    End Function
    Public Function TryAttachToProcess(ByVal proc As Process, Optional automatic As Boolean = False) As Boolean
        If Not (_targetProcessHandle = IntPtr.Zero) Then
            DetachFromProcess()
        End If

        _targetProcess = proc
        _targetProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, False, _targetProcess.Id)
        If _targetProcessHandle = 0 Then
            If Not automatic Then 'Showing 2 message boxes as soon as you start the program is too annoying.
                MessageBox.Show("Failed to attach to process.Please run Dark Souls PC Gizmo with administrative privileges.")
            End If

            Return False
        Else
            'if we get here, all connected and ready to use ReadProcessMemory()
            Return True
            'MessageBox.Show("OpenProcess() OK")
        End If

    End Function
    Public Sub DetachFromProcess()
        If Not (_targetProcessHandle = IntPtr.Zero) Then
            _targetProcess = Nothing
            Try
                CloseHandle(_targetProcessHandle)
                _targetProcessHandle = IntPtr.Zero
                'MessageBox.Show("MemReader::Detach() OK")
            Catch ex As Exception
                MessageBox.Show("Warning: MemoryManager::DetachFromProcess::CloseHandle error " & Environment.NewLine & ex.Message)
            End Try
        End If
    End Sub

    Private Sub checkDarkSoulsVersion()
        If (ReadUInt32(&H400080) = &HCE9634B4&) Then
            exeVER = "Debug"
        ElseIf (ReadUInt32(&H400080) = &HE91B11E2&) Then
            exeVER = "Beta"
        ElseIf (ReadUInt32(&H400080) = &HFC293654&) Then
            exeVER = "Release"
        Else
            exeVER = "Unknown"
        End If
    End Sub

    Public Function ReadInt8(ByVal addr As IntPtr) As SByte
        Dim _rtnBytes(0) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 1, vbNull)
        Return _rtnBytes(0)
    End Function
    Public Function ReadInt16(ByVal addr As IntPtr) As Int16
        Dim _rtnBytes(1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 2, vbNull)
        Return BitConverter.ToInt16(_rtnBytes, 0)
    End Function
    Public Function ReadInt32(ByVal addr As IntPtr) As Int32
        Dim _rtnBytes(3) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)

        Return BitConverter.ToInt32(_rtnBytes, 0)
    End Function
    Public Function ReadInt64(ByVal addr As IntPtr) As Int64
        Dim _rtnBytes(7) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
        Return BitConverter.ToInt64(_rtnBytes, 0)
    End Function
    Public Function ReadUInt16(ByVal addr As IntPtr) As UInt16
        Dim _rtnBytes(1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 2, vbNull)
        Return BitConverter.ToUInt16(_rtnBytes, 0)
    End Function
    Public Function ReadUInt32(ByVal addr As IntPtr) As UInt32
        Dim _rtnBytes(3) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
        Return BitConverter.ToUInt32(_rtnBytes, 0)
    End Function
    Public Function ReadUInt64(ByVal addr As IntPtr) As UInt64
        Dim _rtnBytes(7) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
        Return BitConverter.ToUInt64(_rtnBytes, 0)
    End Function
    Public Function ReadFloat(ByVal addr As IntPtr) As Single
        Dim _rtnBytes(3) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
        Return BitConverter.ToSingle(_rtnBytes, 0)
    End Function
    Public Function ReadDouble(ByVal addr As IntPtr) As Double
        Dim _rtnBytes(7) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
        Return BitConverter.ToDouble(_rtnBytes, 0)
    End Function
    Public Function ReadIntPtr(ByVal addr As IntPtr) As IntPtr
        Dim _rtnBytes(IntPtr.Size - 1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, IntPtr.Size, Nothing)
        If IntPtr.Size = 4 Then
            Return New IntPtr(BitConverter.ToUInt32(_rtnBytes, 0))
        Else
            Return New IntPtr(BitConverter.ToInt64(_rtnBytes, 0))
        End If
    End Function
    Public Function ReadBytes(ByVal addr As IntPtr, ByVal size As Int32) As Byte()
        Dim _rtnBytes(size - 1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, size, vbNull)
        Return _rtnBytes
    End Function
    Private Function ReadAsciiStr(ByVal addr As UInteger) As String
        Dim Str As String = ""
        Dim cont As Boolean = True
        Dim loc As Integer = 0

        Dim bytes(&H10) As Byte

        ReadProcessMemory(_targetProcessHandle, addr, bytes, &H10, vbNull)

        While (cont And loc < &H10)
            If bytes(loc) > 0 Then

                Str = Str + Convert.ToChar(bytes(loc))

                loc += 1
            Else
                cont = False
            End If
        End While

        Return Str
    End Function
    Private Function ReadUnicodeStr(ByVal addr As UInteger) As String
        Dim Str As String = ""
        Dim cont As Boolean = True
        Dim loc As Integer = 0

        Dim bytes(&H20) As Byte


        ReadProcessMemory(_targetProcessHandle, addr, bytes, &H20, vbNull)

        While (cont And loc < &H20)
            If bytes(loc) > 0 Then

                Str = Str + Convert.ToChar(bytes(loc))

                loc += 2
            Else
                cont = False
            End If
        End While

        Return Str
    End Function

    Public Sub WriteInt32(ByVal addr As IntPtr, val As Int32)
        WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
    End Sub
    Public Sub WriteUInt32(ByVal addr As IntPtr, val As UInt32)
        WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
    End Sub
    Public Sub WriteFloat(ByVal addr As IntPtr, val As Single)
        WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
    End Sub
    Public Sub WriteBytes(ByVal addr As IntPtr, val As Byte())
        WriteProcessMemory(_targetProcessHandle, addr, val, val.Length, Nothing)
    End Sub
    Public Sub WriteAsciiStr(addr As UInteger, str As String)
        WriteProcessMemory(_targetProcessHandle, addr, System.Text.Encoding.ASCII.GetBytes(str), str.Length, Nothing)
    End Sub
    Private Sub btnHook_Click(sender As Object, e As EventArgs) Handles btnHook.Click

        If ScanForProcess("DARK SOULS", True) Then
            checkDarkSoulsVersion()
            If Not (exeVER = "Debug" Or exeVER = "Release") then
                MsgBox("Invalid EXE type.")
                Return
            End If

            If exeVER = "Release" Then hooks = rlsHooks
            If exeVER = "Debug" Then hooks = dbgHooks

            dgvEvents.Rows.Clear
            dgvEvents.Columns.Clear

            dgvEvents.Columns.Add("name", "Name")
            dgvEvents.Columns.Add("id", "ID")
            dgvEvents.Columns.Add("value", "Value")

            dgvEvents.Columns("name").ValueType = GetType(String)
            dgvEvents.Columns("id").ValueType = GetType(Integer)
            dgvEvents.Columns("value").ValueType = GetType(Integer)

            dgvEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            dgvEvents.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders

        

            refTimer = New System.Windows.Forms.Timer
            refTimer.Interval = 30
            refTimer.Enabled = True

        




            initFlagHook1()
            initFlagHook2()
            initGetFlagFunc()
            initSetFlagFunc()



        Else
            MsgBox("Failed for some reason")

        End If

    End Sub

    Private sub initFlagHook1()
        hook1mem = VirtualAllocEx(_targetProcessHandle, 0, &H8000, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
        Dim oldProtectionOut As UInteger
        VirtualProtectEx(_targetProcessHandle, hook1mem, &H8000, PAGE_EXECUTE_READWRITE, oldProtectionOut)
        isHooked = True

        Dim a As New asm

        a.AddVar("hook", hooks("hook1"))
        a.AddVar("newmem", hook1mem)
        a.AddVar("vardump", hook1mem + &H400)
        a.AddVar("hookreturn", hooks("hook1return"))
        a.AddVar("startloop", 0)
        a.AddVar("exitloop", 0)
            
        a.pos = hook1mem
        a.Asm("pushad")
        a.Asm("mov eax, vardump")

        a.Asm("startloop:")
        a.Asm("mov ecx, [eax]")
        a.Asm("cmp ecx, 0")
        a.Asm("je exitloop")

        a.Asm("add eax, 0x8")
        a.Asm("jmp startloop")

        a.Asm("exitloop:")
        a.Asm("mov [eax], edx")
        a.Asm("mov edx, [esp+0x24]")
        a.Asm("mov [eax+4], edx")
        a.Asm("popad")
        a.Asm("call " & hooks("hook1seteventflag").toint32())
        a.Asm("jmp hookreturn")

        WriteProcessMemory(_targetProcessHandle, hook1mem, a.bytes, a.bytes.length, 0)


        a.Clear
        a.AddVar("newmem", hook1mem)
        a.pos = hooks("hook1").toint32()
        a.asm("jmp newmem")

        WriteProcessMemory(_targetProcessHandle, hooks("hook1"), a.bytes, a.bytes.length, 0)
    End sub
    Private sub initFlagHook2()
        hook2mem = VirtualAllocEx(_targetProcessHandle, 0, &H8000, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
        Dim oldProtectionOut As UInteger
        VirtualProtectEx(_targetProcessHandle, hook2mem, &H8000, PAGE_EXECUTE_READWRITE, oldProtectionOut)

        Dim a As New asm

        a.AddVar("hook", hooks("hook2"))
        a.AddVar("newmem", hook2mem)
        a.AddVar("vardump", hook2mem + &H400)
        a.AddVar("hookreturn", hooks("hook2return"))
        a.AddVar("startloop", 0)
        a.AddVar("exitloop", 0)
            
        a.pos = hook2mem
        a.Asm("pushad")
        a.Asm("mov eax, vardump")

        a.Asm("startloop:")
        a.Asm("mov ebx, [eax]")
        a.Asm("cmp ebx, 0")
        a.Asm("je exitloop")

        a.Asm("add eax, 0x4")
        a.Asm("jmp startloop")

        a.Asm("exitloop:")
        a.Asm("mov ebx, [ecx+0x28]")
        a.Asm("mov [eax], ebx")
        a.Asm("popad")
        a.Asm("call " & hooks("hook2seteventflag").toint32())
        a.Asm("jmp hookreturn")

        WriteProcessMemory(_targetProcessHandle, hook2mem, a.bytes, a.bytes.length, 0)


        a.Clear
        a.AddVar("newmem", hook2mem)
        a.pos = hooks("hook2").toint32()
        a.asm("jmp newmem")

        WriteProcessMemory(_targetProcessHandle, hooks("hook2"), a.bytes, a.bytes.length, 0)
    End sub
    Private Sub initGetFlagFunc()
        getflagfuncmem = VirtualAllocEx(_targetProcessHandle, 0, &H8000, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
        Dim oldProtectionOut As UInteger
        VirtualProtectEx(_targetProcessHandle, getflagfuncmem, &H8000, PAGE_EXECUTE_READWRITE, oldProtectionOut)

        Dim a As New asm

        a.AddVar("newmem", getflagfuncmem)
        a.AddVar("vardump", getflagfuncmem + &H400)

        a.pos = getflagfuncmem
        a.Asm("pushad")
        a.Asm("mov eax, vardump")
        a.Asm("mov eax, [eax]")
        a.Asm("push eax")
        a.Asm("call " & hooks("geteventflagvalue").toint32)
        a.Asm("mov ecx, vardump")
        a.Asm("add ecx, 4")
        a.Asm("mov [ecx], eax")
        a.Asm("add ecx, 4")
        a.Asm("mov eax, 1")
        a.Asm("mov [ecx], eax")
        a.Asm("popad")
        a.Asm("ret")

        WriteProcessMemory(_targetProcessHandle, getflagfuncmem, a.bytes, a.bytes.length, 0)
    End Sub
    Private sub initSetFlagFunc()
        setflagfuncmem = VirtualAllocEx(_targetProcessHandle, 0, &H8000, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
        Dim oldProtectionOut As UInteger
        VirtualProtectEx(_targetProcessHandle, setflagfuncmem, &H8000, PAGE_EXECUTE_READWRITE, oldProtectionOut)

        Dim a As New asm

        a.AddVar("newmem", setflagfuncmem)
        a.AddVar("vardump", setflagfuncmem + &H400)

        a.pos = setflagfuncmem
        a.Asm("pushad")
        a.Asm("mov eax, vardump")
        a.Asm("mov ebx, [eax]")
        a.Asm("add eax, 4")
        a.Asm("mov ecx, [eax]")
        a.Asm("push ecx")
        a.Asm("push ebx")
        'a.Asm("call " & hooks("seteventflagvalue").toint32)
        a.Asm("call " & hooks("seteventflag").toint32)
        a.Asm("popad")
        a.Asm("ret")

        WriteProcessMemory(_targetProcessHandle, setflagfuncmem, a.bytes, a.bytes.length, 0)
    End sub
    
    Private Sub refTimer_Tick() Handles refTimer.Tick
        refTimer.Stop

        Dim loc As IntPtr 
        Dim name As String
        Dim id As integer
        Dim value As integer


        loc = hook1mem + &H400
        Do
            id = ReadInt32(loc)
            value = ReadInt32(loc + 4)
            If id > 0
                WriteInt32(loc, 0)
                WriteInt32(loc+4, 0)
                name = ""
                For each row  As DataGridViewRow In dgvNames.Rows
                    If row.cells("id").value = id Then name = row.cells("name").value
                Next
                dgvEvents.Rows.Add({name, id, value})
                dgvEvents.Rows(dgvEvents.Rows.Count - 1).HeaderCell.Value = TimeOfDay.ToLongTimeString
            End If
            loc +=8
        Loop While id > 0

        loc = hook2mem + &H400
        Do
            id = ReadInt32(loc)
            If id > 0
                WriteInt32(loc, 0)
                name = ""
                For each row  As DataGridViewRow In dgvNames.Rows
                    If row.cells("id").value = id Then name = row.cells("name").value
                Next
                dgvEvents.Rows.Add({name, id})
                dgvEvents.Rows(dgvEvents.Rows.Count - 1).HeaderCell.Value = TimeOfDay.ToLongTimeString
            End If
            loc +=4
        Loop While id > 0



        loc = getflagfuncmem + &H400
        If ReadInt32(loc + 8) = 1 Then
            id = ReadInt32(loc)
            value = ReadInt32(loc + 4)

            txtID.Text = id

            txtValue.Text = math.Floor(value / (2 ^ 7))

            WriteInt32(loc + 8, 0)
        End If

        If dgvEvents.Rows.Count > nmbMaxFlags.Value Then
            dgvEvents.Rows.Remove(dgvEvents.Rows(0))
        End If

        refTimer.start
    End Sub

    

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Version = lblVer.Text

        Dim oldFileArg As String = Nothing
        For Each arg In Environment.GetCommandLineArgs().Skip(1)
            If arg.StartsWith("--old-file=") Then
                oldFileArg = arg.Substring("--old-file=".Length)
            Else
                MsgBox("Unknown command line arguments")
                oldFileArg = Nothing
                Exit For
            End If
        Next
        If oldFileArg IsNot Nothing Then
            If oldFileArg.EndsWith(".old") Then
                Dim t = New Thread(
                    Sub()
                    Try
                        'Give the old version time to shut down
                        Thread.Sleep(1000)
                        File.Delete(oldFileArg)
                    Catch ex As Exception
                        Me.Invoke(Function() MsgBox("Deleting old version failed: " & vbCrLf & ex.Message, MsgBoxStyle.Exclamation))
                    End Try
                End Sub)
                t.Start()
            Else
                MsgBox("Deleting old version failed: Invalid filename ", MsgBoxStyle.Exclamation)
            End If
        End If

        Dim systemType As Type = dgvEvents.GetType
        Dim propertyInfo As PropertyInfo = systemType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        propertyInfo.SetValue(dgvEvents, True, Nothing)
        propertyInfo.SetValue(dgvNames, True, Nothing)

        dgvNames.Columns.Add("id", "ID")
        dgvNames.Columns.Add("name", "Name")

        dgvNames.Columns("id").ValueType = GetType(Integer)
        dgvNames.Columns("name").ValueType = GetType(String)
        dgvNames.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        For each column as DataGridViewColumn In dgvNames.columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next

        loadNames()
        initHooks()

        updatecheck()
    End Sub
    Private sub initHooks()

        rlsHooks.Add("geteventflagvalue", New IntPtr(&HD60340))

        rlsHooks.Add("hook1", New IntPtr(&HBC1CEA))
        rlsHooks.Add("hook1return", New IntPtr(&HBC1CEF))
        rlsHooks.Add("hook1seteventflag", New IntPtr(&HD38CB0))
        rlsHooks.Add("hook2", New IntPtr(&H528CD5))
        rlsHooks.Add("hook2return", New IntPtr(&H528CDA))
        rlsHooks.Add("hook2seteventflag", New IntPtr(&HBBEEB0))

        rlsHooks.Add("seteventflag", New IntPtr(&HD60190))
        rlsHooks.Add("seteventflagvalue", New IntPtr(&HD60360))
    End sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim names(dgvNames.Rows.Count - 2) as String
            dgvNames.Sort(dgvNames.Columns("id"), System.ComponentModel.ListSortDirection.Ascending)

            For i = 0 To dgvNames.Rows.Count - 2
                names(i) = dgvNames.Rows(i).Cells("id").value & "|" & dgvNames.Rows(i).Cells("name").value
            Next
            File.WriteAllLines("DS-EventHook-IDs.txt", names)
            MsgBox("Saved DS-EventHook-IDs.txt")
        Catch ex As Exception
            MsgBox("Save failed." & Environment.NewLine & ex.Message)
        End Try
    End Sub
    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        loadNames
    End Sub
    Private sub loadNames()
        Try
            dgvNames.Rows.Clear
            Dim names() As String = {""}
            If File.Exists("DS-EventHook-IDs.txt") Then
                names = File.ReadAllLines("DS-EventHook-IDs.txt")
            Else
                names = My.Resources.NamedIDs.Split(Environment.newline)
            End If
            For each line In names
                if line.Contains("|") Then
                Dim id As integer = convert.ToInt32(line.Split("|")(0))
                Dim name As String = line.Split("|")(1)
                
                dgvNames.Rows.Add({id, name})
                End If
            Next
            dgvNames.Sort(dgvNames.Columns("id"), System.ComponentModel.ListSortDirection.Ascending)
        Catch ex As Exception
            MsgBox("Error reading stored names." & Environment.NewLine & ex.Message)
        End Try
    End sub

    Private Sub EventsNameChanged(sender As Object, e As System.Windows.Forms.DataGridViewcelleventargs) Handles dgvEvents.CellValueChanged
        if e.ColumnIndex = (0) and dgvEvents.Rows.Count > 0 Then
            Dim name As String
            Dim id As Integer
            Dim exists As Boolean = False


            name = dgvEvents.Rows(e.RowIndex).Cells("name").Value
            id = dgvEvents.Rows(e.RowIndex).Cells("id").Value

            For each row  As DataGridViewRow In dgvNames.Rows
                If row.cells("id").value = id Then
                    row.cells("name").value = name
                    exists = true
                End If
            Next

            If Not exists Then
                dgvNames.Rows.Add({id, name})
            End If

        End If

    End Sub
    Private sub IDSelected(sender as Object, e As DataGridViewCellEventArgs) Handles dgvNames.CellClick
        If e.ColumnIndex = (0) And dgvNames.Rows.Count > 0 Then
            txtID.Text = dgvNames.Rows(e.RowIndex).Cells("id").value
        End If
    End sub


    Private sub unhook()
        isHooked = false
        refTimer.Stop

        VirtualFreeEx(_targetProcessHandle, hook1mem, 0, MEM_RELEASE)
        VirtualFreeEx(_targetProcessHandle, getflagfuncmem, 0, MEM_RELEASE)
        VirtualFreeEx(_targetProcessHandle, setflagfuncmem, 0, MEM_RELEASE)

        Dim tmpbytes() as Byte = {}

        If exeVER = "Release" Then
            tmpbytes = {&He8, &Hc1, &H6f, &H17, 0}
            WriteProcessMemory(_targetProcessHandle, hooks("hook1"), tmpbytes, 5, 0)

            tmpbytes = {&He8, &Hd6, &H61, &H69, 0}
            WriteProcessMemory(_targetProcessHandle, hooks("hook2"), tmpbytes, 5, 0)
        End If
        
    End sub
    Private Sub btnUnhook_Click(sender As Object, e As EventArgs) Handles btnUnhook.Click
        unhook()
    End Sub
    
    Private Sub btnGet_Click(sender As Object, e As EventArgs) Handles btnGet.Click
        WriteInt32(getflagfuncmem + &H400, convert.ToInt32(txtID.text))
        CreateRemoteThread(_targetProcessHandle, 0, 0, getflagfuncmem, 0, 0, 0)
    End Sub
    Private Sub btnSet_Click(sender As Object, e As EventArgs) Handles btnSet.Click
        WriteInt32(setflagfuncmem + &H400, convert.ToInt32(txtID.text))
        WriteInt32(setflagfuncmem + &H404, convert.ToInt32(txtValue.text))
        CreateRemoteThread(_targetProcessHandle, 0, 0, setflagfuncmem, 0, 0, 0)
    End Sub

    Private Sub Form1_exit(sender As Object, e As EventArgs) Handles MyBase.Closing
        if isHooked Then unhook()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim updateWindow As New UpdateWindow(sender.tag)
        updateWindow.ShowDialog()
        If updateWindow.WasSuccessful Then
            Process.Start(updateWindow.NewAssembly, """--old-file=" & updateWindow.OldAssembly & """")
            Me.Close()
        End If
    End Sub
End Class
