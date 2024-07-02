'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     POS Billing Printing
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
' ==========================================================================================
'  iMac [ 02/10/2018 02:25 pm ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

Imports ADODB
Imports ggcAppDriver
Imports System.Drawing

Public Class PRN_Billing
    Private p_oApp As GRider

    Private p_sPOSNo As String      'MIN:       14121419321782091
    Private p_sVATReg As String     'TIN:       941-184-389-000
    Private p_sCompny As String     'Company  : MONARK HOTEL

    Private p_sPermit As String     'Permit No: PR122014-004-D004507-000
    Private p_sSerial As String     'Serial No: L9GF261769
    Private p_sAccrdt As String     'Accrdt No: 038-227471337-000028
    Private p_sTermnl As String     'Termnl No: 02
    Private p_cTrnMde As Char
    Private p_nSCRate As Double
    Private p_dPOSDatex As Date
    Private p_nTableNo As Integer
    Private p_sMergeTb As String

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
    Private psBillNoxx As String        'XXX
    Private psDelivery As String

    Private pnTotalItm As Decimal
    Private pnTotalDue As Decimal
    Private pnDiscAmtV As Decimal
    Private pnDiscAmtN As Decimal

    'jovan 2020-11-07
    Private psCashierNm As String
    Private p_sLogName As String
    Private p_nNoClient As Integer
    Private p_nWithDisc As Integer
    Private p_cSplitTyp As Integer

    'MAC 2018.01.26
    Private pnDiscRteV As Decimal
    Private pnDiscRteN As Decimal
    Private pnAddDiscV As Decimal
    Private pnAddDiscN As Decimal
    Private pnNoClient As Integer
    Private pnWithDisc As Integer
    Private pnSChargex As Decimal

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
    Private pnSplitAmt As Decimal

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

    'jovan 2020-11-07
    Public Property CashierName() As String
        Get
            Return psCashierNm
        End Get
        Set(ByVal value As String)
            psCashierNm = value
        End Set
    End Property
    Public Property Dservice() As String
        Get
            Return psDelivery
        End Get
        Set(ByVal value As String)
            psDelivery = value
        End Set
    End Property

    'jovan 04-28-2021
    Public WriteOnly Property PosDate() As Date
        Set(ByVal Value As Date)
            p_dPOSDatex = Value
        End Set
    End Property

    Public WriteOnly Property BillingNo() As String
        Set(ByVal Value As String)
            psBillNoxx = Value
        End Set
    End Property

    'jovan 3/8/21
    Public Property LogName() As String
        Get
            Return p_sLogName
        End Get
        Set(ByVal value As String)
            p_sLogName = value
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

    Public Property CashPayment() As Decimal
        Get
            Return pnCashTotl
        End Get
        Set(ByVal value As Decimal)
            pnCashTotl = value
        End Set
    End Property

    Public Property ServiceCharge() As Decimal
        Get
            Return pnSChargex
        End Get
        Set(ByVal value As Decimal)
            pnSChargex = value
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

    WriteOnly Property MergeTable As String
        Set(ByVal Value As String)
            p_sMergeTb = Value
        End Set
    End Property

    Public WriteOnly Property SplitAmount() As Decimal
        Set(ByVal Value As Decimal)
            pnSplitAmt = Value
        End Set
    End Property

    Public WriteOnly Property SplitType() As Integer
        Set(ByVal Value As Integer)
            p_cSplitTyp = Value
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
                       ", nSChargex" & _
                       ", cTranMode" & _
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
        p_nSCRate = loDta(0).Item("nSChargex")
        p_cTrnMde = loDta(0).Item("cTranMode")

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
            '.Rows(.Rows.Count - 1).Item("nDiscount") = Discount

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
            '.Rows(.Rows.Count - 1).Item("sClientNm") = sClientNm


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

    Public Function PrintBillneo() As Boolean
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
        builder.Append("BILLING SLIP" & Environment.NewLine)

        If pbReprint Then
            builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
            builder.Append(RawPrint.pxePRINT_CNTR)
            builder.Append(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName") & Environment.NewLine)
        End If

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Cashier
        builder.Append(Environment.NewLine)
        builder.Append(" Billing No.: " & psBillNoxx & Environment.NewLine)
        builder.Append(" Cashier: " & p_sLogName & "/" & psCashierNm & Environment.NewLine)
        If p_nTableNo > 0 Then
            If p_sMergeTb = "" Then
                builder.Append(" Table No.: " & p_nTableNo.ToString.PadLeft(2, "0") & "".PadRight(12) & " " & "DINE-IN".PadLeft(pxeREGLEN) & Environment.NewLine)
            Else
                builder.Append(" Table No.: " & Mid(p_sMergeTb, 1, Len(p_sMergeTb) - 1) & "".PadRight(12) & " " & "DINE-IN".PadLeft(pxeREGLEN) & Environment.NewLine)
            End If
        Else
            builder.Append(" TAKE-OUT " & Environment.NewLine)
        End If

        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" Transaction No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Date : " & pdTransact.Year & "-" & Format(pdTransact.Month, "00") & "-" & Format(pdTransact.Day, "00") & " " & Format(p_oApp.getSysDate, "hh:mm:ss tt") & Environment.NewLine)

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
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN)
                If p_oDTDetail(lnCtr).Item("cVatablex") Then
                    ls4Print = ls4Print
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
            Dim lnVATExclsv = pnTotalDue / lnVatPerc
            Dim lnRateAmntx = lnVATExclsv * (pnDiscRteV / 100)
            Dim lnAddDiscxx = pnAddDiscV / lnVatPerc

            Dim lnAmountDue = pnTotalDue - pnDiscAmtV
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + lnAddDiscxx + pnDiscAmtN)

            Dim lsLess As String = " Less: "
            If pnDiscRteV > 0 Then
                builder.Append(lsLess & p_oDTDiscnt(0).Item("sDiscCard") & Environment.NewLine)
                builder.Append("       " & ("(" & pnDiscRteV & "%)").PadRight(18) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
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
                MsgBox(lnVATablex & vbCrLf &
                        lnExVATDue & vbCrLf &
                        lnDiscAmtN)
                builder.Append(" Total Due".PadRight(25) & " " & Format((lnVATablex + lnExVATDue) - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        Else
            If pnSChargex > 0 Then
                builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            End If
        End If

        'Print Amount Due By subracting the discounts
        builder.Append(" TOTAL AMOUNT DUE".PadRight(25) & " " & Format((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        ''Print Discount Information
        'If Not IsNothing(p_oDTDiscnt) Then
        '    If p_oDTDiscnt.Rows.Count > 0 Then
        '        If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then
        '            builder.Append(Environment.NewLine)
        '            builder.Append("///////////////////////////////////////" & Environment.NewLine)
        '            If InStr(LCase(p_oDTDiscnt(0).Item("sDiscCard")), "sc", CompareMethod.Text) <> 0 Then
        '                If pnDiscAmtN > 0 And pnNoClient > 0 Then
        '                    builder.Append("SENIOR/PWD INFORMATION" & Environment.NewLine)
        '                End If
        '            End If
        '            'add name and signature field
        '            builder.Append("ID No: " & p_oDTDiscnt(0).Item("sIDNumber") & Environment.NewLine)
        '            builder.Append("Name: " & p_oDTDiscnt(0).Item("sClientNm") & Environment.NewLine)
        '            builder.Append("Signature:______________________________" & Environment.NewLine)

        '        End If
        '    End If
        'End If

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        'Print the Footer
        builder.Append(PadCenter("THIS DOCUMENT IS NOT", 40) & Environment.NewLine)
        builder.Append(PadCenter("VALID FOR CLAIM OF INPUT TAX", 40) & Environment.NewLine)

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim cashier_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_CS")
        'Dim cashier_printer As String = "\\192.168.10.12\EPSON TM-U220 Receipt"

        'Print the designation printer location...
        RawPrint.SendStringToPrinter(cashier_printer, builder.ToString())

        'Call writeBill()

        Return True
    End Function

    Public Function PrintBill() As Boolean
        Dim lnVatPerc As Double = 1.12
        Dim lnTotalDue As Decimal = 0
        If Not AddHeader("REPRINT") Then
            MsgBox("Unable to Reprint!")
            Return False
        End If


        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append(Environment.NewLine)

        builder.Append("BILLING SLIP" & Environment.NewLine)

        If pbReprint Then
            builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
            builder.Append(RawPrint.pxePRINT_CNTR)
            builder.Append(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName") & Environment.NewLine)
        End If

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Cashier
        builder.Append(Environment.NewLine)
        builder.Append(" Billing No.: " & psBillNoxx & Environment.NewLine)
        builder.Append(" Transaction No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Date : " & pdTransact.Year & "-" & Format(pdTransact.Month, "00") & "-" & Format(pdTransact.Day, "00") & " " & Format(p_oApp.getSysDate, "hh:mm:ss tt") & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
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

        ''Print Dash Separator(-)
        'builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

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
        'If pnSChargex > 0 Or pnDiscAmtN > 0 And pnDiscAmtV > 0 Then
        builder.Append(" Sub-Total".PadRight(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'End If

            builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        Dim lnExVATDue = pnTotalDue / 1.12
        Dim lnAddVATAmt As Decimal = 0
        'Print Discounts
        If pnDiscAmtV > 0 Then
            'builder.Append(" Less: Discount(s)".PadRight(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Dim lnVATExclsv = pnTotalDue / lnVatPerc
            Dim lnRateAmntx = lnVATExclsv * (pnDiscRteV / 100)
            Dim lnAddDiscxx = pnAddDiscV / lnVatPerc

            Dim lnAmountDue = pnTotalDue - pnDiscAmtV
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + lnAddDiscxx + pnDiscAmtN)

            Dim lsLess As String = " Less: "

            builder.Append(" Net of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            If pnDiscRteV > 0 Then
                builder.Append((lsLess & Math.Round(pnDiscRteV) & "% Discount").PadRight(25) & " " & Format(lnRateAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnAddDiscV > 0 Then
                builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            builder.Append(" Net Sales".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            lnAddVATAmt = (lnExVATDue * 0.12)

            If lnAddVATAmt > 0 Then
                builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnAddVATAmt, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            lnTotalDue = lnExVATDue + lnAddVATAmt
            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

            ElseIf pnDiscAmtN > 0 Then
                'orig code
                'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

                Dim lnVATablex As Decimal = 0
                Dim lnNVATable As Decimal = 0
                Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, lnNVATable)

                lnAddVATAmt = (((lnExVATDue / pnNoClient) * (pnNoClient - pnWithDisc)) * 0.12)

                builder.Append(" Net of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

                lnVATablex = Format(lnVATablex, xsDECIMAL)
                lnExVATDue = Format(lnExVATDue, xsDECIMAL)
                lnDiscAmtN = Format(lnDiscAmtN, xsDECIMAL)

                lnTotalDue = lnExVATDue + lnAddVATAmt
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
                builder.Append(" Net Sales".PadRight(25) & " " & Format(lnExVATDue - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                If lnAddVATAmt > 0 Then
                    builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnAddVATAmt, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                End If

            Else
                Dim lnVATablex As Decimal = 0
            Dim lnNVATable As Decimal = 0

            lnVATablex = Format(lnVATablex, xsDECIMAL)
            lnExVATDue = Format(lnExVATDue, xsDECIMAL)

            lnAddVATAmt = (lnExVATDue * 0.12)
            lnTotalDue = lnExVATDue + lnAddVATAmt
            builder.Append(" Net Sales".PadRight(25) & " " & lnExVATDue.ToString.PadLeft(pxeREGLEN) & Environment.NewLine)
            If lnAddVATAmt > 0 Then
                builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnAddVATAmt, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If


        End If

            If pnSChargex > 0 Then
            builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        End If
        'Print Amount Due By subracting the discounts


        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        builder.Append(" TOTAL AMOUNT DUE".PadRight(25) & " " & Format((lnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        If pnSplitAmt > 0 Then
            If p_cSplitTyp <> 2 Then
                builder.Append(" Partial Bill".PadRight(25) & " " & Format(pnSplitAmt, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If
        End If

        builder.Append(RawPrint.pxePRINT_EMP0)
        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine & Environment.NewLine)

        pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        If Not (p_oApp.BranchCode = "P013") Then
            'Print the Footer
            builder.Append(PadCenter("THIS DOCUMENT IS NOT", 40) & Environment.NewLine)
            builder.Append(PadCenter("VALID FOR CLAIM OF INPUT TAX", 40) & Environment.NewLine)
            builder.Append(PadCenter("PLEASE DEMAND FOR YOUR SALES INVOICE", 40) & Environment.NewLine)
        End If

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim cashier_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_CS")
        'Dim cashier_printer As String = "\\192.168.10.12\EPSON TM-U220 Receipt"

        'Print the designation printer location...
        RawPrint.SendStringToPrinter(cashier_printer, builder.ToString())

        Call writeBill()

        Return True
    End Function

    Private Function writeBill() As Boolean
        Dim lnVatPerc As Double = 1.12
        Dim lnTotalDue As Decimal = 0

        If Not AddHeader("REPRINT") Then
            MsgBox("Unable to Reprint!")
            Return False
        End If


        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append(Environment.NewLine)

        builder.Append("BILLING SLIP" & Environment.NewLine)

        If pbReprint Then
            builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
            builder.Append(RawPrint.pxePRINT_CNTR)
            builder.Append(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName") & Environment.NewLine)
        End If

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Cashier
        builder.Append(Environment.NewLine)
        builder.Append(" Billing No.: " & psBillNoxx & Environment.NewLine)
        builder.Append(" Transaction No.: " & psReferNox & Environment.NewLine)
        builder.Append(" Date : " & pdTransact.Year & "-" & Format(pdTransact.Month, "00") & "-" & Format(pdTransact.Day, "00") & " " & Format(p_oApp.getSysDate, "hh:mm:ss tt") & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(ls4Print & Environment.NewLine)

        'Print Detail of Sales
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnCtr).Item("nQuantity") > 0 Then
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                            UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
            Else
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " +
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

                ls4Print = Format(p_oDTComplx(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                           UCase(p_oDTComplx(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        ''Print Dash Separator(-)
        'builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

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
        'If pnSChargex > 0 Or pnDiscAmtN > 0 And pnDiscAmtV > 0 Then
        builder.Append(" Sub-Total".PadRight(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        'End If

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        Dim lnExVATDue = pnTotalDue / 1.12
        Dim lnAddVATAmt As Decimal = 0
        'Print Discounts
        If pnDiscAmtV > 0 Then
            'builder.Append(" Less: Discount(s)".PadRight(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Dim lnVATExclsv = pnTotalDue / lnVatPerc
            Dim lnRateAmntx = lnVATExclsv * (pnDiscRteV / 100)
            Dim lnAddDiscxx = pnAddDiscV / lnVatPerc

            Dim lnAmountDue = pnTotalDue - pnDiscAmtV
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + lnAddDiscxx + pnDiscAmtN)

            Dim lsLess As String = " Less: "

            builder.Append(" Net of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            If pnDiscRteV > 0 Then
                builder.Append((lsLess & Math.Round(pnDiscRteV) & "% Discount").PadRight(25) & " " & Format(lnRateAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            If pnAddDiscV > 0 Then
                builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            builder.Append(" Net Sales".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnExVATDue * 0.12, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            lnAddVATAmt = (lnExVATDue * 0.12)

            lnTotalDue = lnExVATDue + lnAddVATAmt
            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

        ElseIf pnDiscAmtN > 0 Then
            'orig code
            'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lnVATablex As Decimal = 0
            Dim lnNVATable As Decimal = 0
            Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, lnNVATable)

            lnAddVATAmt = (((lnExVATDue / pnNoClient) * (pnNoClient - pnWithDisc)) * 0.12)

            builder.Append(" Net of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            lnVATablex = Format(lnVATablex, xsDECIMAL)
            lnExVATDue = Format(lnExVATDue, xsDECIMAL)
            lnDiscAmtN = Format(lnDiscAmtN, xsDECIMAL)

            lnTotalDue = lnExVATDue + lnAddVATAmt
            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            builder.Append(" Net Sales".PadRight(25) & " " & Format(lnExVATDue - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            If lnAddVATAmt > 0 Then
                builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnAddVATAmt, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

        Else
            Dim lnVATablex As Decimal = 0
            Dim lnNVATable As Decimal = 0

            lnVATablex = Format(lnVATablex, xsDECIMAL)
            lnExVATDue = Format(lnExVATDue, xsDECIMAL)
            lnTotalDue = lnExVATDue + lnAddVATAmt
            lnAddVATAmt = (lnExVATDue * 0.12)
            builder.Append(" Net Sales".PadRight(25) & " " & lnExVATDue.ToString.PadLeft(pxeREGLEN) & Environment.NewLine)
            If lnAddVATAmt > 0 Then
                builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnAddVATAmt, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If


        End If

        If pnSChargex > 0 Then
            builder.Append(" Service Charge(" & p_nSCRate & "%)".PadRight(8) & " " & Format(pnSChargex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        End If
        'Print Amount Due By subracting the discounts


        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        builder.Append(" TOTAL AMOUNT DUE".PadRight(25) & " " & Format((lnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        If pnSplitAmt > 0 Then
            If p_cSplitTyp <> 2 Then
                builder.Append(" Partial Bill".PadRight(25) & " " & Format(pnSplitAmt, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If
        End If

        builder.Append(RawPrint.pxePRINT_EMP0)
        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine & Environment.NewLine)

        pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        If Not (p_oApp.BranchCode = "P013") Then
            'Print the Footer
            builder.Append(PadCenter("THIS DOCUMENT IS NOT", 40) & Environment.NewLine)
            builder.Append(PadCenter("VALID FOR CLAIM OF INPUT TAX", 40) & Environment.NewLine)
            builder.Append(PadCenter("PLEASE DEMAND FOR YOUR SALES INVOICE", 40) & Environment.NewLine)
        End If

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))
        builder.Append(Environment.NewLine)
        builder.Append(PadCenter("----- END OF SALES INVOICE -----", 40) & Environment.NewLine)
        RawPrint.writeToFile(p_sPOSNo, builder.ToString())
        RawPrint.writeToFile(p_sPOSNo & " " & Format(p_dPOSDatex, "yyyyMMdd"), builder.ToString())

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
        p_oDTDetail.Columns.Add("nDiscount", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nAddDiscx", System.Type.GetType("System.Decimal"))
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

    Private Function getSplitTable(ByVal fsSourceNo As String) As DataTable
        Dim loDT As DataTable

        loDT = p_oApp.ExecuteQuery("SELECT" & _
                                        "  b.sORNumber" & _
                                        ", b.nSalesAmt" & _
                                        ", a.cTranStat" & _
                                    " FROM Order_Split a" & _
                                        " LEFT JOIN Receipt_Master b" & _
                                            " ON a.sTransNox = b.sSourceNo" & _
                                            " AND b.sSourceCd = 'SOSp'" & _
                                    " WHERE a.sReferNox = " & strParm(fsSourceNo) & _
                                    " ORDER BY b.sORNumber" & _
                                        ", a.sTransNox")
        Return loDT
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
