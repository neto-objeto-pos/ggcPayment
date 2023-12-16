'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Credit Card
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
Imports ggcAppDriver
Imports ggcRetailParams
Imports MySql.Data.MySqlClient

Public Class CreditCardNeo

#Region "Constant"
    Private Const pxeMODULENAME As String = "CreditCard"
    Private Const pxeMasterTble As String = "Credit_Card_Trans"

#End Region

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oBank As clsBanks
    Protected p_oTerm As clsTerm
    Protected p_oTerminal As clsBanks
    Protected p_oDataTable As DataTable
    Protected p_sSQLMaster As String
    Protected p_nEditMode As xeEditMode
    Protected p_sBranchCd As String
    Protected p_sSourceNo As String
    Protected p_sSourceCd As String
    Protected p_sPOSNo As String

    Protected p_sBankName As String
    Protected p_sTermnlNm As String
    Protected p_sTermName As String
    Protected p_bCloseForm As Boolean
    Protected p_sTransNox As String
#End Region

#Region "Event"
    Public Event MasterRetrieved(ByVal Row As Integer,
                                 ByVal Index As Object,
                                 ByVal Value As Object)
#End Region

#Region "Properties"
    ReadOnly Property AppDriver() As GRider
        Get
            Return p_oAppDrvr
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

    WriteOnly Property POSNumbr As String
        Set(ByVal Value As String)
            p_sPOSNo = Value
        End Set
    End Property

    ReadOnly Property CloseForm() As Boolean
        Get
            Return p_bCloseForm
        End Get
    End Property

    ReadOnly Property CreditCardTrans() As DataTable
        Get
            Return p_oDataTable
        End Get
    End Property

    Property SourceNo() As String
        Get
            Return p_sSourceNo
        End Get
        Set(ByVal Value As String)
            p_sSourceNo = Value
        End Set
    End Property

    Property SourceCd() As String
        Get
            Return p_sSourceCd
        End Get
        Set(ByVal Value As String)
            p_sSourceCd = Value
        End Set
    End Property

    ReadOnly Property ItemCount() As Long
        Get
            Return p_oDataTable.Rows.Count
        End Get
    End Property

    Property Master(ByVal Row As Integer,
                    ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "stermnlid" : Index = 1
                    Case "sbankidxx" : Index = 2
                    Case "scardnoxx" : Index = 3
                    Case "sapprovno" : Index = 4
                    Case "namountxx" : Index = 5
                    Case "stermidxx" : Index = 6
                    Case "ssourcecd" : Index = 7
                    Case "ssourceno" : Index = 8
                    Case "scollectd" : Index = 9
                    Case "dcollectd" : Index = 10
                    Case "cdebitcrd" : Index = 11
                    Case "ctranstat" : Index = 12
                    Case "stermnlnm" : Index = 13
                        'If p_oDataTable(Row)("sTermnlID") = "" Then Return String.Empty
                        'Dim loRow As DataTable
                        'loRow = p_oTerminal.GetBank(p_oDataTable(Row)("sTermnlID"), True)
                        'If Not IsNothing(loRow) Then
                        '    Return loRow.Rows(0)("sBankName")
                        'Else
                        '    Return String.Empty
                        'End If
                    Case "sbankname" : Index = 14
                        'If p_oDataTable(Row)("sBankIDxx") = "" Then Return String.Empty
                        'Dim loRow As DataTable
                        'loRow = p_oBank.GetBank(p_oDataTable(Row)("sBankIDxx"), True)
                        'If Not IsNothing(loRow) Then
                        '    Return loRow.Rows(0)("sBankName")
                        'Else
                        '    Return String.Empty
                        'End If
                    Case "stermname"
                        If p_oDataTable(Row)("sTermIDxx") = "" Then Return String.Empty
                        Dim loRow As DataTable
                        loRow = p_oTerm.GetTerm(p_oDataTable(Row)("sTermIDxx"), True)
                        If Not IsNothing(loRow) Then
                            Return loRow.Rows(0)("sTermName")
                        Else
                            Return String.Empty
                        End If
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oDataTable(Row)(Index)
        End Get

        Set(ByVal Value As Object)
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "stermnlid" : Index = 1
                    Case "sbankidxx" : Index = 2
                    Case "scardnoxx" : Index = 3
                    Case "sapprovno" : Index = 4
                    Case "namountxx" : Index = 5
                        If Not IsNumeric(Value) Then Value = 0.0
                        RaiseEvent MasterRetrieved(Row, 5, Value)
                    Case "stermIDxx" : Index = 6
                    Case "ssourcecd" : Index = 7
                    Case "ssourceno" : Index = 8
                    Case "scollectd" : Index = 9
                    Case "dcollectd" : Index = 10
                        If Not IsDate(Value) Then Value = p_oAppDrvr.SysDate
                    Case "cdebitcrd" : Index = 11
                    Case "ctranstat" : Index = 12
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                End Select
            End If
            p_oDataTable(Row)(Index) = Value
        End Set
    End Property
#End Region

#Region "Public Function"
    Function SaveTransaction() As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim lnCtr As Integer
        Dim lnTotal As Decimal

        With p_oDataTable
            For lnCtr = 0 To .Rows.Count - 1
                If Not isEntryOK(lnCtr) Then Return False

                lsSQL = "INSERT INTO " & pxeMasterTble & " SET" &
                            "  sTransNox = " & strParm(.Rows(lnCtr)("sTransNox")) &
                            ", sTermnlID = " & strParm(.Rows(lnCtr)("sTermNlID")) &
                            ", sBankIDxx = " & strParm(.Rows(lnCtr)("sBankIDxx")) &
                            ", sCardNoxx = " & strParm(.Rows(lnCtr)("sCardNoxx")) &
                            ", sApprovNo = " & strParm(.Rows(lnCtr)("sApprovNo")) &
                            ", nAmountxx = " & CDec(.Rows(lnCtr)("nAmountxx")) &
                            ", sTermIDxx = " & strParm(.Rows(lnCtr)("sTermIDxx")) &
                            ", sSourceNo = " & strParm(p_sSourceNo) &
                            ", sSourceCd = " & strParm(p_sSourceCd) &
                            ", cDebitCrd = " & strParm(.Rows(lnCtr)("cDebitCrd")) &
                            ", cTranStat = " & strParm(.Rows(lnCtr)("cTranStat")) &
                            ", dModified = " & dateParm(p_oAppDrvr.SysDate) &
                        " ON DUPLICATE KEY UPDATE" &
                            "  sTermnlID = " & strParm(.Rows(lnCtr)("sTermNlID")) &
                            ", sBankIDxx = " & strParm(.Rows(lnCtr)("sBankIDxx")) &
                            ", sCardNoxx = " & strParm(.Rows(lnCtr)("sCardNoxx")) &
                            ", sApprovNo = " & strParm(.Rows(lnCtr)("sApprovNo")) &
                            ", cDebitCrd = " & strParm(.Rows(lnCtr)("cDebitCrd")) &
                            ", nAmountxx = " & CDec(.Rows(lnCtr)("nAmountxx")) &
                            ", sTermIDxx = " & strParm(.Rows(lnCtr)("sTermIDxx"))

                Try
                    lnRow = p_oAppDrvr.ExecuteActionQuery(lsSQL)
                    If lnRow <= 0 Then
                        MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        Return False
                    End If
                Catch ex As Exception
                    Throw ex
                End Try

                lnTotal += CDec(.Rows(lnCtr)("nAmountxx"))
            Next lnCtr
            p_nCreditCard = lnTotal

            If Not p_sTransNox = String.Empty Then
                If p_oAppDrvr.ExecuteActionQuery("DELETE FROM " & pxeMasterTble &
                                                " WHERE sSourceNo = " & strParm(p_sSourceNo) &
                                                    " AND sSourceCd = " & strParm(p_sSourceCd) &
                                                    " AND sTransNox IN(" & p_sTransNox.Substring(0, p_sTransNox.Length - 1) & ")") <= 0 Then
                    'MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                    '                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    'Return False
                End If
            End If
        End With

        Return True
    End Function

    Function OpenTransaction(ByVal sTransNox As String) As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String
        Dim lnCtr As Integer
        Dim lnTotal As Decimal

        lsSQL = AddCondition(getSQL_Master, "a.sTransNox = " & strParm(sTransNox))
        MsgBox(lsSQL)
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return False

        Call createTable()
        With p_oDataTable
            For lnCtr = 0 To loDT.Rows.Count - 1
                Call AddCreditCard()
                .Rows(lnCtr)("sTransNox") = loDT.Rows(lnCtr)("sTransNox")
                .Rows(lnCtr)("sTermnlID") = loDT.Rows(lnCtr)("sTermnlID")
                .Rows(lnCtr)("sTermnlNm") = loDT.Rows(lnCtr)("xTermnlNm")
                .Rows(lnCtr)("sBankIDxx") = loDT.Rows(lnCtr)("sBankIDxx")
                .Rows(lnCtr)("sBankName") = loDT.Rows(lnCtr)("sBankName")
                .Rows(lnCtr)("sCardNoxx") = loDT.Rows(lnCtr)("sCardNoxx")
                .Rows(lnCtr)("sApprovNo") = loDT.Rows(lnCtr)("sApprovNo")
                .Rows(lnCtr)("nAmountxx") = loDT.Rows(lnCtr)("nAmountxx")
                .Rows(lnCtr)("sTermIDxx") = loDT.Rows(lnCtr)("sTermIDxx")
                .Rows(lnCtr)("sSourceCd") = loDT.Rows(lnCtr)("sSourceCd")
                .Rows(lnCtr)("sSourceNo") = loDT.Rows(lnCtr)("sSourceNo")
                .Rows(lnCtr)("sCollectd") = loDT.Rows(lnCtr)("sCollectd")
                .Rows(lnCtr)("dCollectd") = loDT.Rows(lnCtr)("dCollectd")
                .Rows(lnCtr)("cDebitCrd") = loDT.Rows(lnCtr)("cDebitCrd")
                .Rows(lnCtr)("cTranStat") = loDT.Rows(lnCtr)("cTranStat")
                lnTotal += loDT.Rows(lnCtr)("nAmountxx")
            Next lnCtr
            p_nCreditCard = lnTotal
        End With

        Return True
    End Function

    Function OpenBySource() As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String
        Dim lnCtr As Integer
        Dim lnTotal As Decimal

        lsSQL = AddCondition(getSQL_Master, "a.sSourceNo = " & strParm(p_sSourceNo)) &
                                                " AND a.sSourceCd = " & strParm(p_sSourceCd)

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return False

        Call createTable()
        With p_oDataTable
            For lnCtr = 0 To loDT.Rows.Count - 1
                Call AddCreditCard()
                .Rows(lnCtr)("sTransNox") = loDT.Rows(lnCtr)("sTransNox")
                .Rows(lnCtr)("sTermnlID") = loDT.Rows(lnCtr)("sTermnlID")
                .Rows(lnCtr)("sTermnlNm") = loDT.Rows(lnCtr)("xTermnlNm")
                .Rows(lnCtr)("sBankIDxx") = loDT.Rows(lnCtr)("sBankIDxx")
                .Rows(lnCtr)("sBankName") = loDT.Rows(lnCtr)("sBankName")
                .Rows(lnCtr)("sCardNoxx") = loDT.Rows(lnCtr)("sCardNoxx")
                .Rows(lnCtr)("sApprovNo") = loDT.Rows(lnCtr)("sApprovNo")
                .Rows(lnCtr)("nAmountxx") = loDT.Rows(lnCtr)("nAmountxx")
                .Rows(lnCtr)("sTermIDxx") = loDT.Rows(lnCtr)("sTermIDxx")
                .Rows(lnCtr)("sSourceCd") = loDT.Rows(lnCtr)("sSourceCd")
                .Rows(lnCtr)("sSourceNo") = loDT.Rows(lnCtr)("sSourceNo")
                .Rows(lnCtr)("sCollectd") = loDT.Rows(lnCtr)("sCollectd")
                .Rows(lnCtr)("dCollectd") = loDT.Rows(lnCtr)("dCollectd")
                .Rows(lnCtr)("cDebitCrd") = loDT.Rows(lnCtr)("cDebitCrd")
                .Rows(lnCtr)("cTranStat") = loDT.Rows(lnCtr)("cTranStat")
                lnTotal += loDT.Rows(lnCtr)("nAmountxx")
            Next lnCtr
            p_nCreditCard = lnTotal
        End With

        Return True
    End Function

    Function SearchBank(ByVal Row As Integer,
                          Optional Value As Object = "") As Boolean
        Return getBank(Row, Value)
    End Function

    Function SearchBank() As DataTable
        Return p_oBank.GetBank
    End Function

    Function SearchTerm(ByVal Row As Integer,
                          Optional Value As Object = "") As Boolean

        Return getTerm(Row, Value)
    End Function

    Function SearchTerm() As DataTable

        Return p_oTerm.GetTerm
    End Function

    Function SearchTerminal(ByVal Row As Integer,
                          Optional Value As Object = "") As Boolean

        Return getTerminal(Row, Value)
    End Function

    Function SearchTerminal() As DataTable

        Return p_oTerminal.GetBank
    End Function

    Function AddCreditCard() As Boolean
        Dim lnRow As Integer = p_oDataTable.Rows.Count

        If lnRow > 0 Then If Not isEntryOK(lnRow - 1) Then Return False
        If Not SaveTransaction() Then Return False

        With p_oDataTable
            .Rows.Add()
            .Rows(lnRow)("sTransNox") = GetNextCode(pxeMasterTble, "sTransNox", True, p_oAppDrvr.Connection, True, p_sBranchCd)
            .Rows(lnRow)("sTermnlID") = ""
            .Rows(lnRow)("sTermnlNm") = ""
            .Rows(lnRow)("sBankIDxx") = ""
            .Rows(lnRow)("sBankName") = ""
            .Rows(lnRow)("sCardNoxx") = ""
            .Rows(lnRow)("sApprovNo") = ""
            .Rows(lnRow)("nAmountxx") = 0.0
            .Rows(lnRow)("sTermIDxx") = ""
            .Rows(lnRow)("sSourceCd") = 0
            .Rows(lnRow)("sSourceNo") = ""
            .Rows(lnRow)("sSourceNo") = ""
            .Rows(lnRow)("cDebitCrd") = "0"
            .Rows(lnRow)("cTranStat") = Val(xeTranStat.TRANS_OPEN)
        End With

        Return True
    End Function

    Function DeleteCreditCard(ByVal Row As Integer) As Boolean
        With p_oDataTable
            If .Rows.Count - 1 < Row Then Return False
            p_sTransNox = p_sTransNox & "'" & .Rows(Row)("sTransNox") & "',"
            .Rows(Row).Delete()
        End With

        Return True
    End Function
#End Region

#Region "Private function"
    Private Function NewTransaction() As Boolean
        Call createTable()
        Call initMaster()

        Return True
    End Function

    Private Function getBank(ByVal Row As Integer,
                             ByVal Value As String) As Boolean
        Dim lsCondition As String
        Dim lsProcName As String
        Dim loDataRow As DataRow

        lsProcName = "getBank"

        lsCondition = String.Empty
        If Value <> String.Empty Then
            If Value = p_oDataTable(Row)("sBankName") Then Return True
        End If

        loDataRow = p_oBank.SearchBank(Value, False)
        If Not IsNothing(loDataRow) Then
            p_oDataTable(Row)("sBankIDxx") = loDataRow("sBankCode")
            p_oDataTable(Row)("sBankName") = loDataRow("sBankName")
            RaiseEvent MasterRetrieved(Row, 2, loDataRow("sBankName"))
        Else
            p_oDataTable(Row)("sBankIDxx") = ""
            p_oDataTable(Row)("sBankName") = ""
            RaiseEvent MasterRetrieved(Row, 2, String.Empty)
        End If

        Return True
    End Function

    Private Function getTerm(ByVal Row As Integer,
                             ByVal Value As String) As Boolean
        Dim lsCondition As String
        Dim lsProcName As String
        Dim loDataRow As DataRow

        lsProcName = "getTerm"

        lsCondition = String.Empty

        If Value <> String.Empty Then
            If Value = p_sTermName Then Return True
        End If

        loDataRow = p_oTerm.SearchTerm(Value, False)
        If Not IsNothing(loDataRow) Then
            p_oDataTable(Row)("sTermIDxx") = loDataRow("sTermIDxx")
            p_sTermName = loDataRow("sTermName")
            RaiseEvent MasterRetrieved(Row, 6, p_sTermName)
        Else
            p_oDataTable(Row)("sTermName") = ""
            p_sTermName = ""
            RaiseEvent MasterRetrieved(Row, 6, String.Empty)
        End If

        Return True
    End Function

    Private Function getTerminal(ByVal Row As Integer,
                             ByVal Value As String) As Boolean
        Dim lsCondition As String
        Dim lsProcName As String
        Dim loDataRow As DataRow

        lsProcName = "getTerminal"

        lsCondition = String.Empty

        If Value <> String.Empty Then
            If Value = p_sTermnlNm Then Return True
        End If

        loDataRow = p_oBank.SearchBank(Value, False)
        If Not IsNothing(loDataRow) Then
            p_oDataTable(Row)("sTermnlID") = loDataRow("sBankCode")
            p_oDataTable(Row)("sTermnlNm") = loDataRow("sBankName")
            RaiseEvent MasterRetrieved(Row, 1, loDataRow("sBankName"))
        Else
            p_oDataTable(Row)("sTermnlID") = ""
            p_oDataTable(Row)("sTermnlNm") = ""
            RaiseEvent MasterRetrieved(Row, 1, String.Empty)
        End If

        Return True
    End Function

    Private Function getSQL_Master() As String
        Return "SELECT" &
                    "  a.sTransNox" &
                    ", a.sTermnlID" &
                    ", a.sBankIDxx" &
                    ", a.sCardNoxx" &
                    ", a.sApprovNo" &
                    ", a.nAmountxx" &
                    ", a.sTermIDxx" &
                    ", a.sSourceCd" &
                    ", a.sSourceNo" &
                    ", a.sCollectd" &
                    ", a.dCollectd" &
                    ", a.cDebitCrd" &
                    ", a.cTranStat" &
                    ", b.sBankName" &
                    ", c.sBankName xTermnlNm" &
                " FROM " & pxeMasterTble & " a" &
                        " LEFT JOIN Banks b" &
                            " ON a.sBankIDxx = b.sBankCode" &
                        " LEFT JOIN Banks c" &
                            " ON a.sTermnlID = c.sBankCode"
    End Function
#End Region

#Region "Private Procedures"
    Private Sub createTable()
        p_oDataTable = New DataTable
        With p_oDataTable
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("sTermnlID", GetType(String)).MaxLength = 4
            .Columns.Add("sBankIDxx", GetType(String)).MaxLength = 4
            .Columns.Add("sCardNoxx", GetType(String)).MaxLength = 16
            .Columns.Add("sApprovNo", GetType(String)).MaxLength = 8
            .Columns.Add("nAmountxx", GetType(Decimal))
            .Columns.Add("sTermIDxx", GetType(String)).MaxLength = 7
            .Columns.Add("sSourceCd", GetType(String)).MaxLength = 4
            .Columns.Add("sSourceNo", GetType(String)).MaxLength = 20
            .Columns.Add("sCollectd", GetType(String)).MaxLength = 10
            .Columns.Add("dCollectd", GetType(DateTime))
            .Columns.Add("cDebitCrd", GetType(String)).MaxLength = 1
            .Columns.Add("cTranStat", GetType(String)).MaxLength = 1
            .Columns.Add("sTermnlNm", GetType(String)).MaxLength = 30
            .Columns.Add("sBankName", GetType(String)).MaxLength = 30
        End With
    End Sub

    Private Sub initMaster()
        With p_oDataTable
            .Rows.Add()
            .Rows(0)("sTransNox") = getNextTransNo()
            .Rows(0)("sTermnlID") = ""
            .Rows(0)("sBankIDxx") = ""
            .Rows(0)("sCardNoxx") = ""
            .Rows(0)("sApprovNo") = ""
            .Rows(0)("nAmountxx") = 0.0
            .Rows(0)("sTermIDxx") = ""
            .Rows(0)("sSourceCd") = p_sSourceCd
            .Rows(0)("sSourceNo") = p_sSourceNo
            .Rows(0)("cTranStat") = Val(xeTranStat.TRANS_OPEN)
            .Rows(0)("sCollectd") = p_oAppDrvr.UserID
            .Rows(0)("dCollectd") = Now()
            .Rows(0)("sBankName") = ""
            .Rows(0)("sTermnlNm") = ""
            .Rows(0)("cDebitCrd") = "0"
        End With
    End Sub

    Private Function isEntryOK(ByVal fnRowNo As Integer) As Boolean
        ' verify the required fields
        If p_oDataTable.Rows(fnRowNo)("sTermnlID") = String.Empty Then
            MsgBox("Invalid Transaction Terminal Detected!!!" & vbCrLf & vbCrLf &
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        If p_oDataTable.Rows(fnRowNo)("sBankIDxx") = String.Empty Then
            MsgBox("Invalid Transaction Bank Detected!!!" & vbCrLf & vbCrLf &
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        'If p_oDataTable.Rows(fnRowNo)("sCardNoxx") = String.Empty Or Strings.Len(p_oDataTable.Rows(fnRowNo)("sCardNoxx")) < 10 Then
        If p_oDataTable.Rows(fnRowNo)("sCardNoxx") = String.Empty Then
            MsgBox("Invalid Transaction Card Number Detected!!!" & vbCrLf & vbCrLf &
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        If p_oDataTable.Rows(fnRowNo)("sApprovNo") = String.Empty Then
            MsgBox("Invalid Transaction Approved Number Detected!!!" & vbCrLf & vbCrLf &
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        If p_oDataTable.Rows(fnRowNo)("nAmountxx") <= 0.0 Then
            MsgBox("Invalid Transaction Check Amount Detected!!!" & vbCrLf & vbCrLf &
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        Return True
    End Function
#End Region

#Region "Public Procedures"
    Sub ShowCreditCard()
        'p_oFormPayCredit = New frmPayCreditNeo
        'With p_oFormPayCredit
        '    .CreditCard = Me
        '    .TopMost = True
        '    .ShowDialog()
        '    p_bCloseForm = .CloseForm
        'End With
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oAppDrvr = foRider
        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oAppDrvr.BranchCode
        p_oBank = New clsBanks(p_oAppDrvr, False)
        p_oTerminal = New clsBanks(p_oAppDrvr, False)
        p_oTerm = New clsTerm(p_oAppDrvr, False)

        Call NewTransaction()

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub

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
End Class