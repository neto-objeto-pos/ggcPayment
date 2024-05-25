Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmPay
    Private WithEvents poReceipt As Receipt
    Private pnLoadx As Integer
    Private pbLoaded As Boolean
    Private pbCancelled As Boolean
    Private pnBill As Double
    Private pnCharge As Double
    Private poControl As Control
    'Private  printerManager As PrinterManager

    WriteOnly Property Receipt() As Receipt
        Set(ByVal oReceipt As Receipt)
            poReceipt = oReceipt
        End Set
    End Property

    ReadOnly Property Cancelled() As Boolean
        Get
            Return pbCancelled
        End Get
    End Property

    Private Sub frmPay_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        If Not pbLoaded Then
            Call clearFields()
            pbLoaded = False
        End If
    End Sub




    Private Sub frmPay_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        setVisible()

        If pnLoadx = 0 Then
            'showDetail(True)
            'printerManager = New PrinterManager()
            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)

            If poReceipt.OpenBySource() Then
                clearFields()
            End If
            pnBill = poReceipt.myBill
            pnCharge = poReceipt.myCharge

            isDelivery()
            pnLoadx = 1
            End If
    End Sub

    Private Sub frmPay_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        setVisible()
    End Sub
    Private Sub isDelivery()
        If poReceipt.TranType = "2" Then
            cmdButton01.Enabled = False
            cmdButton02.Enabled = False
            cmdButton03.Enabled = False
            cmdButton04.Enabled = False
            txtAmount.Enabled = False
        Else
            cmdButton05.Enabled = False
        End If

    End Sub
    Private Sub cmdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim loChk As Button
        Dim lbCloseForm As Boolean
        loChk = CType(sender, System.Windows.Forms.Button)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loChk.Name, 10))

        Select Case lnIndex
            Case 0 'OK
                'printerManager.Print()
                If Not IsNumeric(txtAmount.Text) Then
                    MsgBox("Invalid Amount Tendered..." & vbCrLf &
                            "Please Verify your entry then try again...", MsgBoxStyle.Critical, "WARNING")
                    GoTo endProc
                Else
                    'we accept no cash payment
                    'If CDec(txtAmount.Text) <= 0.0 Then
                    '    MsgBox("Invalid Amount Tendered..." & vbCrLf &
                    '            "Please Verify your entry then try again...", MsgBoxStyle.Critical, "WARNING")
                    '    GoTo endProc
                    'End If
                End If
                If p_nCheck + p_nCreditCard + p_nGiftCert + p_nCash + p_nSchargex + p_nDelivery >= CDec(lblBill.Text) Then
                    pbCancelled = False
                    Me.Close()
                    Me.Dispose()
                Else
                    MsgBox("Invalid Amount Paid..." & vbCrLf &
                                "Please Verify your entry then try again...", MsgBoxStyle.Critical, "WARNING")
                    GoTo endProc
                End If

                'If CDbl(txtAmount.Text) >= pnBill Then
                '    pbCancelled = False
                '    Me.Close()
                '    Me.Dispose()
                'Else
                '    MsgBox("Invalid Amount Paid..." & vbCrLf & _
                '                "Please Verify your entry then try again...", MsgBoxStyle.Critical, "WARNING")
                '    GoTo endProc
                'End If
            Case 1 'CASH
                'N/A
            Case 2 'CREDIT CARD
                poReceipt.showCreditCard(lbCloseForm)
                If lbCloseForm Then
                    Me.Close()
                    Me.Dispose()
                End If
            Case 3 'CHECK
                poReceipt.showCheck(lbCloseForm)
                If lbCloseForm Then
                    Me.Close()
                    Me.Dispose()
                End If
            Case 4 'GIFT CERT
                poReceipt.showGiftCert(lbCloseForm)
                If lbCloseForm Then
                    Me.Close()
                    Me.Dispose()
                End If
            Case 5 'Delivery
                poReceipt.showDeliverys(lbCloseForm)
                'If lbCloseForm Then
                '    'Me.Close()
                '    'Me.Dispose()
                'End If
        End Select
endProc:
        Exit Sub
    End Sub

    Private Sub showDetail(ByVal lbShow As Boolean)
        Dim lvDetailLoc As New Point(3, 280)
        Dim lvButtonLoc As New Point(3, 470)
        Dim lvMPnelOrgx As New Size(390, 541)
        Dim lvMPnelNewx As New Size(390, 541)
        Dim lvFormOrgxx As New Size(390, 541)
        Dim lvFormNewxx As New Size(390, 541)

        If lbShow Then
            Me.Size = lvFormOrgxx
            pnlDetail.Visible = True
            pnlMain.Size = lvMPnelOrgx
            pnlButtons.Location = lvButtonLoc
        Else
            Me.Size = lvFormNewxx
            pnlDetail.Visible = False
            pnlMain.Size = lvMPnelNewx
            pnlButtons.Location = lvDetailLoc
        End If
    End Sub

    Private Sub setVisible()
        Me.Visible = False
        Me.TransparencyKey = Nothing
        Me.Location = New Point(507, 90)

        txtAmount.MaxLength = 9
        txtAmount.Focus()
        Me.Visible = True
    End Sub


    Private Sub computeChange()
        Dim lnBill As Decimal = CDec(lblBill.Text)

        If p_nGiftCert > 0 And p_nTendered + p_nCheck + p_nCreditCard + p_nDelivery = 0 Then 'GC payment only
            lblChange.Text = "0.00"
        ElseIf p_nGiftCert > 0 And p_nTendered + p_nCheck + p_nCreditCard + p_nDelivery > 0 Then 'GC + Others
            lblChange.Text = FormatNumber((p_nTendered + p_nCheck + p_nCreditCard) - (lnBill - p_nGiftCert), 2)
        ElseIf p_nTendered + p_nCheck + p_nCreditCard + p_nGiftCert + p_nDelivery <> 0 Then
            If p_nGiftCert > 0 Then
                If p_nGiftCert > lnBill Then
                    lblChange.Text = "0.00"
                Else
                    lblChange.Text = FormatNumber((p_nTendered + p_nCheck + p_nCreditCard + p_nGiftCert + p_nDelivery) - lnBill, 2)
                End If
            Else
                lblChange.Text = FormatNumber((p_nTendered + p_nCheck + p_nCreditCard + p_nGiftCert + p_nDelivery) - lnBill, 2)
            End If
        Else
            lblChange.Text = "0.00"
        End If

        lblCash.Text = FormatNumber(p_nTendered, 2)
        lblCreditCard.Text = FormatNumber(p_nCreditCard, 2)
        lblCheck.Text = FormatNumber(p_nCheck, 2)
        lblGiftCheck.Text = FormatNumber(p_nGiftCert, 2)
        lblDelivery.Text = FormatNumber(p_nDelivery, 2)
        lblTotal.Text = FormatNumber(p_nTendered + p_nCheck + p_nCreditCard + p_nGiftCert + p_nDelivery, 2)
    End Sub

    Private Sub clearFields()
        lblBill.Text = FormatNumber(poReceipt.myBill + poReceipt.myCharge, 2)
        p_nSalesAmt = FormatNumber(poReceipt.myBill, 2)
        p_nSchargex = FormatNumber(poReceipt.myCharge, 2)
        lblDiscount.Text = FormatNumber(poReceipt.Master("nDiscount") + poReceipt.Master("nVatDiscx") + poReceipt.Master("nPWDDiscx"), 2)
        txtAmount.Text = FormatNumber(poReceipt.Master("nTendered"), 2)

        Call computeChange()
    End Sub

    Private Sub txtAmount_GotFocus(sender As Object, e As System.EventArgs) Handles txtAmount.GotFocus
        With txtAmount
            .BackColor = Color.Azure
            .SelectAll()
        End With
    End Sub

    Private Sub txtAmount_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtAmount.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtAmount_LostFocus(sender As Object, e As System.EventArgs) Handles txtAmount.LostFocus
        With txtAmount
            .BackColor = SystemColors.Window

            If Not IsNumeric(.Text) Then .Text = 0
            .Text = FormatNumber(.Text, 2)

            poReceipt.Master("nTendered") = CDec(.Text)
            p_nTendered = poReceipt.Master("nTendered")
            poReceipt.Master("nSChargex") = p_nSchargex
            poReceipt.Master("nSalesAmt") = p_nSalesAmt
            If poReceipt.Master("nTendered") > 0.0 Then
                If poReceipt.Master("nTendered") >= (poReceipt.Master("nSalesAmt") + poReceipt.Master("nSChargex")) Then
                    p_nCash = poReceipt.Master("nSalesAmt") + poReceipt.Master("nSChargex")
                Else
                    If (p_nCheck + p_nCreditCard + p_nGiftCert) >= (poReceipt.Master("nSalesAmt") + poReceipt.Master("nSChargex")) Then
                        p_nCash = 0
                    Else
                        p_nCash = Math.Abs((poReceipt.Master("nSalesAmt") + poReceipt.Master("nSChargex")) - (p_nCheck + p_nCreditCard + p_nGiftCert))
                    End If
                End If
            End If

            poReceipt.Master("nCashAmtx") = p_nCash

            Call computeChange()
        End With
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

    Private Sub frmPay_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                pbCancelled = True
                Me.Close()
        End Select
    End Sub
End Class