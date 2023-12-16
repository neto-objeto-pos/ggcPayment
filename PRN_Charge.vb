'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     POS Receipt Printing
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
' Sample Receipt Printing
'1234567890123456789012345678901234567890
'
'             MONARK HOTEL
'   PEDRITO'S BAKESHOP AND RESTAURANT
'   Mc Arthur Highway, Tapuac District
'       Dagupan City, Pangasinan
'     VAT REG TIN: 941-184-389-000
'       MIN : 14121419321782091
'   Permit #: PR122014-004-D004507-000
'      Serial No. : L9GF261769
'****************************************
'QTY DESCRIPTION       UPRICE     AMOUNT 
'  2 123456789012345 2,500.00   5,000.00V
'  1 CLUBHSE SANDWCH   140.00     140.00V
'----------------------------------------
' No of Items: 3
'
' TOTAL                        5,140.00
' Less: Discount(s)              140.00
'       VAT                      500.00
'                         ------------- 
' Amount Due:                  4,500.00
' Cash                         1,000.00
' BDO                          1,000.00
' METROBANK                    1,000.00
' 12345-7890-12                1,500.00
'                         ------------- 
' CHANGE    :                      0.00
'///////////////////////////////////////
'Senior Citizen
'125-234561
'///////////////////////////////////////
'BDO 
'54697******4006
'SWIPED
'Approval Code:005273
'///////////////////////////////////////
'METROBANK
'552097******1519
'SWIPED
'Approval Code: 426235
'///////////////////////////////////////
'Check No: 12345-7890-12
'Bank    : Metrobank
'Date:   : 11/18/2016 
'Amount  : 1,500.00
'----------------------------------------
'
'  VAT Exempt Sales         2,500.00
'  Zero Rated Sales             0.00
'  VAT Sales                1,760.00 
'  VAT Amount                 240.00
' 
' Cust Name: ---------------------------- 
' Address  : ----------------------------
' TIN #    : ----------------------------
' Bus Style: ----------------------------
'
' Cashier: Marlon A. Sayson
' Terminal No.: 02       
' OR No.: 00172015
' Date: 11/18/2016 09:15 am
'****************************************
'       Have A Nice Day! Come Again.
'   This serves as an OFFICIAL RECEIPT
' Telephone (075)653-1347/48 or visit us
'     http://www.pedritosbakeshop.com
'
' ==========================================================================================
'  kalyptus [ 11/16/2016 09:37 am ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

Imports ADODB
Imports ggcAppDriver
Imports System.Drawing

Public Class PRN_Charge
    Private p_oApp As GRider

    Private p_sPOSNo As String      'MIN:       14121419321782091
    Private p_sVATReg As String     'TIN:       941-184-389-000
    Private p_sCompny As String     'Company  : MONARK HOTEL
    Protected p_sACCNox As Char
    Protected p_dACCFrm As Date
    Protected p_dACCTru As Date
    Protected p_sPTUNox As String
    Protected p_dPTUFrm As Date
    Protected p_dPTUTru As Date

    Private p_sPermit As String     'Permit No: PR122014-004-D004507-000
    Private p_sSerial As String     'Serial No: L9GF261769
    Private p_sAccrdt As String     'Accrdt No: 038-227471337-000028
    Private p_sTermnl As String     'Termnl No: 02
    Private p_cTrnMde As Char
    Private p_sTablNo As String     'Table No: 02

    Private p_oDTMaster As DataTable
    Private p_oDTDetail As DataTable
    Private p_oDTComplx As DataTable    'Complimentary
    Private p_oDTGftChk As DataTable    'Gift Check
    Private p_oDTChkPym As DataTable    'Check Payment
    Private p_oDTCredit As DataTable    'Credit Card

    Private p_oDTHeader As DataTable
    Private p_oDTFooter As DataTable
    Private p_oDTDiscnt As DataTable

    'Transaction Master Info
    Private psCashrNme As String
    Private pdTransact As Date          'XXX
    Private psReferNox As String        'XXX
    Private psTransNox As String        'XXX

    Private pnTotalItm As Decimal
    Private pnTotalDue As Decimal
    Private pnDiscAmtV As Decimal
    Private pnDiscAmtN As Decimal

    'MAC 2018.01.26
    Private pnDiscRteV As Decimal
    Private pnDiscRteN As Decimal
    Private pnAddDiscV As Decimal
    Private pnAddDiscN As Decimal
    Private pnNoClient As Integer
    Private pnWithDisc As Integer
    Private pnSChargex As Decimal

    'Jovan
    Private psCashierx As String
    Private p_sLogName As String
    Private p_nTableNo As Integer
    Private p_nNoClient As Integer
    Private p_nWithDisc As Integer
    Private p_nSCRate As Double

    'Total Payments
    Private pnCashTotl As Decimal       'XXX
    Private pnGiftTotl As Decimal
    Private pnChckTotl As Decimal
    Private pnCrdtTotl As Decimal

    'Sale Total Info
    Private pnVatblSle As Decimal
    Private pnVatExSle As Decimal       'XXX
    Private pnZroRtSle As Decimal
    Private pnVatAmntx As Decimal

    'Customer Information
    Private psCustName As String        'XXX
    Private psCustAddx As String        'XXX
    Private psCustTINx As String        'XXX    
    Private psCustBusx As String        'XXX

    Private pbReprint As Boolean

    Private Const pxeQTYLEN As Integer = 4  '+ 1
    Private Const pxeDSCLEN As Integer = 14 '+ 1
    Private Const pxePRCLEN As Integer = 8  '+ 1
    Private Const pxeTTLLEN As Integer = 10
    Private Const pxeREGLEN As Integer = 12
    Private Const pxeLFTMGN As Integer = 3

    Public Property CustName() As String
        Get
            Return psCustName
        End Get
        Set(ByVal value As String)
            psCustName = value
        End Set
    End Property

    Public Property CustAddress() As String
        Get
            Return psCustAddx
        End Get
        Set(ByVal value As String)
            psCustAddx = value
        End Set
    End Property

    Public Property Cashier() As String
        Get
            Return psCashierx
        End Get
        Set(ByVal value As String)
            psCashierx = value
        End Set
    End Property

    Public Property LogName As String
        Get
            Return p_sLogName
        End Get
        Set(ByVal value As String)
            p_sLogName = value
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

    Public Property ReferenceNo() As String
        Get
            Return psReferNox
        End Get
        Set(ByVal value As String)
            psReferNox = value
        End Set
    End Property

    Public Property SourceNo() As String
        Get
            Return psTransNox
        End Get
        Set(ByVal value As String)
            psTransNox = value
        End Set
    End Property

    WriteOnly Property AccrdNumber As String
        Set(ByVal Value As String)
            p_sACCNox = Value
        End Set
    End Property

    WriteOnly Property AccrdFrom As Date
        Set(ByVal Value As Date)
            p_dACCFrm = Value
        End Set
    End Property

    Property ClientNo As Integer
        Get
            Return p_nNoClient
        End Get
        Set(ByVal Value As Integer)
            p_nNoClient = Value
        End Set
    End Property

    Property WithDisc As Integer
        Get
            Return p_nWithDisc
        End Get
        Set(ByVal Value As Integer)
            p_nWithDisc = Value
        End Set
    End Property

    WriteOnly Property TranMode As Char
        Set(ByVal Value As Char)
            p_cTrnMde = Value
        End Set
    End Property

    WriteOnly Property SerialNo As String
        Set(ByVal value As String)
            p_sSerial = value
        End Set
    End Property

    Public Property TableNo() As Integer
        Get
            Return p_nTableNo
        End Get
        Set(ByVal value As Integer)
            p_nTableNo = value
        End Set
    End Property

    Public Property CashPayment() As Decimal
        Get
            Return pnCashTotl
        End Get
        Set(ByVal value As Decimal)
            pnCashTotl = value
        End Set
    End Property

    Public Property NonVatSales() As Decimal
        Get
            Return pnVatExSle
        End Get
        Set(ByVal value As Decimal)
            pnVatExSle = value
        End Set
    End Property

    WriteOnly Property Reprint As Boolean
        Set(ByVal value As Boolean)
            pbReprint = value
        End Set
    End Property

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

    '+++++++++++++++++++++++++
    'AddHeader(Header) As Boolean
    '   - Sets what are to be printed at the Header Section of Receipt
    '     Please exclude the MIN, Vat Reg, Permit No, Serial No, and Accredtn No
    '+++++++++++++++++++++++++
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
            ByVal UnitPrice As Decimal, _
            ByVal isVatable As Boolean,
            ByVal isDetail As Boolean,
            ByVal isCount As Boolean) As Boolean

        With p_oDTDetail

            If .Rows.Count = 0 Then
                pnTotalDue = 0  'Initialize Total Amount Due
                pnZroRtSle = 0  'Initialize Zero Rated Sale
                pnTotalItm = 0  'Initialize Total Item Sold
            End If

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("nQuantity") = Quantity
            .Rows(.Rows.Count - 1).Item("sBriefDsc") = Left(Description, 14)
            .Rows(.Rows.Count - 1).Item("nUnitPrce") = UnitPrice
            .Rows(.Rows.Count - 1).Item("nTotlAmnt") = Quantity * UnitPrice
            .Rows(.Rows.Count - 1).Item("cVatablex") = IIf(isVatable = True, 1, 0)
            .Rows(.Rows.Count - 1).Item("cDetailxx") = IIf(isDetail = True, 1, 0)

            pnTotalDue = pnTotalDue + (Quantity * UnitPrice)

            If isCount Then
                If Quantity > 0 Then
                    pnTotalItm = pnTotalItm + Quantity
                End If
            End If

            If Not isVatable Then
                pnZroRtSle = pnZroRtSle + (Quantity * UnitPrice)
            End If

        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddDetail(Quantity, Description, UnitPrice, isVatable)
    '   - Sets the info of the ITEMS bought...
    '+++++++++++++++++++++++++
    Public Function AddComplement( _
            ByVal Quantity As Integer, _
            ByVal Description As String, _
            ByVal UnitPrice As Decimal, _
            ByVal isVatable As Boolean,
            Optional ByVal isDetail As Boolean = True) As Boolean

        With p_oDTComplx

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("nQuantity") = Quantity
            .Rows(.Rows.Count - 1).Item("sBriefDsc") = Left(Description, 14)
            .Rows(.Rows.Count - 1).Item("nUnitPrce") = UnitPrice
            .Rows(.Rows.Count - 1).Item("nTotlAmnt") = Quantity * UnitPrice
            .Rows(.Rows.Count - 1).Item("cVatablex") = IIf(isVatable = True, 1, 0)

            If isDetail Then
                pnTotalItm = pnTotalItm + Quantity
            End If
        End With

        Return True
    End Function


    '+++++++++++++++++++++++++
    'AddDiscount(IDNumber, DiscCard, Amount, isVatable)
    '   - Sets the info of the discounts for this sales...
    '+++++++++++++++++++++++++
    Public Function AddDiscount( _
            ByVal IDNumber As String, _
            ByVal DiscCard As String, _
            ByVal Amount As Decimal, _
            ByVal isVatable As Boolean) As Boolean

        With p_oDTDiscnt

            If .Rows.Count = 0 Then
                pnDiscAmtV = 0  'VATable Discount
                pnDiscAmtN = 0  'Non-VATable Discount

                pnDiscRteV = 0
                pnAddDiscV = 0
                pnDiscRteN = 0
                pnAddDiscN = 0
                pnNoClient = 0
                pnWithDisc = 0
            End If

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sIDNumber") = IDNumber
            .Rows(.Rows.Count - 1).Item("sDiscCard") = DiscCard
            .Rows(.Rows.Count - 1).Item("nDiscAmnt") = Amount
            .Rows(.Rows.Count - 1).Item("cNoneVATx") = IIf(isVatable = True, 1, 0)

            If isVatable Then
                pnDiscAmtV = pnDiscAmtV + Amount
            Else
                pnDiscAmtN = pnDiscAmtN + Amount
            End If

        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddDiscount(IDNumber, DiscCard, DiscRate, AddDiscx, Amount, isVatable)
    '   - Sets the info of the discounts for this sales...
    '+++++++++++++++++++++++++
    Public Function AddDiscount( _
            ByVal IDNumber As String, _
            ByVal DiscCard As String, _
            ByVal DiscRate As Decimal, _
            ByVal AddDiscx As Decimal, _
            ByVal Amount As Decimal, _
            ByVal isVatable As Boolean, _
            Optional ByVal NoClient As Integer = 1, _
            Optional ByVal WithDisc As Integer = 1, _
            Optional ByVal sClientNm As String = "") As Boolean

        With p_oDTDiscnt
            If .Rows.Count = 0 Then
                pnDiscAmtV = 0  'VATable Discount
                pnDiscAmtN = 0  'Non-VATable Discount

                pnDiscRteV = 0
                pnAddDiscV = 0
                pnDiscRteN = 0
                pnAddDiscN = 0
                pnNoClient = 0
                pnWithDisc = 0
            End If

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sIDNumber") = IDNumber
            .Rows(.Rows.Count - 1).Item("sDiscCard") = DiscCard
            .Rows(.Rows.Count - 1).Item("nDiscRate") = DiscRate
            .Rows(.Rows.Count - 1).Item("nAddDiscx") = AddDiscx
            .Rows(.Rows.Count - 1).Item("nDiscAmnt") = Amount
            .Rows(.Rows.Count - 1).Item("cNoneVATx") = IIf(isVatable = True, 1, 0)
            .Rows(.Rows.Count - 1).Item("nNoClient") = NoClient
            .Rows(.Rows.Count - 1).Item("nWithDisc") = WithDisc
            .Rows(.Rows.Count - 1).Item("sClientNm") = sClientNm


            If isVatable Then
                pnDiscAmtV = pnDiscAmtV + Amount

                'MAC
                pnDiscRteV = pnDiscRteV + DiscRate
                pnAddDiscV = pnAddDiscV + AddDiscx
            Else
                pnDiscAmtN = pnDiscAmtN + Amount

                'MAC
                pnDiscRteN = pnDiscRteN + DiscRate
                pnAddDiscN = pnAddDiscN + AddDiscx
            End If

            pnNoClient = pnNoClient + NoClient
            pnWithDisc = pnWithDisc + WithDisc
        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddHeader(Header) As Boolean
    '   - Sets what are to be printed at the Footer Section of Receipt
    '     Could be greetings, remarks, and/or other info.
    '+++++++++++++++++++++++++
    Public Function AddFooter(ByVal Footer As String) As Boolean
        With p_oDTFooter
            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sFootName") = Left(Trim(Footer), 40)
        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddGiftCoupon(GiftSource, Amount)
    '   - Sets the info of Gift Coupon(s) used as payment
    '+++++++++++++++++++++++++
    Public Function AddGiftCoupon( _
            ByVal GiftSource As String, _
            ByVal Amount As Decimal) As Boolean

        With p_oDTGftChk

            If .Rows.Count = 0 Then pnGiftTotl = 0

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sGiftSrce") = GiftSource
            .Rows(.Rows.Count - 1).Item("nGiftAmnt") = Amount

            pnGiftTotl = pnGiftTotl + Amount

        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddCheck(Bank, CheckNo, CheckDate, Amount)
    '   - Sets the info of check(s) used as payment
    '+++++++++++++++++++++++++
    Public Function AddCheck( _
            ByVal Bank As String, _
            ByVal CheckNo As String, _
            ByVal CheckDate As Date, _
            ByVal Amount As Decimal) As Boolean

        With p_oDTChkPym

            If .Rows.Count = 0 Then pnChckTotl = 0

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sCheckBnk") = Bank
            .Rows(.Rows.Count - 1).Item("sCheckNox") = CheckNo
            .Rows(.Rows.Count - 1).Item("dCheckDte") = CheckDate
            .Rows(.Rows.Count - 1).Item("nCheckAmt") = Amount

            pnChckTotl = pnChckTotl + Amount

        End With


        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddCreditCard(Bank, CardNumber, ApprNo, Amount)
    '   - Sets the info of credit card used as payment
    '+++++++++++++++++++++++++
    Public Function AddCreditCard( _
            ByVal Bank As String, _
            ByVal CardNumber As String, _
            ByVal ApprNo As String, _
            ByVal Amount As Decimal)

        With p_oDTCredit

            If .Rows.Count = 0 Then pnCrdtTotl = 0

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sCardBank") = Bank
            .Rows(.Rows.Count - 1).Item("sCardNoxx") = CardNumber
            .Rows(.Rows.Count - 1).Item("sApprovNo") = ApprNo
            .Rows(.Rows.Count - 1).Item("nCardAmnt") = Amount

            pnCrdtTotl = pnCrdtTotl + Amount

        End With

        Return True
    End Function

    Public Function PrintORx() As Boolean
        Dim lnDeducQTY As Integer
        Dim lnVatPerc As Double = 1.12

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

        If Not AddHeader("Serial No.: " & p_sSerial) Then
            MsgBox("Invalid Serial No.!")
            Return False
        End If

        If Not AddHeader("REPRINT") Then
            MsgBox("Unable to Reprint!")
            Return False
        End If

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)

        For lnCtr = 0 To p_oDTHeader.Rows.Count - 2
            builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
            Debug.Print(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append(Environment.NewLine)

        Select Case p_cTrnMde
            Case "A"
                builder.Append("OFFICIAL RECEIPT" & Environment.NewLine)
            Case "D"
                builder.Append("TRAINING MODE" & Environment.NewLine)
        End Select

        If pbReprint Then
            builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
            builder.Append(RawPrint.pxePRINT_CNTR)
            builder.Append(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName") & Environment.NewLine)
        End If

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Cashier
        builder.Append(Environment.NewLine)
        builder.Append(" Cashier: " & p_sLogName & "/" & psCashierx & Environment.NewLine)
        If p_nTableNo > 0 Then
            builder.Append(" Table No.: " & p_nTableNo & "".PadRight(12) & " " & "DINE-IN".PadLeft(pxeREGLEN) & Environment.NewLine)
        Else
            builder.Append(" TAKE-OUT " & Environment.NewLine)
        End If

        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" OR No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Transaction No.: " & psTransNox & Environment.NewLine)
        builder.Append(" Date : " & Format(pdTransact, "yyyy-mm-dd") & " " & Format(p_oApp.getSysDate, "hh:mm:ss") & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        lnDeducQTY = 0
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnCtr).Item("nQuantity") > 0 Then
                If p_oDTDetail(lnCtr).Item("cDetailxx") = "1" Then
                    If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                        ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    Else
                        ls4Print = String.Empty.PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    End If
                Else
                    If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                        ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    Else
                        ls4Print = "   " & UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                        lnDeducQTY = lnDeducQTY + p_oDTDetail(lnCtr).Item("nQuantity")
                    End If
                End If
            Else
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            End If

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                If p_oDTDetail(lnCtr).Item("cDetailxx") = "1" Then
                    'If p_oDTDetail(lnCtr).Item("nQuantity") < 10 Then
                    '    ls4Print = "  " & Left(ls4Print, pxeQTYLEN + 1 + pxeDSCLEN - 2)
                    'Else
                    '    ls4Print = "   " & Left(ls4Print, pxeQTYLEN + 1 + pxeDSCLEN - 2)
                    'End If
                End If

                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN)
                If p_oDTDetail(lnCtr).Item("cVatablex") Then
                    ls4Print = ls4Print
                    'ls4Print = ls4Print + "V"
                End If

                builder.Append(ls4Print & Environment.NewLine)
            Else
                If p_oDTDetail(lnCtr).Item("cWthPromo") = "1" Then
                    ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce") * p_oDTDetail(lnCtr).Item("nQuantity"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                    ls4Print = "  " & ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce") * p_oDTDetail(lnCtr).Item("nQuantity"), xsDECIMAL).PadLeft(pxeTTLLEN)
                    builder.Append(ls4Print & Environment.NewLine)
                Else
                    builder.Append(Space(2) & ls4Print & Environment.NewLine)
                End If
            End If
        Next

        'Print Detail of Complementary
        If p_oDTComplx.Rows.Count > 0 Then
            builder.Append("COMPLEMENT: " & Environment.NewLine)
            For lnCtr = 0 To p_oDTComplx.Rows.Count - 1

                ls4Print = Format(p_oDTComplx(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTComplx(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" No. of Items: " & pnTotalItm - lnDeducQTY & Environment.NewLine)

        'do we have SC Discount?
        If pnDiscAmtN > 0 And pnNoClient > 0 Then
            'print no of clients and no of with discounts
            builder.Append(" Total No. of Clients: " & p_nNoClient & Environment.NewLine)
            builder.Append(" No. of SC/PWD Clients: " & p_nWithDisc & Environment.NewLine)
        End If
        builder.Append(Environment.NewLine)

        'Print TOTAL Sales    
        If pnSChargex > 0 Or pnDiscAmtN > 0 Or pnDiscAmtV > 0 Then
            builder.Append(" Sub-Total".PadRight(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            If pnDiscAmtN > 0 Or pnDiscAmtV > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            End If
        End If

        Dim lnExVATDue = pnTotalDue / 1.12

        'Print Discounts
        If pnDiscAmtV > 0 Then
            'builder.Append(" Less: Discount(s)".PadRight(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Dim lnVATExclsv = pnTotalDue / lnVatPerc
            Dim lnRateAmntx = lnVATExclsv * (pnDiscRteV / 100)
            Dim lnAddDiscxx = pnAddDiscV / lnVatPerc

            Dim lnAmountDue = pnTotalDue - pnDiscAmtV
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + lnAddDiscxx + pnDiscAmtN)

            Dim lsLess As String = " Less: "
            If pnDiscRteV > 0 Then
                'builder.Append((lsLess & Math.Round(pnDiscRteV) & "% Discount").PadRight(25) & " " & Format(lnRateAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(lsLess & p_oDTDiscnt(0).Item("sDiscCard") & Environment.NewLine)
                builder.Append("       " & ("(" & Format(Math.Round(pnDiscRteV), "#0.0") & "%)").PadRight(18) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                'builder.Append("       " & ("(" & Format(Math.Round(pnDiscRteV), "#0.0") & "%)").PadRight(18) & " " & Format(pnTotalDue * (pnDiscRteV / 100), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnAddDiscV > 0 Then
                builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATExWDsc * 0.12, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        ElseIf pnDiscAmtN > 0 Then
            'orig code
            'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lnVATablex As Decimal = 0
            Dim lnNVATable As Decimal = 0
            Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, lnNVATable)

            If p_nNoClient <> p_nWithDisc Then
                builder.Append(" Price Inclusive of VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lnExVATDue = ((pnTotalDue / pnNoClient) * p_nWithDisc) / 1.12
                builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Else
                builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            If pnSChargex > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
                builder.Append(" Total Due".PadRight(25) & " " & Format((lnVATablex + lnExVATDue) - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            'If lnVATablex > 0 Then
            '    builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            '    builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnExVATDue - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            '    builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        Else
            If pnSChargex > 0 Then
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            Else
                'builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            End If
        End If

        'Print Amount Due By subracting the discounts
        builder.Append(" TOTAL AMOUNT DUE".PadRight(25) & " " & Format((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Cash Payments
        If pnCashTotl > 0 Then
            builder.Append(" Cash".PadRight(25) & " " & Format(pnCashTotl, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If

        'Print Credit Card Payments
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
                           Format(p_oDTCredit(lnCtr).Item("nCardAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Check Payments
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
                           Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Gift Coupon
        If p_oDTGftChk.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce") & " GIFT CHEQUE").PadRight(24) & " " & _
                           Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Line Before change....
        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

        'Print Change
        Dim lnChange As Decimal = (pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN)

        If pnGiftTotl > lnChange Then
            lnChange = 0
        Else
            lnChange = (pnCashTotl + pnChckTotl + pnCrdtTotl + pnGiftTotl) - lnChange
        End If

        builder.Append(" CHANGE".PadRight(25) & " " & Format(lnChange, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Discount Information
        If Not IsNothing(p_oDTDiscnt) Then
            If p_oDTDiscnt.Rows.Count > 0 Then
                If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then
                    builder.Append(Environment.NewLine)
                    builder.Append("///////////////////////////////////////" & Environment.NewLine)
                    If InStr(LCase(p_oDTDiscnt(0).Item("sDiscCard")), "sc", CompareMethod.Text) <> 0 Then
                        If pnDiscAmtN > 0 And pnNoClient > 0 Then
                            builder.Append("SENIOR/PWD INFORMATION" & Environment.NewLine)
                        End If
                    End If
                    'add name and signature field
                    builder.Append("ID No: " & p_oDTDiscnt(0).Item("sIDNumber") & Environment.NewLine)
                    builder.Append("Name: " & p_oDTDiscnt(0).Item("sClientNm") & Environment.NewLine)
                    builder.Append("Signature:______________________________" & Environment.NewLine)

                End If
            End If
        End If

        'Print Credit Card Info
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                'Print Credit Card Bank
                builder.Append(p_oDTCredit(lnCtr).Item("sCardBank") & Environment.NewLine)

                'Print Card Number/Should hide entire card number
                ls4Print = p_oDTCredit(lnCtr).Item("sCardNoxx")
                ls4Print = Left(ls4Print, 5) & "".PadLeft(ls4Print.Length - 9, "*") & Right(ls4Print, 4)
                builder.Append(ls4Print & Environment.NewLine)
                builder.Append("SWIPED" & Environment.NewLine)
                builder.Append("Approval Code: " & p_oDTCredit(lnCtr).Item("sApprovNo") & Environment.NewLine)
            Next
        End If

        'Print Check Payment Info
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                builder.Append("Check No: " & p_oDTChkPym(lnCtr).Item("sCheckNox") & Environment.NewLine)
                builder.Append("Bank    : " & p_oDTChkPym(lnCtr).Item("sCheckBnk") & Environment.NewLine)
                builder.Append("Date:   : " & Format(p_oDTChkPym(lnCtr).Item("dCheckDte"), xsDATE_SHORT) & Environment.NewLine)
                builder.Append("Amount  : " & Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL) & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine & Environment.NewLine)

        'Compute VAT & and other info
        '++++++++++++++++++++++++++++++++++++++
        'VAT is 12 % of sales
        'TODO: load VAT percent of sales from CONFIG
        'pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) / lnVatPerc
        'pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) - pnVatblSle

        pnVatblSle = ((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        pnVatAmntx = ((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        'Print VAT Related info
        builder.Append("  VAT Exempt Sales      " & Format(pnVatExSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  Zero-Rated Sales      " & Format(pnZroRtSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VATable Sales         " & Format(pnVatblSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VAT Amount            " & Format(pnVatAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine & Environment.NewLine)

        If psCustName <> "" Then
            builder.Append(" Cust Name: " & psCustName & Environment.NewLine)
            builder.Append(" Address  : " & psCustAddx & Environment.NewLine)
            builder.Append(" TIN      : " & psCustTINx & Environment.NewLine)
            builder.Append(" Bus Style: " & psCustBusx & Environment.NewLine & Environment.NewLine)
        Else
            builder.Append(" Cust Name: ____________________________" & Environment.NewLine)
            builder.Append(" Address  : ____________________________" & Environment.NewLine)
            builder.Append(" TIN      : ____________________________" & Environment.NewLine)
            builder.Append(" Bus Style: ____________________________" & Environment.NewLine & Environment.NewLine)
        End If

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

        Call WriteOR()

        p_oApp.SaveEvent("0016", "OR No. " & psReferNox, p_sTermnl)

        Return True
    End Function

    Private Function WriteOR() As Boolean
        Dim lnVatPerc As Double = 1.12
        Dim lnDeducQTY As Integer

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)

        For lnCtr = 0 To p_oDTHeader.Rows.Count - 2
            builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
            Debug.Print(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter("CHARGE INVOICE", 40) & Environment.NewLine)
        Select Case p_cTrnMde
            Case "D"
                builder.Append(PadCenter("TRANING MODE", 40) & Environment.NewLine)
        End Select

        If pbReprint Then
            builder.Append(PadCenter(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName"), 40) & Environment.NewLine)
        End If

        builder.Append(Environment.NewLine)

        'Print Cashier
        builder.Append(" Cashier: " & p_sLogName & "/" & psCashierx & Environment.NewLine)
        If p_nTableNo > 0 Then
            builder.Append(" Table No.: " & p_nTableNo & "".PadRight(12) & " " & "DINE-IN".PadLeft(pxeREGLEN) & Environment.NewLine)
        Else
            builder.Append(" TAKE-OUT " & Environment.NewLine)
        End If
        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" CI No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Transaction No.: " & psTransNox & Environment.NewLine)
        builder.Append(" Date : " & Format(CDate(pdTransact), "yyyy-mm-dd") & " " & Format(p_oApp.getSysDate, "hh:mm:ss") & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        lnDeducQTY = 0
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnCtr).Item("nQuantity") > 0 Then
                If p_oDTDetail(lnCtr).Item("cDetailxx") = "1" Then
                    If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                        ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    Else
                        ls4Print = String.Empty.PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    End If
                Else
                    If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                        ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    Else
                        ls4Print = "   " & UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                        lnDeducQTY = lnDeducQTY + p_oDTDetail(lnCtr).Item("nQuantity")
                    End If
                End If
            Else
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            End If

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                If p_oDTDetail(lnCtr).Item("cDetailxx") = "1" Then
                    'If p_oDTDetail(lnCtr).Item("nQuantity") < 10 Then
                    '    ls4Print = "  " & Left(ls4Print, pxeQTYLEN + 1 + pxeDSCLEN - 2)
                    'Else
                    '    ls4Print = "   " & Left(ls4Print, pxeQTYLEN + 1 + pxeDSCLEN - 2)
                    'End If
                End If

                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN)
                If p_oDTDetail(lnCtr).Item("cVatablex") Then
                    ls4Print = ls4Print
                    'ls4Print = ls4Print + "V"
                End If

                builder.Append(ls4Print & Environment.NewLine)
            Else
                If p_oDTDetail(lnCtr).Item("cWthPromo") = "1" Then
                    ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce") * p_oDTDetail(lnCtr).Item("nQuantity"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                    ls4Print = "  " & ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce") * p_oDTDetail(lnCtr).Item("nQuantity"), xsDECIMAL).PadLeft(pxeTTLLEN)
                    builder.Append(ls4Print & Environment.NewLine)
                Else
                    builder.Append(Space(2) & ls4Print & Environment.NewLine)
                End If
            End If
        Next

        'Print Detail of Complementary
        If p_oDTComplx.Rows.Count > 0 Then
            builder.Append("COMPLEMENT: " & Environment.NewLine)
            For lnCtr = 0 To p_oDTComplx.Rows.Count - 1

                ls4Print = Format(p_oDTComplx(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTComplx(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" No. of Items: " & pnTotalItm - lnDeducQTY & Environment.NewLine)

        'do we have SC Discount?
        If pnDiscAmtN > 0 And pnNoClient > 0 Then
            'print no of clients and no of with discounts
            builder.Append(" Total No. of Clients: " & p_nNoClient & Environment.NewLine)
            builder.Append(" No. of SC/PWD Clients: " & p_nWithDisc & Environment.NewLine)
        End If
        builder.Append(Environment.NewLine)

        'Print TOTAL Sales    
        If pnSChargex > 0 Or pnDiscAmtN > 0 Or pnDiscAmtV > 0 Then
            builder.Append(" Sub-Total".PadRight(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            If pnDiscAmtN > 0 Or pnDiscAmtV > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            End If
        End If

        Dim lnExVATDue = pnTotalDue / 1.12

        'Print Discounts
        If pnDiscAmtV > 0 Then
            'builder.Append(" Less: Discount(s)".PadRight(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Dim lnVATExclsv = pnTotalDue / lnVatPerc
            Dim lnRateAmntx = lnVATExclsv * (pnDiscRteV / 100)
            Dim lnAddDiscxx = pnAddDiscV / lnVatPerc

            Dim lnAmountDue = pnTotalDue - pnDiscAmtV
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + lnAddDiscxx + pnDiscAmtN)

            Dim lsLess As String = " Less: "
            If pnDiscRteV > 0 Then
                'builder.Append((lsLess & Math.Round(pnDiscRteV) & "% Discount").PadRight(25) & " " & Format(lnRateAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(lsLess & p_oDTDiscnt(0).Item("sDiscCard") & Environment.NewLine)
                builder.Append("       " & ("(" & Format(Math.Round(pnDiscRteV), "#0.0") & "%)").PadRight(18) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                'builder.Append("       " & ("(" & Format(Math.Round(pnDiscRteV), "#0.0") & "%)").PadRight(18) & " " & Format(pnTotalDue * (pnDiscRteV / 100), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnAddDiscV > 0 Then
                builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATExWDsc * 0.12, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        ElseIf pnDiscAmtN > 0 Then
            'orig code
            'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lnVATablex As Decimal = 0
            Dim lnNVATable As Decimal = 0
            Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, lnNVATable)

            If p_nNoClient <> p_nWithDisc Then
                builder.Append(" Price Inclusive of VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lnExVATDue = ((pnTotalDue / pnNoClient) * p_nWithDisc) / 1.12
                builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Else
                builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            If pnSChargex > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
                builder.Append(" Total Due".PadRight(25) & " " & Format((lnVATablex + lnExVATDue) - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            'If lnVATablex > 0 Then
            '    builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            '    builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnExVATDue - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            '    builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        Else
            If pnSChargex > 0 Then
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            Else
                'builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            End If
        End If

        'Print Amount Due By subracting the discounts
        builder.Append(" TOTAL AMOUNT DUE".PadRight(25) & " " & Format((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Cash Payments
        If pnCashTotl > 0 Then
            builder.Append(" Cash".PadRight(25) & " " & Format(pnCashTotl, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If

        'Print Credit Card Payments
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
                           Format(p_oDTCredit(lnCtr).Item("nCardAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Check Payments
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
                           Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Gift Coupon
        If p_oDTGftChk.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce") & " GIFT CHEQUE").PadRight(24) & " " & _
                           Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Line Before change....
        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

        'Print Change
        Dim lnChange As Decimal = (pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN)

        If pnGiftTotl > lnChange Then
            lnChange = 0
        Else
            lnChange = (pnCashTotl + pnChckTotl + pnCrdtTotl + pnGiftTotl) - lnChange
        End If

        builder.Append(" CHANGE".PadRight(25) & " " & Format(lnChange, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Discount Information
        If Not IsNothing(p_oDTDiscnt) Then
            If p_oDTDiscnt.Rows.Count > 0 Then
                If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then
                    builder.Append(Environment.NewLine)
                    builder.Append("///////////////////////////////////////" & Environment.NewLine)
                    If InStr(LCase(p_oDTDiscnt(0).Item("sDiscCard")), "sc", CompareMethod.Text) <> 0 Then
                        If pnDiscAmtN > 0 And pnNoClient > 0 Then
                            builder.Append("SENIOR/PWD INFORMATION" & Environment.NewLine)
                        End If
                    End If
                    'add name and signature field
                    builder.Append("ID No: " & p_oDTDiscnt(0).Item("sIDNumber") & Environment.NewLine)
                    builder.Append("Name: " & p_oDTDiscnt(0).Item("sClientNm") & Environment.NewLine)
                    builder.Append("Signature:______________________________" & Environment.NewLine)

                End If
            End If
        End If

        'Print Credit Card Info
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                'Print Credit Card Bank
                builder.Append(p_oDTCredit(lnCtr).Item("sCardBank") & Environment.NewLine)

                'Print Card Number/Should hide entire card number
                ls4Print = p_oDTCredit(lnCtr).Item("sCardNoxx")
                ls4Print = Left(ls4Print, 5) & "".PadLeft(ls4Print.Length - 9, "*") & Right(ls4Print, 4)
                builder.Append(ls4Print & Environment.NewLine)
                builder.Append("SWIPED" & Environment.NewLine)
                builder.Append("Approval Code: " & p_oDTCredit(lnCtr).Item("sApprovNo") & Environment.NewLine)
            Next
        End If

        'Print Check Payment Info
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                builder.Append("Check No: " & p_oDTChkPym(lnCtr).Item("sCheckNox") & Environment.NewLine)
                builder.Append("Bank    : " & p_oDTChkPym(lnCtr).Item("sCheckBnk") & Environment.NewLine)
                builder.Append("Date:   : " & Format(p_oDTChkPym(lnCtr).Item("dCheckDte"), xsDATE_SHORT) & Environment.NewLine)
                builder.Append("Amount  : " & Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL) & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine & Environment.NewLine)

        'Compute VAT & and other info
        '++++++++++++++++++++++++++++++++++++++
        'VAT is 12 % of sales
        'TODO: load VAT percent of sales from CONFIG
        'pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) / lnVatPerc
        'pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) - pnVatblSle

        pnVatblSle = ((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        pnVatAmntx = ((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        'Print VAT Related info
        builder.Append("  VAT Exempt Sales      " & Format(pnVatExSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  Zero-Rated Sales      " & Format(pnZroRtSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VATable Sales         " & Format(pnVatblSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VAT Amount            " & Format(pnVatAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine & Environment.NewLine)

        If psCustName <> "" Then
            builder.Append(" Cust Name: " & psCustName & Environment.NewLine)
            builder.Append(" Address  : " & psCustAddx & Environment.NewLine)
            builder.Append(" TIN      : " & psCustTINx & Environment.NewLine)
            builder.Append(" Bus Style: " & psCustBusx & Environment.NewLine & Environment.NewLine)
        Else
            builder.Append(" Cust Name: ____________________________" & Environment.NewLine)
            builder.Append(" Address  : ____________________________" & Environment.NewLine)
            builder.Append(" TIN      : ____________________________" & Environment.NewLine)
            builder.Append(" Bus Style: ____________________________" & Environment.NewLine & Environment.NewLine)
        End If

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        'Print the Footer
        For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
            builder.Append(PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter("----- END OF RECEIPT -----", 40) & Environment.NewLine)
        RawPrint.writeToFile(p_sPOSNo & " " & Format(p_oApp.getSysDate, "yyyyMMdd"), builder.ToString())

        Return True
    End Function

    Private Function WriteORx() As Boolean
        Dim lnVatPerc As Double = 1.12

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)

        For lnCtr = 0 To p_oDTHeader.Rows.Count - 2
            builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
            Debug.Print(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        Select Case p_cTrnMde
            Case "A"
                builder.Append(PadCenter("CHARGE INVOICE", 40) & Environment.NewLine)
            Case "D"
                builder.Append(PadCenter("CHARGE INVOICE", 40) & Environment.NewLine)
                builder.Append(PadCenter("TRANING MODE", 40) & Environment.NewLine)
        End Select

        If pbReprint Then
            builder.Append(PadCenter(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName"), 40) & Environment.NewLine)
        End If

        builder.Append(Environment.NewLine)

        'Print Cashier
        builder.Append(" Cashier: " & p_sLogName & "/" & psCashierx & Environment.NewLine)
        If p_nTableNo > 0 Then
            builder.Append(" Table No.: " & p_nTableNo & Environment.NewLine)
        End If
        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" CI No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Transaction No.: " & psTransNox & Environment.NewLine)
        builder.Append(" Date : " & Format(pdTransact, "yyyy-mm-dd") & " " & Format(p_oApp.getSysDate, "hh:mm:ss") & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnCtr).Item("nQuantity") > 0 Then
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            Else
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            End If

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                If p_oDTDetail(lnCtr).Item("cDetailxx") = "1" Then
                    ls4Print = "  " & Left(ls4Print, pxeQTYLEN + 1 + pxeDSCLEN - 2)
                End If

                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN)
                If p_oDTDetail(lnCtr).Item("cVatablex") Then
                    ls4Print = ls4Print
                    'ls4Print = ls4Print + "V"
                End If

                builder.Append(ls4Print & Environment.NewLine)
            Else
                builder.Append(Space(2) & ls4Print & Environment.NewLine)
            End If
        Next

        'Print Detail of Complementary
        If p_oDTComplx.Rows.Count > 0 Then
            builder.Append("COMPLEMENT: " & Environment.NewLine)
            For lnCtr = 0 To p_oDTComplx.Rows.Count - 1

                ls4Print = Format(p_oDTComplx(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTComplx(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" No. of Items: " & pnTotalItm & Environment.NewLine)

        'do we have SC Discount?
        If pnDiscAmtN > 0 And pnNoClient > 0 Then
            'print no of clients and no of with discounts
            builder.Append(" Total No. of Clients: " & p_nNoClient & Environment.NewLine)
            builder.Append(" No. of SC/PWD Clients: " & p_nWithDisc & Environment.NewLine)
        End If
        builder.Append(Environment.NewLine)

        'Print TOTAL Sales    
        If pnSChargex > 0 Or pnDiscAmtN > 0 Or pnDiscAmtV > 0 Then
            builder.Append(" Sub-Total".PadRight(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        End If

        Dim lnExVATDue = pnTotalDue / 1.12

        'Print Discounts
        If pnDiscAmtV > 0 Then
            'builder.Append(" Less: Discount(s)".PadRight(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Dim lnVATExclsv = pnTotalDue / lnVatPerc
            Dim lnRateAmntx = lnVATExclsv * (pnDiscRteV / 100)
            Dim lnAddDiscxx = pnAddDiscV / lnVatPerc

            Dim lnAmountDue = pnTotalDue - pnDiscAmtV
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + lnAddDiscxx + pnDiscAmtN)

            Dim lsLess As String = " Less: "
            If pnDiscRteV > 0 Then
                builder.Append((lsLess & Math.Round(pnDiscRteV) & "% Discount").PadRight(25) & " " & Format(lnRateAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnAddDiscV > 0 Then
                builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATExWDsc * 0.12, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        ElseIf pnDiscAmtN > 0 Then
            'orig code
            'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lnVATablex As Decimal = 0
            Dim lnNVATable As Decimal = 0
            Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, lnNVATable)

            If p_nNoClient <> p_nWithDisc Then
                builder.Append(" Price Inclusive of VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lnExVATDue = ((pnTotalDue / pnNoClient) * p_nWithDisc) / 1.12
                builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Else
                builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            If pnSChargex > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
                builder.Append(" Total Due".PadRight(25) & " " & Format((lnVATablex + lnExVATDue) - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            'If lnVATablex > 0 Then
            '    builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            '    builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnExVATDue - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            '    builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        Else
            If pnSChargex > 0 Then
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            Else
                'builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            End If
        End If

        'Print Amount Due By subracting the discounts
        builder.Append(" TOTAL AMOUNT DUE".PadRight(25) & " " & Format((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Cash Payments
        If pnCashTotl > 0 Then
            builder.Append(" Cash".PadRight(25) & " " & Format(pnCashTotl, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If

        'Print Credit Card Payments
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
                           Format(p_oDTCredit(lnCtr).Item("nCardAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Check Payments
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
                           Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Gift Coupon
        If p_oDTGftChk.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce") & " GIFT CHEQUE").PadRight(24) & " " & _
                           Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Discount Information
        If Not IsNothing(p_oDTDiscnt) Then
            If p_oDTDiscnt.Rows.Count > 0 Then
                If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then
                    builder.Append(Environment.NewLine)
                    builder.Append("///////////////////////////////////////" & Environment.NewLine)
                    If InStr(LCase(p_oDTDiscnt(0).Item("sDiscCard")), "sc", CompareMethod.Text) <> 0 Then
                        builder.Append("SENIOR/PWD INFORMATION" & Environment.NewLine)
                    End If
                    'add name and signature field
                    builder.Append("ID No: " & p_oDTDiscnt(0).Item("sIDNumber") & Environment.NewLine)
                    builder.Append("Name: " & p_oDTDiscnt(0).Item("sClientNm") & Environment.NewLine)
                    builder.Append("Signature:______________________________" & Environment.NewLine)

                End If
            End If
        End If

        'Print Credit Card Info
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                'Print Credit Card Bank
                builder.Append(p_oDTCredit(lnCtr).Item("sCardBank") & Environment.NewLine)

                'Print Card Number/Should hide entire card number
                ls4Print = p_oDTCredit(lnCtr).Item("sCardNoxx")
                ls4Print = Left(ls4Print, 5) & "".PadLeft(ls4Print.Length - 9, "*") & Right(ls4Print, 4)
                builder.Append(ls4Print & Environment.NewLine)
                builder.Append("SWIPED" & Environment.NewLine)
                builder.Append("Approval Code: " & p_oDTCredit(lnCtr).Item("sApprovNo") & Environment.NewLine)
            Next
        End If

        'Print Check Payment Info
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                builder.Append("Check No: " & p_oDTChkPym(lnCtr).Item("sCheckNox") & Environment.NewLine)
                builder.Append("Bank    : " & p_oDTChkPym(lnCtr).Item("sCheckBnk") & Environment.NewLine)
                builder.Append("Date:   : " & Format(p_oDTChkPym(lnCtr).Item("dCheckDte"), xsDATE_SHORT) & Environment.NewLine)
                builder.Append("Amount  : " & Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL) & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine & Environment.NewLine)

        'Compute VAT & and other info
        '++++++++++++++++++++++++++++++++++++++
        'VAT is 12 % of sales
        'TODO: load VAT percent of sales from CONFIG
        'pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) / lnVatPerc
        'pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) - pnVatblSle

        pnVatblSle = ((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        pnVatAmntx = ((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        'Print VAT Related info
        builder.Append("  VAT Exempt Sales      " & Format(pnVatExSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  Zero-Rated Sales      " & Format(pnZroRtSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VATable Sales         " & Format(pnVatblSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VAT Amount            " & Format(pnVatAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine & Environment.NewLine)

        If psCustName <> "" Then
            builder.Append(" Cust Name: " & psCustName & Environment.NewLine)
            builder.Append(" Address  : " & psCustAddx & Environment.NewLine)
            builder.Append(" TIN      : " & psCustTINx & Environment.NewLine)
            builder.Append(" Bus Style: " & psCustBusx & Environment.NewLine & Environment.NewLine)
        Else
            builder.Append(" Cust Name: ____________________________" & Environment.NewLine)
            builder.Append(" Address  : ____________________________" & Environment.NewLine)
            builder.Append(" TIN      : ____________________________" & Environment.NewLine)
            builder.Append(" Bus Style: ____________________________" & Environment.NewLine & Environment.NewLine)
        End If

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

    Private Function WriteOROldx() As Boolean
        Dim lnVatPerc As Double = 1.12

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)

        For lnCtr = 0 To p_oDTHeader.Rows.Count - 2
            builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
            Debug.Print(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter("CHARGE INVOICE", 40) & Environment.NewLine)

        If pbReprint Then
            builder.Append(PadCenter(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName"), 40) & Environment.NewLine)
        End If

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnCtr).Item("nQuantity") > 0 Then
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            Else
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            End If

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                If p_oDTDetail(lnCtr).Item("cDetailxx") = "1" Then
                    ls4Print = "  " & Left(ls4Print, pxeQTYLEN + 1 + pxeDSCLEN - 2)
                End If

                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN)
                If p_oDTDetail(lnCtr).Item("cVatablex") Then
                    ls4Print = ls4Print
                    'ls4Print = ls4Print + "V"
                End If

                builder.Append(ls4Print & Environment.NewLine)
            Else
                builder.Append(Space(2) & ls4Print & Environment.NewLine)
            End If
        Next

        'Print Detail of Complementary
        If p_oDTComplx.Rows.Count > 0 Then
            builder.Append("COMPLEMENT: " & Environment.NewLine)
            For lnCtr = 0 To p_oDTComplx.Rows.Count - 1

                ls4Print = Format(p_oDTComplx(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTComplx(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append(" No. of Items: " & pnTotalItm & Environment.NewLine)

        'do we have SC Discount?
        If pnDiscAmtN > 0 And pnNoClient > 0 Then
            'print no of clients and no of with discounts
            builder.Append(" Total No. of Clients: " & pnNoClient & Environment.NewLine)
            builder.Append(" No. of SC/PWD Clients: " & pnWithDisc & Environment.NewLine)
        End If

        builder.Append(Environment.NewLine)

        'Print TOTAL Sales
        builder.Append(" Sub-Total".PadRight(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

        Dim lnExVATDue = pnTotalDue / 1.12

        'Print Discounts
        If pnDiscAmtV > 0 Then
            'builder.Append(" Less: Discount(s)".PadRight(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Dim lnVATExclsv = pnTotalDue / lnVatPerc
            Dim lnRateAmntx = lnVATExclsv * (pnDiscRteV / 100)
            Dim lnAddDiscxx = pnAddDiscV / lnVatPerc

            Dim lnAmountDue = pnTotalDue - pnDiscAmtV
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + lnAddDiscxx + pnDiscAmtN)


            builder.Append(" Price Exlusive of VAT".PadRight(25) & " " & Format(lnVATExclsv, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lsLess As String = " Less: "
            If pnDiscRteV > 0 Then
                builder.Append((lsLess & Math.Round(pnDiscRteV) & "% Discount").PadRight(25) & " " & Format(lnRateAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnAddDiscV > 0 Then
                builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnDiscAmtN > 0 Then
                builder.Append((lsLess & "20% SC/PWD DSC").PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATExWDsc * 0.12, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'Print Line before Amount Due
            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        ElseIf pnDiscAmtN > 0 Then
            'orig code
            'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lnVATablex As Decimal = 0
            Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, 0)

            builder.Append(" Price Exlusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            If lnVATablex > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
                builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnExVATDue - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        End If

        'Print Amount Due By subracting the discounts
        builder.Append(" TOTAL AMOUNT DUE :".PadRight(25) & " " & Format(pnTotalDue - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Cash Payments
        If pnCashTotl > 0 Then
            builder.Append(" Cash".PadRight(25) & " " & Format(pnCashTotl, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If

        'Print Credit Card Payments
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
                           Format(p_oDTCredit(lnCtr).Item("nCardAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Check Payments
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
                           Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Gift Coupon
        If p_oDTGftChk.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce") & " GIFT CHEQUE").PadRight(24) & " " & _
                           Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Line Before change....
        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

        'Print Discount Information
        If Not IsNothing(p_oDTDiscnt) Then
            If p_oDTDiscnt.Rows.Count > 0 Then
                If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then

                    builder.Append("///////////////////////////////////////" & Environment.NewLine)

                    For lnCtr = 0 To p_oDTDiscnt.Rows.Count - 1
                        'Print Discount Description
                        builder.Append(p_oDTDiscnt(lnCtr).Item("sDiscCard") & Environment.NewLine)

                        'check if it is SC ID
                        If InStr(LCase(p_oDTDiscnt(lnCtr).Item("sDiscCard")), "sc", CompareMethod.Text) <> 0 Then
                            'add name and signature field
                            builder.Append("    ID No:" & p_oDTDiscnt(lnCtr).Item("sIDNumber") & Environment.NewLine)
                            builder.Append("     Name:______________________________" & Environment.NewLine)
                            builder.Append("Signature:______________________________" & Environment.NewLine)
                        Else
                            'Print Card Number
                            If Trim(p_oDTDiscnt(lnCtr).Item("sIDNumber")) <> "" Then
                                builder.Append("ID No:" & p_oDTDiscnt(lnCtr).Item("sIDNumber") & Environment.NewLine)
                            Else
                                builder.Append("ID No: N/A" & Environment.NewLine)
                            End If
                        End If
                    Next
                End If
            End If
        End If

        'Print Credit Card Info
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                'Print Credit Card Bank
                builder.Append(p_oDTCredit(lnCtr).Item("sCardBank") & Environment.NewLine)

                'Print Card Number/Should hide entire card number
                ls4Print = p_oDTCredit(lnCtr).Item("sCardNoxx")
                ls4Print = Left(ls4Print, 5) & "".PadLeft(ls4Print.Length - 9, "*") & Right(ls4Print, 4)
                builder.Append(ls4Print & Environment.NewLine)
                builder.Append("SWIPED" & Environment.NewLine)
                builder.Append("Approval Code: " & p_oDTCredit(lnCtr).Item("sApprovNo") & Environment.NewLine)
            Next
        End If

        'Print Check Payment Info
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                builder.Append("Check No: " & p_oDTChkPym(lnCtr).Item("sCheckNox") & Environment.NewLine)
                builder.Append("Bank    : " & p_oDTChkPym(lnCtr).Item("sCheckBnk") & Environment.NewLine)
                builder.Append("Date:   : " & Format(p_oDTChkPym(lnCtr).Item("dCheckDte"), xsDATE_SHORT) & Environment.NewLine)
                builder.Append("Amount  : " & Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL) & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine & Environment.NewLine)

        'Compute VAT & and other info
        '++++++++++++++++++++++++++++++++++++++
        'VAT is 12 % of sales
        'TODO: load VAT percent of sales from CONFIG
        'pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) / lnVatPerc
        'pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) - pnVatblSle

        pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        'Print VAT Related info
        builder.Append("  VAT Exempt Sales      " & Format(pnVatExSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  Zero-Rated Sales      " & Format(pnZroRtSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VATable Sales         " & Format(pnVatblSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VAT Amount            " & Format(pnVatAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine & Environment.NewLine)

        If psCustName <> "" Then
            builder.Append(" Cust Name: " & psCustName & Environment.NewLine)
            builder.Append(" Address  : " & psCustAddx & Environment.NewLine)
            builder.Append(" TIN      : " & psCustTINx & Environment.NewLine)
            builder.Append(" Bus Style: " & psCustBusx & Environment.NewLine & Environment.NewLine)
        Else
            builder.Append(" Cust Name: ____________________________" & Environment.NewLine)
            builder.Append(" Address  : ____________________________" & Environment.NewLine)
            builder.Append(" TIN      : ____________________________" & Environment.NewLine)
            builder.Append(" Bus Style: ____________________________" & Environment.NewLine & Environment.NewLine)
        End If

        'Print Cashier
        builder.Append(" Cashier: " & psCashrNme & Environment.NewLine)
        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" CI No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Date : " & Format(pdTransact, "yyyy-mm-dd") & " " & Format(p_oApp.getSysDate, "hh:mm:ss") & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        'Print the Footer
        For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
            builder.Append(PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter("----- END OF INVOICE -----", 40) & Environment.NewLine)
        RawPrint.writeToFile(p_sPOSNo & " " & Format(p_oApp.getSysDate(), "yyyyMMdd"), builder.ToString())

        Return True
    End Function


    Public Function PrintCI() As Boolean
        Dim lnVatPerc As Double = 1.12

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

        If Not AddHeader("Serial No.: " & p_sSerial) Then
            MsgBox("Invalid Serial No.!")
            Return False
        End If

        If Not AddHeader("REPRINT") Then
            MsgBox("Unable to Reprint!")
            Return False
        End If

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)

        For lnCtr = 0 To p_oDTHeader.Rows.Count - 2
            builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
            Debug.Print(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter("CHARGE INVOICE", 40) & Environment.NewLine)
        Select Case p_cTrnMde
            Case "D"
                builder.Append(PadCenter("TRANING MODE", 40) & Environment.NewLine)
        End Select

        If pbReprint Then
            builder.Append(PadCenter(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName"), 40) & Environment.NewLine)
        End If

        builder.Append(Environment.NewLine)

        'Print Cashier
        builder.Append(" Cashier: " & p_sLogName & "/" & psCashierx & Environment.NewLine)
        If p_nTableNo > 0 Then
            builder.Append(" Table No.: " & p_nTableNo & Environment.NewLine)
        End If
        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" CI No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Transaction No.: " & psTransNox & Environment.NewLine)
        builder.Append(" Date : " & Format(pdTransact, "yyyy-mm-dd") & " " & Format(p_oApp.getSysDate, "hh:mm:ss") & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnCtr).Item("nQuantity") > 0 Then
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            Else
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            End If

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                If p_oDTDetail(lnCtr).Item("cDetailxx") = "1" Then
                    ls4Print = "  " & Left(ls4Print, pxeQTYLEN + 1 + pxeDSCLEN - 2)
                End If

                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN)
                If p_oDTDetail(lnCtr).Item("cVatablex") Then
                    ls4Print = ls4Print
                    'ls4Print = ls4Print + "V"
                End If

                builder.Append(ls4Print & Environment.NewLine)
            Else
                builder.Append(Space(2) & ls4Print & Environment.NewLine)
            End If
        Next

        'Print Detail of Complementary
        If p_oDTComplx.Rows.Count > 0 Then
            builder.Append("COMPLEMENT: " & Environment.NewLine)
            For lnCtr = 0 To p_oDTComplx.Rows.Count - 1

                ls4Print = Format(p_oDTComplx(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTComplx(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" No. of Items: " & pnTotalItm & Environment.NewLine)

        'do we have SC Discount?
        If pnDiscAmtN > 0 And pnNoClient > 0 Then
            'print no of clients and no of with discounts
            builder.Append(" Total No. of Clients: " & p_nNoClient & Environment.NewLine)
            builder.Append(" No. of SC/PWD Clients: " & p_nWithDisc & Environment.NewLine)
        End If
        builder.Append(Environment.NewLine)

        'Print TOTAL Sales    
        If pnSChargex > 0 Or pnDiscAmtN > 0 And pnDiscAmtV > 0 Then
            builder.Append(" Sub-Total".PadRight(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        End If

        Dim lnExVATDue = pnTotalDue / 1.12

        'Print Discounts
        If pnDiscAmtV > 0 Then
            'builder.Append(" Less: Discount(s)".PadRight(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Dim lnVATExclsv = pnTotalDue / lnVatPerc
            Dim lnRateAmntx = lnVATExclsv * (pnDiscRteV / 100)
            Dim lnAddDiscxx = pnAddDiscV / lnVatPerc

            Dim lnAmountDue = pnTotalDue - pnDiscAmtV
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + lnAddDiscxx + pnDiscAmtN)

            Dim lsLess As String = " Less: "
            If pnDiscRteV > 0 Then
                builder.Append((lsLess & Math.Round(pnDiscRteV) & "% Discount").PadRight(25) & " " & Format(lnRateAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnAddDiscV > 0 Then
                builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATExWDsc * 0.12, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        ElseIf pnDiscAmtN > 0 Then
            'orig code
            'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lnVATablex As Decimal = 0
            Dim lnNVATable As Decimal = 0
            Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, lnNVATable)

            If p_nNoClient <> p_nWithDisc Then
                builder.Append(" Price Inclusive of VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lnExVATDue = ((pnTotalDue / pnNoClient) * p_nWithDisc) / 1.12
                builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Else
                builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            If pnSChargex > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
                builder.Append(" Total Due".PadRight(25) & " " & Format((lnVATablex + lnExVATDue) - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            'If lnVATablex > 0 Then
            '    builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            '    builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnExVATDue - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            '    builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        Else
            If pnSChargex > 0 Then
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            Else
                'builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            End If
        End If

        'Print Amount Due By subracting the discounts
        builder.Append(" TOTAL AMOUNT DUE".PadRight(25) & " " & Format((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Cash Payments
        If pnCashTotl > 0 Then
            builder.Append(" Cash".PadRight(25) & " " & Format(pnCashTotl, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If

        'Print Credit Card Payments
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
                           Format(p_oDTCredit(lnCtr).Item("nCardAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Check Payments
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
                           Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Gift Coupon
        If p_oDTGftChk.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce") & " GIFT CHEQUE").PadRight(24) & " " & _
                           Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Discount Information
        If Not IsNothing(p_oDTDiscnt) Then
            If p_oDTDiscnt.Rows.Count > 0 Then
                If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then
                    builder.Append(Environment.NewLine)
                    builder.Append("///////////////////////////////////////" & Environment.NewLine)
                    If InStr(LCase(p_oDTDiscnt(0).Item("sDiscCard")), "sc", CompareMethod.Text) <> 0 Then
                        builder.Append("SENIOR/PWD INFORMATION" & Environment.NewLine)
                    End If
                    'add name and signature field
                    builder.Append("ID No: " & p_oDTDiscnt(0).Item("sIDNumber") & Environment.NewLine)
                    builder.Append("Name: " & p_oDTDiscnt(0).Item("sClientNm") & Environment.NewLine)
                    builder.Append("Signature:______________________________" & Environment.NewLine)

                End If
            End If
        End If

        'Print Credit Card Info
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                'Print Credit Card Bank
                builder.Append(p_oDTCredit(lnCtr).Item("sCardBank") & Environment.NewLine)

                'Print Card Number/Should hide entire card number
                ls4Print = p_oDTCredit(lnCtr).Item("sCardNoxx")
                ls4Print = Left(ls4Print, 5) & "".PadLeft(ls4Print.Length - 9, "*") & Right(ls4Print, 4)
                builder.Append(ls4Print & Environment.NewLine)
                builder.Append("SWIPED" & Environment.NewLine)
                builder.Append("Approval Code: " & p_oDTCredit(lnCtr).Item("sApprovNo") & Environment.NewLine)
            Next
        End If

        'Print Check Payment Info
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                builder.Append("Check No: " & p_oDTChkPym(lnCtr).Item("sCheckNox") & Environment.NewLine)
                builder.Append("Bank    : " & p_oDTChkPym(lnCtr).Item("sCheckBnk") & Environment.NewLine)
                builder.Append("Date:   : " & Format(p_oDTChkPym(lnCtr).Item("dCheckDte"), xsDATE_SHORT) & Environment.NewLine)
                builder.Append("Amount  : " & Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL) & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine & Environment.NewLine)

        'Compute VAT & and other info
        '++++++++++++++++++++++++++++++++++++++
        'VAT is 12 % of sales
        'TODO: load VAT percent of sales from CONFIG
        'pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) / lnVatPerc
        'pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) - pnVatblSle

        pnVatblSle = ((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        pnVatAmntx = ((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        'Print VAT Related info
        builder.Append("  VAT Exempt Sales      " & Format(pnVatExSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  Zero-Rated Sales      " & Format(pnZroRtSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VATable Sales         " & Format(pnVatblSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VAT Amount            " & Format(pnVatAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine & Environment.NewLine)

        If psCustName <> "" Then
            builder.Append(" Cust Name: " & psCustName & Environment.NewLine)
            builder.Append(" Address  : " & psCustAddx & Environment.NewLine)
            builder.Append(" TIN      : " & psCustTINx & Environment.NewLine)
            builder.Append(" Bus Style: " & psCustBusx & Environment.NewLine & Environment.NewLine)
        Else
            builder.Append(" Cust Name: ____________________________" & Environment.NewLine)
            builder.Append(" Address  : ____________________________" & Environment.NewLine)
            builder.Append(" TIN      : ____________________________" & Environment.NewLine)
            builder.Append(" Bus Style: ____________________________" & Environment.NewLine & Environment.NewLine)
        End If

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

        Call WriteOR()

        p_oApp.SaveEvent("0016", "OR No. " & psReferNox, p_sTermnl)

        Return True
    End Function


    Public Function PrintOR() As Boolean
        Dim lnDeducQTY As Integer
        Dim lnVatPerc As Double = 1.12

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

        If Not AddHeader("Serial No.: " & p_sSerial) Then
            MsgBox("Invalid Serial No.!")
            Return False
        End If

        If Not AddHeader("REPRINT") Then
            MsgBox("Unable to Reprint!")
            Return False
        End If

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)

        For lnCtr = 0 To p_oDTHeader.Rows.Count - 2
            builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
            Debug.Print(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter("CHARGE INVOICE", 40) & Environment.NewLine)
        Select Case p_cTrnMde
            Case "D"
                builder.Append(PadCenter("TRANING MODE", 40) & Environment.NewLine)
        End Select


        If pbReprint Then
            builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
            builder.Append(RawPrint.pxePRINT_CNTR)
            builder.Append(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName") & Environment.NewLine)
        End If

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Cashier
        builder.Append(Environment.NewLine)
        builder.Append(" Cashier: " & p_sLogName & "/" & psCashierx & Environment.NewLine)
        If p_nTableNo > 0 Then
            builder.Append(" Table No.: " & p_nTableNo & "".PadRight(12) & " " & "DINE-IN".PadLeft(pxeREGLEN) & Environment.NewLine)
        Else
            builder.Append(" TAKE-OUT " & Environment.NewLine)
        End If

        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" CI No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Transaction No.: " & psTransNox & Environment.NewLine)
        builder.Append(" Date : " & Format(CDate(pdTransact), "yyyy-mm-dd") & " " & Format(p_oApp.getSysDate, "hh:mm:ss") & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        lnDeducQTY = 0
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnCtr).Item("nQuantity") > 0 Then
                If p_oDTDetail(lnCtr).Item("cDetailxx") = "1" Then
                    If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                        ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    Else
                        ls4Print = String.Empty.PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    End If
                Else
                    If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                        ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    Else
                        ls4Print = "   " & UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                        lnDeducQTY = lnDeducQTY + p_oDTDetail(lnCtr).Item("nQuantity")
                    End If
                End If
            Else
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            End If

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                If p_oDTDetail(lnCtr).Item("cDetailxx") = "1" Then
                    'If p_oDTDetail(lnCtr).Item("nQuantity") < 10 Then
                    '    ls4Print = "  " & Left(ls4Print, pxeQTYLEN + 1 + pxeDSCLEN - 2)
                    'Else
                    '    ls4Print = "   " & Left(ls4Print, pxeQTYLEN + 1 + pxeDSCLEN - 2)
                    'End If
                End If

                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN)
                If p_oDTDetail(lnCtr).Item("cVatablex") Then
                    ls4Print = ls4Print
                    'ls4Print = ls4Print + "V"
                End If

                builder.Append(ls4Print & Environment.NewLine)
            Else
                If p_oDTDetail(lnCtr).Item("cWthPromo") = "1" Then
                    ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce") * p_oDTDetail(lnCtr).Item("nQuantity"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                    ls4Print = "  " & ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce") * p_oDTDetail(lnCtr).Item("nQuantity"), xsDECIMAL).PadLeft(pxeTTLLEN)
                    builder.Append(ls4Print & Environment.NewLine)
                Else
                    builder.Append(Space(2) & ls4Print & Environment.NewLine)
                End If
            End If
        Next

        'Print Detail of Complementary
        If p_oDTComplx.Rows.Count > 0 Then
            builder.Append("COMPLEMENT: " & Environment.NewLine)
            For lnCtr = 0 To p_oDTComplx.Rows.Count - 1

                ls4Print = Format(p_oDTComplx(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTComplx(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" No. of Items: " & pnTotalItm - lnDeducQTY & Environment.NewLine)

        'do we have SC Discount?
        If pnDiscAmtN > 0 And pnNoClient > 0 Then
            'print no of clients and no of with discounts
            builder.Append(" Total No. of Clients: " & p_nNoClient & Environment.NewLine)
            builder.Append(" No. of SC/PWD Clients: " & p_nWithDisc & Environment.NewLine)
        End If
        builder.Append(Environment.NewLine)

        'Print TOTAL Sales    
        If pnSChargex > 0 Or pnDiscAmtN > 0 Or pnDiscAmtV > 0 Then
            builder.Append(" Sub-Total".PadRight(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            If pnDiscAmtN > 0 Or pnDiscAmtV > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            End If
        End If

        Dim lnExVATDue = pnTotalDue / 1.12

        'Print Discounts
        If pnDiscAmtV > 0 Then
            'builder.Append(" Less: Discount(s)".PadRight(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Dim lnVATExclsv = pnTotalDue / lnVatPerc
            Dim lnRateAmntx = lnVATExclsv * (pnDiscRteV / 100)
            Dim lnAddDiscxx = pnAddDiscV / lnVatPerc

            Dim lnAmountDue = pnTotalDue - pnDiscAmtV
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + lnAddDiscxx + pnDiscAmtN)

            Dim lsLess As String = " Less: "
            If pnDiscRteV > 0 Then
                'builder.Append((lsLess & Math.Round(pnDiscRteV) & "% Discount").PadRight(25) & " " & Format(lnRateAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(lsLess & p_oDTDiscnt(0).Item("sDiscCard") & Environment.NewLine)
                builder.Append("       " & ("(" & Format(Math.Round(pnDiscRteV), "#0.0") & "%)").PadRight(18) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                'builder.Append("       " & ("(" & Format(Math.Round(pnDiscRteV), "#0.0") & "%)").PadRight(18) & " " & Format(pnTotalDue * (pnDiscRteV / 100), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnAddDiscV > 0 Then
                builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATExWDsc * 0.12, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        ElseIf pnDiscAmtN > 0 Then
            'orig code
            'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lnVATablex As Decimal = 0
            Dim lnNVATable As Decimal = 0
            Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, lnNVATable)

            If p_nNoClient <> p_nWithDisc Then
                builder.Append(" Price Inclusive of VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lnExVATDue = ((pnTotalDue / pnNoClient) * p_nWithDisc) / 1.12
                builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Else
                builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            If pnSChargex > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
                builder.Append(" Total Due".PadRight(25) & " " & Format((lnVATablex + lnExVATDue) - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            'If lnVATablex > 0 Then
            '    builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            '    builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnExVATDue - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            '    builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        Else
            If pnSChargex > 0 Then
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            Else
                'builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            End If
        End If

        'Print Amount Due By subracting the discounts
        builder.Append(" TOTAL AMOUNT DUE".PadRight(25) & " " & Format((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Cash Payments
        If pnCashTotl > 0 Then
            builder.Append(" Cash".PadRight(25) & " " & Format(pnCashTotl, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If

        'Print Credit Card Payments
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
                           Format(p_oDTCredit(lnCtr).Item("nCardAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Check Payments
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
                           Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Gift Coupon
        If p_oDTGftChk.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce") & " GIFT CHEQUE").PadRight(24) & " " & _
                           Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Line Before change....
        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

        'Print Change
        Dim lnChange As Decimal = (pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN)

        If pnGiftTotl > lnChange Then
            lnChange = 0
        Else
            lnChange = (pnCashTotl + pnChckTotl + pnCrdtTotl + pnGiftTotl) - lnChange
        End If

        builder.Append(" CHANGE".PadRight(25) & " " & Format(lnChange, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Discount Information
        If Not IsNothing(p_oDTDiscnt) Then
            If p_oDTDiscnt.Rows.Count > 0 Then
                If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then
                    builder.Append(Environment.NewLine)
                    builder.Append("///////////////////////////////////////" & Environment.NewLine)
                    If InStr(LCase(p_oDTDiscnt(0).Item("sDiscCard")), "sc", CompareMethod.Text) <> 0 Then
                        If pnDiscAmtN > 0 And pnNoClient > 0 Then
                            builder.Append("SENIOR/PWD INFORMATION" & Environment.NewLine)
                        End If
                    End If
                    'add name and signature field
                    builder.Append("ID No: " & p_oDTDiscnt(0).Item("sIDNumber") & Environment.NewLine)
                    builder.Append("Name: " & p_oDTDiscnt(0).Item("sClientNm") & Environment.NewLine)
                    builder.Append("Signature:______________________________" & Environment.NewLine)

                End If
            End If
        End If

        'Print Credit Card Info
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                'Print Credit Card Bank
                builder.Append(p_oDTCredit(lnCtr).Item("sCardBank") & Environment.NewLine)

                'Print Card Number/Should hide entire card number
                ls4Print = p_oDTCredit(lnCtr).Item("sCardNoxx")
                ls4Print = Left(ls4Print, 5) & "".PadLeft(ls4Print.Length - 9, "*") & Right(ls4Print, 4)
                builder.Append(ls4Print & Environment.NewLine)
                builder.Append("SWIPED" & Environment.NewLine)
                builder.Append("Approval Code: " & p_oDTCredit(lnCtr).Item("sApprovNo") & Environment.NewLine)
            Next
        End If

        'Print Check Payment Info
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                builder.Append("Check No: " & p_oDTChkPym(lnCtr).Item("sCheckNox") & Environment.NewLine)
                builder.Append("Bank    : " & p_oDTChkPym(lnCtr).Item("sCheckBnk") & Environment.NewLine)
                builder.Append("Date:   : " & Format(p_oDTChkPym(lnCtr).Item("dCheckDte"), xsDATE_SHORT) & Environment.NewLine)
                builder.Append("Amount  : " & Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL) & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine & Environment.NewLine)

        'Compute VAT & and other info
        '++++++++++++++++++++++++++++++++++++++
        'VAT is 12 % of sales
        'TODO: load VAT percent of sales from CONFIG
        'pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) / lnVatPerc
        'pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) - pnVatblSle

        pnVatblSle = ((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        pnVatAmntx = ((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        'Print VAT Related info
        builder.Append("  VAT Exempt Sales      " & Format(pnVatExSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  Zero-Rated Sales      " & Format(pnZroRtSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VATable Sales         " & Format(pnVatblSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VAT Amount            " & Format(pnVatAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine & Environment.NewLine)

        If psCustName <> "" Then
            builder.Append(" Cust Name: " & psCustName & Environment.NewLine)
            builder.Append(" Address  : " & psCustAddx & Environment.NewLine)
            builder.Append(" TIN      : " & psCustTINx & Environment.NewLine)
            builder.Append(" Bus Style: " & psCustBusx & Environment.NewLine & Environment.NewLine)
        Else
            builder.Append(" Cust Name: ____________________________" & Environment.NewLine)
            builder.Append(" Address  : ____________________________" & Environment.NewLine)
            builder.Append(" TIN      : ____________________________" & Environment.NewLine)
            builder.Append(" Bus Style: ____________________________" & Environment.NewLine & Environment.NewLine)
        End If

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

        Call WriteOR()

        p_oApp.SaveEvent("0016", "OR No. " & psReferNox, p_sTermnl)

        Return True
    End Function

    Public Function PrintCIOld() As Boolean
        Dim lnVatPerc As Double = 1.12

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

        If Not AddHeader("Serial No.: " & p_sSerial) Then
            MsgBox("Invalid Serial No.!")
            Return False
        End If

        If Not AddHeader("REPRINT") Then
            MsgBox("Unable to Reprint!")
            Return False
        End If

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append(p_oDTHeader(0).Item("sHeadName") & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        For lnCtr = 1 To p_oDTHeader.Rows.Count - 2
            builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append("CHARGE INVOICE" & Environment.NewLine)

        If pbReprint Then
            builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
            builder.Append(RawPrint.pxePRINT_CNTR)
            builder.Append(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName") & Environment.NewLine)
        End If

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense

        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnCtr).Item("nQuantity") > 0 Then
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            Else
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            End If

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                If p_oDTDetail(lnCtr).Item("cDetailxx") = "1" Then
                    ls4Print = "  " & Left(ls4Print, pxeQTYLEN + 1 + pxeDSCLEN - 2)
                End If

                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN)
                If p_oDTDetail(lnCtr).Item("cVatablex") Then
                    'ls4Print = ls4Print + "V"
                    ls4Print = ls4Print
                End If

                builder.Append(ls4Print & Environment.NewLine)
            Else
                builder.Append(Space(2) & ls4Print & Environment.NewLine)
            End If
        Next

        'Print Detail of Complementary
        If p_oDTComplx.Rows.Count > 0 Then
            builder.Append("COMPLEMENT: " & Environment.NewLine)
            For lnCtr = 0 To p_oDTComplx.Rows.Count - 1

                ls4Print = Format(p_oDTComplx(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTComplx(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append(" No. of Items: " & pnTotalItm & Environment.NewLine)

        'do we have SC Discount?
        If pnDiscAmtN > 0 And pnNoClient > 0 Then
            'print no of clients and no of with discounts
            builder.Append(" Total No. of Clients: " & pnNoClient & Environment.NewLine)
            builder.Append(" No. of SC/PWD Clients: " & pnWithDisc & Environment.NewLine)
        End If

        builder.Append(Environment.NewLine)

        'Print TOTAL Sales
        builder.Append(" Sub-Total".PadRight(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

        Dim lnExVATDue = pnTotalDue / 1.12

        'Print Discounts
        If pnDiscAmtV > 0 Then
            'builder.Append(" Less: Discount(s)".PadRight(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Dim lnVATExclsv = pnTotalDue / lnVatPerc
            Dim lnRateAmntx = lnVATExclsv * (pnDiscRteV / 100)
            Dim lnAddDiscxx = pnAddDiscV / lnVatPerc

            Dim lnAmountDue = pnTotalDue - pnDiscAmtV
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + lnAddDiscxx + pnDiscAmtN)


            builder.Append(" Price Exlusive of VAT".PadRight(25) & " " & Format(lnVATExclsv, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lsLess As String = " Less: "
            If pnDiscRteV > 0 Then
                builder.Append((lsLess & Math.Round(pnDiscRteV) & "% Discount").PadRight(25) & " " & Format(lnRateAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnAddDiscV > 0 Then
                builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnDiscAmtN > 0 Then
                builder.Append((lsLess & "20% SC/PWD DSC").PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATExWDsc * 0.12, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'Print Line before Amount Due
            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        ElseIf pnDiscAmtN > 0 Then
            'orig code
            'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lnVATablex As Decimal = 0
            Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, 0)

            builder.Append(" Price Exlusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            If lnVATablex > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
                builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnExVATDue - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        End If

        builder.Append(RawPrint.pxePRINT_EMP1)          'Double Strike + Condense + Emphasize
        'Print Amount Due By subracting the discounts
        builder.Append(" TOTAL AMOUNT DUE :".PadRight(25) & " " & Format(pnTotalDue - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_EMP0)

        'Print Discount Information
        If Not IsNothing(p_oDTDiscnt) Then
            If p_oDTDiscnt.Rows.Count > 0 Then
                If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then

                    builder.Append("///////////////////////////////////////" & Environment.NewLine)

                    For lnCtr = 0 To p_oDTDiscnt.Rows.Count - 1
                        'Print Discount Description
                        builder.Append(p_oDTDiscnt(lnCtr).Item("sDiscCard") & Environment.NewLine)

                        'check if it is SC ID
                        If InStr(LCase(p_oDTDiscnt(lnCtr).Item("sDiscCard")), "sc", CompareMethod.Text) <> 0 Then
                            'add name and signature field
                            builder.Append("    ID No:" & p_oDTDiscnt(lnCtr).Item("sIDNumber") & Environment.NewLine)
                            builder.Append("     Name:______________________________" & Environment.NewLine)
                            builder.Append("Signature:______________________________" & Environment.NewLine)
                        Else
                            'Print Card Number
                            If Trim(p_oDTDiscnt(lnCtr).Item("sIDNumber")) <> "" Then
                                builder.Append("ID No:" & p_oDTDiscnt(lnCtr).Item("sIDNumber") & Environment.NewLine)
                            Else
                                builder.Append("ID No: N/A" & Environment.NewLine)
                            End If
                        End If
                    Next
                End If
            End If
        End If

        'Print Credit Card Info
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                'Print Credit Card Bank
                builder.Append(p_oDTCredit(lnCtr).Item("sCardBank") & Environment.NewLine)

                'Print Card Number/Should hide entire card number
                ls4Print = p_oDTCredit(lnCtr).Item("sCardNoxx")
                ls4Print = Left(ls4Print, 5) & "".PadLeft(ls4Print.Length - 9, "*") & Right(ls4Print, 4)
                builder.Append(ls4Print & Environment.NewLine)
                builder.Append("SWIPED" & Environment.NewLine)
                builder.Append("Approval Code: " & p_oDTCredit(lnCtr).Item("sApprovNo") & Environment.NewLine)
            Next
        End If

        'Print Check Payment Info
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                builder.Append("///////////////////////////////////////" & Environment.NewLine)
                builder.Append("Check No: " & p_oDTChkPym(lnCtr).Item("sCheckNox") & Environment.NewLine)
                builder.Append("Bank    : " & p_oDTChkPym(lnCtr).Item("sCheckBnk") & Environment.NewLine)
                builder.Append("Date:   : " & Format(p_oDTChkPym(lnCtr).Item("dCheckDte"), xsDATE_SHORT) & Environment.NewLine)
                builder.Append("Amount  : " & Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL) & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine & Environment.NewLine)

        pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        'Print VAT Related info
        builder.Append("  VAT Exempt Sales      " & Format(pnVatExSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  Zero-Rated Sales      " & Format(pnZroRtSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VATable Sales         " & Format(pnVatblSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VAT Amount            " & Format(pnVatAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine & Environment.NewLine)

        If psCustName <> "" Then
            builder.Append(" Cust Name: " & psCustName & Environment.NewLine)
            builder.Append(" Address  : " & psCustAddx & Environment.NewLine)
            builder.Append(" TIN      : " & psCustTINx & Environment.NewLine)
            builder.Append(" Bus Style: " & psCustBusx & Environment.NewLine & Environment.NewLine)
        Else
            builder.Append(" Cust Name: ____________________________" & Environment.NewLine)
            builder.Append(" Address  : ____________________________" & Environment.NewLine)
            builder.Append(" TIN      : ____________________________" & Environment.NewLine)
            builder.Append(" Bus Style: ____________________________" & Environment.NewLine & Environment.NewLine)
        End If

        'Print Cashier
        builder.Append(" Cashier: " & p_oApp.UserName & Environment.NewLine)
        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" CI No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Date : " & Format(pdTransact, "yyyy-mm-dd") & " " & Format(p_oApp.getSysDate, "hh:mm:ss") & Environment.NewLine)

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

        If Not pbReprint Then Call WriteOR()

        p_oApp.SaveEvent("0016", "CI No. " & psReferNox, p_sTermnl)

        Return True
    End Function

    Private Function computePWDSC(ByRef lnVATableAmt As Decimal, ByRef lnNVATableAmt As Decimal)
        'Dim lnDivAmountx = (pnTotalDue + pnSChargex) / pnNoClient   'divide the total amount to the number of customers
        Dim lnDivAmountx = pnTotalDue / pnNoClient   'divide the total amount to the number of customers
        Dim lnDivNonVATx = lnDivAmountx / 1.12                      'less 12% VAT on per customer amount due
        Dim lnSCDiscount = lnDivNonVATx * 0.2                       'discount for every SC
        Dim lnTotSCDiscx = lnSCDiscount * pnWithDisc                'Total PWD/SC discount

        If pnNoClient = pnWithDisc Then
            lnVATableAmt = 0.0
        Else
            lnVATableAmt = lnDivAmountx * Math.Abs(pnNoClient - pnWithDisc)
        End If
        lnNVATableAmt = lnDivNonVATx * pnWithDisc   'Total Non VATable amount
        Return lnTotSCDiscx

        'Dim lnNonVatAmtx = pnTotalDue / 1.12
        'Dim lnPartAmtxV = (pnTotalDue / pnNoClient) * pnWithDisc
        'Dim lnPartAmtxN = (lnNonVatAmtx / pnNoClient) * pnWithDisc
        'Dim lnPWDDiscntx = (lnPartAmtxV - lnPartAmtxN) + (lnPartAmtxN * 0.2)
        'Return lnPWDDiscntx
    End Function

    'Public Function PrintCI() As Boolean
    '    If Not AddHeader("The Monarch Hospitality and Tourism Corp") Then
    '        MsgBox("Invalid Company Name!")
    '        Return False
    '    End If

    '    If Not AddHeader("PEDRITOS PRIMA CAFE") Then
    '        MsgBox("Invalid Client Name!")
    '        Return False
    '    End If

    '    If Not AddHeader("Tapuac District") Then
    '        MsgBox("Invalid Client Address!")
    '        Return False
    '    End If

    '    If Not AddHeader("Dagupan City, Pangasinan") Then
    '        MsgBox("Invalid Town and Address!")
    '        Return False
    '    End If

    '    'If Not AddHeader(p_sCompny) Then
    '    '    MsgBox("Invalid Company Name!")
    '    '    Return False
    '    'End If

    '    'If Not AddHeader(p_oApp.BranchName) Then
    '    '    MsgBox("Invalid Client Name!")
    '    '    Return False
    '    'End If

    '    'If Not AddHeader(p_oApp.Address) Then
    '    '    MsgBox("Invalid Client Address!")
    '    '    Return False
    '    'End If

    '    'If Not AddHeader(p_oApp.TownCity & ", " & p_oApp.Province) Then
    '    '    MsgBox("Invalid Town and Address!")
    '    '    Return False
    '    'End If

    '    'Add Additional Info To the header
    '    '---------------------------------
    '    If Not AddHeader("VAT REG TIN: " & p_sVATReg) Then
    '        MsgBox("Invalid VAT REG TIN No!")
    '        Return False
    '    End If

    '    If Not AddHeader("MIN : " & p_sPOSNo) Then
    '        MsgBox("Invalid Machine Identification Number(MIN)!")
    '        Return False
    '    End If

    '    If Not AddHeader("Permit #: " & p_sPermit) Then
    '        MsgBox("Invalid Permit No!")
    '        Return False
    '    End If

    '    If Not AddHeader("Serial No.: " & p_sSerial) Then
    '        MsgBox("Invalid Serial No.!")
    '        Return False
    '    End If

    '    Dim loPrint As ggcLRReports.clsDirectPrintSF
    '    loPrint = New ggcLRReports.clsDirectPrintSF
    '    'loPrint.PrintFont = New Font("Courier New", 10)
    '    loPrint.PrintBegin()

    '    Dim lnCtr As Integer
    '    Dim lnRowCtr As Integer = 0
    '    Dim ls4Print As String

    '    'Print the header
    '    For lnCtr = 0 To p_oDTHeader.Rows.Count - 1
    '        loPrint.Print(lnRowCtr, 0, PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40))
    '        lnRowCtr = lnRowCtr + 1
    '    Next

    '    'Print Asterisk(*)
    '    loPrint.Print(lnRowCtr, 0, "*".PadLeft(40, "*"))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print TITLE
    '    ls4Print = "QTY" + " " + "DESCRIPTION".PadLeft(pxeDSCLEN) + " " + "UPRICE".PadLeft(pxePRCLEN) + " " + "AMOUNT".PadLeft(pxeTTLLEN)
    '    loPrint.Print(lnRowCtr, 0, ls4Print)
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Detail of Sales
    '    For lnCtr = 0 To p_oDTDetail.Rows.Count - 1

    '        ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
    '                   UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadLeft(pxeDSCLEN) + " "

    '        If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
    '            ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
    '            ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN) + " "
    '            If p_oDTDetail(lnCtr).Item("cVatablex") Then
    '                ls4Print = ls4Print + "V"
    '            End If
    '        End If
    '        loPrint.Print(lnRowCtr, 0, ls4Print)

    '        lnRowCtr = lnRowCtr + 1
    '    Next

    '    'Print Dash Separator(-)
    '    loPrint.Print(lnRowCtr, 0, "-".PadLeft(40, "-"))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Dash Separator(*)
    '    loPrint.Print(lnRowCtr, 0, " No of Items: " & pnTotalItm)
    '    lnRowCtr = lnRowCtr + 2  'There should be space after this part...

    '    'Print TOTAL Sales
    '    loPrint.Print(lnRowCtr, 0, " TOTAL".PadLeft(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Discounts
    '    If pnDiscAmtV > 0 Then
    '        loPrint.Print(lnRowCtr, 0, " Less: Discount(s)".PadLeft(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN))
    '        lnRowCtr = lnRowCtr + 1

    '        If pnDiscAmtN > 0 Then
    '            loPrint.Print(lnRowCtr, 0, "               VAT".PadLeft(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN))
    '            lnRowCtr = lnRowCtr + 1
    '        End If
    '    ElseIf pnDiscAmtN > 0 Then
    '        loPrint.Print(lnRowCtr, 0, "Less: VAT         ".PadLeft(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN))
    '        lnRowCtr = lnRowCtr + 1
    '    End If

    '    'Print Line before Amount Due
    '    loPrint.Print(lnRowCtr, 0, "                         -------------")
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Amount Due By subracting the discounts
    '    loPrint.Print(lnRowCtr, 0, " Amount Due:              " & Format(pnTotalDue - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Cash Payments
    '    If pnCashTotl > 0 Then
    '        loPrint.Print(lnRowCtr, 0, " Cash                     " & Format(pnCashTotl, xsDECIMAL).PadLeft(pxeREGLEN))
    '        lnRowCtr = lnRowCtr + 1
    '    End If

    '    'Print Credit Card Payments
    '    If p_oDTCredit.Rows.Count > 0 Then
    '        For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
    '            ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
    '                       Format(p_oDTCredit(lnCtr).Item("nCardAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
    '            loPrint.Print(lnRowCtr, 0, ls4Print)
    '            lnRowCtr = lnRowCtr + 1
    '        Next
    '    End If

    '    'Print Check Payments
    '    If p_oDTChkPym.Rows.Count > 0 Then
    '        For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
    '            ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
    '                       Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL).PadLeft(pxeREGLEN)
    '            loPrint.Print(lnRowCtr, 0, ls4Print)
    '            lnRowCtr = lnRowCtr + 1
    '        Next
    '    End If

    '    'Print Gift Coupon
    '    If p_oDTGftChk.Rows.Count > 0 Then
    '        For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
    '            ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce")).PadRight(24) & " " & _
    '                       Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
    '            loPrint.Print(lnRowCtr, 0, ls4Print)
    '            lnRowCtr = lnRowCtr + 1
    '        Next
    '    End If

    '    'Print Line Before change....
    '    loPrint.Print(lnRowCtr, 0, "                         -------------")
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Change
    '    Dim lnChange As Decimal = pnTotalDue - (pnDiscAmtV + pnDiscAmtN)
    '    lnChange = (pnCashTotl + pnChckTotl + pnCrdtTotl + pnGiftTotl) - lnChange
    '    loPrint.Print(lnRowCtr, 0, " CHANGE     :".PadLeft(25) & " " & Format(lnChange, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Discount Information
    '    If p_oDTDiscnt.Rows.Count > 0 Then
    '        If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then
    '            loPrint.Print(lnRowCtr, 0, "///////////////////////////////////////")
    '            lnRowCtr = lnRowCtr + 1

    '            For lnCtr = 0 To p_oDTDiscnt.Rows.Count - 1
    '                'Print Discount Description
    '                loPrint.Print(lnRowCtr, 0, p_oDTDiscnt(lnCtr).Item("sDiscCard"))
    '                lnRowCtr = lnRowCtr + 1

    '                'Print Card Number
    '                loPrint.Print(lnRowCtr, 0, p_oDTDiscnt(lnCtr).Item("sIDNumber"))
    '                lnRowCtr = lnRowCtr + 1

    '            Next
    '        End If
    '    End If

    '    'Print Credit Card Info
    '    If p_oDTCredit.Rows.Count > 0 Then
    '        For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
    '            loPrint.Print(lnRowCtr, 0, "///////////////////////////////////////")
    '            lnRowCtr = lnRowCtr + 1
    '            'Print Credit Card Bank
    '            loPrint.Print(lnRowCtr, 0, p_oDTCredit(lnCtr).Item("sCardBank"))
    '            lnRowCtr = lnRowCtr + 1

    '            'Print Card Number/Should hide entire card number
    '            ls4Print = p_oDTCredit(lnCtr).Item("sCardNoxx")
    '            ls4Print = Left(ls4Print, 5) & "".PadLeft(ls4Print.Length - 9, "*") & Right(ls4Print, 4)
    '            loPrint.Print(lnRowCtr, 0, ls4Print)
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "SWIPED")
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "Approval Code: " & p_oDTCredit(lnCtr).Item("sApprovNo"))
    '            lnRowCtr = lnRowCtr + 1
    '        Next
    '    End If

    '    'Print Check Payment Info
    '    If p_oDTChkPym.Rows.Count > 0 Then
    '        For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
    '            loPrint.Print(lnRowCtr, 0, "///////////////////////////////////////")
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "Check No: " & p_oDTChkPym(lnCtr).Item("sCheckNox"))
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "Bank    : " & p_oDTChkPym(lnCtr).Item("sCheckBnk"))
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "Date:   : " & Format(p_oDTChkPym(lnCtr).Item("dCheckDte"), xsDATE_SHORT))
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "Amount  : " & Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL))
    '            lnRowCtr = lnRowCtr + 1
    '        Next
    '    End If

    '    'Print Dash Separator(-)
    '    loPrint.Print(lnRowCtr, 0, "-".PadLeft(40, "-"))
    '    lnRowCtr = lnRowCtr + 2 'There should be space after this part..

    '    'Compute VAT & and other info
    '    '++++++++++++++++++++++++++++++++++++++
    '    'VAT is 12 % of sales
    '    'TODO: load VAT percent of sales from CONFIG
    '    Dim lnVatPerc As Double = 1.12
    '    pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) / lnVatPerc
    '    pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) - pnVatblSle

    '    'Print VAT Related info
    '    loPrint.Print(lnRowCtr, 0, "  VAT Exempt Sales      " & Format(pnVatExSle, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, "  Zero Rated Sales      " & Format(pnZroRtSle, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, "  VAT Sales             " & Format(pnVatblSle, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, "  VAT Amount            " & Format(pnVatAmntx, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 2 'There should be space after this part..

    '    If psCustName <> "" Then
    '        loPrint.Print(lnRowCtr, 0, " Cust Name: " & psCustName)
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " Address  : " & psCustAddx)
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " TIN #    : " & psCustTINx)
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " Bus Style:" & psCustBusx)
    '        lnRowCtr = lnRowCtr + 2 'There should be space after this part..
    '    Else
    '        loPrint.Print(lnRowCtr, 0, " Cust Name: ----------------------------")
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " Address  : ----------------------------")
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " TIN #    : ----------------------------")
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " Bus Style: ----------------------------")
    '        lnRowCtr = lnRowCtr + 2 'There should be space after this part..
    '    End If

    '    'Print Cashier
    '    loPrint.Print(lnRowCtr, 0, " Cashier: " & "Juan Dela Cruz") 'psCashrNme
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, " Terminal No.: " & p_sTermnl)
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, " OR No.: " & psReferNox)
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, " Date : " & Format(pdTransact, xsDATE_TIME))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Asterisk(*)
    '    loPrint.Print(lnRowCtr, 0, "*".PadLeft(40, "*"))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print the Footer
    '    For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
    '        loPrint.Print(lnRowCtr, 0, PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40))
    '        lnRowCtr = lnRowCtr + 1
    '    Next

    '    loPrint.PrintEnd()

    '    Return True
    'End Function

    Private Function PadCenter(ByVal source As String, ByVal length As Integer) As String
        Dim spaces As Integer = length - source.Length
        Dim padLeft As Integer = spaces / 2 + source.Length
        Return source.PadLeft(padLeft, " ").PadRight(length, " ")
    End Function

    Private Sub createDetail()
        p_oDTDetail = New DataTable("Detail")
        p_oDTDetail.Columns.Add("nQuantity", System.Type.GetType("System.Int16"))
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 14
        p_oDTDetail.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nTotlAmnt", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("cDetailxx", System.Type.GetType("System.String")).MaxLength = 1
        'Consider All Sales to be VATABLE
        p_oDTDetail.Columns.Add("cVatablex", System.Type.GetType("System.String")).MaxLength = 1

        'Complimentary
        p_oDTComplx = New DataTable("Complimentary")
        p_oDTComplx.Columns.Add("nQuantity", System.Type.GetType("System.Int16"))
        p_oDTComplx.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 14
        p_oDTComplx.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))
        p_oDTComplx.Columns.Add("nTotlAmnt", System.Type.GetType("System.Decimal"))
        p_oDTComplx.Columns.Add("cDetailxx", System.Type.GetType("System.String")).MaxLength = 1

        'Consider All Sales to be VATABLE
        p_oDTComplx.Columns.Add("cVatablex", System.Type.GetType("System.String")).MaxLength = 1


        'Header Table
        p_oDTHeader = New DataTable("Header")
        p_oDTHeader.Columns.Add("sHeadName", System.Type.GetType("System.String")).MaxLength = 40

        'Footer Table
        p_oDTFooter = New DataTable("Footer")
        p_oDTFooter.Columns.Add("sFootName", System.Type.GetType("System.String")).MaxLength = 40

        p_oDTDiscnt = New DataTable("Discount")
        p_oDTDiscnt.Columns.Add("sIDNumber", System.Type.GetType("System.String")).MaxLength = 35
        p_oDTDiscnt.Columns.Add("sDiscCard", System.Type.GetType("System.String")).MaxLength = 35
        p_oDTDiscnt.Columns.Add("cNoneVATx", System.Type.GetType("System.String")).MaxLength = 1

        p_oDTDiscnt.Columns.Add("nDiscAmnt", System.Type.GetType("System.Decimal")) 'this is the total discount (discrate + adddisc)
        p_oDTDiscnt.Columns.Add("nDiscRate", System.Type.GetType("System.Decimal")) 'MAC 2018.01.26
        p_oDTDiscnt.Columns.Add("nAddDiscx", System.Type.GetType("System.Decimal")) 'MAC 2018.01.26
        p_oDTDiscnt.Columns.Add("nNoClient", System.Type.GetType("System.Int32")) 'MAC 2018.01.26
        p_oDTDiscnt.Columns.Add("nWithDisc", System.Type.GetType("System.Int32")) 'MAC 2018.01.26
        p_oDTDiscnt.Columns.Add("sClientNm", System.Type.GetType("System.String")).MaxLength = 120 ' Jovan 2021-04-19
    End Sub

    Private Sub createGiftCheck()
        p_oDTGftChk = New DataTable("GiftChec")
        p_oDTGftChk.Columns.Add("nGiftAmnt", System.Type.GetType("System.Decimal"))
        p_oDTGftChk.Columns.Add("sGiftSrce", System.Type.GetType("System.String")).MaxLength = 23
    End Sub

    Private Sub createCheck()
        p_oDTChkPym = New DataTable("Check")
        p_oDTChkPym.Columns.Add("nCheckAmt", System.Type.GetType("System.Decimal"))
        p_oDTChkPym.Columns.Add("sCheckBnk", System.Type.GetType("System.String")).MaxLength = 32
        p_oDTChkPym.Columns.Add("sCheckNox", System.Type.GetType("System.String")).MaxLength = 23
        p_oDTChkPym.Columns.Add("dCheckDte", System.Type.GetType("System.DateTime"))
    End Sub

    Private Sub createCreditCard()
        p_oDTCredit = New DataTable("CreditCard")
        p_oDTCredit.Columns.Add("nCardAmnt", System.Type.GetType("System.Decimal"))
        p_oDTCredit.Columns.Add("sCardBank", System.Type.GetType("System.String")).MaxLength = 32
        p_oDTCredit.Columns.Add("sCardNoxx", System.Type.GetType("System.String")).MaxLength = 23
        p_oDTCredit.Columns.Add("sApprovNo", System.Type.GetType("System.String")).MaxLength = 10
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTMaster = Nothing
        p_oDTDetail = Nothing
        p_oDTComplx = Nothing
        p_oDTChkPym = Nothing
        p_oDTCredit = Nothing
        p_oDTGftChk = Nothing

        p_oDTHeader = Nothing
        p_oDTFooter = Nothing
        p_oDTDiscnt = Nothing

        pbReprint = False

        'Get Cashier Name from GRider
        psCashrNme = p_oApp.UserName

        Call createDetail()
        Call createCheck()
        Call createCreditCard()
        Call createGiftCheck()

        p_sPOSNo = Environment.GetEnvironmentVariable("RMS-CRM-No")      'MIN
        p_sVATReg = Environment.GetEnvironmentVariable("REG-TIN-No")     'VAT REG No.
        p_sCompny = Environment.GetEnvironmentVariable("RMS-CLT-NM")     'Monark 
    End Sub

    'Public Sub testOR()
    '    Dim loReceipt As ggcMiscParam.PRN_Receipt
    '    loReceipt = New ggcMiscParam.PRN_Receipt(p_oAppDriver)
    '    'If loReceipt.InitMachine() Then
    '    'Set Details
    '    loReceipt.AddDetail(2, "123456789012345", 2500, True)
    '    loReceipt.AddDetail(1, "CLUBHSE SANDWCH", 140, True)

    '    loReceipt.CashPayment = 4500
    '    loReceipt.ReferenceNo = "00172015"
    '    loReceipt.NonVatSales = 2500
    '    loReceipt.Transaction_Date = p_oAppDriver.SysDate

    '    If Not loReceipt.PrintCI Then
    '        MsgBox("Can't print OR")
    '        Exit Sub
    '    End If
    '    'End If
    'End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
