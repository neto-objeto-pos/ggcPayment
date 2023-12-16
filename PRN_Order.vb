'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Sales Order Printing
'
' Copyright 2016 and Beyond
' All Rights Reserved
' ºººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººº
' €  All  rights reserved. No part of this  software  €€  This Software is Owned by        €
' €  may be reproduced or transmitted in any form or  €€                                   €
' €  by   any   means,  electronic   or  mechanical,  €€    GUANZON MERCHANDISING CORP.    €
' €  including recording, or by information  storage  €€     Guanzon Bldg. Perez Blvd.     €
' €  and  retrieval  systems, without  prior written  €€           Dagupan City            €
' €  from the author.                                 €€  Tel No. 522-1085 ; 522-9275      €
' ºººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººº
' Sales Order Printing Sample
'1234567890123456789012345678901234567890
'
'             MONARK HOTEL
'   PEDRITO'S BAKESHOP AND RESTAURANT
'   Mc Arthur Highway, Tapuac District
'       Dagupan City, Pangasinan
'****************************************
'QTY DESCRIPTION       UPRICE     AMOUNT 
'  2 123456789012345 2,500.00   5,000.00
'  1 CLUBHSE SANDWCH   140.00     140.00
'----------------------------------------
'  3 item(s)                   
'
' TABLE NO: XXX
' CTRL NO : XXXXXXXX
' WAITER  : Marlon A. Sayson
' Date    : 11/18/2016 09:15 am
'****************************************
'
' ==========================================================================================
'  kalyptus [ 11/21/2016 09:07 am ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

Imports ADODB
Imports ggcAppDriver
Imports System.Drawing

Public Class PRN_Order
    Private p_oApp As GRider
    Private p_sCompny As String     'Company  : MONARK HOTEL
    Private p_sPOSNo As String      'MIN:       14121419321782091
    Private p_sVATReg As String     'TIN:       941-184-389-000

    Private p_sPermit As String     'Permit No: PR122014-004-D004507-000
    Private p_sSerial As String     'Serial No: L9GF261769
    Private p_sAccrdt As String     'Accrdt No: 038-227471337-000028
    Private p_sTermnl As String     'Termnl No: 02

    Private p_oDTDetail As DataTable
    Private p_oDTHeader As DataTable
    Private p_oDTFooter As DataTable
    Private p_oDT As DataTable

    Private pnTotalItm As Decimal
    Private psContrlNo As String
    Private psWaiterxx As String
    Private psTableNox As String
    Private pdTransact As Date

    Private p_sCashier As String
    Private p_sReferNox As String
    Private p_sLogName As String

    Private Const pxeQTYLEN As Integer = 3  '+ 1
    Private Const pxeDSCLEN As Integer = 15 '+ 1
    Private Const pxePRCLEN As Integer = 8  '+ 1
    Private Const pxeTTLLEN As Integer = 10
    Private Const pxeREGLEN As Integer = 12
    Private Const pxeLFTMGN As Integer = 3

    Public Property Transaction_Date() As Date
        Get
            Return pdTransact
        End Get
        Set(ByVal value As Date)
            pdTransact = value
        End Set
    End Property

    Public Property ControlNo() As String
        Get
            Return psContrlNo
        End Get
        Set(ByVal value As String)
            psContrlNo = value
        End Set
    End Property

    WriteOnly Property Cashier As String
        Set(ByVal value As String)
            p_sCashier = value
        End Set
    End Property

    WriteOnly Property LogName As String
        Set(ByVal value As String)
            p_sLogName = value
        End Set
    End Property

    WriteOnly Property ReferNox As String
        Set(ByVal value As String)
            p_sReferNox = value
        End Set
    End Property
    Public Property Waiter() As String
        Get
            Return psWaiterxx
        End Get
        Set(ByVal value As String)
            psWaiterxx = value
        End Set
    End Property

    Public WriteOnly Property Terminal() As String
        Set(ByVal value As String)
            p_sTermnl = value
        End Set
    End Property

    Public Property TableNo() As String
        Get
            Return psTableNox
        End Get
        Set(ByVal value As String)
            psTableNox = value
        End Set
    End Property

    Private Function AddHeader(ByVal Header As String, Optional ByVal HLen As Integer = 40) As Boolean
        With p_oDTHeader
            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sHeadName") = Left(Trim(Header), HLen)
        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddDetail(Quantity, Description, UnitPrice, isVatable)
    '   - Sets the info of the ITEMS bought...
    '+++++++++++++++++++++++++
    Public Function AddDetail(
            ByVal Quantity As Integer,
            ByVal Description As String,
            ByVal UnitPrice As Decimal,
            ByVal isCount As Boolean,
            ByVal Printer As String,
            ByVal FDescript As String) As Boolean

        With p_oDTDetail

            If .Rows.Count = 0 Then
                pnTotalItm = 0  'Initialize Total Item Sold
            End If

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("nQuantity") = Quantity
            .Rows(.Rows.Count - 1).Item("sBriefDsc") = Left(Description, 15)
            .Rows(.Rows.Count - 1).Item("nUnitPrce") = UnitPrice
            .Rows(.Rows.Count - 1).Item("nTotlAmnt") = Quantity * UnitPrice
            .Rows(.Rows.Count - 1).Item("sPrntPath") = Printer
            .Rows(.Rows.Count - 1).Item("sDescript") = Left(FDescript, 50)

            If isCount Then
                pnTotalItm = pnTotalItm + Quantity
            End If

        End With

        Return True
    End Function

    Public Function AddFooter(ByVal Footer As String) As Boolean
        With p_oDTFooter
            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sFootName") = Left(Trim(Footer), 40)
        End With
        Return True
    End Function

    Public Function PrintOrder() As Boolean

        If Not AddHeader(p_sCompny, 40) Then
            MsgBox("Invalid Company Name!")
            Return False
        End If

        If Not AddHeader(p_oApp.BranchName) Then
            MsgBox("Invalid Client Name!")
            Return False
        End If

        If Not AddHeader(p_oApp.Address) Then
            MsgBox("Invalid Client Address!")
            Return False
        End If

        If Not AddHeader(p_oApp.TownCity & ", " & p_oApp.Province) Then
            MsgBox("Invalid Town and Address!")
            Return False
        End If


        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(" Table No.: " & psTableNox.ToString.PadLeft(2, "0") & Environment.NewLine)
        builder.Append(" Order Slip No.: " & psContrlNo & Environment.NewLine)
        builder.Append(" Cashier: " & p_sLogName & "/" & p_sCashier & Environment.NewLine)
        builder.Append(" Date: " & Format(pdTransact, xsDATE_TIME) & Environment.NewLine)
        builder.Append(" Transaction No: " & p_sReferNox & Environment.NewLine)
        'For lnCtr = 1 To p_oDTHeader.Rows.Count - 1
        '    builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        'Next
        'builder.Append(Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                       UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN) + " "
            End If
            builder.Append(ls4Print & Environment.NewLine)
        Next

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append(" Terminal No: " & p_sTermnl & Environment.NewLine)
        builder.Append(" No of Items: " & pnTotalItm & Environment.NewLine & Environment.NewLine)

        ''Print Asterisk(*)
        'builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        ''Print the Footer
        'For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
        '    builder.Append(PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40) & Environment.NewLine)
        'Next

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))
        'Dim Printer_Name As String = "\\192.168.10.12\EPSON TM-U220 Receipt"
        Dim ordertk_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_TK")
        'Dim ordertk_printer As String = "\\192.168.10.12\EPSON TM-U220 Receipt"
        'Print the designation printer location...
        RawPrint.SendStringToPrinter(ordertk_printer, builder.ToString())

        'Dim kitchen_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_KN")
        'If kitchen_printer <> "n/a" And kitchen_printer <> "" Then
        '    RawPrint.SendStringToPrinter(kitchen_printer, builder.ToString())
        'End If

        processCategory()

        Return True
    End Function

    Private Function processCategory() As Boolean
        Dim dt As DataTable = p_oDTDetail

        dt.DefaultView.Sort = "sPrntPath ASC"
        dt = dt.DefaultView.ToTable()

        createDT()
        For lnCtr = 0 To dt.Rows.Count - 1
            If IFNull(dt.Rows(lnCtr).Item("sPrntPath"), "") <> "" Then
                If lnCtr + 1 < dt.Rows.Count Then
                    p_oDT.Rows.Add()
                    p_oDT.Rows(p_oDT.Rows.Count - 1).Item("nQuantity") = dt.Rows(lnCtr).Item("nQuantity")
                    p_oDT.Rows(p_oDT.Rows.Count - 1).Item("sDescript") = dt.Rows(lnCtr).Item("sDescript")
                    p_oDT.Rows(p_oDT.Rows.Count - 1).Item("nUnitPrce") = dt.Rows(lnCtr).Item("nUnitPrce")
                    p_oDT.Rows(p_oDT.Rows.Count - 1).Item("cDetailxx") = dt.Rows(lnCtr).Item("cDetailxx")
                    p_oDT.Rows(p_oDT.Rows.Count - 1).Item("nTotlAmnt") = dt.Rows(lnCtr).Item("nTotlAmnt")

                    If dt(lnCtr).Item("sPrntPath") <> dt(lnCtr + 1).Item("sPrntPath") Then
                        If dt(lnCtr).Item("sPrntPath") <> "" Then
                            PrintByCategory(dt(lnCtr).Item("sPrntPath"))
                            createDT()
                        End If
                    End If
                Else
                    p_oDT.Rows.Add()
                    p_oDT.Rows(p_oDT.Rows.Count - 1).Item("nQuantity") = dt.Rows(lnCtr).Item("nQuantity")
                    p_oDT.Rows(p_oDT.Rows.Count - 1).Item("sDescript") = dt.Rows(lnCtr).Item("sDescript")
                    p_oDT.Rows(p_oDT.Rows.Count - 1).Item("nUnitPrce") = dt.Rows(lnCtr).Item("nUnitPrce")
                    p_oDT.Rows(p_oDT.Rows.Count - 1).Item("cDetailxx") = dt.Rows(lnCtr).Item("cDetailxx")
                    p_oDT.Rows(p_oDT.Rows.Count - 1).Item("nTotlAmnt") = dt.Rows(lnCtr).Item("nTotlAmnt")
                    If dt(lnCtr).Item("sPrntPath") <> "" Then
                        PrintByCategory(dt(lnCtr).Item("sPrntPath"))
                    End If
                End If
            End If
        Next

        Return True
    End Function

    Private Sub createDT()
        p_oDT = New DataTable("DT")
        p_oDT.Columns.Add("nQuantity", System.Type.GetType("System.Int16"))
        p_oDT.Columns.Add("sDescript", System.Type.GetType("System.String")).MaxLength = 128
        p_oDT.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))
        p_oDT.Columns.Add("nTotlAmnt", System.Type.GetType("System.Decimal"))
        p_oDT.Columns.Add("cDetailxx", System.Type.GetType("System.String")).MaxLength = 1
        p_oDT.Columns.Add("sPrntPath", System.Type.GetType("System.String")).MaxLength = 128
    End Sub

    Private Function PrintByCategory(ByVal fsPrinter As String) As Boolean

        If Not AddHeader(p_sCompny, 40) Then
            MsgBox("Invalid Company Name!")
            Return False
        End If

        If Not AddHeader(p_oApp.BranchName) Then
            MsgBox("Invalid Client Name!")
            Return False
        End If

        If Not AddHeader(p_oApp.Address) Then
            MsgBox("Invalid Client Address!")
            Return False
        End If

        If Not AddHeader(p_oApp.TownCity & ", " & p_oApp.Province) Then
            MsgBox("Invalid Town and Address!")
            Return False
        End If

        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(" Table No.: " & psTableNox.ToString.PadLeft(2, "0") & Environment.NewLine)
        builder.Append(" Order Slip No.: " & psContrlNo & Environment.NewLine)
        builder.Append(" Cashier: " & p_sLogName & "/" & p_sCashier & Environment.NewLine)
        builder.Append(" Date: " & Format(pdTransact, xsDATE_TIME) & Environment.NewLine)
        builder.Append(" Transaction No: " & p_sReferNox & Environment.NewLine)
        'For lnCtr = 1 To p_oDTHeader.Rows.Count - 1
        '    builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        'Next
        'builder.Append(Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sale
        pnTotalItm = 0

        For lnCtr = 0 To p_oDT.Rows.Count - 1
            ls4Print = Format(p_oDT(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                       UCase(p_oDT(lnCtr).Item("sDescript")).PadRight(pxeDSCLEN + pxePRCLEN + pxeTTLLEN) + " "

            If p_oDT(lnCtr).Item("nUnitPrce") > 0 Then
                'ls4Print = ls4Print + Format(p_oDT(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                'ls4Print = ls4Print + Format(p_oDT(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN) + " "
            End If
            builder.Append(ls4Print & Environment.NewLine)

            pnTotalItm = pnTotalItm + p_oDT(lnCtr).Item("nQuantity")
        Next

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append(" Terminal No: " & p_sTermnl & Environment.NewLine)
        builder.Append(" No of Items: " & pnTotalItm & Environment.NewLine & Environment.NewLine)

        ''Print Asterisk(*)
        'builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        ''Print the Footer
        'For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
        '    builder.Append(PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40) & Environment.NewLine)
        'Next

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))

        'Dim ordertk_printer As String = "\\192.168.10.12\EPSON TM-U220 Receipt"

        Dim ordertk_printer As String = fsPrinter
        'Print the designation printer location...
        RawPrint.SendStringToPrinter(ordertk_printer, builder.ToString())

        'If kitchen_printer <> "n/a" And kitchen_printer <> "" Then
        '    RawPrint.SendStringToPrinter(kitchen_printer, builder.ToString())
        'End If


        Return True
    End Function


    Private Sub createDetail()
        p_oDTDetail = New DataTable("Detail")
        p_oDTDetail.Columns.Add("nQuantity", System.Type.GetType("System.Int16"))
        p_oDTDetail.Columns.Add("sDescript", System.Type.GetType("System.String")).MaxLength = 128
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 15
        p_oDTDetail.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nTotlAmnt", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("cDetailxx", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("sCategrID", System.Type.GetType("System.String")).MaxLength = 4
        p_oDTDetail.Columns.Add("sPrntPath", System.Type.GetType("System.String")).MaxLength = 128

        'Header Table
        p_oDTHeader = New DataTable("Header")
        p_oDTHeader.Columns.Add("sHeadName", System.Type.GetType("System.String")).MaxLength = 40

        'Footer Table
        p_oDTFooter = New DataTable("Footer")
        p_oDTFooter.Columns.Add("sFootName", System.Type.GetType("System.String")).MaxLength = 40
    End Sub

    Private Function PadCenter(ByVal source As String, ByVal length As Integer) As String
        Dim spaces As Integer = length - source.Length
        Dim padLeft As Integer = spaces / 2 + source.Length
        Return source.PadLeft(padLeft, " ").PadRight(length, " ")
    End Function

    '+++++++++++++++++++++++++
    'InitMachine() As Boolean
    '   - Initializes and Validates the POS Machine
    '+++++++++++++++++++++++++
    Public Function InitMachine() As Boolean
        If p_sPOSNo = "" Then
            MsgBox("Invalid Machine Identification Info Detected...")
            Return False
        End If

        Dim lsSQL As String
        lsSQL = "SELECT" & _
                       "  sAccredtn" & _
                       ", sPermitNo" & _
                       ", sSerialNo" & _
                       ", nPOSNumbr" & _
               " FROM Cash_Reg_Machine" & _
               " WHERE sIDNumber = " & strParm(p_sPOSNo)

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count <> 1 Then
            MsgBox("Invalid Config for MIN Detected...")
            Return False
        End If

        p_sAccrdt = loDta(0).Item("sAccredtn")
        p_sPermit = loDta(0).Item("sPermitNo")
        p_sSerial = loDta(0).Item("sSerialNo")
        p_sPOSNo = loDta(0).Item("nPOSNumbr")

        Return True
    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTDetail = Nothing
        p_oDTHeader = Nothing
        p_oDTFooter = Nothing

        p_sCompny = Environment.GetEnvironmentVariable("RMS-CLT-NM")

        Call createDetail()
    End Sub

    'Public Sub testOrder()
    '    Dim loReceipt As ggcMiscParam.PRN_Order

    '    loReceipt = New ggcMiscParam.PRN_Order(p_oApp)
    '    'If loReceipt.InitMachine() Then
    '    'Set Header
    '    loReceipt.AddHeader("MONARK HOTEL")
    '    loReceipt.AddHeader("PEDRITO'S BAKESHOP AND RESTAURANT")
    '    loReceipt.AddHeader("Mc Arthur Highway, Tapuac District")
    '    loReceipt.AddHeader("Dagupan City, Pangasinan")

    '    'Set Details
    '    loReceipt.AddDetail(2, "123456789012345", 2500)
    '    loReceipt.AddDetail(1, "CLUBHSE SANDWCH", 140)

    '    loReceipt.Transaction_Date = Now()
    '    loReceipt.TableNo = "5"
    '    loReceipt.Waiter = "Marlon A. Sayson"
    '    loReceipt.ControlNo = "250014"

    '    If Not loReceipt.PrintOrder Then
    '        MsgBox("Can't print Order")
    '        Exit Sub
    '    End If
    '    'End If
    'End Sub

End Class
