Imports System.Drawing
Imports System.Windows.Forms
Imports ggcAppDriver
Public Class frmRLC
    Private p_oRLC As PRN_RLC_Reading
    Private pnLoadx As Integer
    Private p_oApp As GRider

    Public Sub New(oApp As GRider)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        p_oApp = oApp
    End Sub

    WriteOnly Property RLC() As PRN_RLC_Reading
        Set(ByVal oRLC As PRN_RLC_Reading)
            p_oRLC = oRLC
        End Set
    End Property

    Private Sub frmRLC_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.Close()
            Case Keys.Return, Keys.Down
                SetNextFocus()
            Case Keys.Up
                SetPreviousFocus()
        End Select
    End Sub
    Private Sub frmPay_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If pnLoadx = 0 Then
            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "GotFocus", AddressOf txtField_GotFocus)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "LostFocus", AddressOf txtField_LostFocus)
            Call grpKeyHandler(Me, GetType(TextBox), "txtField", "KeyDown", AddressOf txtField_KeyDown)

            pnLoadx = 1
        End If
    End Sub

    Private Sub cmdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim loChk As Button
        loChk = CType(sender, System.Windows.Forms.Button)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loChk.Name, 10))

        Select Case lnIndex
            Case 0
                If IsDate(txtDate.Text) Then
                    If p_oRLC.resendFile(DateValue(txtDate.Text).Month.ToString.PadLeft(2, "0") & DateValue(txtDate.Text).Day.ToString.PadLeft(2, "0")) Then
                        Me.Close()
                    End If
                End If
            Case 1
                Dim loFormRLCUploaded As frmRLCUploaded

                loFormRLCUploaded = New frmRLCUploaded
                loFormRLCUploaded.ShowDialog()

                loFormRLCUploaded = Nothing
            Case 2
                Me.Close()
        End Select
endProc:
        Exit Sub
    End Sub

    Private Sub txtField_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        loTxt.BackColor = Color.Azure
        loTxt.SelectAll()
    End Sub
    Private Sub txtField_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        Dim loIndex As Integer
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        loTxt.BackColor = SystemColors.Window

        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
                Case 0
                    If Not IsDate(loTxt.Text) Then loTxt.Text = Format(p_oApp.SysDate, "MMM dd, yyyy")
            End Select
        End If
    End Sub

    Private Sub txtField_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.F3 Or e.KeyCode = Keys.Enter Then
            Dim loTxt As TextBox
            loTxt = CType(sender, System.Windows.Forms.TextBox)
            Dim loIndex As Integer
            loIndex = Val(Mid(loTxt.Name, 9))

            If Mid(loTxt.Name, 1, 8) = "txtField" Then

            End If
        End If
    End Sub

    Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or 33554432
            Return cp
        End Get
    End Property

    Private Sub PreventFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub

    Private Sub frmRLC_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        txtDate.Text = Format(p_oApp.SysDate, "MMM dd, yyyy")
    End Sub
End Class