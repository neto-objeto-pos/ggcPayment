'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Merge Bills
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
'  Jheff [ 11/10/2016 02:58 pm ]
'     Start coding this object...
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports ggcAppDriver
Imports ggcRetailParams
Imports MySql.Data.MySqlClient

Public Class CheckPayment

#Region "Constant"
    Private Const pxeMODULENAME As String = "CheckPayment"
    Private Const pxeMasterTble As String = "Check_Payment_Trans"
#End Region

#Region "Event"
    Public Event MasterRetrieved(ByVal Row As Integer, _
                                 ByVal Index As Object, _
                                 ByVal Value As Object)
#End Region

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oDataTable As DataTable
    Protected p_oBank As clsBanks
    Protected p_nEditMode As xeEditMode
    Protected p_sPOSNo As String

    Protected p_sBankName As String
    Protected p_sBranchCd As String
    Protected p_bCloseForm As Boolean
    Protected p_sSourceNo As String
    Protected p_sSourceCd As String
    Protected p_sTransNox As String
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

    ReadOnly Property CheckPaymTrans() As DataTable
        Get
            Return p_oDataTable
        End Get
    End Property

    Property TranDate() As Date
        Get
            Return p_oAppDrvr.getSysDate
        End Get
        Set(ByVal value As Date)

        End Set
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

    Property Master(ByVal Row As Integer, _
                    ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then Index = LCase(Index)
            Select Case Index
                Case "stransnox" : Index = 0
                Case "scustname" : Index = 1
                Case "dtransact" : Index = 2
                Case "sbankidxx" : Index = 3
                Case "schecknox" : Index = 4
                Case "sacctnmbr" : Index = 5
                Case "dcheckdte" : Index = 6
                Case "namountxx" : Index = 7
                Case "sremarksx" : Index = 8
                Case "nclearday" : Index = 9
                Case "ssourcecd" : Index = 10
                Case "ssourceno" : Index = 11
                Case "ctranstat" : Index = 12
                Case "sbankname" : Index = 13
                    'If p_oDataTable(Row)("sBankIDxx") = "" Then Return String.Empty
                    'Dim loRow As DataTable
                    'loRow = p_oBank.GetBank(p_oDataTable(Row)("sBankIDxx"), True)
                    'If Not IsNothing(loRow) Then
                    '    Return loRow.Rows(0)("sBankName")
                    'Else
                    '    Return String.Empty
                    'End If
                Case Else
                    MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                    Return DBNull.Value
            End Select
            Return p_oDataTable(Row)(Index)
        End Get

        Set(ByVal Value As Object)
            Select Case Index
                Case "stransnox" : Index = 0
                Case "scustname" : Index = 1
                Case "dTransact" : Index = 2
                    If Not IsDate(Value) Then Value = p_oAppDrvr.SysDate
                Case "sbankidxx" : Index = 3
                Case "sacctnmbr" : Index = 4
                Case "schecknox" : Index = 5
                Case "dcheckdte" : Index = 6
                    If Not IsDate(Value) Then Value = p_oAppDrvr.SysDate
                    RaiseEvent MasterRetrieved(Row, 6, Value)
                Case "namountxx" : Index = 7
                    If Not IsNumeric(Value) Then Value = 0.0
                    RaiseEvent MasterRetrieved(Row, 7, Value)
                Case "sremarksx" : Index = 8
                Case "nclearday" : Index = 9
                Case "ssourcecd" : Index = 10
                Case "ssourceno" : Index = 11
                Case "ctranstat" : Index = 12
                Case Else
                    MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                    Exit Property
            End Select
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

                lsSQL = "INSERT INTO " & pxeMasterTble & " SET" & _
                            "  sTransNox = " & strParm(.Rows(lnCtr)("sTransNox")) & _
                            ", sCustName = " & strParm(.Rows(lnCtr)("sCustName")) & _
                            ", dTransact = " & dateParm(.Rows(lnCtr)("dTransact")) & _
                            ", sBankIDxx = " & strParm(.Rows(lnCtr)("sBankIDxx")) & _
                            ", sAcctNmbr = " & strParm(.Rows(lnCtr)("sAcctNmbr")) & _
                            ", sCheckNox = " & strParm(.Rows(lnCtr)("sCheckNox")) & _
                            ", dCheckDte = " & dateParm(.Rows(lnCtr)("dCheckDte")) & _
                            ", nAmountxx = " & CDec(.Rows(lnCtr)("nAmountxx")) & _
                            ", sRemarksx = " & strParm(.Rows(lnCtr)("sRemarksx")) & _
                            ", nClearDay = " & CInt(.Rows(lnCtr)("nClearDay")) & _
                            ", sSourceNo = " & strParm(p_sSourceNo) & _
                            ", sSourceCd = " & strParm(p_sSourceCd) & _
                            ", cTranStat = " & strParm(.Rows(lnCtr)("cTranStat")) & _
                            ", dModified = " & dateParm(p_oAppDrvr.SysDate) & _
                        " ON DUPLICATE KEY UPDATE" & _
                            "  sCustName = " & strParm(.Rows(lnCtr)("sCustName")) & _
                            ", dTransact = " & dateParm(.Rows(lnCtr)("dTransact")) & _
                            ", sBankIDxx = " & strParm(.Rows(lnCtr)("sBankIDxx")) & _
                            ", sAcctNmbr = " & strParm(.Rows(lnCtr)("sAcctNmbr")) & _
                            ", sCheckNox = " & strParm(.Rows(lnCtr)("sCheckNox")) & _
                            ", dCheckDte = " & dateParm(.Rows(lnCtr)("dCheckDte")) & _
                            ", nAmountxx = " & CDec(.Rows(lnCtr)("nAmountxx")) & _
                            ", sRemarksx = " & strParm(.Rows(lnCtr)("sRemarksx")) & _
                            ", nClearDay = " & CInt(.Rows(lnCtr)("nClearDay"))

                Try
                    lnRow = p_oAppDrvr.Execute(lsSQL, pxeMasterTble)
                    If lnRow <= 0 Then
                        MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        Return False
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
                lnTotal += CDec(.Rows(lnCtr)("nAmountxx"))
            Next lnCtr
            p_nCheck = lnTotal

            If Not p_sTransNox = String.Empty Then
                If p_oAppDrvr.Execute("DELETE FROM " & pxeMasterTble &
                                                " WHERE sSourceNo = " & strParm(p_sSourceNo) &
                                                    " AND sSourceCd = " & strParm(p_sSourceCd) &
                                                    " AND sTransNox IN(" & p_sTransNox.Substring(0, p_sTransNox.Length - 1) & ")", pxeMasterTble) <= 0 Then
                    'MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                    '                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    'Return False
                End If
            End If
        End With

        Return True
    End Function

    Function SearchBank(ByVal Row As Integer, _
                          Optional Value As Object = "") As Boolean

        Return getBank(Row, Value)
    End Function

    Function SearchBank() As DataTable
        Return p_oBank.GetBank
    End Function

    Function AddCheck() As Boolean
        Dim lnRow As Integer = p_oDataTable.Rows.Count

        If lnRow > 0 Then If Not isEntryOK(lnRow - 1) Then Return False
        If Not SaveTransaction() Then Return False

        With p_oDataTable
            .Rows.Add()
            .Rows(lnRow)("sTransNox") = GetNextCode(pxeMasterTble, "sTransNox", True, p_oAppDrvr.Connection, True, p_sBranchCd)
            .Rows(lnRow)("dTransact") = p_oAppDrvr.SysDate
            .Rows(lnRow)("sBankIDxx") = ""
            .Rows(lnRow)("sCheckNox") = ""
            .Rows(lnRow)("dCheckDte") = p_oAppDrvr.SysDate
            .Rows(lnRow)("nAmountxx") = 0.0
            .Rows(lnRow)("sRemarksx") = ""
            .Rows(lnRow)("nClearDay") = 0
            .Rows(lnRow)("sSourceCd") = ""
            .Rows(lnRow)("sSourceNo") = ""
            .Rows(lnRow)("cTranStat") = Val(xeTranStat.TRANS_OPEN)
            .Rows(lnRow)("sCustName") = ""
            .Rows(lnRow)("sAcctNmbr") = ""
            .Rows(lnRow)("sBankName") = ""
        End With

        Return True
    End Function

    Function DeleteCheck(ByVal Row As Integer) As Boolean
        With p_oDataTable
            If .Rows.Count - 1 < Row Then Return False
            p_sTransNox = p_sTransNox & "'" & .Rows(Row)("sTransNox") & "',"
            .Rows(Row).Delete()
        End With

        Return True
    End Function

    Function OpenTransaction(ByVal sTransNox As String) As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String
        Dim lnCtr As Integer
        Dim lnTotal As Decimal

        lsSQL = AddCondition(getSQL_Master, "a.sTransNox = " & strParm(sTransNox))

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return False

        Call createTable()
        With p_oDataTable
            For lnCtr = 0 To loDT.Rows.Count - 1
                Call AddCheck()
                .Rows(lnCtr)("sTransNox") = loDT.Rows(lnCtr)("sTransNox")
                .Rows(lnCtr)("sCustName") = loDT.Rows(lnCtr)("sCustName")
                .Rows(lnCtr)("dTransact") = loDT.Rows(lnCtr)("dTransact")
                .Rows(lnCtr)("sBankIDxx") = loDT.Rows(lnCtr)("sBankIDxx")
                .Rows(lnCtr)("sAcctNmbr") = loDT.Rows(lnCtr)("sAcctNmbr")
                .Rows(lnCtr)("sCheckNox") = loDT.Rows(lnCtr)("sCheckNox")
                .Rows(lnCtr)("dCheckDte") = loDT.Rows(lnCtr)("dCheckDte")
                .Rows(lnCtr)("nAmountxx") = loDT.Rows(lnCtr)("nAmountxx")
                .Rows(lnCtr)("sRemarksx") = loDT.Rows(lnCtr)("sRemarksx")
                .Rows(lnCtr)("nClearDay") = loDT.Rows(lnCtr)("nClearDay")
                .Rows(lnCtr)("sSourceCd") = loDT.Rows(lnCtr)("sSourceCd")
                .Rows(lnCtr)("sSourceNo") = loDT.Rows(lnCtr)("sSourceNo")
                .Rows(lnCtr)("cTranStat") = loDT.Rows(lnCtr)("cTranStat")
                .Rows(lnCtr)("sBankName") = loDT.Rows(lnCtr)("sBankName")
                lnTotal += loDT.Rows(lnCtr)("nAmountxx")
            Next lnCtr
            p_nCheck = lnTotal
        End With

        Return True
    End Function

    Function OpenBySource() As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String
        Dim lnCtr As Integer
        Dim lnTotal As Decimal

        lsSQL = AddCondition(getSQL_Master, "a.sSourceNo = " & strParm(p_sSourceNo)) & _
                                                " AND a.sSourceCd = " & strParm(p_sSourceCd)

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return False

        Call createTable()
        With p_oDataTable
            For lnCtr = 0 To loDT.Rows.Count - 1
                Call AddCheck()
                .Rows(lnCtr)("sTransNox") = loDT.Rows(lnCtr)("sTransNox")
                .Rows(lnCtr)("sCustName") = loDT.Rows(lnCtr)("sCustName")
                .Rows(lnCtr)("dTransact") = loDT.Rows(lnCtr)("dTransact")
                .Rows(lnCtr)("sBankIDxx") = loDT.Rows(lnCtr)("sBankIDxx")
                .Rows(lnCtr)("sAcctNmbr") = loDT.Rows(lnCtr)("sAcctNmbr")
                .Rows(lnCtr)("sCheckNox") = loDT.Rows(lnCtr)("sCheckNox")
                .Rows(lnCtr)("dCheckDte") = loDT.Rows(lnCtr)("dCheckDte")
                .Rows(lnCtr)("nAmountxx") = loDT.Rows(lnCtr)("nAmountxx")
                .Rows(lnCtr)("sRemarksx") = loDT.Rows(lnCtr)("sRemarksx")
                .Rows(lnCtr)("nClearDay") = loDT.Rows(lnCtr)("nClearDay")
                .Rows(lnCtr)("sSourceCd") = loDT.Rows(lnCtr)("sSourceCd")
                .Rows(lnCtr)("sSourceNo") = loDT.Rows(lnCtr)("sSourceNo")
                .Rows(lnCtr)("cTranStat") = loDT.Rows(lnCtr)("cTranStat")
                lnTotal += loDT.Rows(lnCtr)("nAmountxx")
            Next lnCtr
            p_nCheck = lnTotal
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

    Private Function getBank(ByVal Row As Integer, _
                             ByVal Value As String) As Boolean
        Dim lsCondition As String
        Dim lsProcName As String
        Dim loDataRow As DataRow

        lsProcName = "getBank"

        lsCondition = String.Empty

        'If Value <> String.Empty Then
        '    If Value = p_sBankName Then Return True
        'End If

        If Value <> String.Empty Then
            If Value = p_oDataTable(Row)("sBankName") Then Return True
        End If


        'p_oBank.GetBank()
        loDataRow = p_oBank.SearchBank(Value, False)
        If Not IsNothing(loDataRow) Then
            p_oDataTable(Row)("sBankIDxx") = loDataRow("sBankCode")
            p_oDataTable(Row)("sBankName") = loDataRow("sBankName")
            RaiseEvent MasterRetrieved(Row, 3, loDataRow("sBankName"))
        Else
            p_oDataTable(Row)("sBankIDxx") = ""
            p_oDataTable(Row)("sBankName") = ""
            RaiseEvent MasterRetrieved(Row, 3, String.Empty)
        End If

        Return True
    End Function

    Private Function getSQL_Master() As String
        Return "SELECT" & _
                    "  a.sTransNox" & _
                    ", a.sCustName" & _
                    ", a.dTransact" & _
                    ", a.sBankIDxx" & _
                    ", a.sAcctNmbr" & _
                    ", a.sCheckNox" & _
                    ", a.dCheckDte" & _
                    ", a.nAmountxx" & _
                    ", a.sRemarksx" & _
                    ", a.nClearDay" & _
                    ", a.sSourceCd" & _
                    ", a.sSourceNo" & _
                    ", a.cTranStat" & _
                    ", b.sBankName" & _
                " FROM " & pxeMasterTble & " a" & _
                    ", Banks b" & _
                " WHERE a.sBankIDxx = b.sBankCode"
    End Function
#End Region

#Region "Private Procedures"
    Private Sub createTable()
        p_oDataTable = New DataTable
        With p_oDataTable
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("sCustName", GetType(String)).MaxLength = 64
            .Columns.Add("dTransact", GetType(Date))
            .Columns.Add("sBankIDxx", GetType(String)).MaxLength = 4
            .Columns.Add("sAcctNmbr", GetType(String)).MaxLength = 15
            .Columns.Add("sCheckNox", GetType(String)).MaxLength = 15
            .Columns.Add("dCheckDte", GetType(Date))
            .Columns.Add("nAmountxx", GetType(Decimal))
            .Columns.Add("sRemarksx", GetType(String)).MaxLength = 64
            .Columns.Add("nClearDay", GetType(Integer))
            .Columns.Add("sSourceCd", GetType(String)).MaxLength = 4
            .Columns.Add("sSourceNo", GetType(String)).MaxLength = 20
            .Columns.Add("cTranStat", GetType(String)).MaxLength = 1
            .Columns.Add("sBankName", GetType(String)).MaxLength = 30
        End With
    End Sub

    Private Sub initMaster()
        With p_oDataTable
            .Rows.Add()
            .Rows(0)("sTransNox") = getNextTransNo()
            .Rows(0)("sCustName") = ""
            .Rows(0)("dTransact") = p_oAppDrvr.SysDate
            .Rows(0)("sBankIDxx") = ""
            .Rows(0)("sAcctNmbr") = ""
            .Rows(0)("sCheckNox") = ""
            .Rows(0)("dCheckDte") = p_oAppDrvr.SysDate
            .Rows(0)("nAmountxx") = 0.0
            .Rows(0)("sRemarksx") = ""
            .Rows(0)("nClearDay") = 0
            .Rows(0)("sSourceCd") = ""
            .Rows(0)("sSourceNo") = ""
            .Rows(0)("cTranStat") = Val(xeTranStat.TRANS_OPEN)
            .Rows(0)("sBankName") = ""
        End With
    End Sub

    Private Function isEntryOK(ByVal fnRowNo As Integer) As Boolean
        ' verify the required fields
        If p_oDataTable.Rows(fnRowNo)("sBankIDxx") = String.Empty Then
            MsgBox("Invalid Transaction BankID Detected!!!" & vbCrLf & vbCrLf & _
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        If p_oDataTable.Rows(fnRowNo)("sAcctNmbr") = String.Empty Then
            MsgBox("Invalid Transaction AcctNo Detected!!!" & vbCrLf & vbCrLf & _
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        If p_oDataTable.Rows(fnRowNo)("sCheckNox") = String.Empty Then
            MsgBox("Invalid Transaction CheckNo Detected!!!" & vbCrLf & vbCrLf & _
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        If p_oDataTable.Rows(fnRowNo)("nAmountxx") <= 0.0 Then
            MsgBox("Invalid Transaction Check Amount Detected!!!" & vbCrLf & vbCrLf & _
                   "Verify your Entry then Try Again!!!", vbCritical, "Warning")
            Return False
        End If

        Return True
    End Function
#End Region

#Region "Public Procedures"
    Sub ShowCheck()
        p_oFormCheck = New frmPayCheck
        With p_oFormCheck
            .Check = Me
            .TopMost = True
            .ShowDialog()
            p_bCloseForm = .CloseForm
        End With
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oAppDrvr = foRider
        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oAppDrvr.BranchCode
        p_nEditMode = xeEditMode.MODE_UNKNOWN
        p_oBank = New clsBanks(p_oAppDrvr, False)

        Call NewTransaction()
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

        lsSQL = "SELECT sTransNox" & _
                " FROM " & pxeMasterTble & _
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy") & "%") & _
                " ORDER BY sTransNox DESC" & _
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