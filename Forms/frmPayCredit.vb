Imports System.Drawing
Imports System.Windows.Forms

Public Class frmPayCredit
    Private WithEvents poCreditCard As CreditCard
    Private pnLoadx As Integer
    Private poControl As Control
    Private pbCloseForm As Boolean
    Private pnActiveRow As Integer
    
    WriteOnly Property CreditCard() As CreditCard
        Set(ByVal oCreditCard As CreditCard)
            poCreditCard = oCreditCard
        End Set
    End Property

    ReadOnly Property CloseForm() As Boolean
        Get
            Return pbCloseForm
        End Get
    End Property

    Private Sub frmPayCredit_Keydown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                pbCloseForm = False
                If Not IsNothing(p_oFormPayCredit) Then showForm(2, False)
                If Not IsNothing(p_oFormCheck) Then showForm(3, False)
                If Not IsNothing(p_oFormGC) Then showForm(4, False)
                showForm(1, False)
            Case Keys.Return, Keys.Down
                SetNextFocus()
            Case Keys.Up
                SetPreviousFocus()
        End Select
    End Sub

    Private Sub frmPayCredit_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        setVisible()

        If pnLoadx = 0 Then
            showDetail(True)
            clearFields()
            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "GotFocus", AddressOf txtField_GotFocus)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "LostFocus", AddressOf txtField_LostFocus)
            Call grpKeyHandler(Me, GetType(TextBox), "txtField", "KeyDown", AddressOf txtField_KeyDown)

            Dim row As DataRow
            txtField00.AutoCompleteCustomSource.Clear()
            For Each row In poCreditCard.SearchBank.Rows
                txtField00.AutoCompleteCustomSource.Add(row.Item("sBankName").ToString())
            Next

            txtField00.AutoCompleteSource = AutoCompleteSource.CustomSource
            txtField00.AutoCompleteMode = AutoCompleteMode.SuggestAppend

            txtField03.AutoCompleteCustomSource.Clear()
            For Each row In poCreditCard.SearchTerm.Rows
                txtField03.AutoCompleteCustomSource.Add(row.Item("sTermName").ToString())
            Next

            txtField03.AutoCompleteSource = AutoCompleteSource.CustomSource
            txtField03.AutoCompleteMode = AutoCompleteMode.SuggestAppend

            txtField04.AutoCompleteCustomSource.Clear()
            For Each row In poCreditCard.SearchTerminal.Rows
                txtField04.AutoCompleteCustomSource.Add(row.Item("sBankName").ToString())
            Next

            txtField04.AutoCompleteSource = AutoCompleteSource.CustomSource
            txtField04.AutoCompleteMode = AutoCompleteMode.SuggestAppend

            'open by source code and number
            If poCreditCard.OpenBySource() Then loadOthers()

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
                pbCloseForm = True
                If Not isEntryOk(True) Then GoTo endProc
                If poCreditCard.SaveTransaction() Then
                    Me.Close()
                    Me.Dispose()
                End If
            Case 1, 3, 4
                pbCloseForm = False
                If isEntryOk(False) Then
                    If Not poCreditCard.SaveTransaction() Then
                        Me.Close()
                        Me.Dispose()
                        GoTo endProc
                    End If
                End If

                'Me.Dispose()
                'Me.Close()

                Select Case lnIndex
                    Case 3
                        Dim loCheck As New CheckPayment(poCreditCard.AppDriver)
                        loCheck.SourceCd = poCreditCard.SourceCd
                        loCheck.SourceNo = poCreditCard.SourceNo
                        loCheck.ShowCheck()
                    Case 4
                        Dim loGiftCert As New GiftCerticate(poCreditCard.AppDriver)
                        loGiftCert.SourceCd = poCreditCard.SourceCd
                        loGiftCert.SourceNo = poCreditCard.SourceNo
                        loGiftCert.ShowGiftCert()
                    Case 1
                        Me.Hide()

                End Select
            Case 2 'CREDIT CARD

            Case 5 'ADD CREDIT CARD
                If poCreditCard.AddCreditCard Then
                    Call loadOthers()
                    Call computeChange()
                End If
            Case 6 ' Delete credit Card
                'If poCreditCard.DeleteCreditCard(pnActiveRow) Then
                '    Call loadOthers()
                '    Call computeChange()
                'End If
                If poCreditCard.ItemCount > 1 Then
                    If DataGridView1.RowCount - 1 > 0 Then
                        poCreditCard.DeleteCreditCard(pnActiveRow)
                        loadOthers()
                    Else
                        poCreditCard.DeleteCreditCard(pnActiveRow)
                        poCreditCard.AddCreditCard()
                        loadOthers()
                    End If
                Else
                    poCreditCard.Master(0, "sBankIDxx") = ""
                    poCreditCard.Master(0, "sCardNoxx") = ""
                    poCreditCard.Master(0, "sApprovNo") = ""
                    poCreditCard.Master(0, "stermIDxx") = ""
                    poCreditCard.Master(0, "stermnlid") = ""
                    poCreditCard.Master(0, "nAmountxx") = 0

                    txtField00.Text = ""
                    txtField01.Text = ""
                    txtField02.Text = ""
                    txtField03.Text = ""
                    txtField04.Text = ""
                    txtField05.Text = 0
                    loadOthers()
                End If
        End Select
endProc:
        Exit Sub
    End Sub

    Private Sub txtField_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        poControl = loTxt

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
                    If loTxt.Text <> String.Empty Then poCreditCard.SearchBank(pnActiveRow, loTxt.Text)
                Case 1
                    poCreditCard.Master(pnActiveRow, "sCardNoxx") = loTxt.Text
                Case 2
                    poCreditCard.Master(pnActiveRow, "sApprovNo") = loTxt.Text
                Case 3
                    If loTxt.Text <> String.Empty Then poCreditCard.SearchTerm(pnActiveRow, loTxt.Text)
                Case 4
                    If loTxt.Text <> String.Empty Then poCreditCard.SearchTerminal(pnActiveRow, loTxt.Text)
                Case 5
                    poCreditCard.Master(pnActiveRow, "nAmountxx") = loTxt.Text
            End Select
        End If

        poControl = Nothing
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

    Private Sub showDetail(ByVal lbShow As Boolean)
        Dim lvDetailLoc As New Point(3, 391)
        Dim lvButtonLoc As New Point(3, 391)
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
            'pnlDetail.Visible = True
            pnlMain.Size = lvMPnelNewx
            pnlButtons.Location = lvDetailLoc
        End If

        InitGrid()
    End Sub

    Private Sub setVisible()
        Me.Visible = False
        Me.TransparencyKey = Nothing
        '#559
        Me.Location = New Point(507, 90)

        txtField00.MaxLength = 32
        txtField01.MaxLength = 16
        txtField02.MaxLength = 8
        txtField03.MaxLength = 25
        txtField04.MaxLength = 32
        txtField05.MaxLength = 9

        txtField00.Focus()
        Me.Visible = True
    End Sub

    Private Sub computeChange()
        Dim lnBill As Decimal = CDec(lblBill.Text)

        If p_nGiftCert > 0 And p_nTendered + p_nCheck + p_nCreditCard = 0 Then 'GC payment only
            lblChange.Text = "0.00"
        ElseIf p_nGiftCert > 0 And p_nTendered + p_nCheck + p_nCreditCard > 0 Then 'GC + Others
            lblChange.Text = FormatNumber((p_nTendered + p_nCheck + p_nCreditCard) - (lnBill - p_nGiftCert), 2)
        ElseIf p_nTendered + p_nCheck + p_nCreditCard + p_nGiftCert <> 0 Then
            If p_nGiftCert > 0 Then
                If p_nGiftCert > lnBill Then
                    lblChange.Text = "0.00"
                Else
                    lblChange.Text = FormatNumber((p_nTendered + p_nCheck + p_nCreditCard + p_nGiftCert) - lnBill, 2)
                End If
            Else
                lblChange.Text = FormatNumber((p_nTendered + p_nCheck + p_nCreditCard + p_nGiftCert) - lnBill, 2)
            End If
        Else
            lblChange.Text = "0.00"
        End If
    End Sub

    Private Sub clearFields()
        Dim lnRow As Integer

        lblBill.Text = FormatNumber(p_nSalesAmt + p_nSchargex, 2)
        With poCreditCard
            lnRow = .ItemCount - 1
            txtField00.Text = .Master(lnRow, "sTermnlNm")
            txtField04.Text = .Master(lnRow, "sBankName")
            txtField01.Text = .Master(lnRow, "sCardNoxx")
            txtField02.Text = .Master(lnRow, "sApprovNo")
            txtField03.Text = .Master(lnRow, "sTermName")
            txtField05.Text = FormatNumber(.Master(lnRow, "nAmountxx"), 2)
        End With
        Call computeChange()
    End Sub

    Private Sub poCreditCard_MasterRetrieved(Row As Integer, Index As Integer, Value As Object) Handles poCreditCard.MasterRetrieved
        Select Case Index
            Case 1
                txtField04.Text = Value
            Case 2
                txtField00.Text = Value
            Case 5
                txtField05.Text = FormatNumber(Value, 2)
            Case 6
                txtField03.Text = Value
        End Select
    End Sub

    Private Function isEntryOk(ByVal DisplayMsg As Boolean) As Boolean
        Dim lbDeleted As Boolean

        For lnCtr As Integer = 0 To poCreditCard.ItemCount - 1
            If poCreditCard.Master(lnCtr, "sBankIDxx") = "" Then
                If poCreditCard.DeleteCreditCard(lnCtr) Then
                    If Not lbDeleted Then lbDeleted = True
                End If
            End If
        Next lnCtr
        If lbDeleted Then loadOthers()

        If txtField00.Text = String.Empty Then
            If DisplayMsg Then
                MsgBox("Invalid Bank detected..." & vbCrLf & _
                        "Please verify your entry then try again...", MsgBoxStyle.Critical, "WARNING")
            End If
            Return False
        End If

        If txtField01.Text = String.Empty Or Trim(txtField01.Text.Length > 16) Then
            If DisplayMsg Then
                MsgBox("Invalid Card Number detected..." & vbCrLf &
                        "Please verify your entry then try again...", MsgBoxStyle.Critical, "WARNING")
            End If
            Return False
        End If

        If txtField02.Text = String.Empty Then
            If DisplayMsg Then
                MsgBox("Invalid Approval Number detected..." & vbCrLf & _
                        "Please verify your entry then try again...", MsgBoxStyle.Critical, "WARNING")
            End If
            Return False
        End If

        If txtField04.Text = String.Empty Then
            If DisplayMsg Then
                MsgBox("Invalid Terminal detected..." & vbCrLf & _
                        "Please verify your entry then try again...", MsgBoxStyle.Critical, "WARNING")
                Return False
            End If
        End If

        If CDec(txtField05.Text) = 0.0 Then
            If DisplayMsg Then
                MsgBox("Invalid Amount Paid..." & vbCrLf & _
                        "Please verify your entry then try again...", MsgBoxStyle.Critical, "WARNING")
            End If
            Return False
        End If

        p_nCreditCard = 0.0
        For lnCtr As Integer = 0 To poCreditCard.ItemCount - 1
            p_nCreditCard = p_nCreditCard + poCreditCard.Master(lnCtr, "nAmountxx")
        Next lnCtr

        If DisplayMsg Then
            'p_nSalesAmt - p_nDiscount > p_nCash + p_nCheck + p_nGiftCert + p_nCreditCard
            If CDec(lblBill.Text) > p_nCash + p_nCheck + p_nGiftCert + p_nCreditCard Then
                MsgBox("Invalid Amount Paid..." & vbCrLf & _
                        "Please verify your entry then try again...", MsgBoxStyle.Critical, "WARNING")
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub DataGridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.Click
        If DataGridView1.Rows.Count <= 0 Then Exit Sub
        With DataGridView1
            pnActiveRow = .CurrentCell.RowIndex

            setFieldValue(pnActiveRow)
        End With
    End Sub

    Private Sub txtField05_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtField05.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub

    Private Sub loadOthers()
        Dim lnCtr As Integer
        Dim lnRow As Integer
        Dim lnTotal As Decimal

        Call InitGrid()
        With DataGridView1
            If poCreditCard.ItemCount > 0 Then
                lnRow = poCreditCard.ItemCount
                .RowCount = lnRow
                For lnCtr = 0 To lnRow - 1
                    .Rows(lnCtr).Cells(0).Value = lnCtr + 1
                    .Rows(lnCtr).Cells(1).Value = poCreditCard.Master(lnCtr, "sBankName")
                    .Rows(lnCtr).Cells(2).Value = poCreditCard.Master(lnCtr, "sCardNoxx")
                    .Rows(lnCtr).Cells(3).Value = FormatNumber(poCreditCard.Master(lnCtr, "nAmountxx"), 2)
                    lnTotal = lnTotal + poCreditCard.Master(lnCtr, "nAmountxx")
                Next

                p_nCreditCard = lnTotal
                computeChange()

                .ClearSelection()
                .CurrentCell = .Rows(lnRow - 1).Cells(0)
                .Rows(lnRow - 1).Selected = True

                setFieldValue(lnRow - 1)

                If .Rows.Count > 1 Then showDetail(True)

                txtField00.Focus()
            End If
        End With
    End Sub

    Private Sub setFieldValue(ByVal nRow As Integer)
        With DataGridView1
            pnActiveRow = nRow
            txtField00.Text = poCreditCard.Master(nRow, "sBankName")
            txtField01.Text = poCreditCard.Master(nRow, "sCardNoxx")
            txtField02.Text = poCreditCard.Master(nRow, "sApprovNo")
            txtField03.Text = poCreditCard.Master(nRow, "sTermName")
            txtField04.Text = poCreditCard.Master(nRow, "sTermnlNm")
            txtField05.Text = FormatNumber(poCreditCard.Master(nRow, "nAmountxx"), 2)
            txtField00.Focus()
        End With
    End Sub

    Private Sub InitGrid()
        InitializeDataGrid()
        With DataGridView1
            'Set No of Columns
            .ColumnCount = 4

            'Set Column Headers
            .Columns(0).HeaderText = ""
            .Columns(1).HeaderText = "Bank"
            .Columns(2).HeaderText = "Card No"
            .Columns(3).HeaderText = "Amount"

            'Set Column Sizes
            .Columns(0).Width = 30
            .Columns(1).Width = 150
            .Columns(2).Width = 125
            .Columns(3).Width = 59

            .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable

            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Sub

    Private Sub InitializeDataGrid()
        With DataGridView1
            .Visible = True
            ' Initialize basic DataGridView properties.
            .Dock = DockStyle.Fill
            .BackgroundColor = Color.LightGray
            .BorderStyle = BorderStyle.Fixed3D

            ' Set property values appropriate for read-only display and 
            ' limited interactivity. 
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            .AllowUserToResizeColumns = False
            .ColumnHeadersHeightSizeMode = _
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .AllowUserToResizeRows = False
            .RowHeadersWidthSizeMode = _
                DataGridViewRowHeadersWidthSizeMode.DisableResizing

            ' Set the selection background color for all the cells.
            .DefaultCellStyle.SelectionBackColor = Color.Empty
            .DefaultCellStyle.SelectionForeColor = Color.Black

            ' Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            ' value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            .RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty 'Color.White

            ' Set the background color for all rows and for alternating rows. 
            ' The value for alternating rows overrides the value for all rows. 
            .RowsDefaultCellStyle.BackColor = Color.WhiteSmoke
            .AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro

            ' Set the row and column header styles.
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .RowHeadersDefaultCellStyle.BackColor = Color.Black
        End With

        With DataGridView1.ColumnHeadersDefaultCellStyle
            .BackColor = Color.Navy
            .ForeColor = Color.White
            .Font = New Font(DataGridView1.Font, FontStyle.Bold)
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
End Class