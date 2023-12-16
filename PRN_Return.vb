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
' Sales Return Printing Sample
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
' CASHIER : Marlon A. Sayson
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

Public Class PRN_Return
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

    Private pnTotalItm As Decimal
    Private pnTotalDue As Decimal
    Private pnDiscount As Decimal
    Private psCashier As String
    Private pdTransact As Date
    Private psReferNox As String        'XXX
    Private psReturnNo As String

    Private Const pxeQTYLEN As Integer = 3  '+ 1
    Private Const pxeDSCLEN As Integer = 15 '+ 1
    Private Const pxePRCLEN As Integer = 8  '+ 1
    Private Const pxeTTLLEN As Integer = 10
    Private Const pxeREGLEN As Integer = 12
    Private Const pxeLFTMGN As Integer = 3

    'Jovan 2020-11-07
    Private psCashierNme As String
    'Jovan 2021-03-10
    Private psCashierx As String
    Private p_sLogName As String


    'Public Property CashierName() As String
    '    Get
    '        Return psCashier
    '    End Get
    '    Set(ByVal value As String)
    '        psCashier = value
    '    End Set
    'End Property

    Public Property LogName() As String
        Get
            Return p_sLogName
        End Get
        Set(ByVal value As String)
            p_sLogName = value
        End Set
    End Property

    Public WriteOnly Property Discount() As Decimal
        Set(ByVal value As Decimal)
            pnDiscount = value
        End Set
    End Property

    Public Property Transaction_Date() As Date
        Get
            Return pdTransact
        End Get
        Set(ByVal value As Date)
            pdTransact = value
        End Set
    End Property

    Public Property Cashier() As String
        Get
            Return psCashier
        End Get
        Set(ByVal value As String)
            psCashier = value
        End Set
    End Property

    Public Property CashierName() As String
        Get
            Return psCashierx
        End Get
        Set(ByVal value As String)
            psCashierx = value
        End Set
    End Property

    Public Property ReferenceNo() As String
        Get
            Return psReferNox
        End Get
        Set(ByVal value As String)
            psReferNox = value
        End Set
    End Property

    Public Property ReturnNo() As String
        Get
            Return psReturnNo
        End Get
        Set(ByVal value As String)
            psReturnNo = value
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
    Public Function AddDetail( _
            ByVal Quantity As Integer, _
            ByVal Description As String, _
            ByVal UnitPrice As Decimal) As Boolean

        With p_oDTDetail

            If .Rows.Count = 0 Then
                pnTotalItm = 0  'Initialize Total Item Sold
            End If

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("nQuantity") = Quantity
            .Rows(.Rows.Count - 1).Item("sBriefDsc") = Left(Description, 15)
            .Rows(.Rows.Count - 1).Item("nUnitPrce") = UnitPrice
            .Rows(.Rows.Count - 1).Item("nTotlAmnt") = Quantity * UnitPrice

            pnTotalItm = pnTotalItm + Quantity
            pnTotalDue = pnTotalDue + (Quantity * UnitPrice)
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

    Public Function PrintReturns() As Boolean

        InitMachine()

        If Not AddHeader(p_sCompny) Then
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

        'Add Additional Info To the header
        '---------------------------------
        If Not AddHeader("VAT REG TIN: " & p_sVATReg) Then
            MsgBox("Invalid VAT REG TIN No!")
            Return False
        End If

        If Not AddHeader("MIN : " & p_sPOSNo) Then
            MsgBox("Invalid Machine Identification Number(MIN)!")
            Return False
        End If

        If Not AddHeader("PTU No.: " & p_sPermit) Then
            MsgBox("Invalid Permit No!")
            Return False
        End If

        If Not AddHeader("Serial No.: " & p_sSerial) Then
            MsgBox("Invalid Serial No.!")
            Return False
        End If

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append(p_oDTHeader(0).Item("sHeadName") & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        For lnCtr = 0 To p_oDTHeader.Rows.Count - 1
            builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append("RETURN SLIP" & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense

        builder.Append(RawPrint.pxePRINT_LEFT)
        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1

            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                       UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + ("-" + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL)).PadLeft(pxeTTLLEN) + " "
            End If
            builder.Append(ls4Print & Environment.NewLine)
        Next

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append(" No of Items: " & pnTotalItm & Environment.NewLine & Environment.NewLine)

        'Print TOTAL Sales
        builder.Append(" Sub-Total".PadRight(25) & " " & ("-" + Format(pnTotalDue, xsDECIMAL)).PadLeft(pxeREGLEN) & Environment.NewLine)
        If pnDiscount > 0 Then
            builder.Append(" Less: Discounts".PadRight(25) & " " & Format(pnDiscount, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If
        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_EMP1)
        builder.Append(" RETURN AMOUNT".PadRight(25) & " " & ("-" + Format(pnTotalDue - pnDiscount, xsDECIMAL)).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        'Print Cashier
        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" Return Slip No.: " & psReturnNo & Environment.NewLine)
        builder.Append(" Reference OR No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Date : " & Format(pdTransact, "yyyy-mm-dd") & " " & Format(p_oApp.getSysDate, "hh:mm:ss") & Environment.NewLine)
        builder.Append(" Cashier: " & p_sLogName & "/" & CashierName & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        'Print the Footer
        For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
            builder.Append(PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40) & Environment.NewLine)
        Next

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim cashier_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_CS")

        'Print the designation printer location...
        RawPrint.SendStringToPrinter(cashier_printer, builder.ToString())

        Call WriteReturns()

        Return True
    End Function

    Private Function WriteReturns() As Boolean
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)

        For lnCtr = 0 To p_oDTHeader.Rows.Count - 1
            builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter("RETURN SLIP", 40) & Environment.NewLine)

        builder.Append(Environment.NewLine)

        'Print Cashier
        builder.Append(" Cashier: " & p_sLogName & "/" & psCashierx & Environment.NewLine)
        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" Return Slip No.: " & psReturnNo & Environment.NewLine)
        builder.Append(" Reference OR No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Date : " & Format(pdTransact, "yyyy-mm-dd") & " " & Format(p_oApp.getSysDate, "hh:mm:ss") & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1

            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                       UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + ("-" + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL)).PadLeft(pxeTTLLEN) + " "
            End If
            builder.Append(ls4Print & Environment.NewLine)
        Next

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append(" No of Items: " & pnTotalItm & Environment.NewLine & Environment.NewLine)

        'Print TOTAL Sales
        builder.Append(" Sub-Total".PadRight(25) & " " & ("-" & Format(pnTotalDue, xsDECIMAL)).PadLeft(pxeREGLEN) & Environment.NewLine)
        If pnDiscount > 0 Then
            builder.Append(" Less: Discounts".PadRight(25) & " " & Format(pnDiscount, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If
        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        builder.Append(" RETURN AMOUNT".PadRight(25) & " " & ("-" & Format(pnTotalDue - pnDiscount, xsDECIMAL)).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        'Print the Footer
        For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
            builder.Append(PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter("----- END OF RECEIPT -----", 40) & Environment.NewLine)
        RawPrint.writeToFile(p_sPOSNo & " " & Format(p_oApp.getSysDate(), "yyyyMMdd"), builder.ToString())

        Return True
    End Function

    Private Sub createDetail()
        p_oDTDetail = New DataTable("Detail")
        p_oDTDetail.Columns.Add("nQuantity", System.Type.GetType("System.Int16"))
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 15
        p_oDTDetail.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nTotlAmnt", System.Type.GetType("System.Decimal"))

        'Header Table
        p_oDTHeader = New DataTable("Header")
        p_oDTHeader.Columns.Add("sHeadName", System.Type.GetType("System.String")).MaxLength = 40

        'Footer Table
        p_oDTFooter = New DataTable("Footer")
        p_oDTFooter.Columns.Add("sFootName", System.Type.GetType("System.String")).MaxLength = 40
    End Sub

    Private Function PadCenter(source As String, length As Integer) As String
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
        p_sTermnl = loDta(0).Item("nPOSNumbr")

        Return True
    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTDetail = Nothing
        p_oDTHeader = Nothing
        p_oDTFooter = Nothing

        p_sCompny = Environment.GetEnvironmentVariable("RMS-CLT-NM")
        p_sPOSNo = Environment.GetEnvironmentVariable("RMS-CRM-No")      'MIN
        p_sVATReg = Environment.GetEnvironmentVariable("REG-TIN-No")     'VAT REG No.

        Call createDetail()
    End Sub
End Class
