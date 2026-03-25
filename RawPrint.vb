'Imports System.IO
'Imports System.Runtime.InteropServices
'Imports System.Text

'Public Class RawPrint

'    '==================================================
'    ' DEV MODE SWITCH
'    '==================================================
'    ' True  = do NOT use Win32 printing, save to file
'    ' False = use real ESC/POS printer
'    Public Shared DEV_MODE As Boolean = True

'    '==================================================
'    ' ESC / POS CONSTANTS (LEGACY – DO NOT REMOVE)
'    '==================================================
'    Public Const pxePRINT_INIT As String = Chr(27) & Chr(64)
'    Public Const pxePRINT_FNT0 As String = Chr(27) & Chr(77) & Chr(0)
'    Public Const pxePRINT_FNT1 As String = Chr(27) & Chr(77) & Chr(1)
'    Public Const pxePRINT_FNT2 As String = Chr(27) & Chr(77) & Chr(2)

'    Public Const pxePRINT_EMP0 As String = Chr(27) & Chr(69) & Chr(0)
'    Public Const pxePRINT_EMP1 As String = Chr(27) & Chr(69) & Chr(1)

'    Public Const pxePRINT_DBL0 As String = Chr(27) & Chr(71) & Chr(0)
'    Public Const pxePRINT_DBL1 As String = Chr(27) & Chr(71) & Chr(1)

'    Public Const pxePRINT_LEFT As String = Chr(27) & Chr(97) & Chr(0)
'    Public Const pxePRINT_CNTR As String = Chr(27) & Chr(97) & Chr(1)
'    Public Const pxePRINT_RGHT As String = Chr(27) & Chr(97) & Chr(2)

'    Public Const pxePRINT_ESC As String = Chr(27) & Chr(33)
'    Public Const pxeESC_FNT0 As Integer = 0
'    Public Const pxeESC_FNT1 As Integer = 1
'    Public Const pxeESC_EMPH As Integer = 8
'    Public Const pxeESC_DBLH As Integer = 16
'    Public Const pxeESC_DBLW As Integer = 32
'    Public Const pxeESC_ULINE As Integer = 128

'    Public Const pxePRINT_PRTL As String = Chr(29) & Chr(86) & Chr(0)
'    Public Const pxePRINT_FULL_CUT As String = Chr(&H1D) & "V" & Chr(66) & Chr(0)

'    '==================================================
'    ' FILE OUTPUT HELPERS (USED IN DEV MODE)
'    '==================================================
'    Public Shared Sub writeToFile(ByVal POSNo As String, ByVal value As String)
'        Dim basePath As String = Environment.GetEnvironmentVariable("RMS-EPATH")
'        If String.IsNullOrEmpty(basePath) Then
'            basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\"
'        End If

'        Dim path As String = basePath & POSNo & ".txt"
'        EnsureAsciiFile(path) ' convert existing file if needed
'        File.AppendAllText(path, value & Environment.NewLine, Encoding.ASCII)
'    End Sub

'    Public Shared Sub writeToRLC(ByVal fileName As String, ByVal value As String)
'        Dim basePath As String = Environment.GetEnvironmentVariable("RLC-EPATH")
'        If String.IsNullOrEmpty(basePath) Then
'            basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\"
'        End If

'        Dim path As String = basePath & fileName
'        EnsureAsciiFile(path)
'        File.AppendAllText(path, value & Environment.NewLine, Encoding.ASCII)
'    End Sub

'    '==================================================
'    ' Helper: detect BOM and convert existing file to ASCII if needed
'    '==================================================
'    Private Shared Sub EnsureAsciiFile(ByVal path As String)
'        Try
'            If Not File.Exists(path) Then
'                Return
'            End If

'            Dim raw() As Byte = File.ReadAllBytes(path)
'            Dim enc As Encoding = DetectEncodingFromBom(raw)

'            ' If file is already ASCII (or empty) we do nothing
'            If enc Is Encoding.ASCII OrElse raw.Length = 0 Then
'                Return
'            End If

'            ' Read the textual content using detected encoding and re-write as ASCII
'            Dim text As String = File.ReadAllText(path, enc)
'            ' Optionally sanitize to printable ASCII only:
'            text = StripEscPos(text)
'            File.WriteAllText(path, text, Encoding.ASCII)
'        Catch ex As Exception
'            ' If conversion fails for any reason, best-effort: rename the old file
'            Try
'                Dim bak As String = path & ".bak"
'                If File.Exists(bak) Then File.Delete(bak)
'                File.Move(path, bak)
'            Catch
'            End Try
'        End Try
'    End Sub

'    Private Shared Function DetectEncodingFromBom(ByVal bytes() As Byte) As Encoding
'        If bytes Is Nothing OrElse bytes.Length = 0 Then Return Encoding.ASCII

'        If bytes.Length >= 3 AndAlso bytes(0) = &HEF AndAlso bytes(1) = &HBB AndAlso bytes(2) = &HBF Then
'            Return Encoding.UTF8 ' UTF-8 BOM
'        End If
'        If bytes.Length >= 2 Then
'            If bytes(0) = &HFF AndAlso bytes(1) = &HFE Then
'                Return Encoding.Unicode ' UTF-16 LE (little-endian)
'            End If
'            If bytes(0) = &HFE AndAlso bytes(1) = &HFF Then
'                Return Encoding.BigEndianUnicode ' UTF-16 BE
'            End If
'        End If

'        ' No BOM — assume system default (likely ANSI) but still return system default so read works
'        Return Encoding.Default
'    End Function

'    Private Shared Function StripEscPos(ByVal input As String) As String
'        Dim sb As New System.Text.StringBuilder()
'        For Each ch As Char In input
'            Dim code As Integer = AscW(ch)
'            ' Keep printable ASCII characters only (32-126) and CR/LF
'            If (code >= 32 AndAlso code <= 126) OrElse ch = vbLf OrElse ch = vbCr Then
'                sb.Append(ch)
'            End If
'        Next
'        Return sb.ToString()
'    End Function

'    '==================================================
'    ' STRING PRINT ENTRY POINT (USED BY PRN_* FILES)
'    '==================================================
'    Public Shared Sub SendStringToPrinter(ByVal szPrinterName As String, ByVal szString As String)
'        Dim safePrinter As String
'        If Not String.IsNullOrEmpty(szPrinterName) Then
'            ' add location for the printer (Kitchen, Bar, Turo-Turo)
'            safePrinter = szPrinterName
'        Else
'            ' STILL add location for the printer (Kitchen, Bar, Turo-Turo) 
'            safePrinter = "DEFAULT"
'        End If

'        If DEV_MODE Then

'            ' Remove ESC/POS control codes so text is readable
'            Dim cleanText As String = StripEscPos(szString)

'            ' Make safe filename from printer name
'            If Not String.IsNullOrEmpty(szPrinterName) Then
'                safePrinter = szPrinterName _
'                .Replace("\\", "") _
'                .Replace("/", "_") _
'                .Replace(":", "") _
'                .Replace(" ", "_")
'            End If

'            ' Output folder (Desktop fallback)
'            Dim basePath As String = Environment.GetEnvironmentVariable("RMS-EPATH")
'            If String.IsNullOrEmpty(basePath) Then
'                basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\"
'            End If

'            Dim outFile As String = basePath & "DEV_" & safePrinter & ".txt"

'            ' ensure any existing file is converted to ASCII to avoid mixed-encoding garbage
'            EnsureAsciiFile(outFile)

'            ' Header so multiple prints are readable
'            Dim header As String =
'            "======================================" & vbCrLf &
'            " PRINTER : " & szPrinterName & vbCrLf &
'            " TIME    : " & Now.ToString("yyyy-MM-dd HH:mm:ss") & vbCrLf &
'            "======================================" & vbCrLf

'            File.AppendAllText(outFile, header & cleanText & vbCrLf & vbCrLf, Encoding.ASCII)

'            Exit Sub
'        End If

'        ' ===============================
'        ' PROD MODE → REAL PRINTER
'        ' ===============================
'        Dim pBytes As IntPtr = Marshal.StringToCoTaskMemAnsi(szString)
'        Dim dwCount As Integer = szString.Length

'        SendBytesToPrinter(szPrinterName, pBytes, dwCount)

'        Marshal.FreeCoTaskMem(pBytes)
'    End Sub

'    '==================================================
'    ' RAW PRINT CORE (PRODUCTION ONLY)
'    '==================================================

'    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
'    Public Structure DOCINFOA
'        <MarshalAs(UnmanagedType.LPStr)> Public pDocName As String
'        <MarshalAs(UnmanagedType.LPStr)> Public pOutputFile As String
'        <MarshalAs(UnmanagedType.LPStr)> Public pDataType As String
'    End Structure

'    <DllImport("winspool.drv", EntryPoint:="OpenPrinterA",
'        SetLastError:=True, CharSet:=CharSet.Ansi)>
'    Private Shared Function OpenPrinter(
'        ByVal printerName As String,
'        ByRef hPrinter As IntPtr,
'        ByVal pDefault As IntPtr
'    ) As Boolean
'    End Function

'    <DllImport("winspool.drv", SetLastError:=True)>
'    Private Shared Function ClosePrinter(ByVal hPrinter As IntPtr) As Boolean
'    End Function

'    <DllImport("winspool.drv", EntryPoint:="StartDocPrinterA",
'        SetLastError:=True, CharSet:=CharSet.Ansi)>
'    Private Shared Function StartDocPrinter(
'        ByVal hPrinter As IntPtr,
'        ByVal level As Integer,
'        ByRef di As DOCINFOA
'    ) As Integer
'    End Function

'    <DllImport("winspool.drv", SetLastError:=True)>
'    Private Shared Function EndDocPrinter(ByVal hPrinter As IntPtr) As Boolean
'    End Function

'    <DllImport("winspool.drv", SetLastError:=True)>
'    Private Shared Function StartPagePrinter(ByVal hPrinter As IntPtr) As Boolean
'    End Function

'    <DllImport("winspool.drv", SetLastError:=True)>
'    Private Shared Function EndPagePrinter(ByVal hPrinter As IntPtr) As Boolean
'    End Function

'    <DllImport("winspool.drv", SetLastError:=True)>
'    Private Shared Function WritePrinter(
'        ByVal hPrinter As IntPtr,
'        ByVal pBytes As IntPtr,
'        ByVal count As Integer,
'        ByRef written As Integer
'    ) As Boolean
'    End Function

'Private Shared Function SendBytesToPrinter(
'        printerName As String,
'        pBytes As IntPtr,
'        count As Integer
'    ) As Boolean

'        Dim hPrinter As IntPtr = IntPtr.Zero
'        Dim written As Integer = 0

'        Dim di As New DOCINFOA With {
'            .pDocName = "ESC/POS Receipt",
'            .pDataType = "RAW",
'            .pOutputFile = Nothing
'        }

'        If Not OpenPrinter(printerName, hPrinter, IntPtr.Zero) Then Return False
'        If StartDocPrinter(hPrinter, 1, di) = 0 Then GoTo Cleanup
'        If Not StartPagePrinter(hPrinter) Then GoTo Cleanup
'        WritePrinter(hPrinter, pBytes, count, written)

'Cleanup:
'        EndPagePrinter(hPrinter)
'        EndDocPrinter(hPrinter)
'        ClosePrinter(hPrinter)
'        Return True
'    End Function

'End Class

Imports System.IO
Imports System.Drawing.Printing
Imports System.Runtime.InteropServices

Public Class RawPrint
    ' Structure and API declarions:
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Structure DOCINFOW
        <MarshalAs(UnmanagedType.LPWStr)> Public pDocName As String
        <MarshalAs(UnmanagedType.LPWStr)> Public pOutputFile As String
        <MarshalAs(UnmanagedType.LPWStr)> Public pDataType As String
    End Structure

    '<DllImport("winspool.Drv", EntryPoint:="OpenPrinterW", _
    '   SetLastError:=True, CharSet:=CharSet.Unicode, _
    '   ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
    'Public Shared Function OpenPrinter(ByVal src As String, ByRef hPrinter As IntPtr, ByVal pd As Long) As Boolean
    'End Function

    <DllImport("winspool.Drv", EntryPoint:="OpenPrinterW",
    SetLastError:=True, CharSet:=CharSet.Unicode,
    ExactSpelling:=False, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function OpenPrinter(ByVal src As String, ByRef hPrinter As IntPtr, ByVal pd As Integer) As Boolean
    End Function

    <DllImport("winspool.Drv", EntryPoint:="ClosePrinter",
       SetLastError:=True, CharSet:=CharSet.Unicode,
       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function ClosePrinter(ByVal hPrinter As IntPtr) As Boolean
    End Function
    <DllImport("winspool.Drv", EntryPoint:="StartDocPrinterW",
       SetLastError:=True, CharSet:=CharSet.Unicode,
       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function StartDocPrinter(ByVal hPrinter As IntPtr, ByVal level As Int32, ByRef pDI As DOCINFOW) As Boolean
    End Function
    <DllImport("winspool.Drv", EntryPoint:="EndDocPrinter",
       SetLastError:=True, CharSet:=CharSet.Unicode,
       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function EndDocPrinter(ByVal hPrinter As IntPtr) As Boolean
    End Function
    <DllImport("winspool.Drv", EntryPoint:="StartPagePrinter",
       SetLastError:=True, CharSet:=CharSet.Unicode,
       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function StartPagePrinter(ByVal hPrinter As IntPtr) As Boolean
    End Function
    <DllImport("winspool.Drv", EntryPoint:="EndPagePrinter",
       SetLastError:=True, CharSet:=CharSet.Unicode,
       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function EndPagePrinter(ByVal hPrinter As IntPtr) As Boolean
    End Function
    <DllImport("winspool.Drv", EntryPoint:="WritePrinter",
       SetLastError:=True, CharSet:=CharSet.Unicode,
       ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function WritePrinter(ByVal hPrinter As IntPtr, ByVal pBytes As IntPtr, ByVal dwCount As Int32, ByRef dwWritten As Int32) As Boolean
    End Function

    ' SendBytesToPrinter()
    ' When the function is given a printer name and an unmanaged array of  
    ' bytes, the function sends those bytes to the print queue.
    ' Returns True on success or False on failure.

    Public Const pxePRINT_INIT As String = Chr(27) & Chr(64)
    Public Const pxePRINT_FNT0 As String = Chr(27) & Chr(77) & Chr(0)
    Public Const pxePRINT_FNT1 As String = Chr(27) & Chr(77) & Chr(1)
    Public Const pxePRINT_FNT2 As String = Chr(27) & Chr(77) & Chr(2)

    Public Const pxePRINT_EMP0 As String = Chr(27) & Chr(69) & Chr(0)
    Public Const pxePRINT_EMP1 As String = Chr(27) & Chr(69) & Chr(1)

    Public Const pxePRINT_DBL0 As String = Chr(27) & Chr(71) & Chr(0)
    Public Const pxePRINT_DBL1 As String = Chr(27) & Chr(71) & Chr(1)

    Public Const pxePRINT_LEFT As String = Chr(27) & Chr(97) & Chr(0)
    Public Const pxePRINT_CNTR As String = Chr(27) & Chr(97) & Chr(1)
    Public Const pxePRINT_RGHT As String = Chr(27) & Chr(97) & Chr(2)
    Public Const pxePRINT_PRTL As String = Chr(29) & Chr(86) & Chr(0)

    Public Const pxePRINT_ESC As String = Chr(27) & Chr(33)
    Public Const pxeESC_FNT0 As Integer = 0
    Public Const pxeESC_FNT1 As Integer = 1
    Public Const pxeESC_EMPH As Integer = 8
    Public Const pxeESC_DBLH As Integer = 16
    Public Const pxeESC_DBLW As Integer = 32
    Public Const pxeESC_ULINE As Integer = 128

    Public Const pxePRINT_FULL_CUT As String = Chr(&H1D) & "V" & Chr(66) & Chr(0)

    Public Shared Function SendBytesToPrinter(ByVal szPrinterName As String, ByVal pBytes As IntPtr, ByVal dwCount As Int32) As Boolean
        Dim hPrinter As IntPtr      ' The printer handle.
        Dim dwError As Int32        ' Last error - in case there was trouble.
        Dim di As DOCINFOW          ' Describes your document (name, port, data type).
        Dim dwWritten As Int32      ' The number of bytes written by WritePrinter().
        Dim bSuccess As Boolean     ' Your success code.

        ' Set up the DOCINFO structure.
        With di
            .pDocName = "VB.NET RAW Printing"
            .pDataType = "RAW"
        End With
        ' Assume failure unless you specifically succeed.
        bSuccess = False
        If OpenPrinter(szPrinterName, hPrinter, 0) Then
            ' Check if the handle is valid before proceeding.
            If hPrinter <> IntPtr.Zero Then
                ' Start a new print job.
                If StartDocPrinter(hPrinter, 1, di) Then
                    ' Start a new page within the print job.
                    If StartPagePrinter(hPrinter) Then
                        ' Write your printer-specific bytes to the printer.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, dwWritten)

                        ' Check if writing to the printer was successful.
                        If bSuccess Then
                            ' End the current page within the print job.
                            EndPagePrinter(hPrinter)
                        Else
                            ' Handle WritePrinter failure
                            Console.WriteLine("WritePrinter failed. Error code: " & Marshal.GetLastWin32Error().ToString())
                        End If
                    Else
                        ' Handle StartPagePrinter failure
                        Console.WriteLine("StartPagePrinter failed. Error code: " & Marshal.GetLastWin32Error().ToString())
                    End If

                    ' End the current print job.
                    EndDocPrinter(hPrinter)
                Else
                    ' Handle StartDocPrinter failure
                    Console.WriteLine("StartDocPrinter failed. Error code: " & Marshal.GetLastWin32Error().ToString())
                End If

                ' Close the printer handle.
                ClosePrinter(hPrinter)
            Else
                ' Handle invalid printer handle
                Console.WriteLine("Invalid printer handle obtained from OpenPrinter.")
            End If
        Else
            ' Handle OpenPrinter failure
            Console.WriteLine("OpenPrinter failed. Error code: " & Marshal.GetLastWin32Error().ToString())
        End If

        ' Return the result of the printing operation.
        Return bSuccess
    End Function ' SendBytesToPrinter()

    ' SendFileToPrinter()
    ' When the function is given a file name and a printer name, 
    ' the function reads the contents of the file and sends the
    ' contents to the printer.
    ' Presumes that the file contains printer-ready data.
    ' Shows how to use the SendBytesToPrinter function.
    ' Returns True on success or False on failure.
    Public Shared Function SendFileToPrinter(ByVal szPrinterName As String, ByVal szFileName As String) As Boolean
        ' Open the file.
        Dim fs As New FileStream(szFileName, FileMode.Open)
        ' Create a BinaryReader on the file.
        Dim br As New BinaryReader(fs)
        ' Dim an array of bytes large enough to hold the file's contents.
        Dim bytes(fs.Length) As Byte
        Dim bSuccess As Boolean
        ' Your unmanaged pointer.
        Dim pUnmanagedBytes As IntPtr

        ' Read the contents of the file into the array.
        bytes = br.ReadBytes(fs.Length)
        ' Allocate some unmanaged memory for those bytes.
        pUnmanagedBytes = Marshal.AllocCoTaskMem(fs.Length)
        ' Copy the managed byte array into the unmanaged array.
        Marshal.Copy(bytes, 0, pUnmanagedBytes, fs.Length)
        ' Send the unmanaged bytes to the printer.
        bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, fs.Length)
        ' Free the unmanaged memory that you allocated earlier.
        Marshal.FreeCoTaskMem(pUnmanagedBytes)
        Return bSuccess
    End Function ' SendFileToPrinter()

    ' When the function is given a string and a printer name,
    ' the function sends the string to the printer as raw bytes.
    Public Shared Function SendStringToPrinter(ByVal szPrinterName As String, ByVal szString As String)
        Dim pBytes As IntPtr
        Dim dwCount As Int32
        ' How many characters are in the string?
        dwCount = szString.Length()
        ' Assume that the printer is expecting ANSI text, and then convert
        ' the string to ANSI text.
        pBytes = Marshal.StringToCoTaskMemAnsi(szString)
        ' Send the converted ANSI string to the printer.
        SendBytesToPrinter(szPrinterName, pBytes, dwCount)
        Marshal.FreeCoTaskMem(pBytes)
    End Function

    Public Shared Sub writeToFile(ByVal POSNo As String, ByVal Value As String)
        Dim Path As String = Environment.GetEnvironmentVariable("RMS-EPATH") & POSNo & ".txt"
        Dim Backup As String = Environment.GetEnvironmentVariable("RMS-BPATH") & POSNo & ".txt"

        If Not File.Exists(Path) Then File.Create(Path).Dispose()
        If Not File.Exists(Backup) Then File.Create(Backup).Dispose()

        My.Computer.FileSystem.WriteAllText(Path, Value, True)
        My.Computer.FileSystem.WriteAllText(Backup, Value, True)
    End Sub

    Public Shared Sub writeToRLC(ByVal FileName As String, ByVal Value As String)
        Dim Path As String = Environment.GetEnvironmentVariable("RLC-EPATH") & FileName
        Dim Backup As String = Environment.GetEnvironmentVariable("RLC-BPATH") & FileName

        If Not File.Exists(Path) Then File.Create(Path).Dispose()
        If Not File.Exists(Backup) Then File.Create(Backup).Dispose()

        My.Computer.FileSystem.WriteAllText(Path, Value, True)
        My.Computer.FileSystem.WriteAllText(Backup, Value, True)
    End Sub
End Class
