'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Receipt
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
'
' ==========================================================================================
'  Jheff [ 10/12/2016 02:58 pm ]
'     Start coding this object...
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
#Disable Warning BC40056 ' Namespace or type specified in Imports statement doesn't contain any public member or cannot be found
Imports ggcAppDriver
#Enable Warning BC40056 ' Namespace or type specified in Imports statement doesn't contain any public member or cannot be found
#Disable Warning BC40056 ' Namespace or type specified in Imports statement doesn't contain any public member or cannot be found
Imports ggcRetailParams
#Enable Warning BC40056 ' Namespace or type specified in Imports statement doesn't contain any public member or cannot be found
Imports MySql.Data.MySqlClient
Imports System.Windows.Forms
Imports System.Reflection
Imports System.Drawing.Printing

Public Class Receipt

#Region "Constant"
    Private Const pxeMODULENAME As String = "Receipt"
    Private Const pxeMasterTble As String = "Receipt_Master"
    Private Const xsSignature As String = "08220326"
#End Region

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oDataTable As DataTable
    Protected p_oCheck As CheckPayment
    Protected p_oCreditCard As CreditCard
    Protected p_oGiftCert As GiftCerticate
    Protected p_oDelivery As Delivery
    Protected p_oPayment As Payment
    Protected p_nEditMode As xeEditMode

    Protected p_oFormPay As frmPay
    Protected p_oFormCreditCard As frmPayCreditNeo

    Protected p_oDtaOrder As DataTable
    Protected p_oDtaDiscx As DataTable

    Protected p_sBranchCd As String
    Protected p_bCancelled As Boolean
    Protected p_sMasterNo As String
    Protected p_sSourceNo As String
    Protected p_sSourceCd As String
    Protected p_sPOSNo As String
    Protected p_sCRMNmbr As String
    Protected p_sSerial As String
    Protected p_cTrnMde As Char
    Protected p_sACCNox As Char
    Protected p_dACCFrm As Date
    Protected p_dACCTru As Date
    Protected p_sPTUNox As String
    Protected p_dPTUFrm As Date
    Protected p_dPTUTru As Date
    Protected p_cSplitTyp As Integer
    Protected p_sSplitSrc As String

    Protected p_sControlNo As String
    Protected p_sBillingNo As String

    Protected p_nNonVATxx As Decimal
    Protected p_nDiscAmtx As Decimal


    Protected p_sCashierx As String
    Protected p_dPOSDatex As Date
    Protected p_sMergeTbl As String
#End Region

    'jovan added this private variable to print at recepiit
    Private p_nWithDisc As Integer
    Private p_nNoClient As Integer
    Private p_nTableNo As Integer
    Private p_sTrantype As String
    Private p_sLogName As String
    Private pnBill As Decimal

    Private pnBillSplitted As Decimal
    Private pnCharge As Decimal

#Region "Properties"

    Property TableNo As Integer
        Get
            Return p_nTableNo
        End Get
        Set(ByVal Value As Integer)
            p_nTableNo = Value
        End Set
    End Property
    Property TranType As String
        Get
            Return p_sTrantype
        End Get
        Set(ByVal Value As String)
            p_sTrantype = Value
        End Set
    End Property
    Property myBill As Double
        Get
            Return pnBill
        End Get
        Set(ByVal Value As Double)
            pnBill = Value
        End Set
    End Property
    Property myCharge As Double
        Get
            Return pnCharge
        End Get
        Set(ByVal Value As Double)
            pnCharge = Value
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


    ReadOnly Property AppDriver As GRider
        Get
            Return p_oAppDrvr
        End Get
    End Property

    ReadOnly Property CashAmount As Decimal
        Get
            Return p_nCash
        End Get
    End Property

    ReadOnly Property CreditCardAmount As Decimal
        Get
            Return p_nCreditCard
        End Get
    End Property

    ReadOnly Property CheckAmount As Decimal
        Get
            Return p_nCheck
        End Get
    End Property

    ReadOnly Property GCAmount As Decimal
        Get
            Return p_nGiftCert
        End Get
    End Property
    ReadOnly Property DSAmount As Decimal
        Get
            Return p_nDelivery
        End Get
    End Property

    Property Branch() As String
        Get
            Return p_sBranchCd
        End Get
        Set(ByVal Value As String)
            p_sBranchCd = Value
        End Set
    End Property

    Property SourceNo As String
        Set(ByVal Value As String)
            p_sSourceNo = Value
        End Set
        Get
            Return p_sSourceNo
        End Get
    End Property

    WriteOnly Property LogName As String
        Set(ByVal value As String)
            p_sLogName = value
        End Set
    End Property

    WriteOnly Property BillingNo As String
        Set(ByVal Value As String)
            p_sBillingNo = Value
        End Set
    End Property

    WriteOnly Property MasterNo As String
        Set(ByVal Value As String)
            p_sMasterNo = Value
        End Set
    End Property

    Property SourceCd As String
        Set(ByVal Value As String)
            p_sSourceCd = Value
        End Set
        Get
            Return p_sSourceCd
        End Get
    End Property

    WriteOnly Property ControlNo As String
        Set(ByVal Value As String)
            p_sControlNo = Value
        End Set
    End Property

    WriteOnly Property SerialNo As String
        Set(ByVal value As String)
            p_sSerial = value
        End Set
    End Property

    WriteOnly Property Cashier As String
        Set(ByVal Value As String)
            p_sCashierx = Value
        End Set
    End Property

    Property POSNumbr As String
        Get
            Return p_sPOSNo
        End Get
        Set(ByVal Value As String)
            p_sPOSNo = Value
        End Set
    End Property

    WriteOnly Property CRMNumbr As String
        Set(ByVal Value As String)
            p_sCRMNmbr = Value
        End Set
    End Property

    WriteOnly Property TranMode As Char
        Set(ByVal Value As Char)
            p_cTrnMde = Value
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

    WriteOnly Property AccrdThru As Date
        Set(ByVal Value As Date)
            p_dACCTru = Value
        End Set
    End Property

    WriteOnly Property PTUNumber As String
        Set(ByVal Value As String)
            p_sPTUNox = Value
        End Set
    End Property

    WriteOnly Property PTUFrom As Date
        Set(ByVal Value As Date)
            p_dPTUFrm = Value
        End Set
    End Property

    WriteOnly Property PTUThru As Date
        Set(ByVal Value As Date)
            p_dPTUTru = Value
        End Set
    End Property

    WriteOnly Property SalesOrder() As DataTable
        Set(ByVal oData As DataTable)
            p_oDtaOrder = oData
        End Set
    End Property

    Public Property Discounts() As DataTable
        Get
            Return p_oDtaDiscx
        End Get
        Set(ByVal oData As DataTable)
            p_oDtaDiscx = oData
        End Set

    End Property

    WriteOnly Property DiscAmount() As Decimal
        Set(ByVal value As Decimal)
            p_nDiscAmtx = value
        End Set
    End Property

    WriteOnly Property NonVAT As Decimal
        Set(ByVal value As Decimal)
            p_nNonVATxx = value
        End Set
    End Property

    WriteOnly Property PosDate() As Date
        Set(ByVal Value As Date)
            p_dPOSDatex = Value
        End Set
    End Property

    WriteOnly Property MergeTable() As String
        Set(ByVal Value As String)
            p_sMergeTbl = Value
        End Set
    End Property

    Public WriteOnly Property SplitType() As Integer
        Set(ByVal Value As Integer)
            p_cSplitTyp = Value
        End Set
    End Property

    Public WriteOnly Property SplitSource() As String
        Set(ByVal Value As String)
            p_sSplitSrc = Value
        End Set
    End Property

    Property Master(ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "dtransact" : Index = 1
                    Case "sornumber" : Index = 2
                    Case "nsalesamt" : Index = 3
                    Case "nvatsales" : Index = 4
                    Case "nvatamtxx" : Index = 5
                    Case "ndiscount" : Index = 6
                    Case "ntendered" : Index = 7
                    Case "ncashamtx" : Index = 8
                    Case "ssourcecd" : Index = 9
                    Case "ssourceno" : Index = 10
                    Case "nvatdiscx" : Index = 11
                    Case "npwddiscx" : Index = 12
                    Case "scashierx" : Index = 13
                    Case "ctranstat" : Index = 14
                    Case "nschargex" : Index = 15
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oDataTable(0)(Index)
        End Get

        Set(ByVal Value As Object)
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "dtransact" : Index = 1
                        If Not IsDate(Value) Then Value = p_oAppDrvr.SysDate
                    Case "sornumber" : Index = 2
                    Case "nsalesamt" : Index = 3
                        p_nSalesAmt = Value
                    Case "nvatsales" : Index = 4
                    Case "nvatamtxx" : Index = 5
                    Case "ndiscount" : Index = 6
                        p_nDiscount = Value
                    Case "ntendered" : Index = 7
                    Case "ncashamtx" : Index = 8
                    Case "ssourcecd" : Index = 9
                    Case "ssourceno" : Index = 10
                    Case "nvatdiscx" : Index = 11
                    Case "npwddiscx" : Index = 12
                    Case "scashierx" : Index = 13
                    Case "ctranstat" : Index = 14
                    Case "nschargex" : Index = 15
                        p_nSchargex = Value
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                End Select
            End If
            p_oDataTable(0)(Index) = Value
        End Set
    End Property

#End Region

#Region "Public Function"
    Function payTransaction() As Boolean
        Dim lbSuccess As Boolean

        lbSuccess = SaveTransaction()
        'MsgBox("Save Receipt")

        If lbSuccess Then
            CheckPrinter("EPSON TM-U220 Receipt")
            If (p_oAppDrvr.BranchCode = "P013") Then
                Dim lnRep As Integer
                lnRep = MsgBox("Do you want to print Bill?", vbQuestion & vbYesNo, "CONFIRMATION")
                If lnRep = vbNo Then Return False
                printBilling()
            Else
                printReciept()
            End If

        End If
            'MsgBox("Print Receipt")

            Return lbSuccess
    End Function

    Function printBilling(Optional ByVal bReprint As Boolean = False) As Boolean
        Dim lnCtr As Integer
        Dim loPrint As PRN_Billing

        loPrint = New PRN_Billing(p_oAppDrvr)

        With loPrint
            If Not .InitMachine Then
                Return False
            End If

            If CDate(Format(p_oDataTable.Rows(0)("dTransact"), xsDATE_SHORT)) < CDate(Format(p_oAppDrvr.getSysDate, xsDATE_SHORT)) Then
                .Transaction_Date = CDate(Format(p_oDataTable.Rows(0)("dTransact"), xsDATE_SHORT))
            Else
                .Transaction_Date = p_oAppDrvr.getSysDate
            End If

            '.ReferenceNo = p_sControlNo.PadLeft(10, "0")
            .ReferenceNo = p_sMasterNo
            .BillingNo = p_sBillingNo
            .Reprint = bReprint
            .CashierName = getCashier(p_oAppDrvr.UserID)
            Debug.Print(p_oDataTable.Rows(0)("nSChargex"))
            .ServiceCharge = p_oDataTable.Rows(0)("nSChargex")
            .LogName = p_sLogName
            .PosDate = p_dPOSDatex
            .SplitType = p_cSplitTyp

            If Not IsNothing(p_oDtaOrder) Then
                Dim lnSlPrc As Double
                Dim lnComplmnt As Integer = 0
                Dim lnQuantity As Integer = 0
                For lnCtr = 0 To p_oDtaOrder.Rows.Count - 1
                    'Get compliment for the master item
                    If p_oDtaOrder(lnCtr)("cDetailxx") = xeLogical.NO Then
                        lnComplmnt = p_oDtaOrder(lnCtr)("nComplmnt")
                        lnQuantity = p_oDtaOrder(lnCtr)("nQuantity")
                    End If

                    'Do not include REVERSE(D) orders here...
                    If p_oDtaOrder(lnCtr)("cReversed") = xeLogical.NO Then
                        'Compute unit price here...
                        lnSlPrc = (p_oDtaOrder(lnCtr).Item("nUnitPrce") *
                                    (100 - p_oDtaOrder(lnCtr).Item("nDiscount")) / 100 -
                                    p_oDtaOrder(lnCtr).Item("nAddDiscx"))

                        Dim lnDiv As Double
                        lnDiv = p_oDtaOrder(lnCtr)("nQuantity") / lnQuantity

                        If lnQuantity - lnComplmnt > 0 Then
                            .AddDetail(lnDiv * (lnQuantity - lnComplmnt),
                                       p_oDtaOrder(lnCtr)("sBriefDsc"),
                                       lnSlPrc,
                                       True,
                                       p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                                       IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False))
                        End If

                        If lnComplmnt > 0 Then
                            .AddComplement(lnDiv * lnComplmnt,
                                       p_oDtaOrder(lnCtr)("sBriefDsc"),
                                       0,
                                       True, IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False))
                        End If
                    Else
                        'kalyptus - 2017.01.27 09:42am
                        'Print reverse items
                        If p_oDtaOrder(lnCtr)("cReversex") = "+" Then
                            'If p_oDtaOrder(lnCtr)("cPrintedx") = "0" Then
                            '    .AddDetail(p_oDtaOrder(lnCtr)("nQuantity") * -1,
                            '           "Void-" & p_oDtaOrder(lnCtr)("sBriefDsc"),
                            '           0,
                            '           True,
                            '           p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                            '           IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False),
                            '           p_oDtaOrder(lnCtr)("cWthPromo") = "1", p_oDtaOrder(lnCtr)("nDiscount"), p_oDtaOrder(lnCtr)("nAddDiscx"))
                            'Else
                            'Compute unit price here...
                            lnSlPrc = (p_oDtaOrder(lnCtr).Item("nUnitPrce") *
                                            (100 - p_oDtaOrder(lnCtr).Item("nDiscount")) / 100 -
                                            p_oDtaOrder(lnCtr).Item("nAddDiscx"))

                            Dim lnDiv As Double
                            lnDiv = p_oDtaOrder(lnCtr)("nQuantity") / lnQuantity

                            If lnQuantity - lnComplmnt > 0 Then
                                .AddDetail(lnDiv * (lnQuantity - lnComplmnt),
                                               "*" & p_oDtaOrder(lnCtr)("sBriefDsc"),
                                               p_oDtaOrder(lnCtr)("nUnitPrce"),
                                               True,
                                               p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                                               IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False))
                            End If
                            'End If
                        Else
                            .AddDetail(p_oDtaOrder(lnCtr)("nQuantity") * -1,
                                   "Void-" & p_oDtaOrder(lnCtr)("sBriefDsc"),
                                    p_oDtaOrder(lnCtr)("nUnitPrce"),
                                   True,
                                   p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                                   IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False))
                        End If
                    End If
                Next
                .SplitAmount = IFNull(p_oDtaOrder.Rows(0)("nAmountxx"), 0)
            End If
            'End If

            Dim loDR As DataRow
            Dim loDiscCard As clsDiscountCards
            loDiscCard = New clsDiscountCards(p_oAppDrvr)

            If Not IsNothing(p_oDtaDiscx) Then
                'loDR = loDiscCard.SearchCard(p_oDtaDiscx(0)("sCategrID"), True) sCardIDxx
                loDR = loDiscCard.SearchCard(p_oDtaDiscx(0)("sCardIDxx"), True)

                If Not IsNothing(loDR) Then
                    '.AddDiscount(p_oDtaDiscx(0)("sIDNumber"), _
                    '             loDR("sCardDesc"), _
                    '             p_nDiscAmtx, IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True))

                    'MAC

                    .AddDiscount(p_oDtaDiscx(0)("sIDNumber"),
                                 loDR("sCardDesc"),
                                 p_oDtaDiscx(0)("nDiscRate"),
                                 p_oDtaDiscx(0)("nDiscAmtx"),
                                 p_nDiscAmtx,
                                 IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True),
                                 p_oDtaDiscx(0)("nNoClient"),
                                 p_oDtaDiscx(0)("nWithDisc"),
                                 p_oDtaDiscx(0)("sClientNm"))
                Else
                    '.AddDiscount("", _
                    '             "", _
                    '             p_nDiscAmtx, IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True))

                    'MAC
                    .AddDiscount("",
                                 "",
                                 p_oDtaDiscx(0)("nDiscRate"),
                                 p_oDtaDiscx(0)("nDiscAmtx"),
                                 p_nDiscAmtx,
                                 IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True))
                End If
            End If

            .NonVatSales = p_nNonVATxx
            .CashPayment = p_oDataTable(0)("nTendered")

            Select Case p_cTrnMde
                Case "A"
                    .AddFooter("This serves as your SALES INVOICE.")
                Case "D"
                    .AddFooter("This is not an SALES INVOICE.")
            End Select
            .AddFooter("Thank you, and please come again.")
            .AddFooter("")

            .AddFooter("RMJ Business Solution")
            .AddFooter("32 Pogo Grande")
            .AddFooter("Dagupan City, Pangasinan 2400")
            .AddFooter("NON VAT REG TIN #: 942-188-655-00000")
            .AddFooter("ACCR No.: 0049421886552021051421")
            .AddFooter("ACCR Validity: 2021/07/28 - 2026/07/28")
            .AddFooter("PTU No.: FP072021-004-0296573-00002")
            .AddFooter("PTU Validity: 2021/08/03 - 2026/08/03")

            .AddFooter("")
            .AddFooter("THIS DOCUMENT SHALL BE VALID")
            .AddFooter("FOR FIVE(5) YEARS FROM THE DATE OF")
            .AddFooter("THE PERMIT TO USE")
            .AddFooter("THIS DOCUMENT IS NOT VALID")
            .AddFooter("FOR CLAIM OF INPUT TAX")

            Return .PrintBill()
        End With
    End Function

    Function printCancelled(ByVal sSourceNo As String,
                            Optional ByVal bReprint As Boolean = False) As Boolean
        Dim lnCtr As Integer
        Dim loPrint As PRN_CancelledReceipt

        loPrint = New PRN_CancelledReceipt(p_oAppDrvr)

        With loPrint
            If Not .InitMachine Then Return False

            If CDate(Format(p_oDataTable.Rows(0)("dTransact"), xsDATE_SHORT)) < CDate(Format(p_oAppDrvr.getSysDate, xsDATE_SHORT)) Then
                .Transaction_Date = CDate(Format(p_oDataTable.Rows(0)("dTransact"), xsDATE_SHORT))
            Else
                .Transaction_Date = p_oAppDrvr.getSysDate
            End If

            .ReferenceNo = p_oDataTable(0)("sORNumber")
            .SourceNo = sSourceNo
            .TrasactionNo = p_sMasterNo
            .Reprint = bReprint
            .CashierName = getCashier(p_oAppDrvr.UserID)
            .LogName = p_sLogName

            Debug.Print(p_oDataTable.Rows(0)("nSChargex"))
            .ServiceCharge = p_oDataTable.Rows(0)("nSChargex")
            .ClientNo = p_nNoClient
            .WithDisc = p_nWithDisc
            .TableNo = p_nTableNo
            .PosDate = p_dPOSDatex
            .MergeTable = p_sMergeTbl
            .SplitType = p_cSplitTyp

            If Not IsNothing(p_oDtaOrder) Then
                Dim lnSlPrc As Double
                Dim lnComplmnt As Integer = 0
                Dim lnQuantity As Integer = 0
                For lnCtr = 0 To p_oDtaOrder.Rows.Count - 1
                    'Get compliment for the master item
                    If p_oDtaOrder(lnCtr)("cDetailxx") = xeLogical.NO Then
                        lnComplmnt = p_oDtaOrder(lnCtr)("nComplmnt")
                        lnQuantity = p_oDtaOrder(lnCtr)("nQuantity")
                    End If

                    'Do not include REVERSE(D) orders here...
                    If p_oDtaOrder(lnCtr)("cReversed") = xeLogical.NO Then
                        'Compute unit price here...
                        lnSlPrc = (p_oDtaOrder(lnCtr).Item("nUnitPrce") *
                                    (100 - p_oDtaOrder(lnCtr).Item("nDiscount")) / 100 -
                                    p_oDtaOrder(lnCtr).Item("nAddDiscx"))

                        Dim lnDiv As Double
                        lnDiv = p_oDtaOrder(lnCtr)("nQuantity") / lnQuantity

                        If lnQuantity - lnComplmnt > 0 Then
                            .AddDetail(lnDiv * (lnQuantity - lnComplmnt),
                                       p_oDtaOrder(lnCtr)("sBriefDsc"),
                                       p_oDtaOrder(lnCtr)("nUnitPrce"),
                                       True,
                                       p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                                       IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False),
                                       p_oDtaOrder(lnCtr)("cWthPromo") = "1", p_oDtaOrder(lnCtr)("nDiscount"), p_oDtaOrder(lnCtr)("nAddDiscx"))
                        End If

                        If lnComplmnt > 0 Then
                            .AddComplement(lnDiv * lnComplmnt,
                                       p_oDtaOrder(lnCtr)("sBriefDsc"),
                                       0,
                                       True, IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False))
                        End If
                    Else
                        'kalyptus - 2017.01.27 09:42am
                        'Print reverse items
                        If p_oDtaOrder(lnCtr)("cReversex") = "+" Then
                            If p_oDtaOrder(lnCtr)("cPrintedx") = "0" Then
                                .AddDetail(p_oDtaOrder(lnCtr)("nQuantity") * -1,
                                       "Void-" & p_oDtaOrder(lnCtr)("sBriefDsc"),
                                       0,
                                       True,
                                       p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                                       IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False),
                                       p_oDtaOrder(lnCtr)("cWthPromo") = "1", p_oDtaOrder(lnCtr)("nDiscount"), p_oDtaOrder(lnCtr)("nAddDiscx"))
                            Else
                                'Compute unit price here...
                                lnSlPrc = (p_oDtaOrder(lnCtr).Item("nUnitPrce") *
                                            (100 - p_oDtaOrder(lnCtr).Item("nDiscount")) / 100 -
                                            p_oDtaOrder(lnCtr).Item("nAddDiscx"))

                                Dim lnDiv As Double
                                lnDiv = p_oDtaOrder(lnCtr)("nQuantity") / lnQuantity

                                If lnQuantity - lnComplmnt > 0 Then
                                    .AddDetail(lnDiv * (lnQuantity - lnComplmnt),
                                               "*" & p_oDtaOrder(lnCtr)("sBriefDsc"),
                                               p_oDtaOrder(lnCtr)("nUnitPrce"),
                                               True,
                                               p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                                               IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False),
                                               p_oDtaOrder(lnCtr)("cWthPromo") = "1", p_oDtaOrder(lnCtr)("nDiscount"), p_oDtaOrder(lnCtr)("nAddDiscx"))
                                End If
                            End If
                        Else
                            .AddDetail(p_oDtaOrder(lnCtr)("nQuantity") * -1,
                                   "Void-" & p_oDtaOrder(lnCtr)("sBriefDsc"),
                                    p_oDtaOrder(lnCtr)("nUnitPrce"),
                                   True,
                                   p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                                   IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False),
                                   p_oDtaOrder(lnCtr)("cWthPromo") = "1", p_oDtaOrder(lnCtr)("nDiscount"), p_oDtaOrder(lnCtr)("nAddDiscx"))
                        End If
                    End If
                Next
            End If
            'End If

            Dim loDR As DataRow
            Dim loDiscCard As clsDiscountCards
            loDiscCard = New clsDiscountCards(p_oAppDrvr)

            If Not IsNothing(p_oDtaDiscx) Then
                'loDR = loDiscCard.SearchCard(p_oDtaDiscx(0)("sDiscCard"), True)
                loDR = loDiscCard.SearchCard(p_oDtaDiscx(0)("sCardIDxx"), True)

                If Not IsNothing(loDR) Then
                    .AddDiscount(p_oDtaDiscx(0)("sIDNumber"),
                                 loDR("sCardDesc"),
                                 p_oDtaDiscx(0)("nDiscRate"),
                                 p_oDtaDiscx(0)("nDiscAmtx"),
                                 p_nDiscAmtx,
                                 IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True),
                                 p_oDtaDiscx(0)("nNoClient"),
                                 p_oDtaDiscx(0)("nWithDisc"),
                                 p_oDtaDiscx(0)("sClientNm"))
                Else
                    .AddDiscount("",
                                 "",
                                 p_oDtaDiscx(0)("nDiscRate"),
                                 p_oDtaDiscx(0)("nDiscAmtx"),
                                 p_nDiscAmtx,
                                 IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True))
                End If
            End If

            .NonVatSales = p_nNonVATxx
            .CashPayment = p_oDataTable(0)("nTendered")

            Dim loBank As clsBanks
            loBank = New clsBanks(p_oAppDrvr)

            Call getCreditCard()
            If p_oDtaCCard.Rows.Count > 0 Then
                For lnCtr = 0 To p_oDtaCCard.Rows.Count - 1
                    If p_oDtaCCard(lnCtr)("nAmountxx") > 0 Then
                        loDR = loBank.SearchBank(p_oDtaCCard(lnCtr)("sBankIDxx"), True)

                        .AddCreditCard(loDR("sBankName"),
                                       p_oDtaCCard(lnCtr)("sCardNoxx"),
                                       p_oDtaCCard(lnCtr)("sApprovNo"),
                                       p_oDtaCCard(lnCtr)("nAmountxx"))
                    End If
                Next
            End If

            Call getCheck()
            If p_oDtaCheck.Rows.Count > 0 Then
                For lnCtr = 0 To p_oDtaCheck.Rows.Count - 1
                    If p_oDtaCheck(lnCtr)("nAmountxx") > 0 Then
                        loDR = loBank.SearchBank(p_oDtaCheck(lnCtr)("sBankIDxx"), True)

                        .AddCheck(loDR("sBankName"),
                                  p_oDtaCheck(lnCtr)("sCheckNox"),
                                  p_oDtaCheck(lnCtr)("dCheckDte"),
                                  p_oDtaCheck(lnCtr)("nAmountxx"))
                    End If
                Next
            End If

            Call getGiftCert()
            If p_oDtaGCert.Rows.Count > 0 Then
                For lnCtr = 0 To p_oDtaGCert.Rows.Count - 1
                    If p_oDtaGCert(lnCtr)("nAmountxx") > 0 Then
                        .AddGiftCoupon(p_oDtaGCert(lnCtr)("sCompnyCd"),
                                       p_oDtaGCert(lnCtr)("nAmountxx"))
                    End If
                Next
            End If

            Call getDelivery()
            If p_oDtaDlvery.Rows.Count > 0 Then
                For lnCtr = 0 To p_oDtaDlvery.Rows.Count - 1
                    If p_oDtaDlvery(lnCtr)("nAmountxx") > 0 Then
                        .AddDelivery(p_oDtaDlvery(lnCtr)("sRiderIDx"),
                                       p_oDtaDlvery(lnCtr)("nAmountxx"))
                    End If
                Next
            End If

            .AddFooter("")
            .AddFooter("Thank you, and please come again.")
            .AddFooter("")

            .AddFooter("RMJ Business Solutions")
            .AddFooter("32 Pogo Grande")
            .AddFooter("Dagupan City, Pangasinan 2400")
            .AddFooter("VAT REG TIN #: 942-188-655-00000")
            .AddFooter("ACCR No.: 0049421886552021051421")
            .AddFooter("ACCR Validity: 2021/07/28 - 2026/07/28")
            .AddFooter("PTU No.: FP072021-004-0296573-00002")
            .AddFooter("PTU Validity: 2021/08/03 - 2026/08/03")

            .AddFooter("")
            .AddFooter("THIS DOCUMENT SHALL BE VALID")
            .AddFooter("FOR FIVE(5) YEARS FROM THE DATE OF")
            .AddFooter("THE PERMIT TO USE")
            .AddFooter("THIS DOCUMENT IS NOT VALID")
            .AddFooter("FOR CLAIM OF INPUT TAX")

            Return .PrintOR()
        End With
    End Function
    Private Function CheckPrinter(ByVal printerName As String) As Boolean
        Dim cashier_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_CS")
        Try
            Dim printDocument As PrintDocument = New PrintDocument
            printDocument.PrinterSettings.PrinterName = cashier_printer
            Return printDocument.PrinterSettings.IsValid
        Catch ex As System.Exception
            Return False
        End Try
    End Function

    Function printReciept(Optional ByVal bReprint As Boolean = False) As Boolean
        Dim lnCtr As Integer
        Dim loPrint As PRN_Receipt

        loPrint = New PRN_Receipt(p_oAppDrvr)

        With loPrint
            If Not .InitMachine Then
                Return False
            End If
            If CDate(Format(p_oDataTable.Rows(0)("dTransact"), xsDATE_SHORT)) < CDate(Format(p_oAppDrvr.getSysDate, xsDATE_SHORT)) Then
                .Transaction_Date = CDate(Format(p_oDataTable.Rows(0)("dTransact"), xsDATE_SHORT))
            Else
                .Transaction_Date = p_oAppDrvr.getSysDate
            End If

            .ReferenceNo = p_oDataTable(0)("sORNumber")
            .TrasactionNo = p_sMasterNo
            .Reprint = bReprint
            .ServiceCharge = p_oDataTable.Rows(0)("nSChargex")
            .CashierName = getCashier(p_oAppDrvr.UserID)
            .ClientNo = p_nNoClient
            .WithDisc = p_nWithDisc
            .TableNo = p_nTableNo
            .TranType = p_sTrantype
            .LogName = p_sLogName
            .PosDate = p_dPOSDatex
            .MergeTable = p_sMergeTbl
            .SplitType = p_cSplitTyp
            .SourceNo = p_sSplitSrc
            .BillingNo = p_sBillingNo


            If Not IsNothing(p_oDtaOrder) Then
                Dim lnSlPrc As Double
                'If p_oDtaOrder.Rows.Count = 0 Then
                '    'if detail is nothing, this means the order was splitted
                '    .AddDetail(1, _
                '                "MEAL(S)", _
                '                p_oDataTable(0)("nSalesAmt"), _
                '                True, _
                '                True)
                'Else
                Dim lnComplmnt As Integer = 0
                Dim lnQuantity As Integer = 0
                For lnCtr = 0 To p_oDtaOrder.Rows.Count - 1
                    'Get compliment for the master item
                    If p_oDtaOrder(lnCtr)("cDetailxx") = xeLogical.NO Then
                        lnComplmnt = p_oDtaOrder(lnCtr)("nComplmnt")
                        lnQuantity = p_oDtaOrder(lnCtr)("nQuantity")
                    Else
                        If p_oDtaOrder(lnCtr)("cWthPromo") = xeLogical.NO Then
                            lnComplmnt = p_oDtaOrder(lnCtr)("nComplmnt")
                            lnQuantity = p_oDtaOrder(lnCtr)("nQuantity")
                        End If
                    End If

                    'Do not include REVERSE(D) orders here...
                    If p_oDtaOrder(lnCtr)("cReversed") = xeLogical.NO Then
                        'Compute unit price here...
                        lnSlPrc = (p_oDtaOrder(lnCtr).Item("nUnitPrce") *
                                    (100 - p_oDtaOrder(lnCtr).Item("nDiscount")) / 100 -
                                    p_oDtaOrder(lnCtr).Item("nAddDiscx"))

                        Dim lnDiv As Double
                        lnDiv = p_oDtaOrder(lnCtr)("nQuantity") / lnQuantity

                        If lnQuantity - lnComplmnt > 0 Then
                            .AddDetail(lnDiv * (lnQuantity - lnComplmnt),
                                       p_oDtaOrder(lnCtr)("sBriefDsc"),
                                       p_oDtaOrder(lnCtr)("nUnitPrce"),
                                       True,
                                       p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                                       IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False),
                                       p_oDtaOrder(lnCtr)("cWthPromo") = "1", p_oDtaOrder(lnCtr)("nDiscount"), p_oDtaOrder(lnCtr)("nAddDiscx"))
                        End If

                        If lnComplmnt > 0 Then
                            .AddComplement(lnDiv * lnComplmnt,
                                       p_oDtaOrder(lnCtr)("sBriefDsc"),
                                       0,
                                       True, IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False))
                        End If
                    Else
                        'kalyptus - 2017.01.27 09:42am
                        'Print reverse items
                        If p_oDtaOrder(lnCtr)("cReversex") = "+" Then
                            'If p_oDtaOrder(lnCtr)("cPrintedx") = "0" Then
                            '    .AddDetail(p_oDtaOrder(lnCtr)("nQuantity") * -1,
                            '           "Void-" & p_oDtaOrder(lnCtr)("sBriefDsc"),
                            '           0,
                            '           True,
                            '           p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                            '           IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False),
                            '           p_oDtaOrder(lnCtr)("cWthPromo") = "1", p_oDtaOrder(lnCtr)("nDiscount"), p_oDtaOrder(lnCtr)("nAddDiscx"))
                            'Else
                            'Compute unit price here...
                            lnSlPrc = (p_oDtaOrder(lnCtr).Item("nUnitPrce") *
                                            (100 - p_oDtaOrder(lnCtr).Item("nDiscount")) / 100 -
                                            p_oDtaOrder(lnCtr).Item("nAddDiscx"))

                            Dim lnDiv As Double
                            lnDiv = p_oDtaOrder(lnCtr)("nQuantity") / lnQuantity

                            If lnQuantity - lnComplmnt > 0 Then
                                .AddDetail(lnDiv * (lnQuantity - lnComplmnt),
                                               "*" & p_oDtaOrder(lnCtr)("sBriefDsc"),
                                               p_oDtaOrder(lnCtr)("nUnitPrce"),
                                               True,
                                               p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                                               IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False),
                                               p_oDtaOrder(lnCtr)("cWthPromo") = "1", p_oDtaOrder(lnCtr)("nDiscount"), p_oDtaOrder(lnCtr)("nAddDiscx"))
                            End If
                            'End If
                        Else
                            .AddDetail(p_oDtaOrder(lnCtr)("nQuantity") * -1,
                                   "Void-" & p_oDtaOrder(lnCtr)("sBriefDsc"),
                                    p_oDtaOrder(lnCtr)("nUnitPrce"),
                                   True,
                                   p_oDtaOrder(lnCtr)("cDetailxx") = "1",
                                   IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False),
                                   p_oDtaOrder(lnCtr)("cWthPromo") = "1", p_oDtaOrder(lnCtr)("nDiscount"), p_oDtaOrder(lnCtr)("nAddDiscx"))
                        End If
                    End If
                Next
                .SplitAmount = IFNull(p_oDtaOrder.Rows(0)("nAmountxx"), 0)
            End If

            'End If

            Dim loDR As DataRow
            Dim loDiscCard As clsDiscountCards
            loDiscCard = New clsDiscountCards(p_oAppDrvr)

            If Not IsNothing(p_oDtaDiscx) Then
                'loDR = loDiscCard.SearchCard(p_oDtaDiscx(0)("sCategrID"), True) sCardIDxx
                loDR = loDiscCard.SearchCard(p_oDtaDiscx(0)("sCardIDxx"), True)

                If Not IsNothing(loDR) Then
                    '.AddDiscount(p_oDtaDiscx(0)("sIDNumber"), _
                    '             loDR("sCardDesc"), _
                    '             p_nDiscAmtx, IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True))

                    'MAC
                    .AddDiscount(p_oDtaDiscx(0)("sIDNumber"),
                                 loDR("sCardDesc"),
                                 p_oDtaDiscx(0)("nDiscRate"),
                                 p_oDtaDiscx(0)("nDiscAmtx"),
                                 p_nDiscAmtx,
                                 IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True),
                                 p_oDtaDiscx(0)("nNoClient"),
                                 p_oDtaDiscx(0)("nWithDisc"),
                                 p_oDtaDiscx(0)("sClientNm"))
                Else
                    '.AddDiscount("", _
                    '             "", _
                    '             p_nDiscAmtx, IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True))

                    'MAC
                    .AddDiscount("",
                                 "",
                                 p_oDtaDiscx(0)("nDiscRate"),
                                 p_oDtaDiscx(0)("nDiscAmtx"),
                                 p_nDiscAmtx,
                                 IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True))
                End If
            End If

            .NonVatSales = p_nNonVATxx
            .CashPayment = p_oDataTable(0)("nTendered")

            Dim loBank As clsBanks
            loBank = New clsBanks(p_oAppDrvr)

            Call getCreditCard()
            If p_oDtaCCard.Rows.Count > 0 Then
                For lnCtr = 0 To p_oDtaCCard.Rows.Count - 1
                    If p_oDtaCCard(lnCtr)("nAmountxx") > 0 Then
                        loDR = loBank.SearchBank(p_oDtaCCard(lnCtr)("sBankIDxx"), True)

                        .AddCreditCard(loDR("sBankName"),
                                       p_oDtaCCard(lnCtr)("sCardNoxx"),
                                       p_oDtaCCard(lnCtr)("sApprovNo"),
                                       p_oDtaCCard(lnCtr)("nAmountxx"))
                    End If
                Next
            End If

            Call getCheck()
            If p_oDtaCheck.Rows.Count > 0 Then
                For lnCtr = 0 To p_oDtaCheck.Rows.Count - 1
                    If p_oDtaCheck(lnCtr)("nAmountxx") > 0 Then
                        loDR = loBank.SearchBank(p_oDtaCheck(lnCtr)("sBankIDxx"), True)

                        .AddCheck(loDR("sBankName"),
                                  p_oDtaCheck(lnCtr)("sCheckNox"),
                                  p_oDtaCheck(lnCtr)("dCheckDte"),
                                  p_oDtaCheck(lnCtr)("nAmountxx"))
                    End If
                Next
            End If

            Call getGiftCert()
            If p_oDtaGCert.Rows.Count > 0 Then
                For lnCtr = 0 To p_oDtaGCert.Rows.Count - 1
                    If p_oDtaGCert(lnCtr)("nAmountxx") > 0 Then
                        .AddGiftCoupon(p_oDtaGCert(lnCtr)("sCompnyCd"),
                                       p_oDtaGCert(lnCtr)("nAmountxx"))
                    End If
                Next
            End If

            Call getDelivery()
            If p_oDtaDlvery.Rows.Count > 0 Then
                For lnCtr = 0 To p_oDtaDlvery.Rows.Count - 1
                    If p_oDtaDlvery(lnCtr)("nAmountxx") > 0 Then
                        .AddDeliveryServ(p_oDtaDlvery(lnCtr)("sBriefDsc"),
                                       p_oDtaDlvery(lnCtr)("nAmountxx"))
                    End If
                Next
            End If

            .AddFooter("")
            Select Case p_cTrnMde
                Case "A"
                    .AddFooter("This serves as your SALES INVOICE.")
                Case "D"
                    .AddFooter("This is not an SALES INVOICE.")
            End Select
            .AddFooter("Thank you, and please come again.")
            .AddFooter("")

            .AddFooter("RMJ Business Solutions")
            .AddFooter("32 Pogo Grande")
            .AddFooter("Dagupan City, Pangasinan")
            .AddFooter("NON VAT REG TIN: 942-188-655-00000")
            .AddFooter("ACCR No.: 0049421886552021051421")
            .AddFooter("ACCR Validity: 2021/07/28 - 2026/07/28")
            .AddFooter("PTU No.: FP072021-004-0296573-00002")
            '.AddFooter("PTU Validity: 2021/08/03 - 2026/08/03")

            '.AddFooter("")
            '.AddFooter("THIS RECEIPT SHALL BE VALID")
            '.AddFooter("FOR FIVE(5) YEARS FROM THE DATE OF")
            '.AddFooter("THE PERMIT TO USE")

            Return .PrintOR()
        End With
    End Function

    Function OpenBySource() As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String

        lsSQL = AddCondition(getSQL_Master, "sSourceNo = " & strParm(p_sSourceNo)) &
                                       " AND sSourceCd = " & strParm(p_sSourceCd)

        Debug.Print(lsSQL)
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return False

        Call createTable()
        With p_oDataTable
            'For lnCtr = 0 To loDT.Rows.Count - 1
            .Rows.Add()
            .Rows(0)("sTransNox") = loDT.Rows(0)("sTransNox")
            .Rows(0)("dTransact") = loDT.Rows(0)("dTransact")
            .Rows(0)("sORNumber") = loDT.Rows(0)("sORNumber")
            .Rows(0)("nSalesAmt") = loDT.Rows(0)("nSalesAmt")
            .Rows(0)("nVATSales") = loDT.Rows(0)("nVATSales")
            .Rows(0)("nVATAmtxx") = loDT.Rows(0)("nVATAmtxx")
            .Rows(0)("nDiscount") = loDT.Rows(0)("nDiscount")
            .Rows(0)("nTendered") = loDT.Rows(0)("nTendered")
            .Rows(0)("nCashAmtx") = loDT.Rows(0)("nCashAmtx")
            .Rows(0)("nVatDiscx") = loDT.Rows(0)("nVatDiscx")
            .Rows(0)("nPWDDiscx") = loDT.Rows(0)("nPWDDiscx")
            .Rows(0)("sCashierx") = loDT.Rows(0)("sCashierx")
            .Rows(0)("nSChargex") = loDT.Rows(0)("nSChargex")
            'Next lnCtr

            p_nCash = loDT.Rows(0)("nCashAmtx")
            p_nTendered = loDT.Rows(0)("nTendered")
            'p_nDiscAmtx = loDT.Rows(0)("nDiscount")
            'p_nNonVATxx = loDT.Rows(0)("nSalesAmt") + loDT.Rows(0)("nPWDDiscx")

            Debug.Print(loDT.Rows(0)("nSalesAmt"))
            Debug.Print(loDT.Rows(0)("nVATSales"))
            Debug.Print(loDT.Rows(0)("nVATAmtxx"))
            p_nNonVATxx = loDT.Rows(0)("nSalesAmt") - ((loDT.Rows(0)("nVATSales") + IFNull(loDT.Rows(0)("nZeroRatd"), 0) + loDT.Rows(0)("nVATAmtxx") - (loDT.Rows(0)("nDiscount") + loDT.Rows(0)("nPWDDiscx"))))
        End With

        Call computePaymentTotal()

        Return True
    End Function
#End Region

#Region "Private function"
    Private Sub computePaymentTotal()
        Dim lnCtr As Integer
        Dim lsSQL As String
        Dim lsCondition As String
        Dim loDT As DataTable

        lsCondition = "sSourceNo = " & strParm(p_sSourceNo) &
                        " AND sSourceCd = " & strParm(p_sSourceCd)

        lsSQL = "SELECT nCashAmtx, nTendered FROM " & pxeMasterTble
        lsSQL = AddCondition(lsSQL, lsCondition)
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            p_nCash = 0.0
            p_nTendered = 0.0
        Else
            p_nCash = CDbl(loDT(0)(0))
            p_nTendered = CDbl(loDT(0)(1))
        End If

        lsSQL = "SELECT nAmountxx FROM Credit_Card_Trans WHERE cTranStat = '0'"
        lsSQL = AddCondition(lsSQL, lsCondition)
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            p_nCreditCard = 0.0
        Else
            For lnCtr = 0 To loDT.Rows.Count - 1
                p_nCreditCard = p_nCreditCard + CDbl(loDT(lnCtr)(0))
            Next
        End If

        lsSQL = "SELECT nAmountxx FROM Check_Payment_Trans WHERE cTranStat = '0'"
        lsSQL = AddCondition(lsSQL, lsCondition)
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            p_nCheck = 0.0
        Else
            For lnCtr = 0 To loDT.Rows.Count - 1
                p_nCheck = p_nCheck + CDbl(loDT(lnCtr)(0))
            Next
        End If

        lsSQL = "SELECT nAmountxx FROM Gift_Certificate_Trans WHERE cTranStat = '0'"
        lsSQL = AddCondition(lsSQL, lsCondition)
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            p_nGiftCert = 0.0
        Else
            For lnCtr = 0 To loDT.Rows.Count - 1
                p_nGiftCert = p_nGiftCert + CDbl(loDT(lnCtr)(0))
            Next
        End If

        lsSQL = "SELECT nAmountxx FROM Delivery_Service_Trans WHERE cTranStat = '0'"
        lsSQL = AddCondition(lsSQL, lsCondition)
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            p_nDelivery = 0.0
        Else
            For lnCtr = 0 To loDT.Rows.Count - 1
                p_nDelivery = p_nDelivery + CDbl(loDT(lnCtr)(0))
            Next
        End If
    End Sub

    Public Function NewTransaction() As Boolean
        Call createTable()
        Call initMaster()

        Return True
    End Function

    Private Function SaveTransaction() As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim lnCash As Decimal

        With p_oDataTable
            Call ShowReceipt()
            If p_bCancelled = True Then Return False

            If p_oDataTable.Rows(0)("nTendered") > 0.0 Then
                If p_oDataTable.Rows(0)("nTendered") >= p_oDataTable.Rows(0)("nSalesAmt") Then
                    p_oDataTable.Rows(0)("nCashAmtx") = p_oDataTable.Rows(0)("nSalesAmt")
                Else
                    p_oDataTable.Rows(0)("nCashAmtx") = p_oDataTable.Rows(0)("nTendered")
                End If
            End If

            'iMac
            ' added on duplicate update

            If (p_nGiftCert + p_nCreditCard + p_nCheck + p_nDelivery) > 0 Then
                p_nCash = p_nTendered
            End If
            lnCash = p_nCash

            If p_nGiftCert > 0 Then
                If lnCash > p_nGiftCert Then
                    lnCash = (p_oDataTable.Rows(0)("nSalesAmt") + p_oDataTable.Rows(0)("nSChargex")) - p_nGiftCert
                Else
                    lnCash = p_oDataTable.Rows(0)("nSalesAmt") + p_oDataTable.Rows(0)("nSChargex")
                    lnCash = lnCash - p_nGiftCert
                End If
            End If

            If p_nDelivery > 0 Then
                If lnCash > p_nDelivery Then
                    lnCash = (p_oDataTable.Rows(0)("nSalesAmt") + p_oDataTable.Rows(0)("nSChargex")) - p_nDelivery
                Else
                    lnCash = p_oDataTable.Rows(0)("nSalesAmt") + p_oDataTable.Rows(0)("nSChargex")
                    lnCash = lnCash - p_nDelivery
                End If
            End If

            If p_nCreditCard > 0 Then
                'If p_nCreditCard <> p_oDataTable.Rows(0)("nSalesAmt") + p_oDataTable.Rows(0)("nSChargex") Then
                If lnCash > 0 Then
                    lnCash = Math.Abs((p_oDataTable.Rows(0)("nSalesAmt") + p_oDataTable.Rows(0)("nSChargex")) - p_nCreditCard)
                End If
            End If

            If Trim(p_sCashierx) = "" Then
                Dim loDT As New DataTable

                lsSQL = "SELECT * FROM Daily_Summary" &
                            " WHERE sTranDate = " & strParm(Format(p_dPOSDatex, "yyyyMMdd")) &
                                " AND cTranStat = '0'"
                loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

                If loDT.Rows.Count = 1 Then
                    p_sCashierx = loDT.Rows(0)("sCashierx")
                Else
                    p_sCashierx = p_oAppDrvr.UserID
                End If
            End If

            lsSQL = "INSERT INTO " & pxeMasterTble & " SET" &
                        "  sTransNox = " & strParm(p_oDataTable.Rows(0)("sTransNox")) &
                        ", dTransact = " & dateParm(p_oDataTable.Rows(0)("dTransact")) &
                        ", sORNumber = " & strParm(p_oDataTable.Rows(0)("sORNumber")) &
                        ", nSalesAmt = " & CDec(p_oDataTable.Rows(0)("nSalesAmt")) &
                        ", nVATSales = " & CDec(p_oDataTable.Rows(0)("nVATSales")) &
                        ", nVATAmtxx = " & CDec(p_oDataTable.Rows(0)("nVATAmtxx")) &
                        ", nDiscount = " & CDec(p_nDiscount) &
                        ", nTendered = " & CDec(p_nTendered) &
                        ", nCashAmtx = " & CDec(lnCash) &
                        ", nSChargex = " & CDec(p_oDataTable.Rows(0)("nSChargex")) &
                        ", sSourceCd = " & strParm(p_sSourceCd) &
                        ", sSourceNo = " & strParm(p_sSourceNo) &
                        ", nVatDiscx = " & CDec(p_oDataTable.Rows(0)("nVatDiscx")) &
                        ", nPWDDiscx = " & CDec(p_oDataTable.Rows(0)("nPWDDiscx")) &
                        ", sCashierx = " & strParm(p_sCashierx) &
                        ", cTranStat = " & strParm(IFNull(p_oDataTable.Rows(0)("cTranStat"), 0)) &
                    " ON DUPLICATE KEY UPDATE" &
                        "  nSalesAmt = " & CDec(p_oDataTable.Rows(0)("nSalesAmt")) &
                        ", nVATSales = " & CDec(p_oDataTable.Rows(0)("nVATSales")) &
                        ", nVATAmtxx = " & CDec(p_oDataTable.Rows(0)("nVATAmtxx")) &
                        ", nDiscount = " & CDec(p_nDiscount) &
                        ", nTendered = " & CDec(p_nTendered) &
                        ", nCashAmtx = " & CDec(lnCash) &
                        ", nSChargex = " & CDec(p_oDataTable.Rows(0)("nSChargex")) &
                        ", nVatDiscx = " & CDec(p_oDataTable.Rows(0)("nVatDiscx")) &
                        ", nPWDDiscx = " & CDec(p_oDataTable.Rows(0)("nPWDDiscx")) &
                        ", sCashierx = " & strParm(p_sCashierx)

            Try
                lnRow = p_oAppDrvr.Execute(lsSQL, pxeMasterTble)
                If lnRow <= 0 Then
                    MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End With

        lsSQL = ""
        If p_oDataTable.Rows(0)("nVatDiscx") = 0 And p_oDataTable.Rows(0)("nDiscount") > 0 Then
            lsSQL = "UPDATE Daily_Summary SET" &
                        " nTotlDisc = nTotlDisc + 1" &
                " WHERE sTranDate = " & strParm(Format(p_dPOSDatex, "yyyyMMdd")) &
                    " AND sCRMNumbr = " & strParm(p_sCRMNmbr) &
                    " AND sCashierx = " & strParm(p_sCashierx)
            Debug.Print(lsSQL)
        ElseIf p_oDataTable.Rows(0)("nVatDiscx") > 0 And p_oDataTable.Rows(0)("nDiscount") = 0 Then
            lsSQL = "UPDATE Daily_Summary SET" &
                        " nTotSCPWD = nTotSCPWD + 1" &
                " WHERE sTranDate = " & strParm(Format(p_dPOSDatex, "yyyyMMdd")) &
                    " AND sCRMNumbr = " & strParm(p_sCRMNmbr) &
                    " AND sCashierx = " & strParm(p_sCashierx)
            Debug.Print(lsSQL)
        End If

        If lsSQL <> "" Then
            Try
                lnRow = p_oAppDrvr.Execute(lsSQL, "Daily_Summary")
                If lnRow <= 0 Then
                    MsgBox("Unable to Save Receipt!!!" & vbCrLf &
                    "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End If

        If p_bCancelled Then Return False

        p_oAppDrvr.SaveEvent("0015", "Order TN " & p_sSourceNo & "/" & p_sSourceCd & "/" &
                                    "SI No. " & p_oDataTable.Rows(0)("sORNumber") & "/Amount " & p_nTendered + p_nCash, p_sSerial)
        Return True
    End Function

    Private Function getNextTransNo() As String
        Dim loDA As New MySqlDataAdapter
        Dim loDT As New DataTable
        Dim loDS As New DataSet
        Dim lsSQL As String
        Dim lnCounter As Integer
        Dim lnCode As Long
        Dim lnLen As Long
        Dim lsStr As String = ""

        lsSQL = "SELECT sTransNox" &
                " FROM " & pxeMasterTble &
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy") & "%") &
                " ORDER BY sTransNox DESC" &
                " LIMIT 1"

        Try
            loDA.SelectCommand = New MySqlCommand(lsSQL, p_oAppDrvr.Connection)
        Catch ex As MySqlException
            MsgBox(ex.Message)
            Throw ex
        End Try

        loDT.Clear()
        loDA.Fill(loDT)
        If loDT.Rows.Count = 0 Then
            lsSQL = ""

            loDA.FillSchema(loDS, SchemaType.Source)
            lnLen = loDS.Tables(0).Columns(0).MaxLength
            lnCode = 1

            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)

            lsSQL = loDT.Rows(0).Item("sTransNox")
            lnLen = Len(lsSQL)

            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        End If

        If lsSQL = "" Then
            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        Else
            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        End If

        If lsSQL = "" Then
            Return Format(lnCode, lsStr.PadRight(lnCounter, "0"))
        Else
            Return Left(lsSQL, lnCounter) & Format(lnCode, lsStr.PadRight(lnLen - lnCounter, "0"))
        End If
    End Function

    Private Function getNextORNumber() As String
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = "SELECT sORNumber" &
                " FROM " & pxeMasterTble &
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sPOSNo & "%") &
                " ORDER BY sORNumber DESC" &
                " LIMIT 1"

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            Return 1
        Else
            Return CInt(loDT(0).Item("sORNumber")) + 1
        End If
    End Function

    Private Function getSQL_Master() As String
        Return "SELECT" &
                    "  sTransNox" &
                    ", dTransact" &
                    ", sORNumber" &
                    ", nSalesAmt" &
                    ", nVATSales" &
                    ", nVATAmtxx" &
                    ", nDiscount" &
                    ", nTendered" &
                    ", nCashAmtx" &
                    ", nVatDiscx" &
                    ", nPWDDiscx" &
                    ", sCashierx" &
                    ", cTranStat" &
                    ", nSChargex" &
                    ", nZeroRatd" &
                " FROM " & pxeMasterTble
    End Function
#End Region

#Region "Private Procedures"
    Private Sub createTable()
        p_oDataTable = New DataTable
        With p_oDataTable
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("dTransact", GetType(Date))
            .Columns.Add("sORNumber", GetType(String)).MaxLength = 15
            .Columns.Add("nSalesAmt", GetType(Decimal))
            .Columns.Add("nVATSales", GetType(Decimal))
            .Columns.Add("nVATAmtxx", GetType(Decimal))
            .Columns.Add("nDiscount", GetType(Decimal))
            .Columns.Add("nTendered", GetType(Decimal))
            .Columns.Add("nCashAmtx", GetType(Decimal))
            .Columns.Add("sSourceCd", GetType(String)).MaxLength = 4
            .Columns.Add("sSourceNo", GetType(String)).MaxLength = 20
            .Columns.Add("nVatDiscx", GetType(Decimal))
            .Columns.Add("nPWDDiscx", GetType(Decimal))
            .Columns.Add("sCashierx", GetType(String)).MaxLength = 10
            .Columns.Add("cTranStat", GetType(String)).MaxLength = 1
            .Columns.Add("nSChargex", GetType(Decimal))
        End With
    End Sub

    Private Sub initMaster()
        With p_oDataTable
            .Rows.Add()
            .Rows(0)("sTransNox") = getNextTransNo()
            .Rows(0)("dTransact") = p_oAppDrvr.SysDate
            .Rows(0)("sORNumber") = Strings.Right("000000000000000" & getNextORNumber().ToString(), 15)
            .Rows(0)("nSalesAmt") = 0.0
            .Rows(0)("nVATSales") = 0.0
            .Rows(0)("nVATAmtxx") = 0.0
            .Rows(0)("nDiscount") = 0.0
            .Rows(0)("nTendered") = 0.0
            .Rows(0)("nCashAmtx") = 0.0
            .Rows(0)("sSourceCd") = p_sSourceCd
            .Rows(0)("sSourceNo") = p_sSourceNo
            .Rows(0)("nVatDiscx") = 0.0
            .Rows(0)("nPWDDiscx") = 0.0
            .Rows(0)("sCashierx") = ""
            .Rows(0)("cTranStat") = "0"
            .Rows(0)("nSChargex") = 0.0
        End With
        pnBill = myBill
        pnCharge = myCharge
    End Sub

    Private Sub ShowReceipt()
        With p_oFormPay
            .Receipt = Me
            .TopMost = True
            .ShowDialog()
            p_bCancelled = .Cancelled
        End With
    End Sub

    Private Sub getCreditCard()
        p_oCreditCard = New CreditCard(p_oAppDrvr)

        With p_oCreditCard
            .SourceCd = p_sSourceCd
            .SourceNo = p_sSourceNo

            .OpenBySource()
            p_oDtaCCard = .CreditCardTrans
        End With

        p_oCreditCard = Nothing
    End Sub

    Private Sub getCheck()
        p_oCheck = New CheckPayment(p_oAppDrvr)

        With p_oCheck
            .SourceCd = p_sSourceCd
            .SourceNo = p_sSourceNo

            .OpenBySource()
            p_oDtaCheck = .CheckPaymTrans
        End With

        p_oCheck = Nothing
    End Sub

    Private Sub getGiftCert()
        p_oGiftCert = New GiftCerticate(p_oAppDrvr)

        With p_oGiftCert
            .SourceCd = p_sSourceCd
            .SourceNo = p_sSourceNo

            .OpenBySource()
            p_oDtaGCert = .GiftCertTrans
        End With

        p_oGiftCert = Nothing
    End Sub
    Private Sub getDelivery()
        p_oDelivery = New Delivery(p_oAppDrvr)

        With p_oDelivery
            .SourceCd = p_sSourceCd
            .SourceNo = p_sSourceNo
            .POSNumbr = p_sPOSNo

            .OpenBySource()
            p_oDtaDlvery = .Delivery
        End With

        p_oDelivery = Nothing
    End Sub
#End Region

#Region "Public Procedures"
    Sub showCreditCard(ByRef CloseForm As Boolean)
        p_oCreditCard = New CreditCard(p_oAppDrvr)
        p_oCreditCard.SourceCd = p_sSourceCd
        p_oCreditCard.SourceNo = p_sSourceNo
        p_oCreditCard.ShowCreditCard()
        CloseForm = p_oCreditCard.CloseForm
    End Sub

    Sub showCheck(ByRef CloseForm As Boolean)
        p_oCheck = New CheckPayment(p_oAppDrvr)
        p_oCheck.SourceCd = p_sSourceCd
        p_oCheck.SourceNo = p_sSourceNo
        p_oCheck.ShowCheck()
        CloseForm = p_oCheck.CloseForm
    End Sub

    Sub showGiftCert(ByRef CloseForm As Boolean)
        p_oGiftCert = New GiftCerticate(p_oAppDrvr)
        p_oGiftCert.SourceCd = p_sSourceCd
        p_oGiftCert.SourceNo = p_sSourceNo
        p_oGiftCert.ShowGiftCert()
        CloseForm = p_oGiftCert.CloseForm
    End Sub

    Sub showDeliverys(ByRef CloseForm As Boolean)
        p_oDelivery = New Delivery(p_oAppDrvr)
        p_oDelivery.SourceCd = p_sSourceCd
        p_oDelivery.SourceNo = p_sSourceNo
        p_oDelivery.POSNumbr = p_sPOSNo
        p_oDelivery.ShowDeliverys()
        CloseForm = p_oDelivery.CloseForm
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oAppDrvr = foRider

        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oAppDrvr.BranchCode
        p_oFormPay = New frmPay

        p_nCash = 0
        p_nCreditCard = 0
        p_nCheck = 0
        p_nGiftCert = 0
        p_nDelivery = 0
        p_nSalesAmt = 0
        p_nDiscount = 0
        p_nTendered = 0
        p_nSchargex = 0

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub

    Public Function getCashier(ByVal sCashierx As String) As String
        Dim lsSQL As String
        Dim lsCashierNm
        Dim loDta As DataTable

        lsSQL = "SELECT" & _
                    " a.sUserName" & _
                    " FROM xxxSysUser a" & _
                    " WHERE a.sUserIDxx = " & strParm(sCashierx)

        loDta = p_oAppDrvr.ExecuteQuery(lsSQL)
        If loDta.Rows.Count = 0 Then
            lsCashierNm = ""
        Else
            lsCashierNm = Decrypt(loDta(0).Item("sUserName"), xsSignature)
        End If

        loDta = Nothing

        Return lsCashierNm

    End Function
End Class