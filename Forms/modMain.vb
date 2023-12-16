Imports System.Reflection
Imports System.Drawing
Imports System.Windows.Forms

Module modMain
    Public p_nCash As Decimal
    Public p_nCreditCard As Decimal
    Public p_nCheck As Decimal
    Public p_nGiftCert As Decimal
    Public p_nSalesAmt As Decimal
    Public p_nDiscount As Decimal
    Public p_nTendered As Decimal
    Public p_nSchargex As Decimal

    Public p_oDtaGCert As DataTable
    Public p_oDtaCheck As DataTable
    Public p_oDtaCCard As DataTable

    Public p_oFormPayCredit As frmPayCredit
    Public p_oFormCheck As frmPayCheck
    Public p_oFormGC As frmPayGC
    Public p_oFormPay As frmPay

    Private Declare Sub keybd_event Lib "user32.dll" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As UInteger, ByVal dwExtraInfo As UInteger)
    'Sub Main()
    '    Dim ls4Print As String
    '    ls4Print = "1435422222224545"
    '    ls4Print = Left(ls4Print, 12) & "".PadLeft(4, "*")
    '    Debug.Print(ls4Print)
    '    End
    'End Sub
    'This method can handle all events using EventHandler
    Public Sub grpEventHandler(ByVal foParent As Control, ByVal foType As Type, ByVal fsGroupNme As String, ByVal fsEvent As String, ByVal foAddress As EventHandler)
        Dim loTxt As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = foType Then
                'Handle events for this controls only
                If LCase(Mid(loTxt.Name, 1, Len(fsGroupNme))) = LCase(fsGroupNme) Then
                    If foType = GetType(TextBox) Then
                        Dim loObj = DirectCast(loTxt, TextBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(CheckBox) Then
                        Dim loObj = DirectCast(loTxt, CheckBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(Button) Then
                        Dim loObj = DirectCast(loTxt, Button)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    End If
                End If 'LCase(Mid(loTxt.Name, 1, 8)) = "txtfield"
            Else
                If loTxt.HasChildren Then
                    Call grpEventHandler(loTxt, foType, fsGroupNme, fsEvent, foAddress)
                End If
            End If
        Next 'loTxt In loControl.Controls
    End Sub

    'This method can handle all events using CancelEventHandler
    Public Sub grpCancelHandler(ByVal foParent As Control, ByVal foType As Type, ByVal fsGroupNme As String, ByVal fsEvent As String, ByVal foAddress As System.ComponentModel.CancelEventHandler)
        Dim loTxt As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = foType Then
                'Handle events for this controls only
                If LCase(Mid(loTxt.Name, 1, Len(fsGroupNme))) = LCase(fsGroupNme) Then
                    If foType = GetType(TextBox) Then
                        Dim loObj = DirectCast(loTxt, TextBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(CheckBox) Then
                        Dim loObj = DirectCast(loTxt, CheckBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(Button) Then
                        Dim loObj = DirectCast(loTxt, Button)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    End If
                End If 'LCase(Mid(loTxt.Name, 1, 8)) = "txtfield"
            Else
                If loTxt.HasChildren Then
                    Call grpCancelHandler(loTxt, foType, fsGroupNme, fsEvent, foAddress)
                End If
            End If
        Next 'loTxt In loControl.Controls
    End Sub

    'This method can handle all events using KeyEventHandler
    Public Sub grpKeyHandler(ByVal foParent As Control, ByVal foType As Type, ByVal fsGroupNme As String, ByVal fsEvent As String, ByVal foAddress As KeyEventHandler)
        Dim loTxt As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = foType Then
                'Handle events for this controls only
                If LCase(Mid(loTxt.Name, 1, Len(fsGroupNme))) = LCase(fsGroupNme) Then
                    If foType = GetType(TextBox) Then
                        Dim loObj = DirectCast(loTxt, TextBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(CheckBox) Then
                        Dim loObj = DirectCast(loTxt, CheckBox)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    ElseIf foType = GetType(Button) Then
                        Dim loObj = DirectCast(loTxt, Button)
                        Dim loEvent As EventInfo = foType.GetEvent(fsEvent)
                        loEvent.AddEventHandler(loObj, foAddress)
                    End If
                End If 'LCase(Mid(loTxt.Name, 1, 8)) = "txtfield"
            Else
                If loTxt.HasChildren Then
                    Call grpKeyHandler(loTxt, foType, fsGroupNme, fsEvent, foAddress)
                End If
            End If
        Next 'loTxt In loControl.Controls
    End Sub

    'This method can handle all events using EventHandler
    Public Function FindTextBox(ByVal foParent As Control, ByVal fsName As String) As Control
        Dim loTxt As Control
        Static loRet As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = GetType(TextBox) Then
                'Handle events for this controls only
                If LCase(loTxt.Name) = LCase(fsName) Then
                    loRet = loTxt
                End If
            Else
                If loTxt.HasChildren Then
                    Call FindTextBox(loTxt, fsName)
                End If
            End If
        Next 'loTxt In loControl.Controls

        Return loRet
    End Function

    Public Sub SetNextFocus()
        keybd_event(&H9, 0, 0, 0)
        keybd_event(&H9, 0, &H2, 0)
    End Sub

    Public Sub SetPreviousFocus()
        keybd_event(&H10, 0, 0, 0)
        keybd_event(&H9, 0, 0, 0)
        keybd_event(&H10, 0, &H2, 0)
    End Sub

    Sub showForm(ByVal lnForm As Integer, ByVal lbShow As Boolean)
        If lbShow Then
            Select Case lnForm
                Case 1 'cash
                    'p_oFormPay.TopMost = True
                    'p_oFormPay.Show()
                Case 2 'credit
                    p_oFormPayCredit.TopMost = True
                    p_oFormPayCredit.Show()
                Case 3 'check
                    p_oFormCheck.TopMost = True
                    p_oFormCheck.Show()
                Case 4 'gc
                    p_oFormGC.TopMost = True
                    p_oFormGC.Show()
            End Select
        Else
            Select Case lnForm
                Case 1 'cash
                    'p_oFormPay.Hide()
                Case 2 'credit
                    p_oFormPayCredit.Hide()
                Case 3 'check
                    p_oFormCheck.Hide()
                Case 4 'gc
                    p_oFormGC.Hide()
            End Select
        End If
    End Sub

    'This method can handle all events using EventHandler
    Public Function FindLabel(ByVal foParent As Control, ByVal fsName As String) As Control
        Dim loTxt As Control
        Static loRet As Control
        For Each loTxt In foParent.Controls
            If loTxt.GetType = GetType(Label) Then
                'Handle events for this controls only
                If LCase(loTxt.Name) = LCase(fsName) Then
                    loRet = loTxt
                End If
            Else
                If loTxt.HasChildren Then
                    Call FindLabel(loTxt, fsName)
                End If
            End If
        Next 'loTxt In loControl.Controls

        Return loRet
    End Function
End Module

