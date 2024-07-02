'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Terminal Z Printing Object
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
'  kalyptus [ 01/03/2017 05:03 pm ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports ADODB
Imports ggcAppDriver
Imports ggcReceipt

Public Class PRN_TZ_Reading
    Private p_oApp As GRider

    Private p_oDTMaster As DataTable

    Private p_sPOSNo As String      'MIN:       14121419321782091
    Private p_sVATReg As String     'TIN:       941-184-389-000
    Private p_sCompny As String     'Company  : MONARK HOTEL

    Private p_sPermit As String     'Permit No: PR122014-004-D004507-000
    Private p_sSerial As String     'Serial No: L9GF261769
    Private p_sAccrdt As String     'Accrdt No: 038-227471337-000028
    Private p_sTermnl As String     'Termnl No: 02
    Private p_nZRdCtr As Integer
    Private p_bWasRLCClt As Boolean

    'jovan 2020-11-07
    Private psCashierNm As String

    Private p_bBackEnd As Boolean = False

    Private Const pxeLFTMGN As Integer = 3

    Private Const p_sMasTable As String = "Daily_Summary"
    Private Const p_sMsgHeadr As String = "Daily Summary"

    Public Property CashierName() As String
        Get
            Return psCashierNm
        End Get
        Set(ByVal value As String)
            psCashierNm = value
        End Set
    End Property

    WriteOnly Property isBackend() As Boolean
        Set(ByVal value As Boolean)
            p_bBackEnd = value
        End Set
    End Property

    Public Function PrintTZReading(ByVal sFromDate As String,
                                   ByVal sThruDate As String,
                                   ByVal sCRMNumbr As String,
                                   ByVal bBackendx As Boolean,
                                   Optional nZReadCtr As Integer = 0) As Boolean
        'Get configuration of machine
        If Not initMachine() Then
            Return False
        End If
        If bBackendx Then p_nZRdCtr = nZReadCtr

        'print daily sales
        If Not doPrintTZReading(sFromDate, sThruDate, sCRMNumbr) Then
            MsgBox("Unable to perform Terminal Z Reading!!", , p_sMsgHeadr)
            Return False
        Else
            If Not bBackendx Then
                Call doWriteTZReading(sFromDate, sThruDate, sCRMNumbr)

                'Update the reset counter(nZReadCtr) at the Cash_Reg_Machine table
                Dim lsSQL As String
                lsSQL = "UPDATE Cash_Reg_Machine" &
                    " SET nZReadCtr = " & p_nZRdCtr &
                        ", nEODCtrxx = nEODCtrxx + 1" &
                    " WHERE sIDNumber = " & strParm(sCRMNumbr)
                p_oApp.Execute(lsSQL, "Cash_Reg_Machine")

                lsSQL = "UPDATE Daily_Summary" &
                    " SET cTranStat = '2'" &
                        ", nZReadCtr = nZReadCtr + 1" &
                    " WHERE sCRMNumbr = " & strParm(sCRMNumbr) &
                        " AND sTranDate BETWEEN " & strParm(sFromDate) & " AND " & strParm(sThruDate)

                p_oApp.Execute(lsSQL, "Daily_Summary")

                lsSQL = "UPDATE Table_Master" &
                               " SET cStatusxx = '0'" &
                                    ", dReserved = NULL" &
                                    ", nOccupnts = 0"
                p_oApp.Execute(lsSQL, "Table_Master")

                If p_bWasRLCClt Then
                    Dim loRLC As PRN_RLC_Reading

                    loRLC = New PRN_RLC_Reading(p_oApp)
                    If loRLC.generateRLC(sFromDate, sThruDate, p_sPOSNo) Then
                        MsgBox("End of Day Transaction Summary successfully printed.", MsgBoxStyle.Information, "Notice")
                    End If
                End If

                MsgBox("Z-Reading was perform successfully!!", , p_sMsgHeadr)
                p_oApp.SaveEvent("0022", "Date: " & sFromDate & " to " & sThruDate, p_sTermnl)
            End If
        End If

        Return True
    End Function

    'Prints the result of Terminal Reading/DAILY SALES SUMMARY
    Private Function doPrintTZReading(ByVal sFromDate As String, ByVal sThruDate As String, ByVal sCRMNumbr As String) As Boolean
        Dim lsSQL As String
        lsSQL = AddCondition(getSQ_Master, "sTranDate BETWEEN " & strParm(sFromDate) & " AND " & strParm(sThruDate) & _
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) & _
                                      " AND cTranStat IN ('1', '2')")

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            MsgBox("There are no transaction for this date....", , p_sMsgHeadr)
            Return False
        End If

        'iMac 2018.02.10
        'get previous day accumulated sale
        lsSQL = "SELECT nAccuSale FROM Daily_Summary" & _
                " WHERE sTranDate < " & strParm(sFromDate) & _
                    " AND sCRMNumbr = " & strParm(sCRMNumbr) & _
                    " AND cTranStat IN ('1', '2')" & _
                " ORDER BY dClosedxx DESC LIMIT 1"

        Dim loDT As DataTable
        Dim lnPrevSale As Decimal
        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnPrevSale = 0
        Else
            lnPrevSale = loDT(0)("nAccuSale")
        End If

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer

        'builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        p_sCompny = "The Monarch Hospitality & Tourism Corp."
        builder.Append(PadCenter(Trim(p_sCompny), 20) & Environment.NewLine)

        'Print the header
        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)

        'p_sVATReg = "469-083-682-002"
        builder.Append(PadCenter("VAT REG TIN: " & p_sVATReg, 40) & Environment.NewLine)
        'p_sPOSNo = "22010313392685363"
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine)
        'p_sPermit = "FP072021-004-0313874-000"
        builder.Append(PadCenter("PTU No.: " & p_sPermit, 40) & Environment.NewLine)
        'p_sSerial = "WCC6Y5NUS72X"
        builder.Append(PadCenter("Serial No. : " & p_sSerial, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append("Z-READING" & Environment.NewLine)

        'Get the transaction date thru reverse formatting the sTrandate field
        sFromDate = Left(sFromDate, 4) & "-" & Mid(sFromDate, 5, 2) & "-" & Right(sFromDate, 2)
        sThruDate = Left(sThruDate, 4) & "-" & Mid(sThruDate, 5, 2) & "-" & Right(sThruDate, 2)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Cashier
        builder.Append(Environment.NewLine)
        builder.Append("DATE      :" & Format(CDate(sFromDate), "dd-MMM-yyyy") & " to " & Format(CDate(sThruDate), "dd-MMM-yyyy") & Environment.NewLine)
        builder.Append("TERMINAL  :" & p_sTermnl & Environment.NewLine)
        'builder.Append("TERMINAL #:" & p_sSerial & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim lnOpenBalx As Decimal = 0
        Dim lnCPullOut As Decimal = 0

        Dim lnCashAmnt As Decimal = 0
        Dim lnSChargex As Decimal = 0
        Dim lnChckAmnt As Decimal = 0
        Dim lnCrdtAmnt As Decimal = 0
        Dim lnChrgAmnt As Decimal = 0
        Dim lnGiftAmnt As Decimal = 0

        Dim lnSalesAmt As Decimal = 0
        Dim lnVATSales As Decimal = 0
        Dim lnVATAmtxx As Decimal = 0
        Dim lnZeroRatd As Decimal = 0
        Dim lnNonVATxx As Decimal = 0   'Non-Vat means Vat Exempt
        Dim lnDiscount As Decimal = 0   'Regular Discount
        Dim lnVatDiscx As Decimal = 0   '12% VAT Discount
        Dim lnPWDDiscx As Decimal = 0   'Senior/PWD Discount

        Dim lnReturnsx As Decimal = 0   'Returns
        Dim lnVoidAmnt As Decimal = 0   'Void Transactions
        Dim lnVoidCntx As Integer = 0

        Dim lsORNoFrom As String = loDta(0).Item("sORNoFrom")
        Dim lsORNoThru As String = loDta(0).Item("sORNoThru")

        For lnCtr = 0 To loDta.Rows.Count - 1
            'Determing Beginning SI for this Terminal X Reading
            If loDta(lnCtr).Item("sORNoFrom") < lsORNoFrom And loDta(lnCtr).Item("sORNoFrom") <> "" Then
                lsORNoFrom = loDta(lnCtr).Item("sORNoFrom")
            End If

            'Determing Beginning SI for this Terminal X Reading
            If loDta(lnCtr).Item("sORNoThru") > lsORNoThru Then
                lsORNoThru = loDta(lnCtr).Item("sORNoThru")
            End If

            'Compute Gross Sales
            lnSalesAmt = lnSalesAmt + loDta(lnCtr).Item("nSalesAmt")

            'Compute VAT Related Sales
            lnVATSales = lnVATSales + loDta(lnCtr).Item("nVATSales")
            lnVATAmtxx = lnVATAmtxx + loDta(lnCtr).Item("nVATAmtxx")
            lnZeroRatd = lnZeroRatd + loDta(lnCtr).Item("nZeroRatd")

            'Compute Discounts
            lnDiscount = lnDiscount + loDta(lnCtr).Item("nDiscount")
            lnVatDiscx = lnVatDiscx + loDta(lnCtr).Item("nVatDiscx")
            lnPWDDiscx = lnPWDDiscx + loDta(lnCtr).Item("nPWDDiscx")

            'Compute Returns/Refunds/Void Transactions
            lnReturnsx = lnReturnsx + loDta(lnCtr).Item("nReturnsx")
            lnVoidAmnt = lnVoidAmnt + loDta(lnCtr).Item("nVoidAmnt")
            lnVoidCntx = lnVoidCntx + loDta(lnCtr).Item("nVoidCntx")

            'Compute Cashier Collection Info
            lnOpenBalx = lnOpenBalx + loDta(lnCtr).Item("nOpenBalx")
            lnCPullOut = lnCPullOut + loDta(lnCtr).Item("nCPullOut")

            lnCashAmnt = lnCashAmnt + loDta(lnCtr).Item("nCashAmnt")
            lnSChargex = lnSChargex + loDta(lnCtr).Item("nSChargex")
            lnChckAmnt = lnChckAmnt + loDta(lnCtr).Item("nChckAmnt")
            lnCrdtAmnt = lnCrdtAmnt + loDta(lnCtr).Item("nCrdtAmnt")
            lnChrgAmnt = lnChrgAmnt + loDta(lnCtr).Item("nChrgAmnt")
            lnGiftAmnt = lnGiftAmnt + loDta(lnCtr).Item("nGiftAmnt")
            lnNonVATxx = lnNonVATxx + loDta(lnCtr).Item("nNonVATxx")
        Next

        'Compute for VAT Exempt Sales
        'lnNonVATxx = (lnSalesAmt + lnSChargex) - (lnVATSales + lnZeroRatd + lnVATAmtxx + lnPWDDiscx + lnVatDiscx + lnDiscount)
        'lnNonVATxx = lnNonVATxx + lnPWDDiscx
        lnNonVATxx = lnNonVATxx
        'lnNonVATxx = lnSalesAmt - (lnVATSales + lnZeroRatd + lnVATAmtxx)

        'Print the begging and ending OR
        builder.Append(Environment.NewLine)
        builder.Append(" Beginning SI  :  " & lsORNoFrom & Environment.NewLine)
        builder.Append(" Ending SI     :  " & lsORNoThru & Environment.NewLine & Environment.NewLine)

        builder.Append(" Beginning Balance  : ".PadRight(24) & Format(lnPrevSale, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt) - ((lnDiscount - lnPWDDiscx) + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Print the Computation of NET Sales
        builder.Append(Environment.NewLine)
        builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex + (lnDiscount - lnPWDDiscx) + lnVatDiscx + lnPWDDiscx + lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        If lnSChargex > 0 Then
            builder.Append(" Less : Service Charge".PadRight(24) & Format(lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
            builder.Append("        Regular Discnt".PadRight(24) & Format(lnDiscount - lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        Else
            builder.Append(" Less : Regular Discnt".PadRight(24) & Format(lnDiscount - lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        End If

        'builder.Append("        VAT SC/PWD".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        20% SC/PWD Disc.".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        Returns".PadRight(24) & Format(lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP1)

        builder.Append(" NET SALES".PadRight(24) & Format(lnSalesAmt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append(" NET SALES".PadRight(24) & Format((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx + lnSChargex), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)

        'Display a space in between NEW Sales and VAT Related Info
        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)

        builder.Append(" VATABLE Sales".PadRight(24) & Format(lnVATSales, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Amount".PadRight(24) & Format(lnVATAmtxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ZERO Rated Sales".PadRight(24) & Format(lnZeroRatd, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between VAT Related Info and SENIOR/PWD Discount Info
        'builder.Append(Environment.NewLine)

        'builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("     Less: 20% Discount:".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("           Less 12% VAT:".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between SENIOR/PWD Discount Info & Collection Info
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append(" Collection Info:" & Environment.NewLine)
        builder.Append("  Petty Cash".PadRight(24) & Format(lnOpenBalx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Withdrawal".PadRight(24) & Format(lnCPullOut, xsDECIMAL).PadLeft(13) & Environment.NewLine & Environment.NewLine)

        'builder.Append("  Cashbox Amount".PadRight(24) & Format(lnOpenBalx + (lnCashAmnt + lnSChargex) - lnCPullOut - lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cash".PadRight(24) & Format((lnOpenBalx + lnCashAmnt) - (lnCPullOut + lnReturnsx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cheque".PadRight(24) & Format(lnChckAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Credit Card".PadRight(24) & Format(lnCrdtAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Gift Cheque".PadRight(24) & Format(lnGiftAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("  Company Accounts".PadRight(24) & Format(lnChrgAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append("              Z-COUNTER : ".PadRight(26) & p_nZRdCtr.ToString.PadLeft(11) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP1)
        'builder.Append("ACCUMULATED GRAND TOTAL : ".PadRight(26) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("ACCUMULATED GRAND TOTAL : ".PadRight(26) & Format(lnPrevSale + ((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" Void SI Count: ".PadRight(24) & Format(lnVoidCntx, xsINTEGER).PadLeft(13) & Environment.NewLine)
        builder.Append(" Void SI Amount: ".PadRight(24) & Format(lnVoidAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        'Dim cashier_printer As String = "\\192.168.10.12\EPSON TM-U220 Receipt"
        Dim cashier_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_CS")

        'Print the designation printer location...
        RawPrint.SendStringToPrinter(cashier_printer, builder.ToString())

        Return True
    End Function

    Public Function doPrintTZReadingReg(ByVal sFromDate As String,
                                        ByVal sThruDate As String,
                                        ByVal sCRMNumbr As String,
                                        ByVal nZReading As Integer) As Boolean

        If Not initMachine() Then
            Return False
        End If

        Dim lsSQL As String
        lsSQL = AddCondition(getSQ_Master, "sTranDate BETWEEN " & strParm(sFromDate) & " AND " & strParm(sThruDate) &
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                      " AND cTranStat IN ('1', '2')")

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        'iMac 2018.02.10
        'get previous day accumulated sale
        lsSQL = "SELECT nAccuSale FROM Daily_Summary" &
                " WHERE sTranDate < " & strParm(sFromDate) &
                    " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                    " AND cTranStat IN ('1', '2')" &
                " ORDER BY dClosedxx DESC LIMIT 1"

        Dim loDT As DataTable
        Dim lnPrevSale As Decimal
        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnPrevSale = 0
        Else
            lnPrevSale = loDT(0)("nAccuSale")
        End If

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer

        'builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        p_sCompny = "The Monarch Hospitality & Tourism Corp."
        builder.Append(PadCenter(Trim(p_sCompny), 20) & Environment.NewLine)

        'Print the header
        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)

        p_sVATReg = "469-083-682-002"
        builder.Append(PadCenter("VAT REG TIN: " & p_sVATReg, 40) & Environment.NewLine)
        p_sPOSNo = "22010313392685364"
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine)
        p_sPermit = "FP012022-004-0313875-000"
        builder.Append(PadCenter("PTU No.: " & p_sPermit, 40) & Environment.NewLine)
        p_sSerial = "WCC6Y4VEA7V0"
        builder.Append(PadCenter("Serial No. : " & p_sSerial, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append("Z-READING" & Environment.NewLine)

        'Get the transaction date thru reverse formatting the sTrandate field
        sFromDate = Left(sFromDate, 4) & "-" & Mid(sFromDate, 5, 2) & "-" & Right(sFromDate, 2)
        sThruDate = Left(sThruDate, 4) & "-" & Mid(sThruDate, 5, 2) & "-" & Right(sThruDate, 2)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Cashier
        builder.Append(Environment.NewLine)
        builder.Append("DATE      :" & Format(CDate(sFromDate), "dd-MMM-yyyy") & " to " & Format(CDate(sThruDate), "dd-MMM-yyyy") & Environment.NewLine)
        builder.Append("TERMINAL  :" & p_sTermnl & Environment.NewLine)
        'builder.Append("TERMINAL #:" & p_sSerial & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim lnOpenBalx As Decimal = 0
        Dim lnCPullOut As Decimal = 0

        Dim lnCashAmnt As Decimal = 0
        Dim lnSChargex As Decimal = 0
        Dim lnChckAmnt As Decimal = 0
        Dim lnCrdtAmnt As Decimal = 0
        Dim lnChrgAmnt As Decimal = 0
        Dim lnGiftAmnt As Decimal = 0

        Dim lnSalesAmt As Decimal = 0
        Dim lnVATSales As Decimal = 0
        Dim lnVATAmtxx As Decimal = 0
        Dim lnZeroRatd As Decimal = 0
        Dim lnNonVATxx As Decimal = 0   'Non-Vat means Vat Exempt
        Dim lnDiscount As Decimal = 0   'Regular Discount
        Dim lnVatDiscx As Decimal = 0   '12% VAT Discount
        Dim lnPWDDiscx As Decimal = 0   'Senior/PWD Discount

        Dim lnReturnsx As Decimal = 0   'Returns
        Dim lnVoidAmnt As Decimal = 0   'Void Transactions
        Dim lnVoidCntx As Integer = 0

        Dim lsORNoFrom As String = loDta(0).Item("sORNoFrom")
        Dim lsORNoThru As String = loDta(0).Item("sORNoThru")

        For lnCtr = 0 To loDta.Rows.Count - 1
            'Determing Beginning SI for this Terminal X Reading
            If loDta(lnCtr).Item("sORNoFrom") < lsORNoFrom And loDta(lnCtr).Item("sORNoFrom") <> "" Then
                lsORNoFrom = loDta(lnCtr).Item("sORNoFrom")
            End If

            'Determing Beginning SI for this Terminal X Reading
            If loDta(lnCtr).Item("sORNoThru") > lsORNoThru Then
                lsORNoThru = loDta(lnCtr).Item("sORNoThru")
            End If

            'Compute Gross Sales
            lnSalesAmt = lnSalesAmt + loDta(lnCtr).Item("nSalesAmt")

            'Compute VAT Related Sales
            lnVATSales = lnVATSales + loDta(lnCtr).Item("nVATSales")
            lnVATAmtxx = lnVATAmtxx + loDta(lnCtr).Item("nVATAmtxx")
            lnZeroRatd = lnZeroRatd + loDta(lnCtr).Item("nZeroRatd")

            'Compute Discounts
            lnDiscount = lnDiscount + loDta(lnCtr).Item("nDiscount")
            lnVatDiscx = lnVatDiscx + loDta(lnCtr).Item("nVatDiscx")
            lnPWDDiscx = lnPWDDiscx + loDta(lnCtr).Item("nPWDDiscx")

            'Compute Returns/Refunds/Void Transactions
            lnReturnsx = lnReturnsx + loDta(lnCtr).Item("nReturnsx")
            lnVoidAmnt = lnVoidAmnt + loDta(lnCtr).Item("nVoidAmnt")
            lnVoidCntx = lnVoidCntx + loDta(lnCtr).Item("nVoidCntx")

            'Compute Cashier Collection Info
            lnOpenBalx = lnOpenBalx + loDta(lnCtr).Item("nOpenBalx")
            lnCPullOut = lnCPullOut + loDta(lnCtr).Item("nCPullOut")

            lnCashAmnt = lnCashAmnt + loDta(lnCtr).Item("nCashAmnt")
            lnSChargex = lnSChargex + loDta(lnCtr).Item("nSChargex")
            lnChckAmnt = lnChckAmnt + loDta(lnCtr).Item("nChckAmnt")
            lnCrdtAmnt = lnCrdtAmnt + loDta(lnCtr).Item("nCrdtAmnt")
            lnChrgAmnt = lnChrgAmnt + loDta(lnCtr).Item("nChrgAmnt")
            lnGiftAmnt = lnGiftAmnt + loDta(lnCtr).Item("nGiftAmnt")
            lnNonVATxx = lnNonVATxx + loDta(lnCtr).Item("nNonVATxx")
        Next

        'Compute for VAT Exempt Sales
        'lnNonVATxx = (lnSalesAmt + lnSChargex) - (lnVATSales + lnZeroRatd + lnVATAmtxx + lnPWDDiscx + lnVatDiscx + lnDiscount)
        'lnNonVATxx = lnNonVATxx + lnPWDDiscx

        lnNonVATxx = lnNonVATxx
        'lnNonVATxx = lnSalesAmt - (lnVATSales + lnZeroRatd + lnVATAmtxx)

        'Print the begging and ending OR
        builder.Append(Environment.NewLine)
        builder.Append(" Beginning SI  :  " & lsORNoFrom & Environment.NewLine)
        builder.Append(" Ending SI     :  " & lsORNoThru & Environment.NewLine & Environment.NewLine)

        builder.Append(" Beginning Balance  : ".PadRight(24) & Format(lnPrevSale, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Print the Computation of NET Sales
        builder.Append(Environment.NewLine)
        builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        If lnSChargex > 0 Then
            builder.Append(" Less : Service Charge".PadRight(24) & Format(lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
            builder.Append("        Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        Else
            builder.Append(" Less : Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        End If

        builder.Append("        VAT SC/PWD".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        20% SC/PWD Disc.".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        Returns".PadRight(24) & Format(lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP1)
        builder.Append(" NET SALES".PadRight(24) & Format((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx + lnSChargex), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)

        'Display a space in between NEW Sales and VAT Related Info
        builder.Append(Environment.NewLine)

        builder.Append(" VATABLE Sales".PadRight(24) & Format(lnVATSales, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Amount".PadRight(24) & Format(lnVATAmtxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ZERO Rated Sales".PadRight(24) & Format(lnZeroRatd, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between VAT Related Info and SENIOR/PWD Discount Info
        'builder.Append(Environment.NewLine)

        'builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("     Less: 20% Discount:".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("           Less 12% VAT:".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between SENIOR/PWD Discount Info & Collection Info
        builder.Append(Environment.NewLine)

        builder.Append(" Collection Info:" & Environment.NewLine)
        builder.Append("  Petty Cash".PadRight(24) & Format(lnOpenBalx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Withdrawal".PadRight(24) & Format(lnCPullOut, xsDECIMAL).PadLeft(13) & Environment.NewLine & Environment.NewLine)

        'builder.Append("  Cashbox Amount".PadRight(24) & Format(lnOpenBalx + (lnCashAmnt + lnSChargex) - lnCPullOut - lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cash".PadRight(24) & Format((lnOpenBalx + lnCashAmnt) - (lnCPullOut + lnReturnsx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cheque".PadRight(24) & Format(lnChckAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Credit Card".PadRight(24) & Format(lnCrdtAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Gift Cheque".PadRight(24) & Format(lnGiftAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("  Company Accounts".PadRight(24) & Format(lnChrgAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append("              Z-COUNTER : ".PadRight(26) & nZReading.ToString.PadLeft(11) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP1)
        'builder.Append("ACCUMULATED GRAND TOTAL : ".PadRight(26) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("ACCUMULATED GRAND TOTAL : ".PadRight(26) & Format(lnPrevSale + ((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" Void SI Count: ".PadRight(24) & Format(lnVoidCntx, xsINTEGER).PadLeft(13) & Environment.NewLine)
        builder.Append(" Void SI Amount: ".PadRight(24) & Format(lnVoidAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim cashier_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_CS")
        'Dim cashier_printer As String = "\\192.168.10.12\EPSON TM-U220 Receipt"

        'Print the designation printer location...
        RawPrint.SendStringToPrinter(cashier_printer, builder.ToString())

        Return True
    End Function

    Private Function doWriteTZReading(ByVal sFromDate As String, ByVal sThruDate As String, ByVal sCRMNumbr As String) As Boolean
        Dim lsSQL As String
        lsSQL = AddCondition(getSQ_Master, "sTranDate BETWEEN " & strParm(sFromDate) & " AND " & strParm(sThruDate) & _
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) & _
                                      " AND cTranStat IN ('1', '2')")

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            MsgBox("There are no transaction for this date....", , p_sMsgHeadr)
            Return False
        End If

        'iMac 2018.02.10
        'get previous day accumulated sale
        lsSQL = "SELECT sTranDate, nAccuSale FROM Daily_Summary" & _
                " WHERE sTranDate < " & strParm(sFromDate) & _
                    " AND sCRMNumbr = " & strParm(sCRMNumbr) & _
                    " AND cTranStat IN ('1', '2')" & _
                " ORDER BY dClosedxx DESC LIMIT 1"

        Dim loDT As DataTable
        Dim lnPrevSale As Decimal
        Debug.Print(lsSQL)
        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnPrevSale = 0
        Else
            lnPrevSale = loDT(0)("nAccuSale")
        End If

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

        builder.Append(PadCenter("Z-READING", 40) & Environment.NewLine)

        'Get the transaction date thru reverse formatting the sTrandate field
        sFromDate = Left(sFromDate, 4) & "-" & Mid(sFromDate, 5, 2) & "-" & Right(sFromDate, 2)
        sThruDate = Left(sThruDate, 4) & "-" & Mid(sThruDate, 5, 2) & "-" & Right(sThruDate, 2)

        builder.Append(Environment.NewLine)
        If sFromDate = sThruDate Then
            builder.Append("DATE      :" & Format(CDate(sFromDate), "dd-MMM-yyyy") & " to " & Format(CDate(sThruDate), "dd-MMM-yyyy") & Environment.NewLine)
        Else
            builder.Append("DATE      :" & Format(CDate(loDT.Rows(0)("sTranDate")), "dd-MMM-yyyy") & " to " & Format(CDate(sThruDate), "dd-MMM-yyyy") & Environment.NewLine)
        End If
        builder.Append("TERMINAL  :" & p_sTermnl & Environment.NewLine)
        'builder.Append("TERMINAL #:" & p_sSerial & Environment.NewLine)

        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim lnOpenBalx As Decimal = 0
        Dim lnCPullOut As Decimal = 0

        Dim lnCashAmnt As Decimal = 0
        Dim lnSChargex As Decimal = 0
        Dim lnChckAmnt As Decimal = 0
        Dim lnCrdtAmnt As Decimal = 0
        Dim lnChrgAmnt As Decimal = 0
        Dim lnGiftAmnt As Decimal = 0

        Dim lnSalesAmt As Decimal = 0
        Dim lnVATSales As Decimal = 0
        Dim lnVATAmtxx As Decimal = 0
        Dim lnZeroRatd As Decimal = 0
        Dim lnNonVATxx As Decimal = 0   'Non-Vat means Vat Exempt
        Dim lnDiscount As Decimal = 0   'Regular Discount
        Dim lnVatDiscx As Decimal = 0   '12% VAT Discount
        Dim lnPWDDiscx As Decimal = 0   'Senior/PWD Discount

        Dim lnReturnsx As Decimal = 0   'Returns
        Dim lnVoidAmnt As Decimal = 0   'Void Transactions
        Dim lnVoidCntx As Integer = 0

        Dim lsORNoFrom As String = loDta(0).Item("sORNoFrom")
        Dim lsORNoThru As String = loDta(0).Item("sORNoThru")

        For lnCtr = 0 To loDta.Rows.Count - 1
            'Determing Beginning SI for this Terminal X Reading
            If loDta(lnCtr).Item("sORNoFrom") < lsORNoFrom And loDta(lnCtr).Item("sORNoFrom") <> "" Then
                lsORNoFrom = loDta(lnCtr).Item("sORNoFrom")
            End If

            'Determing Beginning SI for this Terminal X Reading
            If loDta(lnCtr).Item("sORNoThru") > lsORNoThru Then
                lsORNoThru = loDta(lnCtr).Item("sORNoThru")
            End If

            'Compute Gross Sales
            lnSalesAmt = lnSalesAmt + loDta(lnCtr).Item("nSalesAmt")

            'Compute VAT Related Sales
            lnVATSales = lnVATSales + loDta(lnCtr).Item("nVATSales")
            lnVATAmtxx = lnVATAmtxx + loDta(lnCtr).Item("nVATAmtxx")
            lnZeroRatd = lnZeroRatd + loDta(lnCtr).Item("nZeroRatd")

            'Compute Discounts
            lnDiscount = lnDiscount + loDta(lnCtr).Item("nDiscount")
            lnVatDiscx = lnVatDiscx + loDta(lnCtr).Item("nVatDiscx")
            lnPWDDiscx = lnPWDDiscx + loDta(lnCtr).Item("nPWDDiscx")

            'Compute Returns/Refunds/Void Transactions
            lnReturnsx = lnReturnsx + loDta(lnCtr).Item("nReturnsx")
            lnVoidAmnt = lnVoidAmnt + loDta(lnCtr).Item("nVoidAmnt")
            lnVoidCntx = lnVoidCntx + loDta(lnCtr).Item("nVoidCntx")

            'Compute Cashier Collection Info
            lnOpenBalx = lnOpenBalx + loDta(lnCtr).Item("nOpenBalx")
            lnCPullOut = lnCPullOut + loDta(lnCtr).Item("nCPullOut")

            lnCashAmnt = lnCashAmnt + loDta(lnCtr).Item("nCashAmnt")
            lnSChargex = lnSChargex + loDta(lnCtr).Item("nSChargex")
            lnChckAmnt = lnChckAmnt + loDta(lnCtr).Item("nChckAmnt")
            lnCrdtAmnt = lnCrdtAmnt + loDta(lnCtr).Item("nCrdtAmnt")
            lnChrgAmnt = lnChrgAmnt + loDta(lnCtr).Item("nChrgAmnt")
            lnGiftAmnt = lnGiftAmnt + loDta(lnCtr).Item("nGiftAmnt")
            lnNonVATxx = lnNonVATxx + loDta(lnCtr).Item("nNonVATxx")
        Next

        'Compute for VAT Exempt Sales
        'lnNonVATxx = (lnSalesAmt + lnSChargex) - (lnVATSales + lnZeroRatd + lnVATAmtxx + lnPWDDiscx + lnVatDiscx + lnDiscount)
        'lnNonVATxx = lnNonVATxx + lnPWDDiscx
        lnNonVATxx = lnNonVATxx
        'lnNonVATxx = lnSalesAmt - (lnVATSales + lnZeroRatd + lnVATAmtxx)

        'Print the begging and ending OR
        builder.Append(Environment.NewLine)
        builder.Append(" Beginning SI  :  " & lsORNoFrom & Environment.NewLine)
        builder.Append(" Ending SI     :  " & lsORNoThru & Environment.NewLine & Environment.NewLine)

        builder.Append(" Beginning Balance  : ".PadRight(24) & Format(lnPrevSale, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt) - ((lnDiscount - lnPWDDiscx) + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Print the Computation of NET Sales
        builder.Append(Environment.NewLine)
        builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex + (lnDiscount - lnPWDDiscx) + lnVatDiscx + lnPWDDiscx + lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        If lnSChargex > 0 Then
            builder.Append(" Less : Service Charge".PadRight(24) & Format(lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
            builder.Append("        Regular Discnt".PadRight(24) & Format(lnDiscount - lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        Else
            builder.Append(" Less : Regular Discnt".PadRight(24) & Format(lnDiscount - lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        End If

        'builder.Append("        VAT SC/PWD".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        20% SC/PWD Disc.".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        Returns".PadRight(24) & Format(lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP1)

        builder.Append(" NET SALES".PadRight(24) & Format(lnSalesAmt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append(" NET SALES".PadRight(24) & Format((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx + lnSChargex), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)

        'Display a space in between NEW Sales and VAT Related Info
        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)

        builder.Append(" VATABLE Sales".PadRight(24) & Format(lnVATSales, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Amount".PadRight(24) & Format(lnVATAmtxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ZERO Rated Sales".PadRight(24) & Format(lnZeroRatd, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between VAT Related Info and SENIOR/PWD Discount Info
        'builder.Append(Environment.NewLine)

        'builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("     Less: 20% Discount:".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("           Less 12% VAT:".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between SENIOR/PWD Discount Info & Collection Info
        builder.Append(Environment.NewLine)

        builder.Append(" Collection Info:" & Environment.NewLine)
        builder.Append("  Petty Cash".PadRight(24) & Format(lnOpenBalx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Withdrawal".PadRight(24) & Format(lnCPullOut, xsDECIMAL).PadLeft(13) & Environment.NewLine & Environment.NewLine)

        'builder.Append("  Cashbox Amount".PadRight(24) & Format(lnOpenBalx + (lnCashAmnt + lnSChargex) - lnCPullOut - lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cash".PadRight(24) & Format((lnOpenBalx + lnCashAmnt) - (lnCPullOut + lnReturnsx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cheque".PadRight(24) & Format(lnChckAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Credit Card".PadRight(24) & Format(lnCrdtAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Gift Cheque".PadRight(24) & Format(lnGiftAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("  Company Accounts".PadRight(24) & Format(lnChrgAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append("              Z-COUNTER : ".PadRight(26) & p_nZRdCtr.ToString.PadLeft(11) & Environment.NewLine)
        'builder.Append("ACCUMULATED GRAND TOTAL : ".PadRight(26) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("ACCUMULATED GRAND TOTAL : ".PadRight(26) & Format(lnPrevSale + ((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" Void SI Count: ".PadRight(24) & Format(lnVoidCntx, xsINTEGER).PadLeft(13) & Environment.NewLine)
        builder.Append(" Void SI Amount: ".PadRight(24) & Format(lnVoidAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)

        RawPrint.writeToFile(p_sPOSNo, builder.ToString())
        If sFromDate = sThruDate Then
            RawPrint.writeToFile(p_sPOSNo & " " & Format(CDate(sFromDate), "yyyyMMdd"), builder.ToString())
            RawPrint.writeToFile(p_sPOSNo & " Z-READING" & " " & Format(CDate(sFromDate), "yyyyMMdd"), builder.ToString())
        Else
            RawPrint.writeToFile(p_sPOSNo & " " & Format(CDate(loDT.Rows(0)("sTranDate")), "yyyyMMdd"), builder.ToString())
            RawPrint.writeToFile(p_sPOSNo & " Z-READING" & " " & Format(CDate(loDT.Rows(0)("sTranDate")), "yyyyMMdd"), builder.ToString())
        End If

        Return True
    End Function

    Public Function doWriteTZReadingReg(ByVal sFromDate As String, ByVal sThruDate As String, ByVal sCRMNumbr As String, ByVal nZReading As Integer) As Boolean
        If Not initMachine() Then
            Return False
        End If

        Dim lsSQL As String
        lsSQL = AddCondition(getSQ_Master, "sTranDate BETWEEN " & strParm(sFromDate) & " AND " & strParm(sThruDate) &
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                      " AND cTranStat IN ('1', '2')")

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            MsgBox("There are no transaction for this date....", , p_sMsgHeadr)
            Return False
        End If

        'iMac 2018.02.10
        'get previous day accumulated sale
        lsSQL = "SELECT sTranDate, nAccuSale FROM Daily_Summary" &
                " WHERE sTranDate < " & strParm(sFromDate) &
                    " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                    " AND cTranStat IN ('1', '2')" &
                " ORDER BY dClosedxx DESC LIMIT 1"

        Dim loDT As DataTable
        Dim lnPrevSale As Decimal
        Debug.Print(lsSQL)
        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnPrevSale = 0
        Else
            lnPrevSale = loDT(0)("nAccuSale")
        End If

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)
        p_sCompny = "The Monarch Hospitality & Tourism Corp."
        builder.Append(PadCenter(Trim(p_sCompny), 40) & Environment.NewLine)

        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)

        p_sVATReg = "469-083-682-002"
        builder.Append(PadCenter("VAT REG TIN: " & p_sVATReg, 40) & Environment.NewLine)
        p_sPOSNo = "22010313392685364"
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine)
        p_sPermit = "FP012022-004-0313875-000"
        builder.Append(PadCenter("PTU No.: " & p_sPermit, 40) & Environment.NewLine)
        p_sSerial = "WCC6Y4VEA7V0"
        builder.Append(PadCenter("Serial No. : " & p_sSerial, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(PadCenter("Z-READING", 40) & Environment.NewLine)

        'Get the transaction date thru reverse formatting the sTrandate field
        sFromDate = Left(sFromDate, 4) & "-" & Mid(sFromDate, 5, 2) & "-" & Right(sFromDate, 2)
        sThruDate = Left(sThruDate, 4) & "-" & Mid(sThruDate, 5, 2) & "-" & Right(sThruDate, 2)

        builder.Append(Environment.NewLine)
        If sFromDate = sThruDate Then
            builder.Append("DATE      :" & Format(CDate(sFromDate), "dd-MMM-yyyy") & " to " & Format(CDate(sThruDate), "dd-MMM-yyyy") & Environment.NewLine)
        Else
            builder.Append("DATE      :" & Format(CDate(loDT.Rows(0)("sTranDate")), "dd-MMM-yyyy") & " to " & Format(CDate(sThruDate), "dd-MMM-yyyy") & Environment.NewLine)
        End If
        builder.Append("TERMINAL  :" & p_sTermnl & Environment.NewLine)
        'builder.Append("TERMINAL #:" & p_sSerial & Environment.NewLine)

        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim lnOpenBalx As Decimal = 0
        Dim lnCPullOut As Decimal = 0

        Dim lnCashAmnt As Decimal = 0
        Dim lnSChargex As Decimal = 0
        Dim lnChckAmnt As Decimal = 0
        Dim lnCrdtAmnt As Decimal = 0
        Dim lnChrgAmnt As Decimal = 0
        Dim lnGiftAmnt As Decimal = 0

        Dim lnSalesAmt As Decimal = 0
        Dim lnVATSales As Decimal = 0
        Dim lnVATAmtxx As Decimal = 0
        Dim lnZeroRatd As Decimal = 0
        Dim lnNonVATxx As Decimal = 0   'Non-Vat means Vat Exempt
        Dim lnDiscount As Decimal = 0   'Regular Discount
        Dim lnVatDiscx As Decimal = 0   '12% VAT Discount
        Dim lnPWDDiscx As Decimal = 0   'Senior/PWD Discount

        Dim lnReturnsx As Decimal = 0   'Returns
        Dim lnVoidAmnt As Decimal = 0   'Void Transactions
        Dim lnVoidCntx As Integer = 0

        Dim lsORNoFrom As String = loDta(0).Item("sORNoFrom")
        Dim lsORNoThru As String = loDta(0).Item("sORNoThru")

        For lnCtr = 0 To loDta.Rows.Count - 1
            'Determing Beginning SI for this Terminal X Reading
            If loDta(lnCtr).Item("sORNoFrom") < lsORNoFrom And loDta(lnCtr).Item("sORNoFrom") <> "" Then
                lsORNoFrom = loDta(lnCtr).Item("sORNoFrom")
            End If

            'Determing Beginning SI for this Terminal X Reading
            If loDta(lnCtr).Item("sORNoThru") > lsORNoThru Then
                lsORNoThru = loDta(lnCtr).Item("sORNoThru")
            End If

            'Compute Gross Sales
            lnSalesAmt = lnSalesAmt + loDta(lnCtr).Item("nSalesAmt")

            'Compute VAT Related Sales
            lnVATSales = lnVATSales + loDta(lnCtr).Item("nVATSales")
            lnVATAmtxx = lnVATAmtxx + loDta(lnCtr).Item("nVATAmtxx")
            lnZeroRatd = lnZeroRatd + loDta(lnCtr).Item("nZeroRatd")

            'Compute Discounts
            lnDiscount = lnDiscount + loDta(lnCtr).Item("nDiscount")
            lnVatDiscx = lnVatDiscx + loDta(lnCtr).Item("nVatDiscx")
            lnPWDDiscx = lnPWDDiscx + loDta(lnCtr).Item("nPWDDiscx")

            'Compute Returns/Refunds/Void Transactions
            lnReturnsx = lnReturnsx + loDta(lnCtr).Item("nReturnsx")
            lnVoidAmnt = lnVoidAmnt + loDta(lnCtr).Item("nVoidAmnt")
            lnVoidCntx = lnVoidCntx + loDta(lnCtr).Item("nVoidCntx")

            'Compute Cashier Collection Info
            lnOpenBalx = lnOpenBalx + loDta(lnCtr).Item("nOpenBalx")
            lnCPullOut = lnCPullOut + loDta(lnCtr).Item("nCPullOut")

            lnCashAmnt = lnCashAmnt + loDta(lnCtr).Item("nCashAmnt")
            lnSChargex = lnSChargex + loDta(lnCtr).Item("nSChargex")
            lnChckAmnt = lnChckAmnt + loDta(lnCtr).Item("nChckAmnt")
            lnCrdtAmnt = lnCrdtAmnt + loDta(lnCtr).Item("nCrdtAmnt")
            lnChrgAmnt = lnChrgAmnt + loDta(lnCtr).Item("nChrgAmnt")
            lnGiftAmnt = lnGiftAmnt + loDta(lnCtr).Item("nGiftAmnt")
            lnNonVATxx = lnNonVATxx + loDta(lnCtr).Item("nNonVATxx")
        Next

        'Compute for VAT Exempt Sales
        'lnNonVATxx = (lnSalesAmt + lnSChargex) - (lnVATSales + lnZeroRatd + lnVATAmtxx + lnPWDDiscx + lnVatDiscx + lnDiscount)
        'lnNonVATxx = lnNonVATxx + lnPWDDiscx
        lnNonVATxx = lnNonVATxx
        'lnNonVATxx = lnSalesAmt - (lnVATSales + lnZeroRatd + lnVATAmtxx)

        'Print the begging and ending OR
        builder.Append(Environment.NewLine)
        builder.Append(" Beginning SI  :  " & lsORNoFrom & Environment.NewLine)
        builder.Append(" Ending SI     :  " & lsORNoThru & Environment.NewLine)

        builder.Append(Environment.NewLine)
        builder.Append(" Beginning Balance  : ".PadRight(24) & Format(lnPrevSale, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Print the Computation of NET Sales
        builder.Append(Environment.NewLine)
        builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        If lnSChargex > 0 Then
            builder.Append(" Less : Service Charge".PadRight(24) & Format(lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
            builder.Append("        Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        Else
            builder.Append(" Less : Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        End If

        builder.Append("        VAT SC/PWD".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        20% SC/PWD Disc.".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        Returns".PadRight(24) & Format(lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)

        builder.Append(" NET SALES".PadRight(24) & Format((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx + lnSChargex), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Display a space in between NEW Sales and VAT Related Info
        builder.Append(Environment.NewLine)

        builder.Append(" VATABLE Sales".PadRight(24) & Format(lnVATSales, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Amount".PadRight(24) & Format(lnVATAmtxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ZERO Rated Sales".PadRight(24) & Format(lnZeroRatd, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between VAT Related Info and SENIOR/PWD Discount Info
        'builder.Append(Environment.NewLine)

        'builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("     Less: 20% Discount:".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("           Less 12% VAT:".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between SENIOR/PWD Discount Info & Collection Info
        builder.Append(Environment.NewLine)

        builder.Append(" Collection Info:" & Environment.NewLine)
        builder.Append("  Petty Cash".PadRight(24) & Format(lnOpenBalx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Withdrawal".PadRight(24) & Format(lnCPullOut, xsDECIMAL).PadLeft(13) & Environment.NewLine & Environment.NewLine)

        'builder.Append("  Cashbox Amount".PadRight(24) & Format(lnOpenBalx + (lnCashAmnt + lnSChargex) - lnCPullOut - lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cash".PadRight(24) & Format((lnOpenBalx + lnCashAmnt) - (lnCPullOut + lnReturnsx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cheque".PadRight(24) & Format(lnChckAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Credit Card".PadRight(24) & Format(lnCrdtAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Gift Cheque".PadRight(24) & Format(lnGiftAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("  Company Accounts".PadRight(24) & Format(lnChrgAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append("              Z-COUNTER : ".PadRight(26) & nZReading.ToString.PadLeft(11) & Environment.NewLine)
        'builder.Append("ACCUMULATED GRAND TOTAL : ".PadRight(26) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("ACCUMULATED GRAND TOTAL : ".PadRight(26) & Format(lnPrevSale + ((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" Void SI Count: ".PadRight(24) & Format(lnVoidCntx, xsINTEGER).PadLeft(13) & Environment.NewLine)
        builder.Append(" Void SI Amount: ".PadRight(24) & Format(lnVoidAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)

        RawPrint.writeToFile(p_sPOSNo & " Z-READING" & " " & Format(CDate(sFromDate), "yyyyMMdd"), builder.ToString())

        Return True
    End Function

    Private Function initMachine() As Boolean
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
                       ", nZReadCtr" &
                       ", cRLCPOSxx" &
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
        p_nZRdCtr = loDta(0).Item("nZReadCtr") + 1
        p_bWasRLCClt = IIf(loDta(0).Item("cRLCPOSxx") = 0, False, True)

        Return True
    End Function

    Private Function PadCenter(source As String, length As Integer) As String
        Dim spaces As Integer = length - source.Length
        Dim padLeft As Integer = spaces / 2 + source.Length
        Return source.PadLeft(padLeft, " ").PadRight(length, " ")
    End Function

    Private Function getSQ_Master() As String
        Return "SELECT a.sTranDate" & _
                    ", a.sCRMNumbr" & _
                    ", a.sCashierx" & _
                    ", a.nOpenBalx" & _
                    ", a.nCPullOut" & _
                    ", a.nSalesAmt" & _
                    ", a.nVATSales" & _
                    ", a.nVATAmtxx" & _
                    ", a.nNonVATxx" & _
                    ", a.nZeroRatd" & _
                    ", a.nDiscount" & _
                    ", a.nPWDDiscx" & _
                    ", a.nVatDiscx" & _
                    ", a.nReturnsx" & _
                    ", a.nVoidAmnt" & _
                    ", a.nAccuSale" & _
                    ", a.nCashAmnt" & _
                    ", a.nChckAmnt" & _
                    ", a.nCrdtAmnt" & _
                    ", a.nChrgAmnt" & _
                    ", a.nSChargex" & _
                    ", a.sORNoFrom" & _
                    ", a.sORNoThru" & _
                    ", a.nZReadCtr" & _
                    ", a.nGiftAmnt" & _
                    ", a.cTranStat" & _
                    ", a.nVoidCntx" & _
                " FROM " & p_sMasTable & " a" & _
                " ORDER BY sTranDate ASC"

    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTMaster = Nothing

        p_sPOSNo = Environment.GetEnvironmentVariable("RMS-CRM-No")      'MIN
        p_sVATReg = Environment.GetEnvironmentVariable("REG-TIN-No")     'VAT REG No.
        p_sCompny = Environment.GetEnvironmentVariable("RMS-CLT-NM")
    End Sub
End Class
