Imports ADODB
Imports ggcAppDriver
Imports System.Drawing

Public Class PRN_CancelledReceipt
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

    Private p_oDTMaster As DataTable
    Private p_oDTDetail As DataTable
    Private p_oDTComplx As DataTable    'Complimentary
    Private p_oDTGftChk As DataTable    'Gift Check
    Private p_oDTChkPym As DataTable    'Check Payment
    Private p_oDTCredit As DataTable    'Credit Card

    'jovan 2020-11-07
    Private psCashierNm As String

    'Jovan
    Private p_sLogName As String
    Private p_nTableNo As Integer
    Private p_sMergeTb As String
    Private p_nNoClient As Integer
    Private p_nWithDisc As Integer
    Private p_dPOSDatex As Date
    Private p_cSplitTyp As Integer

    Private p_oDTHeader As DataTable
    Private p_oDTFooter As DataTable
    Private p_oDTDiscnt As DataTable

    'Transaction Master Info
    Private psCashrNme As String
    Private pdTransact As Date          'XXX
    Private psReferNox As String        'XXX
    Private psSourceNo As String        'XXX
    Private psTransNox As String        'XXX

    Private pnTotalItm As Decimal
    Private pnTotalDue As Decimal
    Private pnDiscAmtV As Decimal
    Private pnDiscAmtN As Decimal
    Private pnSplitAmt As Decimal

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
    Private pnDSTotl As Decimal

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
    Private Const xsSignature As String = "08220326"

    Public Property CashierName() As String
        Get
            Return psCashierNm
        End Get
        Set(ByVal value As String)
            psCashierNm = value
        End Set
    End Property

    'jovan 03-10-21
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

    WriteOnly Property SourceNo As String
        Set(ByVal value As String)
            psSourceNo = value
        End Set
    End Property

    Public Property TrasactionNo() As String
        Get
            Return psTransNox
        End Get
        Set(ByVal value As String)
            psTransNox = value
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

    Public Property ClientNo() As Integer
        Get
            Return p_nNoClient
        End Get
        Set(ByVal value As Integer)
            p_nNoClient = value
        End Set
    End Property

    Public Property WithDisc() As Integer
        Get
            Return p_nWithDisc
        End Get
        Set(ByVal value As Integer)
            p_nWithDisc = value
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

    WriteOnly Property MergeTable As String
        Set(ByVal Value As String)
            p_sMergeTb = Value
        End Set
    End Property

    Public WriteOnly Property PosDate() As Date
        Set(ByVal Value As Date)
            p_dPOSDatex = Value
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
        lsSQL = "SELECT" &
                       "  sAccredtn" &
                       ", sPermitNo" &
                       ", sSerialNo" &
                       ", nPOSNumbr" &
                       ", cTranMode" &
                       ", nSChargex" &
               " FROM Cash_Reg_Machine" &
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
        p_cTrnMde = loDta(0).Item("cTranMode")
        p_nSCRate = loDta(0).Item("nSChargex")

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
    Public Function AddDetail(
            ByVal Quantity As Integer,
            ByVal Description As String,
            ByVal UnitPrice As Decimal,
            ByVal isVatable As Boolean,
            ByVal isDetail As Boolean,
            ByVal isCount As Boolean,
            ByVal isWthPrmo As Boolean,
            ByVal Discount As Decimal,
            ByVal AddDiscx As Decimal) As Boolean

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
            .Rows(.Rows.Count - 1).Item("cWthPromo") = IIf(isWthPrmo = True, 1, 0)
            .Rows(.Rows.Count - 1).Item("nDiscount") = Discount
            .Rows(.Rows.Count - 1).Item("nAddDiscx") = AddDiscx

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
    Public Function AddDiscount(
            ByVal IDNumber As String,
            ByVal DiscCard As String,
            ByVal Amount As Decimal,
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
    Public Function AddDiscount(
            ByVal IDNumber As String,
            ByVal DiscCard As String,
            ByVal DiscRate As Decimal,
            ByVal AddDiscx As Decimal,
            ByVal Amount As Decimal,
            ByVal isVatable As Boolean,
            Optional ByVal NoClient As Integer = 1,
            Optional ByVal WithDisc As Integer = 1,
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
    Public Function AddGiftCoupon(
            ByVal GiftSource As String,
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
    'AddGiftCoupon(GiftSource, Amount)
    '   - Sets the info of Gift Coupon(s) used as payment
    '+++++++++++++++++++++++++
    Public Function AddDelivery(
            ByVal DeliverySrc As String,
            ByVal Amount As Decimal) As Boolean

        With p_oDtaDlvery

            If .Rows.Count = 0 Then pnGiftTotl = 0

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sGiftSrce") = DeliverySrc
            .Rows(.Rows.Count - 1).Item("nDeliveryAmt") = Amount

            pnDSTotl = pnDSTotl + Amount

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

    Private Function WriteOR() As Boolean
        Dim lnVatPerc As Double = 1.12
        Dim lnDeducQTY As Integer
        Dim lnSplitAmt As Decimal = 0
        Dim lnQTYDiscx As Integer = 0
        Dim lbByCategx As Boolean = False
        Dim lnDisctAmt As Decimal = 0

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter(Trim(p_sCompny), 40) & Environment.NewLine)

        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)

        builder.Append(PadCenter("VAT REG TIN: " & p_sVATReg, 40) & Environment.NewLine)
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine)
        builder.Append(PadCenter("PTU No.: " & p_sPermit, 40) & Environment.NewLine)
        builder.Append(PadCenter("Serial No. : " & p_sSerial, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(PadCenter("CANCELLED SALES INVOICE", 40) & Environment.NewLine)
        Select Case p_cTrnMde
            Case "D"
                builder.Append(PadCenter("TRAINING MODE", 40) & Environment.NewLine)
        End Select

        If pbReprint Then
            builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
            builder.Append(RawPrint.pxePRINT_CNTR)
            builder.Append(p_oDTHeader(p_oDTHeader.Rows.Count - 1).Item("sHeadName") & Environment.NewLine)
        End If

        'Print Cashier
        builder.Append(Environment.NewLine)
        builder.Append(" Cashier: " & p_sLogName & "/" & psCashierNm & Environment.NewLine)
        'builder.Append(" Table No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" Ref. No.: " & "OR" & psReferNox & Environment.NewLine)
        builder.Append(" Transaction No.: " & psTransNox & Environment.NewLine)
        builder.Append(" Date : " & pdTransact.Year & "-" & Format(pdTransact.Month, "00") & "-" & Format(pdTransact.Day, "00") & " " & Format(p_oApp.getSysDate, "hh:mm:ss tt") & Environment.NewLine)

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
                        If p_oDTDetail(lnCtr).Item("nDiscount") > 0 Or p_oDTDetail(lnCtr).Item("nAddDiscx") > 0 Then
                            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                              UCase(Left(p_oDTDetail(lnCtr).Item("sBriefDsc"), 11) & "(D)").PadRight(pxeDSCLEN) + " "
                            lnQTYDiscx = lnQTYDiscx + p_oDTDetail(lnCtr).Item("nQuantity")
                            lnDisctAmt = lnDisctAmt + p_oDTDetail(lnCtr).Item("nQuantity") * p_oDTDetail(lnCtr).Item("nUnitPrce")
                            If Not lbByCategx Then lbByCategx = True
                        Else
                            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                               UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                        End If
                    Else
                        ls4Print = String.Empty.PadLeft(pxeQTYLEN) + " " +
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    End If
                Else
                    If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                        If p_oDTDetail(lnCtr).Item("nDiscount") > 0 Or p_oDTDetail(lnCtr).Item("nAddDiscx") > 0 Then
                            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                                        UCase(Left(p_oDTDetail(lnCtr).Item("sBriefDsc"), 11) & "(D)").PadRight(pxeDSCLEN) + " "
                            lnQTYDiscx = lnQTYDiscx + p_oDTDetail(lnCtr).Item("nQuantity")
                            lnDisctAmt = lnDisctAmt + p_oDTDetail(lnCtr).Item("nQuantity") * p_oDTDetail(lnCtr).Item("nUnitPrce")
                            If Not lbByCategx Then lbByCategx = True
                        Else
                            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                                        UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                        End If
                    Else
                        ls4Print = "   " & UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                        lnDeducQTY = lnDeducQTY + p_oDTDetail(lnCtr).Item("nQuantity")
                    End If
                End If
            Else
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " +
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

                ls4Print = Format(p_oDTComplx(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " + _
                           UCase(p_oDTComplx(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append(" No. of Items: " & pnTotalItm * -1 & Environment.NewLine)

        'do we have SC Discount?
        If pnDiscAmtN > 0 And pnNoClient > 0 Then
            'print no of clients and no of with discounts
            builder.Append(" Total No. of Clients: " & pnNoClient & Environment.NewLine)
            builder.Append(" No. of SC/PWD Clients: " & pnWithDisc & Environment.NewLine)
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
            Dim lnVATExWDsc = lnVATExclsv - (lnRateAmntx + (lnAddDiscxx * lnQTYDiscx) + pnDiscAmtN)

            Dim lsLess As String = " Less: "
            If pnDiscRteV > 0 Then
                'builder.Append((lsLess & Math.Round(pnDiscRteV) & "% Discount").PadRight(25) & " " & Format(lnRateAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(lsLess & p_oDTDiscnt(0).Item("sDiscCard") & Environment.NewLine)
                If Not lbByCategx Then
                    builder.Append("       " & ("(" & CDbl(pnDiscRteV) & "%)").PadRight(18) & " " & Format((pnDiscAmtV - pnAddDiscV), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                Else
                    builder.Append("       " & ("(" & CDbl(pnDiscRteV) & "%)" & " * P" & lnDisctAmt).PadRight(18) & " " & Format((pnDiscAmtV - (Math.Abs(pnAddDiscV * lnQTYDiscx))), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                End If

                'builder.Append("       " & ("(" & Format(Math.Round(pnDiscRteV), "#0.0") & "%)").PadRight(18) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                'builder.Append("       " & ("(" & Format(Math.Round(pnDiscRteV), "#0.0") & "%)").PadRight(18) & " " & Format(pnTotalDue * (pnDiscRteV / 100), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "

                If pnAddDiscV > 0 Then
                    'builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                    If Not lbByCategx Then
                        builder.Append(("       " & "P" & Math.Round(pnAddDiscV) & "  Discount").PadRight(25) & " " & Format(pnAddDiscV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                    Else
                        builder.Append(("       " & "P" & Math.Round(pnAddDiscV) & " * " & lnQTYDiscx & "  Discount").PadRight(25) & " " & Format(pnAddDiscV * lnQTYDiscx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                    End If

                    lsLess = "       "
                End If
            Else
                If pnAddDiscV > 0 Then
                    builder.Append(lsLess & p_oDTDiscnt(0).Item("sDiscCard") & Environment.NewLine)
                    'builder.Append((lsLess & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(lnAddDiscxx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                    If Not lbByCategx Then
                        builder.Append(("       " & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(pnAddDiscV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                    Else
                        builder.Append(("       " & "P" & Math.Round(pnAddDiscV) & " * " & lnQTYDiscx & " Discount").PadRight(25) & " " & Format(pnAddDiscV * lnQTYDiscx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                    End If

                    lsLess = "       "
                End If
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            'builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATExWDsc * 0.12, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            'builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        ElseIf pnDiscAmtN > 0 Then
            'orig code
            'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lnVATablex As Decimal = 0
            Dim lnNVATable As Decimal = 0
            Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, lnNVATable)
            Dim lnSCPWAmtx As Decimal = (pnTotalDue / pnNoClient) * p_nWithDisc
            Dim lnSCPWNetx As Decimal = lnSCPWAmtx - (lnSCPWAmtx - (lnSCPWAmtx / lnVatPerc))

            'If p_nNoClient <> p_nWithDisc Then
            '    builder.Append(" Price Inclusive of VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            '    lnExVATDue = ((pnTotalDue / pnNoClient) * p_nWithDisc) / 1.12
            '    builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'Else
            '    builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'End If

            'builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            If p_nNoClient <> p_nWithDisc Then
                builder.Append(" SC/PWD Sales".PadRight(25) & " " & Format(lnSCPWAmtx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Less: 12% VAT".PadRight(25) & " " & Format(lnSCPWAmtx - lnNVATable, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" VAT Exempt Sales".PadRight(25) & " " & Format(lnNVATable, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Net Sales SC/PWD".PadRight(25) & " " & Format(Math.Floor(100 * (lnSCPWNetx - (lnSCPWNetx * 0.2))) / 100, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Regular Sales".PadRight(25) & " " & Format(pnTotalDue - lnSCPWAmtx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

                'builder.Append(" Price Inclusive of VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                'builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Else
                'SC/PWD Sales		 486.67 
                'less 12 vat		 52.15 
                'Vat Exempt Sales		 434.52 
                'less: 20% sc/pwd		 86.90 
                'SC/PWD Sales, net		 347.62 
                'Regular Sales		 243.33 
                builder.Append(" Less: 12% VAT".PadRight(25) & " " & Format(pnTotalDue - lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" VAT Exempt Sales".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

                'builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            If pnSChargex > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
                builder.Append(" Total Due".PadRight(25) & " " & Format(lnVATablex + (lnSCPWNetx - lnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                'builder.Append(" Total Due".PadRight(25) & " " & Format((lnVATablex + lnExVATDue) - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
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

        builder.Append(" TOTAL AMOUNT DUE :".PadRight(25) & " " & Format(((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN)) * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        If p_cSplitTyp <> 2 Then
            Dim lnCurSplit As Integer = 0
            Dim lnCtr As Integer
            Dim loDT As DataTable = getSplitTable(psSourceNo)
            Dim lsPartial As String

            For lnCtr = 0 To loDT.Rows.Count - 1
                If loDT.Rows(lnCtr).Item("cTranStat") = xeTranStat.TRANS_POSTED Then
                    lsPartial = " PAID " & "(OR" & loDT.Rows(lnCtr).Item("sORNumber") & ")"
                    builder.Append(lsPartial.PadRight(28) & " " & "-" & Format(loDT.Rows(lnCtr).Item("nSalesAmt"), xsDECIMAL) & "".PadLeft(pxeREGLEN) & Environment.NewLine)
                    lnCurSplit = lnCurSplit + 1
                End If
            Next

            lsPartial = " Partial Bill " & "(" & lnCurSplit + 1 & "/" & loDT.Rows.Count & ")"
            builder.Append(lsPartial.PadRight((Len(lsPartial) + 14) - Len(Format(pnSplitAmt, xsDECIMAL))) & " " & Format(pnSplitAmt, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            lnSplitAmt = pnSplitAmt
        End If

        'Print Cash Payments
        If pnCashTotl > 0 Then
            builder.Append(" Cash".PadRight(25) & " " & Format(pnCashTotl * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If

        'Print Credit Card Payments
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
                           Format(p_oDTCredit(lnCtr).Item("nCardAmnt") * -1, xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Check Payments
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
                           Format(p_oDTChkPym(lnCtr).Item("nCheckAmt") * -1, xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Gift Coupon
        If p_oDTGftChk.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce") & " GIFT CHEQUE").PadRight(24) & " " & _
                           Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt") * -1, xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Line Before change....
        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

        Dim lnChange As Decimal
        If p_cSplitTyp <> 2 Then
            lnChange = (pnSplitAmt + pnSChargex) - (pnDiscAmtV + pnDiscAmtN)
        Else
            lnChange = (pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN)
        End If

        If pnGiftTotl > lnChange Then
            lnChange = 0
        Else
            lnChange = (pnCashTotl + pnChckTotl + pnCrdtTotl + pnGiftTotl) - lnChange
        End If

        builder.Append(" CHANGE           :".PadRight(25) & " " & Format(lnChange * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

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

        'pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        'pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        If p_cSplitTyp <> 2 Then
            If pnDiscAmtV > 0 Then
                pnVatblSle = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
                pnVatAmntx = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle
            ElseIf pnDiscAmtN > 0 Then
                pnVatblSle = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + (pnVatExSle * lnVatPerc))) / lnVatPerc
                pnVatAmntx = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + (pnVatExSle * lnVatPerc))) - pnVatblSle
            Else
                pnVatblSle = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
                pnVatAmntx = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle
            End If
        Else
            If pnDiscAmtV > 0 Then
                pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
                pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle
            ElseIf pnDiscAmtN > 0 Then
                pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + (pnVatExSle * lnVatPerc))) / lnVatPerc
                pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + (pnVatExSle * lnVatPerc))) - pnVatblSle
            Else
                pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
                pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle
            End If
        End If

        'Print VAT Related info
        builder.Append("  VAT Exempt Sales      " & Format(pnVatExSle * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  Zero-Rated Sales      " & Format(pnZroRtSle * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VATable Sales         " & Format(pnVatblSle * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VAT Amount            " & Format(pnVatAmntx * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine & Environment.NewLine)

        builder.Append(Environment.NewLine & Environment.NewLine)
        builder.Append(PadCenter("____________________________", 40) & Environment.NewLine)
        builder.Append(PadCenter("Customer Signature", 40) & Environment.NewLine & Environment.NewLine & Environment.NewLine)

        builder.Append(PadCenter("____________________________", 40) & Environment.NewLine)
        builder.Append(PadCenter("Voided/Cancelled/Refunded By", 40) & Environment.NewLine & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        'Print the Footer
        For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
            builder.Append(PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter("----- END OF SALES INVOICE -----", 40) & Environment.NewLine)
        'RawPrint.writeToFile(p_sPOSNo & " " & Format(p_oApp.getSysDate(), "yyyyMMdd"), builder.ToString())
        RawPrint.writeToFile(p_sPOSNo, builder.ToString())
        RawPrint.writeToFile(p_sPOSNo & " " & Format(p_dPOSDatex, "yyyyMMdd"), builder.ToString())

        Return True
    End Function

    Public Function PrintOR() As Boolean
        Dim lnVatPerc As Double = 1.12
        Dim lbByCategx As Boolean = False
        Dim lnDeducQTY As Integer
        Dim lnSplitAmt As Decimal = 0
        Dim lnQTYDiscx As Integer = 0
        Dim lnDisctAmt As Decimal = 0

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
        For lnCtr = 0 To p_oDTHeader.Rows.Count - 2
            builder.Append(PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        builder.Append(Environment.NewLine)
        'builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append("CANCELLED SALES INVOICE" & Environment.NewLine)
        Select Case p_cTrnMde
            Case "D"
                builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
                builder.Append(RawPrint.pxePRINT_CNTR)
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
        builder.Append(" Cashier: " & p_sLogName & "/" & psCashierNm & Environment.NewLine)
        'builder.Append(" Table No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(" Ref. No.: " & "OR" & psReferNox & Environment.NewLine)
        builder.Append(" Transaction No.: " & psTransNox & Environment.NewLine)
        builder.Append(" Date : " & pdTransact.Year & "-" & Format(pdTransact.Month, "00") & "-" & Format(pdTransact.Day, "00") & " " & Format(p_oApp.getSysDate, "hh:mm:ss tt") & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

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
                        If p_oDTDetail(lnCtr).Item("nDiscount") > 0 Or p_oDTDetail(lnCtr).Item("nAddDiscx") > 0 Then
                            If pnDiscAmtV > 0 Then
                                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                                            UCase(Left(p_oDTDetail(lnCtr).Item("sBriefDsc"), 11) & "(D)").PadRight(pxeDSCLEN) + " "
                                lnQTYDiscx = lnQTYDiscx + p_oDTDetail(lnCtr).Item("nQuantity")
                                lnDisctAmt = lnDisctAmt + p_oDTDetail(lnCtr).Item("nQuantity") * p_oDTDetail(lnCtr).Item("nUnitPrce")
                                If Not lbByCategx Then lbByCategx = True
                            Else
                                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                                            UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                            End If
                        Else
                            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                               UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                        End If
                    Else
                        ls4Print = String.Empty.PadLeft(pxeQTYLEN) + " " +
                           UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                    End If
                Else
                    If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                        If p_oDTDetail(lnCtr).Item("nDiscount") > 0 Or p_oDTDetail(lnCtr).Item("nAddDiscx") > 0 Then
                            If pnDiscAmtV > 0 Then
                                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                                            UCase(Left(p_oDTDetail(lnCtr).Item("sBriefDsc"), 11) & "(D)").PadRight(pxeDSCLEN) + " "
                                lnQTYDiscx = lnQTYDiscx + p_oDTDetail(lnCtr).Item("nQuantity")
                                lnDisctAmt = lnDisctAmt + p_oDTDetail(lnCtr).Item("nQuantity") * p_oDTDetail(lnCtr).Item("nUnitPrce")
                                If Not lbByCategx Then lbByCategx = True
                            Else
                                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                                        UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                            End If
                        Else
                            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " +
                                        UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                        End If
                    Else
                        ls4Print = "   " & UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "
                        lnDeducQTY = lnDeducQTY + p_oDTDetail(lnCtr).Item("nQuantity")
                    End If
                End If
            Else
                ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " +
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

                ls4Print = Format(p_oDTComplx(lnCtr).Item("nQuantity") * -1, "0").PadLeft(pxeQTYLEN) + " " + _
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
                If Not lbByCategx Then
                    builder.Append("       " & ("(" & CDbl(pnDiscRteV) & "%)").PadRight(18) & " " & Format((pnDiscAmtV - pnAddDiscV), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                Else
                    builder.Append("       " & ("(" & CDbl(pnDiscRteV) & "%)" & " * P" & lnDisctAmt).PadRight(18) & " " & Format((pnDiscAmtV - (Math.Abs(pnAddDiscV * lnQTYDiscx))), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                End If

                'builder.Append("       " & ("(" & Format(Math.Round(pnDiscRteV), "#0.0") & "%)").PadRight(18) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                'builder.Append("       " & ("(" & Format(Math.Round(pnDiscRteV), "#0.0") & "%)").PadRight(18) & " " & Format(pnTotalDue * (pnDiscRteV / 100), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                lsLess = "       "

                If pnAddDiscV > 0 Then
                    If Not lbByCategx Then
                        builder.Append(("       " & "P" & Math.Round(pnAddDiscV) & "  Discount").PadRight(25) & " " & Format(pnAddDiscV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                    Else
                        builder.Append(("       " & "P" & Math.Round(pnAddDiscV) & " * " & lnQTYDiscx & "  Discount").PadRight(25) & " " & Format(pnAddDiscV * lnQTYDiscx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                    End If

                    lsLess = "       "
                End If
            Else
                If pnAddDiscV > 0 Then
                    builder.Append(lsLess & p_oDTDiscnt(0).Item("sDiscCard") & Environment.NewLine)
                    If Not lbByCategx Then
                        builder.Append(("       " & "P" & Math.Round(pnAddDiscV) & " Discount").PadRight(25) & " " & Format(pnAddDiscV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                    Else
                        builder.Append(("       " & "P" & Math.Round(pnAddDiscV) & " * " & lnQTYDiscx & " Discount").PadRight(25) & " " & Format(pnAddDiscV * lnQTYDiscx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                    End If

                    lsLess = "       "
                End If
            End If

            builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
            'builder.Append(" Net Sales (w/o VAT)".PadRight(25) & " " & Format(lnVATExWDsc, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'builder.Append(" Add: VAT".PadRight(25) & " " & Format(lnVATExWDsc * 0.12, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            'builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
        ElseIf pnDiscAmtN > 0 Then
            'orig code
            'builder.Append(" Less: Senior/PWD DSC".PadRight(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            Dim lnVATablex As Decimal = 0
            Dim lnNVATable As Decimal = 0
            Dim lnDiscAmtN As Decimal = computePWDSC(lnVATablex, lnNVATable)
            Dim lnSCPWAmtx As Decimal = (pnTotalDue / pnNoClient) * p_nWithDisc
            Dim lnSCPWNetx As Decimal = lnSCPWAmtx - (lnSCPWAmtx - (lnSCPWAmtx / lnVatPerc))

            'If p_nNoClient <> p_nWithDisc Then
            '    builder.Append(" Price Inclusive of VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            '    lnExVATDue = ((pnTotalDue / pnNoClient) * p_nWithDisc) / 1.12
            '    builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'Else
            '    builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            'End If

            'builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            If p_nNoClient <> p_nWithDisc Then
                builder.Append(" SC/PWD Sales".PadRight(25) & " " & Format(lnSCPWAmtx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Less: 12% VAT".PadRight(25) & " " & Format(lnSCPWAmtx - lnNVATable, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" VAT Exempt Sales".PadRight(25) & " " & Format(lnNVATable, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Net Sales SC/PWD".PadRight(25) & " " & Format(Math.Floor(100 * (lnSCPWNetx - (lnSCPWNetx * 0.2))) / 100, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Regular Sales".PadRight(25) & " " & Format(pnTotalDue - lnSCPWAmtx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

                'builder.Append(" Price Inclusive of VAT".PadRight(25) & " " & Format(lnVATablex, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                'builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            Else
                'SC/PWD Sales		 486.67 
                'less 12 vat		 52.15 
                'Vat Exempt Sales		 434.52 
                'less: 20% sc/pwd		 86.90 
                'SC/PWD Sales, net		 347.62 
                'Regular Sales		 243.33 
                builder.Append(" Less: 12% VAT".PadRight(25) & " " & Format(pnTotalDue - lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" VAT Exempt Sales".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                builder.Append(" Less: 20% SC/PWD Disc.".PadRight(25) & " " & Format(lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

                'builder.Append(" Price Exclusive of VAT".PadRight(25) & " " & Format(lnExVATDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If

            If pnSChargex > 0 Then
                builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)
                builder.Append(" Total Due".PadRight(25) & " " & Format(lnVATablex + (lnSCPWNetx - lnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
                'builder.Append(" Total Due".PadRight(25) & " " & Format((lnVATablex + lnExVATDue) - lnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
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

        builder.Append(RawPrint.pxePRINT_EMP1)          'Double Strike + Condense + Emphasize
        'Print Amount Due By subracting the discounts
        builder.Append(" TOTAL AMOUNT DUE :".PadRight(25) & " " & Format(((pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN)) * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        If p_cSplitTyp <> 2 Then
            Dim lnCurSplit As Integer = 0
            Dim lnCtr As Integer
            Dim loDT As DataTable = getSplitTable(psSourceNo)
            Dim lsPartial As String

            For lnCtr = 0 To loDT.Rows.Count - 1
                If loDT.Rows(lnCtr).Item("cTranStat") = xeTranStat.TRANS_POSTED Then
                    lsPartial = " PAID " & "(OR" & loDT.Rows(lnCtr).Item("sORNumber") & ")"
                    builder.Append(lsPartial.PadRight(28) & " " & "-" & Format(loDT.Rows(lnCtr).Item("nSalesAmt"), xsDECIMAL) & "".PadLeft(pxeREGLEN) & Environment.NewLine)
                    lnCurSplit = lnCurSplit + 1
                End If
            Next

            lsPartial = " Partial Bill " & "(" & lnCurSplit + 1 & "/" & loDT.Rows.Count & ")"
            builder.Append(lsPartial.PadRight((Len(lsPartial) + 14) - Len(Format(pnSplitAmt, xsDECIMAL))) & " " & Format(pnSplitAmt, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            lnSplitAmt = pnSplitAmt
        End If

        builder.Append(RawPrint.pxePRINT_EMP0)

        'Print Cash Payments
        If pnCashTotl > 0 Then
            builder.Append(" Cash".PadRight(25) & " " & Format(pnCashTotl * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If

        'Print Credit Card Payments
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
                           Format(p_oDTCredit(lnCtr).Item("nCardAmnt") * -1, xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Check Payments
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
                           Format(p_oDTChkPym(lnCtr).Item("nCheckAmt") * -1, xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Gift Coupon
        If p_oDTGftChk.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce") & " GIFT CHEQUE").PadRight(24) & " " & _
                           Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt") * -1, xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(ls4Print & Environment.NewLine)
            Next
        End If

        'Print Line Before change....
        builder.Append(" ".PadRight(25) & " " & "-".PadLeft(pxeREGLEN, "-") & Environment.NewLine)

        Dim lnChange As Decimal
        If p_cSplitTyp <> 2 Then
            lnChange = (pnSplitAmt + pnSChargex) - (pnDiscAmtV + pnDiscAmtN)
        Else
            lnChange = (pnTotalDue + pnSChargex) - (pnDiscAmtV + pnDiscAmtN)
        End If

        If pnGiftTotl > lnChange Then
            lnChange = 0
        Else
            lnChange = (pnCashTotl + pnChckTotl + pnCrdtTotl + pnGiftTotl) - lnChange
        End If

        builder.Append(RawPrint.pxePRINT_EMP1)          'Double Strike + Condense + Emphasize
        builder.Append(" CHANGE           :".PadRight(25) & " " & Format(lnChange * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_EMP0)          'Double Strike + Condense + Emphasize

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

        'pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
        'pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle

        If p_cSplitTyp <> 2 Then
            If pnDiscAmtV > 0 Then
                pnVatblSle = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
                pnVatAmntx = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle
            ElseIf pnDiscAmtN > 0 Then
                pnVatblSle = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + (pnVatExSle * lnVatPerc))) / lnVatPerc
                pnVatAmntx = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + (pnVatExSle * lnVatPerc))) - pnVatblSle
            Else
                pnVatblSle = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
                pnVatAmntx = ((lnSplitAmt + pnSChargex) - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle
            End If
        Else
            If pnDiscAmtV > 0 Then
                pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
                pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle
            ElseIf pnDiscAmtN > 0 Then
                pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + (pnVatExSle * lnVatPerc))) / lnVatPerc
                pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + (pnVatExSle * lnVatPerc))) - pnVatblSle
            Else
                pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) / lnVatPerc
                pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnZroRtSle + pnVatExSle + pnDiscAmtN)) - pnVatblSle
            End If
        End If

        'Print VAT Related info
        builder.Append("  VAT Exempt Sales      " & Format(pnVatExSle * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  Zero-Rated Sales      " & Format(pnZroRtSle * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VATable Sales         " & Format(pnVatblSle * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append("  VAT Amount            " & Format(pnVatAmntx * -1, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine & Environment.NewLine)

        builder.Append(Environment.NewLine & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append("____________________________" & Environment.NewLine)
        builder.Append("Customer Signature" & Environment.NewLine & Environment.NewLine & Environment.NewLine)

        builder.Append("____________________________" & Environment.NewLine)
        builder.Append("Voided/Cancelled/Refunded By" & Environment.NewLine & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_LEFT)

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

        Return True
    End Function

    Private Function getSplitTable(ByVal fsSourceNo As String) As DataTable
        Dim loDT As DataTable

        loDT = p_oApp.ExecuteQuery("SELECT" &
                                        "  b.sORNumber" &
                                        ", b.nSalesAmt" &
                                        ", a.cTranStat" &
                                    " FROM Order_Split a" &
                                        " LEFT JOIN 
                                        _Master b" &
                                            " ON a.sTransNox = b.sSourceNo" &
                                            " AND b.sSourceCd = 'SOSp'" &
                                    " WHERE a.sReferNox = " & strParm(fsSourceNo) &
                                    " ORDER BY b.sORNumber" &
                                        ", a.sTransNox")
        Return loDT
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

    Public Function getCashier(ByVal sCashierx As String) As String
        Dim lsSQL As String
        Dim lsCashierNm
        Dim loDta As DataTable

        lsSQL = "SELECT" & _
                    " a.sUserName" & _
                    " FROM xxxSysUser a" & _
                    " WHERE a.sUserIDxx = " & strParm(sCashierx)

        loDta = p_oApp.ExecuteQuery(lsSQL)
        If loDta.Rows.Count = 0 Then
            lsCashierNm = ""
        Else
            lsCashierNm = Decrypt(loDta(0).Item("sUserName"), xsSignature)
        End If

        loDta = Nothing

        Return lsCashierNm

    End Function

    Private Sub createDetail()
        p_oDTDetail = New DataTable("Detail")
        p_oDTDetail.Columns.Add("nQuantity", System.Type.GetType("System.Int16"))
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 14
        p_oDTDetail.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nTotlAmnt", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("cDetailxx", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("cWthPromo", System.Type.GetType("System.String")).MaxLength = 1
        'Consider All Sales to be VATABLE
        p_oDTDetail.Columns.Add("cVatablex", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("nDiscount", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nAddDiscx", System.Type.GetType("System.Decimal"))

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
        p_oDTDiscnt.Columns.Add("sClientNm", System.Type.GetType("System.String")).MaxLength = 120 ' Jovan 2021-03-12
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

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
